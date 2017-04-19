using IwAutoUpdater.DAL.WebAccess.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using Moq;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CheckUrlHttpStatusIs200Test
    {
        private CheckUrlHttpStatusIs200 _checkUrlHttpStatusIs200;

        string _url = "http://www.somethingsomethingTest.net/";
        private UpdatePackageMock _updatePackageMock;
        private Mock<IHtmlGetter> _htmlGetterMock;
        private LoggerMock _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.PackageName = "CheckUrlHttpStatusIs200Test";

            _htmlGetterMock = new Mock<IHtmlGetter>();

            _loggerMock = new LoggerMock();

            _checkUrlHttpStatusIs200 = new CheckUrlHttpStatusIs200(_url, _updatePackageMock, _htmlGetterMock.Object, _loggerMock);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _checkUrlHttpStatusIs200 = null;

            _updatePackageMock = null;
        }

        [TestMethod]
        public void CheckUrlHttpStatusIs200Test_HtmlDownloadOkay_ResultIsTrue()
        {
            _htmlGetterMock.Setup(mock => mock.DownloadHtml(It.IsAny<string>(), null, null, It.IsAny<ProxySettings>())).Returns(new HtmlDownload()
            {
                Content = "content",
                HttpStatusCode = 200
            });

            var actual = _checkUrlHttpStatusIs200.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 0);
            _htmlGetterMock.Verify(mock => mock.DownloadHtml(It.IsAny<string>(), null, null, It.IsAny<ProxySettings>()), Times.Once);
            Assert.IsTrue(actual.Successful);
        }

        [TestMethod]
        public void CheckUrlHttpStatusIs200Test_HtmlDownloadBad_ResultIsFalseAndErrorsIsFilled()
        {
            var failedContent = "failedContent";

            _htmlGetterMock.Setup(mock => mock.DownloadHtml(It.IsAny<string>(), null, null, It.IsAny<ProxySettings>())).Returns(new HtmlDownload()
            {
                Content = failedContent,
                HttpStatusCode = 500
            });

            var actual = _checkUrlHttpStatusIs200.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 1);
            Assert.AreEqual(failedContent, actual.Errors.First().Text);
            _htmlGetterMock.Verify(mock => mock.DownloadHtml(It.IsAny<string>(), null, null, It.IsAny<ProxySettings>()), Times.Once);
            Assert.IsFalse(actual.Successful);
        }
    }
}
