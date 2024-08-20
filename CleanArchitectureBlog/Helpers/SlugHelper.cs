using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace CleanArchitectureBlog.Helpers
{
    public static class SlugHelper
    {
        public static string Slugify(string title)
        {
            title = title
                .Replace("ı", "i")
                .Replace("ş", "s")
                .Replace("ç", "c")
                .Replace("ö", "o")
                .Replace("ü", "u")
                .Replace("ğ", "g")
                .Replace("İ", "i")
                .Replace("Ş", "s")
                .Replace("Ç", "c")
                .Replace("Ö", "o")
                .Replace("Ü", "u")
                .Replace("Ğ", "g");
            var normalizedString = title.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            var allowedChars = "abcdefghijklmnopqrstuvwxyz0123456789-";

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    var lowerChar = char.ToLower(c);
                    if (allowedChars.Contains(lowerChar) || lowerChar == ' ')
                    {
                        stringBuilder.Append(lowerChar == ' ' ? '-' : lowerChar);
                    }
                }
            }

            return Regex.Replace(stringBuilder.ToString(), @"-+", "-").Trim('-');
        }
    }
}
