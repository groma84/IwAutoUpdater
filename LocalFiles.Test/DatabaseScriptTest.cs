using IwAutoUpdater.DAL.LocalFiles.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles.Test
{
    [TestClass]
    public class DatabaseScriptTest
    {
        private IDatabaseScript _databaseScript;
        private string _path = "InterWattDBV123.ddl";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _databaseScript = new DatabaseScript();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _databaseScript = null;
        }

        [TestMethod]
        public void DatabaseScriptTest_LoadSingleScript()
        {
            var actual = _databaseScript.Load(_path);
            Assert.IsNotNull(actual);
            Assert.AreEqual(123, actual.Version);
            Assert.AreEqual(2, actual.Lines.Length);
            Assert.AreEqual("Zeile1", actual.Lines[0]);
            Assert.AreEqual("Zeile2", actual.Lines[1]);
        }
    }
}
