using IwAutoUpdater.DAL.WebAccess.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System.Linq;

namespace IwAutoUpdater.BLL.Commands.Test
{
    [TestClass]
    public class CheckUrlHttpStatusIs200Test
    {
        private CheckUrlHttpStatusIs200 _checkUrlHttpStatusIs200;

        string _url = "http://www.somethingsomethingTest.net/";
        private UpdatePackageMock _updatePackageMock;
        private HtmlGetterMock _htmlGetterMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _updatePackageMock = new UpdatePackageMock();
            _updatePackageMock.PackageName = "CheckUrlHttpStatusIs200Test";

            _htmlGetterMock = new HtmlGetterMock();

            _checkUrlHttpStatusIs200 = new CheckUrlHttpStatusIs200(_url, _updatePackageMock, _htmlGetterMock);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _checkUrlHttpStatusIs200 = null;

            _updatePackageMock = null;
            _htmlGetterMock = null;
        }

        [TestMethod]
        public void CheckUrlHttpStatusIs200Test_HtmlDownloadOkay_ResultIsTrue()
        {
            _htmlGetterMock.DownloadHtmlWithProxy = new HtmlDownload()
            {
                Content = "content",
                HttpStatusCode = 200
            };

            var actual = _checkUrlHttpStatusIs200.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 0);
            Assert.AreEqual(1, _htmlGetterMock.DownloadHtmlCalledWithProxy);
            Assert.IsTrue(actual.Successful);
        }

        [TestMethod]
        public void CheckUrlHttpStatusIs200Test_HtmlDownloadBad_ResultIsFalseAndErrorsIsFilled()
        {
            var failedContent = "failedContent";

            _htmlGetterMock.DownloadHtmlWithProxy = new HtmlDownload()
            {
                Content = failedContent,
                HttpStatusCode = 500
            };

            var actual = _checkUrlHttpStatusIs200.Do();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Errors.Count() == 1);
            Assert.AreEqual(failedContent, actual.Errors.First().Text);
            Assert.AreEqual(1, _htmlGetterMock.DownloadHtmlCalledWithProxy);
            Assert.IsFalse(actual.Successful);
        }
    }
}
