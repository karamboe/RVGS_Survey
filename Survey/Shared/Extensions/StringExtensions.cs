using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string ToProper(this string s)
        {
            return new string(s.CharsToTitleCase().ToArray());
        }

        public static IEnumerable<char> CharsToTitleCase(this string s)
        {
            bool newWord = true;
            foreach (char c in s)
            {
                if (newWord) { yield return Char.ToUpper(c); newWord = false; }
                else yield return Char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }

        public static string[] Wrap(this string source, int lineLength)
        {
            var result = new List<string>();
            var currentLine = new StringBuilder();

            var words = source.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (currentLine.Length + word.Length > lineLength)
                {
                    result.Add(currentLine.ToString());
                    currentLine.Clear();
                }
                currentLine.Append(currentLine.Length == 0 ? word : " " + word);
            }
            result.Add(currentLine.ToString());

            return result.ToArray();
        }

        public static string FormatWith(this string formatString, params object[] args)
        {
            return args == null || args.Length == 0 ? formatString : string.Format(formatString, args);
        }

        public static String TruncateLongString(this string str, int maxLength)
        {
            return str.Substring(0, System.Math.Min(str.Length, maxLength)) + (str.Length > maxLength ? "..." : "");
        }

        public static string Left(this string str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - Math.Min(length, str.Length));
        }

        public static string Strip(this string str, string stripString)
        {
            return str.Replace(stripString, "");
        }
        public static string Strip(this string str, string[] stripStrings)
        {
            string ret = str;
            foreach (var a in stripStrings)
            {
                ret = ret.Strip(a);
            }
            return ret;
        }

        public static string GetValueOrEmpty(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return str.Trim();
            }
            else
                return string.Empty;
        }

        public static bool ContainsOnlyGivenChar(this string str, char checkChar)
        {
            bool ret = true;
            foreach (var a in str.ToCharArray())
            {
                if (a != checkChar)
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }

        public static string ToBase64String(this string str)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64String(this string str)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
