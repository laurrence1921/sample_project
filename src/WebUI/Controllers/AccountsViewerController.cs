using CleanArchitecture.Application.AccountsViewer.Commands;
using CleanArchitecture.Application.AccountsViewer.Models;
using CleanArchitecture.Application.AccountsViewer.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

public class AccountsViewerController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<bool>> UploadBalance(IFormFile file)
    {
        return await Mediator.Send(new UploadBalanceCommand { File = file });
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountsListDto>>> GetBalances([FromQuery] string month)
    {
        return await Mediator.Send(new QueryCurrentBalances { Month = month });
    }

}
