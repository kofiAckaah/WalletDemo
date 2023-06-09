﻿using System;
using Domain.Enum;

namespace Domain.Entities
{
    public class Wallet : IEntity
    {
        public Wallet()
        {
            var lazy= new Lazy<ApplicationUser>();
            WalletOwner = lazy.Value;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string NumberHash { get; set; }
        public bool IsMomo { get; set; }
        public AccountScheme AccountScheme { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public Guid WalletOwnerId { get; set; }
        public ApplicationUser WalletOwner { get; set; }
    }
}
