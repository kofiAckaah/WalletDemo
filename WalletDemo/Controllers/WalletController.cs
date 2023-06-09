using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.Requests;

namespace WalletDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;

        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }
        

        [HttpGet("allWallets")]
        public async Task<IActionResult> GetUserWallets()
        {
            var response = await walletService.GetAllWallets(HttpContext.User.Identity.Name);
            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            var badResponse = new ContentResult
            {
                StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                Content = response.ToString(),
                ContentType = "json"
            };
            return badResponse;
        }

        [HttpGet("wallet/{walletName}")]
        public async Task<IActionResult> GetUserWallets(string walletName)
        {
            var response = await walletService.GetSingleWallet(walletName, HttpContext.User.Identity.Name);
            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            var badResponse = new ContentResult
            {
                StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                Content = response.ToString(),
                ContentType = "json"
            };
            return badResponse;
        }

        [HttpGet("adminWallets")]
        public async Task<IActionResult> GetAdminUserWallets()
        {
            var response = await walletService.GetAllUserWallets(HttpContext.User.Identity.Name);
            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }
            var badResponse = new ContentResult { StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, Content = response.ToString(), ContentType = "json" };
            return badResponse;
        }

        [HttpPost("addWallet")]
        public async Task<IActionResult> AddWallet(WalletRequest request)
        {
            var response = await walletService.CreateWallet(request, HttpContext.User.Identity.Name);
            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }
            var badResponse = new ContentResult { StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, Content = response.ToString(), ContentType = "json" };
            return badResponse;
        }

        [HttpDelete("deleteWallet/{walletName}")]
        public async Task<IActionResult> DeleteWallet(string walletName)
        {
            var response = await walletService.DeleteWallet(walletName, HttpContext.User.Identity.Name);
            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }
            var badResponse = new ContentResult { StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, Content = response.ToString(), ContentType = "json" };
            return badResponse;
        }
    }
}
