using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Millify
{
    public static class Milli
    {
        
        public static bool IncludePositiveSign = false;
        
        private const string PositiveSign = "müsbət";
        private const string NegativeSign = "mənfi";
        private const string AzeVowels = "AaIıOoUuEeƏəİiÖöÜü";
        private static readonly char[] AzeVowelsAll = AzeVowels.ToCharArray();

        // 16-cidən -> 16-cıdan
        public static string FixNumberTail(string str)
        {
            var sb = new StringBuilder();
            var matches = Regex.Matches(str, @"(?<g_number>\d+)(?<g_tail>[^\s]+)", RegexOptions.Singleline);
            int uponToIndex = 0;
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                string num = match.Groups["g_number"].Value;
                string tail = match.Groups["g_tail"].Value;

                sb.Append(str.Substring(uponToIndex, match.Index - uponToIndex));
                sb.Append(num);
                if (decimal.TryParse(num, out decimal d))
                {
                    string numberSpelling = Spell(d);
                    string fixedConcat = AddSuffix(numberSpelling, tail);
                    sb.Append(fixedConcat.Substring(fixedConcat.Length - tail.Length));
                }
                else
                {
                    
                }
                uponToIndex = match.Index + match.Length;
            }
            sb.Append(str.Substring(uponToIndex));
            return sb.ToString();
        }

        // experimental
        public static string AddSuffix(this string root, string suffix)
        {
            return root + FixSuffix(root, suffix);
        }
        
        private static string FixSuffix(string root, string suffix)
        {
            int lastIndexOfVowel = root.LastIndexOfAny(AzeVowelsAll);
            
            if (lastIndexOfVowel == -1)
            {
                return suffix;
            }

            var lastVowel = root[lastIndexOfVowel];
 
            string v4 = lastVowel.ToString();

            lastVowel = char.ToLower(lastVowel);
            
            switch (lastVowel)
            {
                case 'a':
                    v4 = "ı";
                    break;
                case 'o':
                    v4 = "u";
                    break;
                case 'e':
                case 'ə':
                    v4 = "i";
                    break;
                case 'ö':
                    v4 = "ü";
                    break;
            }
            string v4U = AzeVowels[AzeVowels.IndexOf(v4[0]) - 1].ToString();
            bool isThin = AzeVowels.IndexOf(lastVowel) >= 8;
            string v2 = isThin? "ə" : "a";
            string v2U = AzeVowels[AzeVowels.IndexOf(v2[0]) - 1].ToString();
            suffix = Regex.Replace(suffix, "[aə]", v2);
            suffix = Regex.Replace(suffix, "[AƏ]", v2U);
            suffix = Regex.Replace(suffix, "[ıiuü]", v4);
            suffix = Regex.Replace(suffix, "[IİUÜ]", v4U);

            if (lastIndexOfVowel == root.Length - 1 && AzeVowelsAll.Contains(suffix[0]))
                suffix = suffix.Substring(1);
            
            return suffix;
        }

   

        // az.wikipedia.org/wiki/Natural_ədədlər
        private static readonly string[] names = 
        {
            "sıfır", 
            "bir", "iki", "üç", "dörd", "beş", "altı", "yeddi", "səkkiz", "doqquz",
            "on", "iyirmi", "otuz", "qırx", "əlli", "altmış", "yetmiş", "səksən", "doxsan", 
            "yüz",
            "min", "milyon", "milyard", "trilyon", "kvadrilyon", "kvintilyon", "sextilyon", "septilyon", "oktilyon",
            // can be extended https://tr.wikipedia.org/wiki/Büyük_sayıların_adları
        };
        
        // TODO use LRU cache
        private static string NumberToWords(decimal number, int rank)
        {
            if (number == 0)
                if (rank == 0) // just zero
                    return names[0];
                else
                    return ""; // ignore other zeros
            
            string result = "";
            int digit = (int)(number % 10);
            
            if (rank > 0 && rank % 3 == 0 && number % 1000 > 0)
                result = " " + names[19 + rank / 3];

            if ((rank - 2) % 3 == 0 && digit > 0)
                result = " " + names[19] + result;

            if (digit != 0 && !(digit == 1 && (rank % 3 == 2 || rank == 3 && number < 10)))
                result = " " + names[9 * (rank % 3 % 2) + digit] + result;
            return NumberToWords(Math.Truncate(number / 10), ++rank) + result;
        }
        
        private static string NumberToWords(decimal number)
        {
            return NumberToWords(number, 0).TrimStart();
        }
        
        private static string SignedNumberToWords(decimal number)
        {
            string sign = string.Empty;
            decimal absNumber = 0;
            if (number < 0)
            {
                sign = NegativeSign;
                absNumber = -number;
            }
            else 
            {
                absNumber = number;
                if (number > 0 && IncludePositiveSign)
                {
                    sign = PositiveSign;
                }
            }
            
            string abs = NumberToWords(absNumber);
            if (sign != string.Empty)
                return $"{sign} {abs}";
            return abs;
        }


        private static int GetFloatedRank(this decimal number)
        {
            string[] parts = number.ToString("F99", CultureInfo.InvariantCulture).Split('.');
            if (parts.Length < 2)
            {
                return 0;
            }
            return parts[1].TrimEnd('0').Length;
        }

        
        // extention methods
        public static string Spell(this ulong number)
        {
            return SignedNumberToWords(number);
        }
        
        public static string Spell(this long number)
        {
            return SignedNumberToWords(number);
        }
        
        public static string Spell(this float number, int decimalPlaces = -1)
        {
            return Spell((decimal) number, decimalPlaces);
        }

        public static string Spell(this double number, int decimalPlaces = -1)
        {
            return Spell((decimal) number, decimalPlaces);
        }

        //  1-ci, 2-ci, 3-cü, 25-ci
        public static string AsOrdinal(this long number)
        {
            return number + FixSuffix(number.Spell(), "-ci");
        }
        

        // 1 -> birinci, 13 -> on üçüncü
        public static string SpellAsOrdinal(this long number)
        {
            return number.Spell().AddSuffix("inci");
            
        }
        
        // 3.14 -> üç tam yüzdə on dörd
        public static string Spell(this decimal number, int decimalPlaces = -1)
        {
            decimal integerPart = Math.Truncate(number);
            decimal decimalPart = number - integerPart;
            string integer = SignedNumberToWords(integerPart);
            int rankShift = decimalPlaces == -1 ? GetFloatedRank(decimalPart) : decimalPlaces;
            if (rankShift <= 0)
                return integer;
            decimal rank = (decimal)Math.Pow(10, rankShift);
            string rankStr = NumberToWords(rank);
            string etalon = AddSuffix(rankStr, "da"); //onda, yüzdə, mində, ...
            decimal decimalPartShifted = (decimalPart * rank);
            string decimalStr = NumberToWords(decimalPartShifted);
            return $"{integer} tam {etalon} {decimalStr}".ToLower();
        }
        
        // 6.5 -> altı manat əlli qəpik
        public static string AsCurrency(this decimal number, string nominalName = "manat", string coinName = "qəpik", bool numbersAsWords = false)
        {
            decimal integerPart = Math.Truncate(number);
            int decimalPart = (int)(100*(number-integerPart));
            string nominal = numbersAsWords ? Spell(integerPart) : integerPart.ToString(CultureInfo.InvariantCulture);
            string coins = numbersAsWords ? Spell(decimalPart) : decimalPart.ToString();
            if (integerPart == 0)
                return $"{coins} {coinName}";
            if (decimalPart == 0)
                return $"{nominal} {nominalName}";
            return $"{nominal} {nominalName} {coins} {coinName}";
        }
        
        
        public static string SpellAsCurrency(this decimal number, string nominalName = "manat", string coinName = "qəpik", bool numbersAsWords = false)
        {
            return number.AsCurrency(numbersAsWords: true);
        }

    }
}