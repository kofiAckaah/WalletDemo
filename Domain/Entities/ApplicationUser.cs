using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity
    {
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
