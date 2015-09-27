using IwAutoUpdater.DAL.WebAccess.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IwAutoUpdater.DAL.WebAccess.Test
{
    [TestClass]
    public class HtmlGetterTest
    {
        private IHtmlGetter _htmlGetter;

        string _correctUrl = "https://www.interwatt.net/";
        string _notExistingUrl = "http://www.heise.de/124aegag";

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _htmlGetter = new HtmlGetter();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _htmlGetter = null;
        }

        [TestMethod]
        public void HtmlGetterTest_DownloadHtml_OhneProxy_ExistierendeUrl_StatusCode200()
        {
            var actual = _htmlGetter.DownloadHtml(_correctUrl);
            Assert.AreEqual(200, actual.HttpStatusCode);
            Assert.IsFalse(String.IsNullOrEmpty(actual.Content));
        }

        [TestMethod]
        public void HtmlGetterTest_DownloadHtml_OhneProxy_KaputteUrl_StatusCode404()
        {
            var actual = _htmlGetter.DownloadHtml(_notExistingUrl);
            Assert.AreEqual(404, actual.HttpStatusCode);
            Assert.IsFalse(String.IsNullOrEmpty(actual.Content)); // Fehlerseite kommt trotzdem zurück
        }
    }
}
