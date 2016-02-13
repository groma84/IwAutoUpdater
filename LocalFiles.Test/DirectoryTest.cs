using IwAutoUpdater.DAL.LocalFiles.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.IO;
using System.Linq;

namespace IwAutoUpdater.DAL.LocalFiles.Test
{
    [TestClass]
    public class DirectoryTest
    {
        string _path = "DirectoryTestDirectory";
        private IDirectory _directory;
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _loggerMock = new LoggerMock();

            _directory = new Directory(_loggerMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (System.IO.Directory.Exists(_path))
            {
                System.IO.Directory.Delete(_path, true);
            }

            _directory = null;
            _loggerMock = null;

        }

        [TestMethod]
        public void DirectoryTest_Delete_DirectoryExists()
        {
            System.IO.Directory.CreateDirectory(_path);

            Assert.IsTrue(System.IO.Directory.Exists(_path));
            _directory.Delete(_path);
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void DirectoryTest_Delete_DirectoryDoesNotExist()
        {
            Assert.IsFalse(System.IO.Directory.Exists(_path));
            _directory.Delete(_path);
            Assert.IsFalse(System.IO.Directory.Exists(_path));
        }

        [TestMethod]
        public void DirectoryTest_GetFiles()
        {
            var p1 = Path.Combine(_path, "file1.txt");
            var p2 = Path.Combine(_path, "file2.txt");
            System.IO.Directory.CreateDirectory(_path);
            File.Create(p1).Dispose();
            File.Create(p2).Dispose();

            {
                var actual = _directory.GetFiles(_path, "*.txt").ToArray();
                Assert.AreEqual(2, actual.Length);
                Assert.AreEqual(p1, actual[0]);
                Assert.AreEqual(p2, actual[1]);
            }

            {
                var actual = _directory.GetFiles(_path, "*.banane").ToArray();
                Assert.AreEqual(0, actual.Length);
            }
        }
    }
}
