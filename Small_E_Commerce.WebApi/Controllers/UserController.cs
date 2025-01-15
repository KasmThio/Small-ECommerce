using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Small_E_Commerce.Application;
using Small_E_Commerce.Application.Users.Commands;

namespace Small_E_Commerce.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(ISender sender) : ControllerBase
{


    [HttpPost("create-admin")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest request)
    {
        var command = new CreateAdminCommand(request);
        var result = await sender.Send(command);

        return result.IsSuccess
            ? Ok()
            : BadRequest();
    }
    
    [HttpPost("admin-login")]
    [AllowAnonymous]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var command = new AdminLoginCommand(request);
        var result = await sender.Send(command);

        return Ok(result);
    }
    
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> CreateUser([FromBody] SignUpRequest request)
    {
        var command = new SignUpCommand(request);
        var result = await sender.Send(command);

        return Ok(result);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request);
        var result = await sender.Send(command);

        return Ok(result);
    }
}