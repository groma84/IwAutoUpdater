using System.Text.RegularExpressions;

namespace IwAutoUpdater.CrossCutting.Base
{
    public static class ExtensionMethods
    {
        public static string ReplaceIgnoreCase(this string str, string oldText, string newText)
        {
            return Regex.Replace(str,
                Regex.Escape(oldText),
                Regex.Replace(newText, "\\$[0-9]+", @"$$$0"),
                RegexOptions.IgnoreCase);
        }
    }
}
