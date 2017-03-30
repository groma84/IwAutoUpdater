using IwAutoUpdater.DAL.Updates.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CheckFreeDiskspaceTest
    {
        private Mock<IUpdatePackage> _updatePackageMock;
        private Mock<IUpdatePackageAccess> _updatePackageAccessMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new Mock<IUpdatePackage>();
            _updatePackageAccessMock = new Mock<IUpdatePackageAccess>();

            _updatePackageAccessMock
                .Setup(m => m.GetFilenameOnly())
                .Returns("test.zip");

            _updatePackageMock
                .Setup(m => m.Access)
                .Returns(_updatePackageAccessMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void CheckFreeDiskspace_C_EnoughFreeSpace() 
		{
            var cfd = new CheckFreeDiskspace(@"C:", _updatePackageMock.Object);
            var result = cfd.Do();
            Assert.IsTrue(result.Successful);
            Assert.IsFalse(result.Errors.Any());
		}
    }
}
