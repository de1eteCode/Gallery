using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;

/// <summary>
/// Базовый API контроллер
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : Controller
{
}