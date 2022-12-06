﻿using CleanArchitecture.Application.Account.Models;
using CleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<UserViewModel> SignIn(string username, string password, bool rememberMe, bool lockoutOnFailure);
}
