using System.ComponentModel;

namespace Domain.Enum
{
    public enum AccountScheme
    {
        [Description("Visa Card")]
        Visa = 0,

        [Description("MasterCard")]
        MasterCard = 1,

        [Description("MTN Mobile Money")]
        MTN = 3,

        [Description("VodafoneCash")]
        Vodafone = 4,

        [Description("AirtelTigo Money")]
        AirtelTigo = 5,
    }
}
