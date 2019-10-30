using System.Linq;
using ACE.Database;
using ACE.Entity.Enum.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Spell_Projectiles
{
    [TestClass]
    public class WCID_20973
    {
        public static uint WCID = 20973;
        public static double ExpectedMaximumVelocity = 40;

        [TestMethod]
        public void WCID_Has_Correct_MaximumVelocity()
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(WCID);
            var maximumVelocity =
                weenie.WeeniePropertiesFloat.FirstOrDefault(w => w.Type == (int)PropertyFloat.MaximumVelocity)?.Value;

            Assert.AreEqual(ExpectedMaximumVelocity, maximumVelocity);
        }
    }
}
