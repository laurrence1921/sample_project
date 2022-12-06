using CleanArchitecture.Application.Account.Commands;
using CleanArchitecture.Application.Account.Models;
using CleanArchitecture.Application.AccountsViewer.Commands;
using CleanArchitecture.Application.AccountsViewer.Models;
using CleanArchitecture.Application.AccountsViewer.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;


public class AccountController : ApiControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserViewModel>> Login([FromBody] LoginCommand login)
    {
        return await Mediator.Send(login);
    }
}
