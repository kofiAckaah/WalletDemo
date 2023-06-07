using Microsoft.AspNetCore.Identity;

namespace WalletDemo.Extensions
{
    public static class IdentityConfigurationExtension
    {
        public static void ConfigureIdentityOptions(this IdentityOptions options)
        {
            options.Password = PasswordOptions;
            options.User.RequireUniqueEmail = true;
            options.SignIn = SignInOptions;
            options.Tokens = TokenOptions;
        }

        private static readonly TokenOptions TokenOptions = new TokenOptions()
        {
            EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider
        };

        private static readonly PasswordOptions PasswordOptions = new PasswordOptions()
        {
            RequireDigit = false,
            RequiredLength = 4,
            RequireNonAlphanumeric = false,
            RequireLowercase = false,
            RequireUppercase = false,
            RequiredUniqueChars = 2,
        };

        private static readonly SignInOptions SignInOptions = new SignInOptions()
        {
            RequireConfirmedPhoneNumber = false,
            RequireConfirmedAccount = false,
            RequireConfirmedEmail = false,
        };
    }
}
