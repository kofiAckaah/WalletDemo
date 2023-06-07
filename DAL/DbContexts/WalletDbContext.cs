using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.DbContexts
{
    public class WalletDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public WalletDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
    }
}
