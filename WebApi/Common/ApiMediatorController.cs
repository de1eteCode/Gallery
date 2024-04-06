using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiMediatorController : Controller
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
}