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
            Assert.Equal(expected, Milli.Spell(number));
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
            Assert.Equal(expected, Milli.Spell(number));
        }



        [Theory]
        [InlineData(-1, "mənfi bir")]
        [InlineData(-22, "mənfi iyirmi iki")]
        [InlineData(-0, "sıfır")]
        public void TestNegativeNumbers(decimal number, string expected)
        {
            Assert.Equal(expected, Milli.Spell(number));   
        }
        
    }
}