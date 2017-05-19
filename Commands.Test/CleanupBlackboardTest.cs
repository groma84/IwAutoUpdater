using IwAutoUpdater.DAL.Updates.Contracts;
using IWAutoUpdater.CrossCutting.SFW.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CleanupBlackboardTest
    {
        private CleanupBlackboard _cleanupBlackboard;
        private Mock<IUpdatePackage> _updatePackageMock;
        private Mock<IBlackboard> _blackboardMock;

        string _packageName1 = "CleanupBlackboardTest";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new Mock<IUpdatePackage>();
            _updatePackageMock.SetupGet(mock => mock.PackageName).Returns(_packageName1);

            _blackboardMock = new Mock<IBlackboard>();

            _cleanupBlackboard = new CleanupBlackboard(_updatePackageMock.Object, _blackboardMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void CleanupBlackboardTest_OneInThenEmpty()
        {
            var actual = _cleanupBlackboard.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 0);
            Assert.IsTrue(actual.Successful);

            _blackboardMock.Verify(mock => mock.Clear(_packageName1), Times.Once);
        }
    }
}
