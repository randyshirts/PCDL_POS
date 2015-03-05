using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RocketPos.Common.Helpers;

namespace ItemListTests.Functional_Tests
{
    [TestClass]
    public class ConfigsTests
    {
        [TestMethod]
        public void ConfigSettingsLoadsBoolean()
        {
             Assert.IsInstanceOfType(ConfigSettings.IS_ACTIVE_FLAG, typeof(bool));
             Assert.IsTrue(ConfigSettings.IS_ACTIVE_FLAG.Equals(true));
        }

        [TestMethod]
        public void ConfigSettingsLoadsString()
        {
            Assert.IsInstanceOfType(ConfigSettings.DEFAULT_STATE, typeof(string));
            Assert.IsTrue(ConfigSettings.DEFAULT_STATE.Equals("CA"));
        }
    }
}
