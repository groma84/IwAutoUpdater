using IwAutoUpdater.DAL.LocalFiles.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.DAL.LocalFiles.Test
{
    [TestClass]
    public class SingleFileTest
    {
        string _path = "SingleFileTest.zip";
        private ISingleFile _singleFile;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _singleFile = new SingleFile();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _singleFile = null;
        }

        [TestMethod]
        public void SingleFileTest_CreateCheckDeleteFile()
        {
            DateTime nun = DateTime.Now;
            using (var fs = File.Create(_path))
            {              
            }

            var content = new byte[] { byte.Parse("123") };

            Assert.IsTrue(_singleFile.DoesExist(_path));
            Assert.AreEqual(nun.ToString(), _singleFile.GetLastModified(_path).ToString());

            Assert.IsTrue(_singleFile.Write(_path, content));
            Assert.AreEqual(content[0], File.ReadAllBytes(_path)[0]);

            Assert.IsTrue(_singleFile.Delete(_path));
            Assert.IsFalse(_singleFile.DoesExist(_path));
            Assert.IsFalse(File.Exists(_path));
        }
    }
}
