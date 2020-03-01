using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Millify
{
    public static class Milli
    {
        private static readonly string[] Names = 
        {
            "sıfır ", "bir", "iki", "üç", "dörd", "beş", "altı", "yeddi", "səkkiz", "doqquz",
            "on", "iyirmi", "otuz", "qırx", "əlli", "altmış", "yetmiş", "səksən", "doxsan", "yüz", 
            "min", "milyon", "milyard", "trilyon", "kvadrilyon", "kvintilyon", "sextilyon", "septilyon", "oktilyon",
            // extendable
        };


        private const string AzeVowels = "aıoueəiöü";
        
        private static readonly Dictionary<string, string> V4Map = "a:ı,o:u,e:i,ə:i,ö:ü"
            .Split(',')
            .Select(x => x.Split(':'))
            .ToDictionary(kv => kv[0], kv => kv[1]);
        
        
        public static string AddSuffix(this string root, string suffix)
        {
            if (AzeVowels.Contains(root.Last()) && AzeVowels.Contains(suffix.First()))
                root = root.Remove(root.Length - 1);
            return root + AdoptSuffix(root, suffix);
        }
        
        private static string AdoptSuffix(string root, string suffix)
        {
            var i = AzeVowels.Max(root.LastIndexOf);
            if (i == -1)
                return suffix;
            
            var lastVowel = root[i].ToString();
            
            bool isHigh = AzeVowels.IndexOf(lastVowel, StringComparison.Ordinal) > 3;

            if(!V4Map.TryGetValue(lastVowel, out string v4))
                v4 = lastVowel;
            
            string v2 = isHigh? "ə" : "a";
            suffix = Regex.Replace(suffix, "[aə]", v2);
            suffix = Regex.Replace(suffix, "[ıiuü]", v4);
            return suffix;
        }

   
        // this algorithm works only with nonnegative integers
        // decimal used because its integer range is larger than int's, lon'g and ulong's
        private static string NumberToWordsInternal(decimal number, int rank=0, bool bankMode=false)
        {
            if (number == 0 && rank == 0)
                return Names[0];
            if (number == 0)
                return "";
            
            string tail = "";
            int digit = (int)(number % 10);
            
            if (digit != 0 && (bankMode || !(digit == 1 && (rank % 3 == 2 || rank == 3 && number < 10))))
                tail =  Names[9 * (rank % 3 % 2) + digit] + " ";

            if (rank % 3 == 2 && digit > 0)
                tail += Names[19] + " ";

            if (rank > 0 && rank % 3 == 0 && number % 1_000 > 0)
                tail += Names[19 + rank / 3] + " ";

            return NumberToWordsInternal(Math.Truncate(number / 10), rank + 1, bankMode) + tail;
        }
        
        private static string NumberToWords(decimal number, bool bankMode=false)
        {
            return NumberToWordsInternal(number, 0, bankMode).TrimEnd();
        }
        
        private static string SignedNumberToWords(decimal number, bool includePositiveSign=false)
        {
            string sign = string.Empty;
            decimal absNumber = 0;
            if (number < 0)
            {
                sign = "mənfi ";
                absNumber = -number;
            }
            else 
            {
                absNumber = number;
                if (number > 0 && includePositiveSign)
                {
                    sign = "müsbət ";
                }
            }
            
            string abs = NumberToWords(absNumber);
            return sign + abs;
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
        public static string Spell(ulong number)
        {
            return SignedNumberToWords(number);
        }
        
        public static string Spell(long number)
        {
            return SignedNumberToWords(number);
        }

        
        public static string AsOrdinal(long number)
        {
            return number + AdoptSuffix(Spell(number), "-ci");
        }
        

        public static string SpellAsOrdinal(long number)
        {
            return Spell(number).AddSuffix("inci");
            
        }
        
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
        
        public static string AsCurrency(decimal number, string nominalName = "manat", string coinName = "qəpik", bool numbersAsWords = false, bool bankMode=false)
        {
            decimal integerPart = Math.Truncate(number);
            int decimalPart = (int)(100*(number-integerPart));
            string nominal = numbersAsWords ? Spell(integerPart) : integerPart.ToString(CultureInfo.InvariantCulture);
            string coins = numbersAsWords ? Spell(decimalPart) : decimalPart.ToString();
            if (integerPart == 0 && !bankMode)
                return $"{coins} {coinName}";
            if (decimalPart == 0 && !bankMode)
                return $"{nominal} {nominalName}";
            return $"{nominal} {nominalName} {coins} {coinName}";
        }
        
        
        public static string SpellAsCurrency(decimal number, string nominalName = "manat", string coinName = "qəpik", bool numbersAsWords = false)
        {
            return AsCurrency(number, numbersAsWords: true);
        }

    }
}