using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Millify.tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestNumberToWordWithDecimal()
        {
            Assert.AreEqual(
                Millify.NumberToWord(3.14), 
                "üç tam yüzdə on dörd");

            Assert.AreEqual(
                Millify.NumberToWord(1.618), 
                "bir tam mində altı yüz on səkkiz");
            Assert.AreEqual(Millify.NumberToWord(0.0101), "sıfır tam on mində yüz bir");

            Assert.AreEqual(
                Millify.NumberToWord(2120134.320421, 6),
                "iki milyon yüz iyirmi min yüz otuz dörd tam bir milyonda üç yüz iyirmi min dörd yüz iyirmi bir");
            
        }

        [TestMethod]
        public void TestConcatWithHarmony()
        {
            Assert.AreEqual(Millify.ConcatWithHarmony("siz", "da"), "sizdə");
            Assert.AreEqual(Millify.ConcatWithHarmony("blur yaxşı oyun", "iydi"), "blur yaxşı oyunuydu");
            Assert.AreEqual(Millify.FixNumberTail("23-cidən"), "23-cüdən");
        }

        [TestMethod]
        public void TestNumberToCurrency()
        {
            Assert.AreEqual(
                Millify.NumberToCurrency(2.5), 
                "2 man. 50 qəp.");

            Assert.AreEqual(
                Millify.NumberToCurrency(350.24, "dollar", "sent"), 
                "350 dollar 24 sent");

            Assert.AreEqual(
                Millify.NumberToCurrency(39.99,integerSuffix:"manat",decimalSuffix:"qəpik", numberToWord:true), 
                "otuz doqquz manat doğsan doqquz qəpik");
        }


    }
}