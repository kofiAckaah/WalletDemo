using System;
using System.Linq;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using Shared.Requests;
using System.Threading.Tasks;
using Shared.Enums;
using Shared.Responses;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthenticationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<ResponseWrapper<string>> SaveUserAsync(RegisterRequest request, string role)
        {
            try
            {
                var exists = await userManager.FindByEmailAsync(request.Username);
                if (exists != null)
                    return new ResponseWrapper<string> { Message = "User already exists."};

                var user = new ApplicationUser()
                {
                    UserName = request.Username,
                    Email = request.Username,
                    PhoneNumber = request.PhoneNumber
                };
                
                var result = await userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var currentUser = await userManager.FindByNameAsync(user.UserName);
                    await userManager.AddToRoleAsync(currentUser, role);
                    return new ResponseWrapper<string>{Message = "User created successfully", Status = ResponseStatus.Success};
                }

                return new ResponseWrapper<string>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseWrapper<string>();
            }
        }

        public async Task<ResponseWrapper<string>> LoginUSerAsync(LoginRequest request)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(request.Username);
                if (user == null)
                    return new ResponseWrapper<string>();

                var roles = await userManager.GetRolesAsync(user);

                return new ResponseWrapper<string> { Status = ResponseStatus.Success, Data = roles.FirstOrDefault()};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseWrapper<string>();
            }
        }
    }
}
