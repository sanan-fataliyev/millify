using System;
using System.Linq;
using System.Globalization.Aze;

namespace System.Globalization.Aze
{
    class Program
    {
        static void Main(string[] args)
        {
            //testing
            //some error prone numbers
            var errorProneNums = new[] { 0, 10, 11, 100, 101, 110, 1000, 1001, 1010, 1100, 1200, 2000, 2222, 10000, 21100, 111111, 1234567, 21000000, 1000222222 };
         
            var errorProneResult = string.Join("\n", errorProneNums.Select(num => $"{num}  ->  {Millify.NumberToWord(num)}"));

            // test with 20 big random numbers
            var rnd = new Random();
            var randomNums = Enumerable.Range(0, 20).Select(i => rnd.Next(10000, int.MaxValue));
            var randomNumsResult = string.Join(Environment.NewLine, randomNums.Select(num => $"{num}  ->  {Millify.NumberToWord(num)}"));

            Console.WriteLine("Xətaya meyilli ədədlər:\n" + errorProneResult);
            Console.WriteLine("20 təsadüfi böyük ədəd:\n" + randomNumsResult);


        }

    }
}
