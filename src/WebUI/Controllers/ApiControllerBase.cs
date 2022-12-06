using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    [NonAction]
    protected string GetHeaderValue(string headerName)
    {
        string headerValue = string.Empty;
        try
        {
            headerValue = Request.Headers[headerName]; //header.Value;
        }
        catch (Exception)
        {
        }

        return headerValue;
    }

    protected string Token => GetHeaderValue("AccessToken");
}
