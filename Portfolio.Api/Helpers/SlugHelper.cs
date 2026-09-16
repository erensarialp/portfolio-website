using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Portfolio.Api.Helpers
{
    public static class SlugHelper
    {
        public static string Generate(string text)
        {
            text = text.ToLowerInvariant();

            text = text
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c");

            text = text.Normalize(NormalizationForm.FormD);

            var chars = text
                .Where(c =>
                    CharUnicodeInfo.GetUnicodeCategory(c)
                    != UnicodeCategory.NonSpacingMark)
                .ToArray();

            text = new string(chars)
                .Normalize(NormalizationForm.FormC);

            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            text = Regex.Replace(text, @"\s+", "-");

            text = Regex.Replace(text, @"-+", "-");

            return text.Trim('-');
        }
    }
}