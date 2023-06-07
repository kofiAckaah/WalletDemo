using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using Shared.Requests;
using System.Threading.Tasks;
using Shared.Enums;

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

        public async Task<GenericResponse> SaveUserAsync(RegisterRequest request, string role)
        {
            try
            {
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
                }

                return GenericResponse.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GenericResponse.Failed;
            }
            
        }
    }
}
