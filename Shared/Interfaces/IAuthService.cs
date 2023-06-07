using Shared.Requests;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.Interfaces
{
    public interface IAuthService
    {
        Task<GenericResponse> SaveUserAsync(RegisterRequest request, string role);
    }
}
