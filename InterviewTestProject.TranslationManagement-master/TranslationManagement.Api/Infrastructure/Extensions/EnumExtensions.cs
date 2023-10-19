using System.ComponentModel;
using System;
using System.Linq;

namespace TranslationManagement.Api.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description attribute of an enum
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
                return string.Empty;

            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var custAttr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            DescriptionAttribute attribute = custAttr?.SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
