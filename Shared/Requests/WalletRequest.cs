using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class WalletRequest
    {
        [Required]
        public string Name { get; set; }
        public string Number { get; set; }
        public bool IsMomo { get; set; }
        public AccountScheme AccountScheme { get; set; }
    }
}
