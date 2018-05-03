using System;
using System.Linq;

namespace XamForms.Controls
{
    public static class ExtensionUtilsString
    {
        public static string FirstCharToUpper(this string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Not null or empty");
            }
            return input.ToCharArray().First().ToString().ToUpper() + input.Substring(1);            
        }
    }
}
