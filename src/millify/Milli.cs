using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Millify
{
    public static class Milli
    {

        public static bool IncludePositiveSign = false;
        
        public const string PositiveSign = "müsbət";
        public const string NegativeSign = "mənfi";
        
        private const string AzeVowelsUpper = "AIOUEƏİÖÜ";
        private const string AzeVowelsLower = "aıoueəiöü";
        private const string AzeAlphaUpper = "ABCÇDEƏFGĞHXIİJKQLMNOÖPRSŞTUÜVYZ";
        private const string AzeAlphaLower = "abcçdeəfgğhxıijklmnoöprsştuüvyz";
        private const string AzeConsonantsUpper   = "BCÇDFGĞHXJKQLMNPRSŞTVYZ";
        private const string AzeConsonantsOrdered = "_HBPCÇDTVFGKĞXJŞZSYẊQKL_M_N_R_";

        private static readonly HashSet<char> AllAzeLetters = new HashSet<char>(AzeAlphaUpper + AzeAlphaLower);


        private static readonly Dictionary<char, CharInfos> __charInfoCache;

        static Milli()
        {
            
            // initialize cache
            __charInfoCache = new Dictionary<char, CharInfos>(AzeAlphaLower.Length + AzeAlphaUpper.Length);

            foreach (char c in AzeAlphaUpper)
            {
                GetCharInfos(c);
            }
            foreach (char c in AzeAlphaUpper)
            {
                GetCharInfos(c);
            }
        }
        
        

        [Flags]
        public enum CharInfos
        {
            Simvol = 1 << 0,
            Rəqəm = 1 << 1,
            Hərf = 1 << 2,
            Böyük = 1 << 3,
            Kiçik = 1 << 4,
            Sait = 1 << 5,
            İncə = 1 << 6,
            Qalın = 1 << 7,
            Açıq = 1 << 8,
            Bağlı = 1 << 9,
            Dodaqlanan = 1 << 10,
            Dodaqlanmayan = 1 << 11,
            Samit = 1 << 12,
            Kar = 1 << 13,
            Cingiltili = 1 << 14,
            Sonor = 1 << 15,
            Burun = 1 << 16
        }

        #region Alpha methods

        static int IndexOfInsentensive(string source, char c)
        {
            return source.ToLower().IndexOf(char.ToLower(c));
        }


        private static bool IsAzeLetter(char c)
        {
            return IndexOfInsentensive(AzeAlphaUpper, c) != -1;
        }


        private static int VowelIndex(char c)
        {
            var index = AzeVowelsUpper.IndexOf(c);
            if(index == -1)
                index = AzeVowelsLower.IndexOf(c);
            return index;
        }

        private static bool IsVowel(char c)
        {
            return VowelIndex(c) != -1;
        }

        private static bool IsThin(char c)
        {
            return VowelIndex(c) > 3;
        }

        private static bool IsClosed(char c)
        {
            return IndexOfInsentensive("ıiuü", c) != -1;
        }

        private static bool IsLipping(char c)
        {
            return IndexOfInsentensive("ouöü", c) != -1;
        }

        private static int ConsonantIndex(char c)
        {
            return c == '_' ? -1 : IndexOfInsentensive(AzeConsonantsOrdered, c);
        }

        private static bool IsConsonant(char c)
        {
            return ConsonantIndex(c) != -1;
        }

        private static bool IsDeafConsonant(char c)
        {
            return ConsonantIndex(c) % 2 == 1;
        }

        private static bool IsSonorConsonant(char c)
        {
            return ConsonantIndex(c) > 23;
        }

        private static bool IsNoseConsonant(char c)
        {
            return IndexOfInsentensive("nm", c) != -1;
        }

        private static string ToASCII(this string azeStr)
        {
            return new StringBuilder(azeStr).Replace('ı', 'i')
                .Replace('ə', 'e')
                .Replace('ö', 'o')
                .Replace('ü', 'u')
                .Replace("ç", "ch")
                .Replace('ğ', 'g')
                .Replace("ş", "sh")
                .Replace('ü', 'u')
                .Replace('İ', 'I')
                .ToString();
        }

        private static int LastVowelIndex(this string str)
        {
            for (var i = str.Length - 1; i >= 0; i--)
                if (IndexOfInsentensive(AzeVowelsLower, str[i]) != -1)
                    return i;
            return -1;
        }

        public static char GetLastVowel(this string str)
        {
            // str.LastOrDefault(c => c.GetCharInfos().HasFlag(CharInfos.Sait));
            var lastVowelIndex = LastVowelIndex(str);
            if (lastVowelIndex != -1)
                return str[lastVowelIndex];
            return '\0';
        }

        #endregion

        // 16-cidən -> 16-cıdan
        public static string FixNumberTail(string str)
        {
            var result = new StringBuilder();
            var matches = Regex.Matches(str, @"(?<g_number>\d+)(?<g_tail>[^\s]+)", RegexOptions.Singleline);

            var uponToIndex = 0;

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var num = match.Groups["g_number"].Value;
                var tail = match.Groups["g_tail"].Value;

                result.Append(str.Substring(uponToIndex, match.Index - uponToIndex));
                result.Append(num);

                var numberSpelling = Spell(long.Parse(num));

                var fixedConcat = AddSuffix(numberSpelling, tail);
                result.Append(fixedConcat.Substring(fixedConcat.Length-tail.Length));
                uponToIndex = match.Index + match.Length;
            }
            result.Append(str.Substring(uponToIndex));

            return result.ToString();

        }

        //ahəng qanunu. 
        public static string AddSuffix(this string root, string suffix)
        {
            //2011-cidən başlayaraq
            //2010-cudan başlayaraq
            //2013-cüdən başlayaraq
            //2016-cıdan başlayaraq

            //bir-cidən başlayaraq
            //on-cudan başlayaraq
            //üç-cüdən başlayaraq
            //altı-cıdan başlayaraq

            var lastVowel = root.GetLastVowel();

            var lastVowelInfos = lastVowel.GetCharInfos();

            if (lastVowelInfos.HasFlag(CharInfos.Sait))
            {

                char fourChar = '\0';

                lastVowel = char.ToLower(lastVowel);
                switch (lastVowel)
                {
                    case 'a':
                        fourChar = 'ı';
                        break;
                    case 'o':
                        fourChar = 'u';
                        break;
                    case 'e':
                    case 'ə':
                        fourChar = 'i';
                        break;
                    case 'ö':
                        fourChar = 'ü';
                        break;
                    default:
                        fourChar = lastVowel;
                        break;
                }

                char twinChar = lastVowelInfos.HasFlag(CharInfos.İncə) ? 'ə' : 'a';


                suffix = Regex.Replace(suffix, "[aə]", twinChar.ToString());
                suffix = Regex.Replace(suffix, "[AƏ]", char.ToUpper(twinChar).ToString());
                suffix = Regex.Replace(suffix, "[ıiuü]", fourChar.ToString());
                suffix = Regex.Replace(suffix, "[IİUÜ]", char.ToUpper(fourChar).ToString());
            }

            return root+suffix;
        }

        private static CharInfos GetCharInfos(this char c)
        {
            if (__charInfoCache.TryGetValue(c, out var fromCache))
            {
                return fromCache;
            }
            
            CharInfos result = (CharInfos)0;

            if (char.IsSymbol(c))
            {
                result |= CharInfos.Simvol;
            }
            else if (char.IsDigit(c))
            {
                result |= CharInfos.Rəqəm;
            }
            else if (char.IsLetter(c))
            {

                result |= CharInfos.Hərf;

                if (char.IsUpper(c))
                {
                    result |= CharInfos.Böyük;
                }
                else if(char.IsLetter(c))
                {
                    result |= CharInfos.Kiçik;
                }

                if (!IsAzeLetter(c))
                {
                    return result;
                }

                if (IsVowel(c))
                {
                    result |= CharInfos.Sait;

                    if (IsThin(c))
                    {
                        result |= CharInfos.İncə;
                    }
                    else
                    {
                        result |= CharInfos.Qalın;
                    }

                    if (IsClosed(c))
                    {
                        result |= CharInfos.Bağlı;
                    }
                    else
                    {
                        result |= CharInfos.Açıq;
                    }

                    if (IsLipping(c))
                    {
                        result |= CharInfos.Dodaqlanan;
                    }
                    else
                    {
                        result |= CharInfos.Dodaqlanmayan;
                    }

                }
                else if (IsConsonant(c))
                {
                    result |= CharInfos.Samit;

                    if (IsDeafConsonant(c))
                    {
                        result |= CharInfos.Kar;
                    }
                    else
                    {
                        result |= CharInfos.Cingiltili;

                        if (IsSonorConsonant(c))
                        {
                            result |= CharInfos.Sonor;

                            if (IsNoseConsonant(c))
                            {
                                result |= CharInfos.Burun;
                            }
                        }

                    }

                }

            }

            __charInfoCache[c] = result;
            return result;
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


        //TODO support floating numbers
        //TODO also write vise versa ( str -> long)
        //TODO support negative numbers

        public static (string, decimal) SignAndAbs(decimal number)
        {
            if (number < 0)
                return (NegativeSign, 0-number);
            if(number > 0 && IncludePositiveSign)
                return (PositiveSign, number);
            return (string.Empty, number);
        }
        
        public static string Spell(ulong number)
        {
            return SignedNumberToWords(number);
        }
        
        public static string Spell(long number)
        {
            return SignedNumberToWords(number);
        }

        public static string SignedNumberToWords(decimal number)
        {
            (string sign, decimal absNumber) = SignAndAbs(number);
            string abs = NumberToWords(absNumber);
            if (sign != string.Empty)
                return $"{sign} {abs}";
            return abs;
        }


        private static int GetFloatedRank(decimal number)
        {
            string[] parts = number.ToString("F99", CultureInfo.InvariantCulture).Split('.');
            if (parts.Length < 2)
            {
                return 0;
            }
            return parts[1].TrimEnd('0').Length;
        }
        
        public static string Spell(float number, int decimalPlaces = -1)
        {
            return Spell((decimal) number, decimalPlaces);
        }

        public static string Spell(double number, int decimalPlaces = -1)
        {
            return Spell((decimal) number, decimalPlaces);
        }

        public static string Spell(decimal number, int decimalPlaces = -1)
        {
            decimal integerPart = Math.Truncate(number);
            decimal decimalPart = number - integerPart;
            string integer = SignedNumberToWords(integerPart);
            int rankShift = decimalPlaces ==-1 ? GetFloatedRank(decimalPart) : decimalPlaces;
            if (rankShift <= 0)
                return integer;
            decimal rank = (decimal)Math.Pow(10, rankShift);
            string rankStr = NumberToWords(rank);
            string etalon = AddSuffix(rankStr, "da"); //onda, yüzdə, mində, ...
            decimal decimalPartShifted = (decimalPart * rank);
            string decimalStr = NumberToWords(decimalPartShifted);
            return $"{integer} tam {etalon} {decimalStr}".ToLower();
        }

        public static string NumberToCurrency(double number, string integerSuffix = "man.", string decimalSuffix = "qəp.", bool numberToWord = false)
        {
            long integerPart = (long) number;
            int decimalPart = (int)Math.Truncate(100*(number-integerPart));
            return $"{(numberToWord ? Spell(integerPart) : integerPart.ToString())} {integerSuffix} {(numberToWord ? Spell(decimalPart) : decimalPart.ToString())} {decimalSuffix}";
        }

        
    }
}