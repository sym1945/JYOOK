using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace JYOOK.Application
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                var result = attributes.First().Description;
                return result;
            }

            return value.ToString();
        }
    }
}