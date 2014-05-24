using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eventually.Test
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public void WhenMessageIsSend_SubscribersAreNotified()
        {
            string received1 = null;
            string received2 = null;

            var bus = new Bus();

            bus.Subscribe<string>(x => received1 = x);
            bus.Subscribe<string>(x => received2 = x);
            bus.Publish("hello");

            Assert.AreEqual("hello", received1);
            Assert.AreEqual("hello", received2);
        }
    }
}
