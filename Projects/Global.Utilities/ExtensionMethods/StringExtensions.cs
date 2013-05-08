using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Global.Utilities.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string SeperateWords(this string word)
        {
            string newWords = string.Empty;

            if (!string.IsNullOrEmpty(word))
            {
                Regex seperator = new Regex("[A-Z0-9]");

                foreach (char letter in word)
                {
                    if (seperator.IsMatch(letter.ToString()))
                    {
                        newWords += " ";
                    }

                    newWords += letter;
                }
            }

            return newWords;
        }

		public static string Pluralize(this string word)
		{
			bool useUpper = string.Equals(word, word.ToUpper());

			if (word.EndsWith("y", StringComparison.CurrentCultureIgnoreCase))
			{
				return useUpper
					? word.TrimEnd('Y') + "IES"
					: word.TrimEnd('y') + "ies";
			}
			else if (word.EndsWith("s", StringComparison.CurrentCultureIgnoreCase)
				|| word.EndsWith("x", StringComparison.CurrentCultureIgnoreCase))
			{
				return useUpper
					? word + "ES"
					: word + "es";
			}
			else
			{
				return useUpper
					? word + "S"
					: word + "s";
			}
		}
    }
}
