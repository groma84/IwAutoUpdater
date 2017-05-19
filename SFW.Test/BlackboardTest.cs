using IWAutoUpdater.CrossCutting.SFW;
using IWAutoUpdater.CrossCutting.SFW.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IwAutoUpdater.CrossCutting.SFW.Test
{
    [TestClass]
    public class BlackboardTest
    {
        private IBlackboard _blackboard;

        string _key1 = "key1";
        BlackboardEntry _entry1 = new BlackboardEntry
        {
            Content = "_entry1"
        };

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _blackboard = new Blackboard();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _blackboard = null;

        }

        [TestMethod]
        public void BlackboardTest_AddGet()
        {
            {
                var actual = _blackboard.Get(_key1).ToArray();
                Assert.AreEqual(0, actual.Length);
            }

            _blackboard.Add(_key1, _entry1);

            {
                var actual = _blackboard.Get(_key1).ToArray();
                Assert.AreEqual(1, actual.Length);
                Assert.AreEqual(_entry1, actual[0]);
            }
        }
    }
}
