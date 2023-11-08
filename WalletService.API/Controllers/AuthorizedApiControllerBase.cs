using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WalletService.API.Controllers;

[Route("api/[controller]")]
// [Authorize]
[ApiController]
public class AuthorizedApiControllerBase : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}