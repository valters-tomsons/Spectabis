using System.Text.RegularExpressions;

namespace Spectabis_WPF.Domain
{
    public static class StringExtensions
    {
        //Thanks to devperez over at Emulation discord server! :*

        public static string ToSanitizedString(this string input)
        {
            Regex regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regex.Replace(input, "");
        }
    }
}
