using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventually.Test
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void WhenRaised_MultipleSubscribersAreNotified()
        {
            var changed = new Event<string>();

            string received1 = null;
            string received2 = null;

            changed.Subscribe(s => received1 = s);
            changed.Subscribe(s => received2 = s);
            changed.Raise("hello");

            Assert.AreEqual("hello", received1);
            Assert.AreEqual("hello", received2);
        }
    }
}
