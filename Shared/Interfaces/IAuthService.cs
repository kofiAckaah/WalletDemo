using Shared.Requests;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Responses;

namespace Shared.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Log user in.
        /// </summary>
        /// <param name="request">A <see cref="LoginRequest"/> model.</param>
        /// <returns>A string.</returns>
        Task<ResponseWrapper<string>> LoginUSerAsync(LoginRequest request);
        
        /// <summary>
        /// Save new user.
        /// </summary>
        /// <param name="request">A <see cref="RegisterRequest"/> model.</param>
        /// <param name="role">Role of the user.</param>
        /// <returns>A string.</returns>
        Task<ResponseWrapper<string>> SaveUserAsync(RegisterRequest request, string role);
    }
}
