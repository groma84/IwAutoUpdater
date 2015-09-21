using IwAutoUpdater.CrossCutting.SFW.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.CrossCutting.SFW.Test
{
    [TestClass]
    public class NowGetterTest
    {
        private INowGetter _nowGetter;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _nowGetter = new NowGetter() as INowGetter;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _nowGetter = null;
        }

        [TestMethod]
        public void NowGetterTest_NowAndUtcNow()
        {
            // ToString() ist hier ein Hack, um die Zeit nicht mit maximaler Genauigkeit zu vergleichen
            Assert.AreEqual(DateTime.Now.ToString(), _nowGetter.Now.ToString());
            Assert.AreEqual(DateTime.UtcNow.ToString(), _nowGetter.UtcNow.ToString());
        }
    }
}
