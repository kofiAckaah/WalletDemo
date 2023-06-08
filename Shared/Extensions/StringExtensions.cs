using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
