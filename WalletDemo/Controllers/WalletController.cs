using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.Requests;

namespace WalletDemo.Controllers
{
    /// <summary>
    /// Controller with endpoints to manage user wallets.
    /// </summary>
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
        
        /// <summary>
        /// Get wallets by a particular user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/>.</returns>
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

        /// <summary>
        /// Gets a single wallet
        /// </summary>
        /// <param name="walletName"></param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        [HttpGet("wallet/{walletName}")]
        public async Task<IActionResult> GetSingleWallets(string walletName)
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

        /// <summary>
        /// Gets a single wallet by an admin
        /// </summary>
        /// <param name="walletName"></param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        [HttpGet("wallet/{walletName}")]
        public async Task<IActionResult> AdminGetSingleWallets(string walletName)
        {
            var response = await walletService.AdminGetSingleWallet(walletName, HttpContext.User.Identity.Name);
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
