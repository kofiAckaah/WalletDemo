using Shared.Requests;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Responses;

namespace Shared.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseWrapper<string>> LoginUSerAsync(LoginRequest request);
        Task<ResponseWrapper<string>> SaveUserAsync(RegisterRequest request, string role);
    }
}
