using System.Text.RegularExpressions;

namespace Portfolio.Api.Services.Chat
{
    public static class ChatInputParser
    {
        private static readonly string[] TurkishMonths =
        {
            "ocak",
            "şubat",
            "mart",
            "nisan",
            "mayıs",
            "mayis",
            "haziran",
            "temmuz",
            "ağustos",
            "agustos",
            "eylül",
            "eylul",
            "ekim",
            "kasım",
            "kasim",
            "aralık",
            "aralik"
        };

        public static bool IsMeaningfulMessage(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var cleaned = input.Trim();

            return cleaned.Length >= 2;
        }

        public static bool IsValidProjectDetails(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var text = input.Trim();

            /*
             * "Şanzal", "test", "a" gibi yanlışlıkla
             * gönderilen çok kısa mesajları proje açıklaması
             * kabul etmiyoruz.
             */
            if (text.Length < 15)
            {
                return false;
            }

            var wordCount =
                text.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries)
                    .Length;

            return wordCount >= 3;
        }

        public static string? ExtractDeadline(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            var text = input.Trim();

            /*
             * 25 Eylül
             * 25 eylüle kadar
             */
            foreach (var month in TurkishMonths)
            {
                var monthPattern =
                    $@"(?i)\b\d{{1,2}}\s+{Regex.Escape(month)}(?:['’]?[a-zçğıöşü]*)?(?:\s+kadar)?";

                var monthMatch =
                    Regex.Match(
                        text,
                        monthPattern);

                if (monthMatch.Success)
                {
                    return monthMatch.Value.Trim();
                }
            }

            /*
             * 25/09/2026
             * 25.09
             * 25-09-2026
             */
            var numericDate =
                Regex.Match(
                    text,
                    @"\b\d{1,2}[./-]\d{1,2}(?:[./-]\d{2,4})?\b");

            if (numericDate.Success)
            {
                return numericDate.Value.Trim();
            }

            /*
             * yarın, haftaya, bu ayın sonunda vb.
             */
            var relativePatterns =
                new[]
                {
                    @"(?i)\byarın\b",
                    @"(?i)\bhaftaya\b",
                    @"(?i)\bgelecek hafta\b",
                    @"(?i)\bbu hafta\b",
                    @"(?i)\bbu ay\b",
                    @"(?i)\bgelecek ay\b",
                    @"(?i)\bay sonuna\b",
                    @"(?i)\bhafta sonuna\b",
                    @"(?i)\b\d+\s+gün\b",
                    @"(?i)\b\d+\s+hafta\b"
                };

            foreach (var pattern in relativePatterns)
            {
                var match =
                    Regex.Match(
                        text,
                        pattern);

                if (match.Success)
                {
                    return match.Value.Trim();
                }
            }

            return null;
        }

        public static string? ExtractBudget(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            /*
             * 1000-2000 TL
             * 1.000 - 2.000₺
             */
            var rangeMatch =
                Regex.Match(
                    input,
                    @"(?i)\b(\d[\d\.\s]*\s*[-–]\s*\d[\d\.\s]*\s*(?:tl|₺))\b");

            if (rangeMatch.Success)
            {
                return NormalizeWhitespace(
                    rangeMatch.Groups[1].Value);
            }

            /*
             * 2000 TL
             */
            var singleMatch =
                Regex.Match(
                    input,
                    @"(?i)\b(\d[\d\.\s]*\s*(?:tl|₺))\b");

            if (singleMatch.Success)
            {
                return NormalizeWhitespace(
                    singleMatch.Groups[1].Value);
            }

            return null;
        }

        public static bool ContainsNameDeclaration(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            return
                Regex.IsMatch(
                    input,
                    @"(?i)\bismim\b") ||
                Regex.IsMatch(
                    input,
                    @"(?i)\badım\b") ||
                Regex.IsMatch(
                    input,
                    @"(?i)\bbenim adım\b");
        }

        public static string ExtractName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var text = input.Trim();

            var patterns = new[]
            {
                @"(?i)\bismim\s+([^,.]+)",
                @"(?i)\bbenim\s+adım\s+([^,.]+)",
                @"(?i)\badım\s+([^,.]+)"
            };

            foreach (var pattern in patterns)
            {
                var match =
                    Regex.Match(
                        text,
                        pattern);

                if (!match.Success)
                {
                    continue;
                }

                var name =
                    match.Groups[1]
                        .Value
                        .Trim();

                name = CutNameAtExtraInformation(
                    name);

                return CleanName(name);
            }

            /*
             * Sistem özellikle isim beklerken kullanıcı
             * sadece "Eren Sarıalp" yazabilir.
             */
            return CleanName(text);
        }

        public static bool IsValidName(string input)
        {
            var name =
                ExtractName(input);

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (name.Length < 2 ||
                name.Length > 80)
            {
                return false;
            }

            /*
             * İçinde aşırı sayı varsa isim değildir.
             */
            var digitCount =
                name.Count(char.IsDigit);

            return digitCount == 0;
        }

        public static string? ExtractEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            var match =
                Regex.Match(
                    input,
                    @"[A-Z0-9._%+\-]+@[A-Z0-9.\-]+\.[A-Z]{2,}",
                    RegexOptions.IgnoreCase);

            return match.Success
                ? match.Value.Trim()
                : null;
        }

        public static string? ExtractPhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            /*
             * Türkiye cep telefonu:
             * 5464337575
             * 0546 433 75 75
             * +90 546 433 75 75
             */
            var match =
                Regex.Match(
                    input,
                    @"(?:\+?90[\s\-]?)?(?:0?5\d{2})[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}");

            if (!match.Success)
            {
                return null;
            }

            return NormalizePhone(
                match.Value);
        }

        public static string NormalizePhone(
            string phone)
        {
            var digits =
                new string(
                    phone.Where(char.IsDigit)
                        .ToArray());

            if (
                digits.StartsWith("90") &&
                digits.Length == 12
            )
            {
                digits =
                    digits[2..];
            }

            if (
                digits.StartsWith("0") &&
                digits.Length == 11
            )
            {
                digits =
                    digits[1..];
            }

            return digits;
        }

        public static string CleanPhoneForDisplay(
            string phone)
        {
            var normalized =
                NormalizePhone(phone);

            if (normalized.Length != 10)
            {
                return phone;
            }

            return
                $"{normalized[..3]} " +
                $"{normalized.Substring(3, 3)} " +
                $"{normalized.Substring(6, 2)} " +
                $"{normalized.Substring(8, 2)}";
        }

        private static string CutNameAtExtraInformation(
            string input)
        {
            var separators =
                new[]
                {
                    " ve bütç",
                    " bütçem",
                    " telefon",
                    " tel ",
                    " numaram",
                    " e-posta",
                    " eposta",
                    " email",
                    " mail",
                    " ve mail",
                    " ve telefon"
                };

            var result =
                input;

            foreach (var separator in separators)
            {
                var index =
                    result.IndexOf(
                        separator,
                        StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    result =
                        result[..index];

                    break;
                }
            }

            return result.Trim();
        }

        private static string CleanName(
            string input)
        {
            var name =
                input.Trim(
                    ' ',
                    '.',
                    ',',
                    ';',
                    ':',
                    '!',
                    '?');

            return NormalizeWhitespace(name);
        }

        private static string NormalizeWhitespace(
            string input)
        {
            return Regex.Replace(
                    input.Trim(),
                    @"\s+",
                    " ")
                .Trim();
        }
    }
}