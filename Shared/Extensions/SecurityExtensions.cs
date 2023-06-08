using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Domain.Enum;

namespace Shared.Extensions
{
    public static class SecurityExtensions
    {
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsNumber);
        }
        public static string HashAccountNumber(this string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
                return string.Empty;

            using var sha = new System.Security.Cryptography.SHA256Managed();
            var textData = System.Text.Encoding.UTF8.GetBytes(accountNumber);
            var hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public static bool AccountNumberIsValid(this string accountNumber, string dbValue)
        {
            return accountNumber.HashAccountNumber().Equals(dbValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string HideNumber(this string accountNumber, bool isMomo)
        {
            if (accountNumber == null)
                return string.Empty;
            var asterisks = isMomo ? "****" : "**********";
            return $"{accountNumber[..6]}{asterisks}";
        }

        public static bool VerifyNumber(this string number, AccountScheme scheme)
        {
            switch (scheme)
            {
                case AccountScheme.Visa:
                case AccountScheme.MasterCard:
                    return number.IsValidCardNumber(scheme);
                case AccountScheme.MTN:
                case AccountScheme.Vodafone:
                case AccountScheme.AirtelTigo:
                    return number.IsValidPhoneNumber();
            }
            return false;
        }

        private static bool PassesLuhnTest(string cardNumber)
        {
            //Clean the card number- remove dashes and spaces
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

            //Convert card number into digits array
            var digits = new int[cardNumber.Length];
            for (var len = 0; len < cardNumber.Length; len++)
            {
                digits[len] = int.Parse(cardNumber.Substring(len, 1));
            }

            //Luhn Algorithm
            //Adapted from code available on Wikipedia at
            //http://en.wikipedia.org/wiki/Luhn_algorithm
            var sum = 0;
            var alt = false;
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = digits[i];
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                    {
                        curDigit -= 9;
                    }
                }
                sum += curDigit;
                alt = !alt;
            }

            //If Mod 10 equals 0, the number is good and this will return true
            return sum % 10 == 0;
        }

        private static Regex CardTest(string cardRegex) => new Regex(cardRegex);
            
        public static bool IsValidCardNumber(this string cardNum, AccountScheme cardType)
        {
            const string cardRegex= "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

            return CardTest(cardRegex).Match(cardNum).Groups[cardType.ToString()].Success && PassesLuhnTest(cardNum);
        }

        public static string ToUniqueWalletName(this string walletName)
        {
            if (string.IsNullOrEmpty(walletName))
                return walletName;
            if (!walletName.Any(char.IsDigit))
                walletName = $"{walletName}1";
            else
                if (int.TryParse(new string(walletName.Where(char.IsDigit).ToArray()), out var val))
                    walletName = $"{walletName}{val + 1}";
            return walletName;
        }
    }}
