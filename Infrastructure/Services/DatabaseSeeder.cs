using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Shared.Constants;
using Shared.Interfaces;

namespace Infrastructure.Services
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IUnitOfWork unitOfWork;

        public DatabaseSeeder(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task SeedRoles()
        {
            string[] roleNames = { RoleConstants.AdminRole, RoleConstants.UserRole };

            foreach (var roleName in roleNames)
            {
                var roleExist = unitOfWork.Repository<ApplicationRole>().Entities.FirstOrDefault(r=>r.Name == roleName);
                if (roleExist == null)
                {
                    var role = new ApplicationRole()
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                    };
                    await unitOfWork.Repository<ApplicationRole>().AddAsync(role);
                    await unitOfWork.Commit(CancellationToken.None);
                }
            }
        }
    }
    }
