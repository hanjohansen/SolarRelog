using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SolarRelog.Controllers;

[ApiController]
[Route("api")]
public class Endpoint : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
}