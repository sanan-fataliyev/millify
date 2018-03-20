using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace millify.tests
{
    [TestFixture]
    class TestTestClass
    {
        //test test
        [Test]
        public void TestMethodThatWillFail()
        {
            Assert.That(true, Is.False);
        }

    }
}
