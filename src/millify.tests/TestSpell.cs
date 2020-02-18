using System.Collections.Generic;
using Xunit;

namespace Millify.Tests
{
    
    public class TestSpell
    {
        [Theory]
        [InlineData(0, "sıfır")]
        [InlineData(1, "bir")]
        [InlineData(2, "iki")]
        [InlineData(3, "üç")]
        [InlineData(4, "dörd")]
        [InlineData(5, "beş")]
        [InlineData(6, "altı")]
        [InlineData(7, "yeddi")]
        [InlineData(8, "səkkiz")]
        [InlineData(9, "doqquz")]
        
        [InlineData(10, "on")]
        [InlineData(15, "on beş")]
        [InlineData(20, "iyirmi")]
        [InlineData(30, "otuz")]
        [InlineData(40, "qırx")]
        [InlineData(50, "əlli")]
        [InlineData(60, "altmış")]
        [InlineData(70, "yetmiş")]
        [InlineData(80, "səksən")]
        [InlineData(90, "doxsan")]
        [InlineData(99, "doxsan doqquz")]
        
        [InlineData(100, "yüz")]
        [InlineData(101, "yüz bir")]
        [InlineData(110, "yüz on")]
        [InlineData(111, "yüz on bir")]
        [InlineData(256, "iki yüz əlli altı")]
        [InlineData(404, "dörd yüz dörd")]
        [InlineData(999, "doqquz yüz doxsan doqquz")]
        [InlineData(1000, "min")]
        [InlineData(1001, "min bir")]
        [InlineData(1010, "min on")]
        [InlineData(1011, "min on bir")]
        [InlineData(1100, "min yüz")]
        [InlineData(1110, "min yüz on")]
        [InlineData(1111, "min yüz on bir")]
        [InlineData(2000, "iki min")]
        [InlineData(2077, "iki min yetmiş yeddi")]
        [InlineData(7777, "yeddi min yeddi yüz yetmiş yeddi")]
        
        [InlineData(10_000, "on min")]
        [InlineData(33_813, "otuz üç min səkkiz yüz on üç")]
        
        [InlineData(999_999, "doqquz yüz doxsan doqquz min doqquz yüz doxsan doqquz")]
        
        [InlineData(1_000_000, "bir milyon")]
        [InlineData(123_456_789, "yüz iyirmi üç milyon dörd yüz əlli altı min yeddi yüz səksən doqquz")]
        
        // https://youtu.be/ZoN7-pMdQdg
        [InlineData(327_000_000, "üç yüz iyirmi yeddi milyon")]
        
        [InlineData(1_000_000_000, "bir milyard")]
        [InlineData(2_222_222_222, "iki milyard iki yüz iyirmi iki milyon iki yüz iyirmi iki min iki yüz iyirmi iki")]
        
        //18,446,744,073,709,551,615
        [InlineData(ulong.MaxValue, "on səkkiz kvintilyon dörd yüz qırx altı kvadrilyon yeddi yüz qırx dörd trilyon yetmiş üç milyard yeddi yüz doqquz milyon beş yüz əlli bir min altı yüz on beş")]
        
        
        public void TestIntegers(ulong number, string expected)
        {
            Assert.Equal(expected, number.Spell());
        }


        [Theory]
        
        [InlineData(3.14, "üç tam yüzdə on dörd")]
        [InlineData(2.5, "iki tam onda beş")]
        [InlineData(1.618, "bir tam mində altı yüz on səkkiz")]
        [InlineData(5.0, "beş")]
        [InlineData(99.99, "doxsan doqquz tam yüzdə doxsan doqquz")]
        [InlineData(12345.54321, "on iki min üç yüz qırx beş tam yüz mində əlli dörd min üç yüz iyirmi bir")]
        public void TestFloatingNumbers(decimal number, string expected)
        {
            Assert.Equal(expected, number.Spell());
        }



        [Theory]
        [InlineData(-1, "mənfi bir")]
        [InlineData(-22, "mənfi iyirmi iki")]
        [InlineData(-0, "sıfır")]
        public void TestNegativeNumbers(decimal number, string expected)
        {
            Assert.Equal(expected, number.Spell());   
        }


        [Theory]
        [InlineData(1, "1-ci")]
        [InlineData(2, "2-ci")]
        [InlineData(3, "3-cü")]
        [InlineData(4, "4-cü")]
        [InlineData(5, "5-ci")]
        [InlineData(6, "6-cı")]
        [InlineData(7, "7-ci")]
        [InlineData(8, "8-ci")]
        [InlineData(9, "9-cu")]

        [InlineData(13, "13-cü")]
        [InlineData(1995, "1995-ci")]
        [InlineData(2019, "2019-cu")]

        [InlineData(0, "0-cı")]
        [InlineData(10, "10-cu")]
        [InlineData(100, "100-cü")]
        [InlineData(1000, "1000-ci")]
        [InlineData(1010, "1010-cu")]
        public void TestAsOrdinal(long number, string expected)
        {
            Assert.Equal(expected, number.AsOrdinal());
        }
        
        
        [Theory]
        [InlineData(1, "birinci")]
        [InlineData(2, "ikinci")]
        [InlineData(3, "üçüncü")]
        [InlineData(4, "dördüncü")]
        [InlineData(5, "beşinci")]
        [InlineData(6, "altıncı")]
        [InlineData(7, "yeddinci")]
        [InlineData(8, "səkkizinci")]
        [InlineData(9, "doqquzuncu")]

        [InlineData(13, "on üçüncü")]
        [InlineData(1995, "min doqquz yüz doxsan beşinci")]
        [InlineData(2019, "iki min on doqquzuncu")]

        [InlineData(0, "sıfırıncı")]
        [InlineData(10, "onuncu")]
        [InlineData(100, "yüzüncü")]
        [InlineData(1000, "mininci")]
        [InlineData(1010, "min onuncu")]
        public void TestSpellAsOrdinal(long number, string expected)
        {
            Assert.Equal(expected, number.SpellAsOrdinal());
        }
        

        [Theory]
        [InlineData(2.6, "2 manat 60 qəpik")]
        [InlineData(5, "5 manat")]
        [InlineData(0.5, "50 qəpik")]
        [InlineData(0.05, "5 qəpik")]
        [InlineData(99.99, "99 manat 99 qəpik")]
        public void TestAsCurrency(decimal number, string expected)
        {
            Assert.Equal(expected, number.AsCurrency());   
        }
        
        
        [Theory]
        [InlineData(2.6, "iki manat altmış qəpik")]
        [InlineData(5, "beş manat")]
        [InlineData(0.3, "otuz qəpik")]
        [InlineData(0.05, "beş qəpik")]
        [InlineData(99.99, "doxsan doqquz manat doxsan doqquz qəpik")]
        public void TestSpellAsCurrency(decimal number, string expected)
        {
            Assert.Equal(expected, number.SpellAsCurrency());   
        }

        
        [Theory]
        [InlineData(2.6, "2 dollar 60 cent")]
        [InlineData(5, "5 dollar")]
        [InlineData(0.5, "50 cent")] // https://youtu.be/5qm8PH4xAss?t=43
        [InlineData(0.05, "5 cent")]
        [InlineData(99.99, "99 dollar 99 cent")]
        public void TestAsCurrencyUSD(decimal number, string expected)
        {
            Assert.Equal(expected, number.AsCurrency(nominalName:"dollar", coinName:"cent"));   
        }
        
    }
}