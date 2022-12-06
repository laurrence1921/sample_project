using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Account.Models;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IApplicationDbContext _context;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _context = context;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<UserViewModel> SignIn(string username, string password, bool rememberMe, bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await SignIn(_context, _userManager, user);
        }

        if (result.IsLockedOut)
        {
            //_logger.LogWarning(2, "User account locked out.");
            throw new Exception("Lockout");
        }

        throw new Exception("Invalid username or password");
    }

    #region private things
    private static async Task<UserViewModel> SignIn(IApplicationDbContext db, UserManager<ApplicationUser> userManager,
        ApplicationUser user, CancellationToken cancellationToken = default)
    {
        if (user is null)
        {
            throw new Exception("User not found");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SeCur3K3yH3re!@#$"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        var expiryDate = DateTime.Now.AddMinutes(30);
        var accesstoken = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: expiryDate,
            signingCredentials: creds);

        string token = "";
        var apptoken = await db.ApplicationUserTokens.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

        if (apptoken is null)
        {
            token = new JwtSecurityTokenHandler().WriteToken(accesstoken);
            await db.ApplicationUserTokens.AddAsync(new ApplicationUserToken { CreatedDate = DateTime.Now, 
                LoginProvider = "Email", 
                Name = user.Email, 
                UserId = user.Id, 
                ValidityDate = expiryDate, 
                Value = token, DeviceId = "",
                IpAddress = "127.0.0.1"
            });
            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            token = apptoken.Value;
        }

        //user.LastLoggedInDate = DateTime.Now;
        //db.ApplicationUsers.Update(user);
        //await db.SaveChangesAsync(cancellationToken);

        var roles = await userManager.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "User";

        var userModel = new UserViewModel
        {
            AccessToken = token,
            Id = user.Id,
            FullName = user.UserName,
            Email = user.Email,
            RoleName = role
        };

        return userModel;
    }

    #endregion
}
