using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shared.Constants;
using Shared.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Microsoft.AspNet.Identity;
using Shared.Extensions;

namespace WalletDemo.Controllers
{
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
                    if (!model.PhoneNumber.IsValidPhoneNumber())
                        return new ContentResult
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Content = "Phone numbers should be only numbers",
                            ContentType = "string"
                        };
                    var response = await authService.SaveUserAsync(model, RoleConstants.UserRole);
                    if (response.Status == ResponseStatus.Failed)
                        return new ContentResult
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Content = response.Message,
                            ContentType = "string"
                        };

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, RoleConstants.UserRole)
                    };

                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    await HttpContext.SignInAsync(DefaultAuthenticationTypes.ApplicationCookie, new ClaimsPrincipal(identity));

                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Content = response.Message,
                        ContentType = "string"
                    }; ;
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
                    if (!model.PhoneNumber.IsValidPhoneNumber())
                        return new ContentResult
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Content = "Phone numbers should be only numbers",
                            ContentType = "string"
                        };
                    var response = await authService.SaveUserAsync(model, RoleConstants.UserRole);
                    if (response.Status == ResponseStatus.Failed)
                        return new ContentResult
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Content = response.Message,
                            ContentType = "string"
                        };
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, RoleConstants.AdminRole)
                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Content = response.Message,
                        ContentType = "string"
                    }; ;
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
            if (ModelState.IsValid)
            {
                var response = await authService.LoginUSerAsync(model);
                if (response.Status == ResponseStatus.Failed)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, response.Data)
            };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                await HttpContext.SignInAsync(DefaultAuthenticationTypes.ApplicationCookie, new ClaimsPrincipal(identity));

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // Logout action
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}