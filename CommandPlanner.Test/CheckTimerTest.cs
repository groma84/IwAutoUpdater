using IwAutoUpdater.BLL.CommandPlanner.Contracts;
using IwAutoUpdater.CrossCutting.SFW.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwAutoUpdater.BLL.CommandPlanner.Test
{
    [TestClass]
    public class CheckTimerTest
    {
        private NowGetterMock _nowGetter;
        private ICheckTimer _checkTimer;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _nowGetter = new NowGetterMock();
            _checkTimer = new CheckTimer(_nowGetter) as ICheckTimer;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _nowGetter = null;
            _checkTimer = null;
        }

        [TestMethod]
        public void CheckTimerTest_FittingTime_True()
        {
            _nowGetter.Now = new DateTime(2010, 1, 1, 15, 15, 15);
            var actual = _checkTimer.IsCheckForUpdatesNecessary(15);

            Assert.IsTrue(actual);

        }

        [TestMethod]
        public void CheckTimerTest_UnfittingTime_False()
        {
            _nowGetter.Now = new DateTime(2010, 1, 1, 15, 17, 0);
            var actual = _checkTimer.IsCheckForUpdatesNecessary(15);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckTimerTest_CheckIntervalNegative_ArgumentException()
        {
            _checkTimer.IsCheckForUpdatesNecessary(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckTimerTest_CheckIntervalLargerThanOneDay_ArgumentException()
        {
            _checkTimer.IsCheckForUpdatesNecessary(1440);
        }
    }
}
