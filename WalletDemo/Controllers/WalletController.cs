using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using System.Threading.Tasks;
using Shared.Enums;

namespace WalletDemo.Controllers
{
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
        public async Task<IActionResult> Get()
        {
            var response = await walletService.GetAllWallets(HttpContext.User.Identity.Name);
            return new ContentResult
            {
                StatusCode = response.Status == ResponseStatus.Success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                Content = response.Message,
                ContentType = "string"
            };
        }
    }
}
