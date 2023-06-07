using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Shared.Constants;
using Shared.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService authService;

    public AccountController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await authService.SaveUserAsync(model, RoleConstants.UserRole);
                if (response == GenericResponse.Failed)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                // Create claims for the authenticated user
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, RoleConstants.UserRole)
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return Ok();
            }

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("admin/register")]
    public async Task<IActionResult> AdminRegister(RegisterRequest model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await authService.SaveUserAsync(model, RoleConstants.UserRole);
                if (response == GenericResponse.Failed)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                // Create claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, RoleConstants.UserRole)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return Ok();
            }

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // Login action
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        // Validate the user's credentials
        if (ModelState.IsValid)
        {
            // Your code to authenticate the user from a database or other data store

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
                // Add additional claims if needed
            };

            // Create identity from the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user using the authentication middleware
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok();
        }

        // If the login fails, return an error response
        return BadRequest(ModelState);
    }

    // Logout action
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // Sign out the user
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }
}
