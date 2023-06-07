using System;
using Domain.Enum;

namespace Domain.Entities
{
    public class Wallet : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsMomo { get; set; }
        public AccountScheme AccountScheme { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public string WalletOwnerId { get; set; }
        public ApplicationUser WalletOwner { get; set; }
    }
}
