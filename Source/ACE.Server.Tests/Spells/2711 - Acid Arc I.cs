using System;
using System.IO;
using System.Linq;
using System.Numerics;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Spells
{
    [TestClass]
    public class _2711___Acid_Arc_I
    {
        public static Entity.Spell Spell;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            // copy config.js and initialize configuration
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.js"),
                ".\\Config.js", true);
            ConfigManager.Initialize();

            DatabaseManager.Initialize();
            DatabaseManager.Start();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory, true);
            GuidManager.Initialize();
            HouseManager.Initialize();
            PropertyManager.Initialize();
            WorldManager.Initialize();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            DatabaseManager.Stop();
        }

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            Spell = new ACE.Server.Entity.Spell(2711);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            
        }

        [TestInitialize()]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void GetProjectileSpellType_Returns_Correct_ProjectileType()
        {
            var projectileType = SpellProjectile.GetProjectileSpellType(Spell.Id);
            Assert.AreEqual(SpellProjectile.ProjectileSpellType.Arc, projectileType);
        }

        // TODO: Break this out to a WCID folder
        [TestMethod]
        public void Spell_Has_Correct_MaximumVelocity()
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(Spell.Wcid);
            var maximumVelocity =
                weenie.WeeniePropertiesFloat.FirstOrDefault(w => w.Type == (int) PropertyFloat.MaximumVelocity)?.Value;

            Assert.AreEqual(40d, maximumVelocity);
        }


        [TestMethod]
        public void CreateSpellProjectile_Returns_Correct_WCID()
        {
            var caster = WorldObjectFactory.CreateNewWorldObject(1);
            caster.Name = "Player";
            caster.InitPhysicsObj();
            // Location = 0x9A220035 [160.126114 102.768822 108.005005] -0.679814 0.000000 0.000000 -0.733385 - Qalaba'r Festival Grounds
            caster.Location = new Position(0x9A220035, new Vector3(160.126114f, 102.768822f, 108.005005f), new Quaternion(0f, 0f, -0.7333848f, -0.6798138f));

            var target = WorldObjectFactory.CreateNewWorldObject(6077);
            target.Name = "Oak Target Drudge";
            target.InitPhysicsObj();
            // Location = 0x9A220035 [150 102 108.0033] 0.7071068 0 0 -0.7071068 - Qalaba'r Festival Grounds
            target.Location = new Position(0x9A220035, new Vector3(150f, 102f, 108.0033f), new Quaternion(0f, 0f, - 0.7071068f, 0.7071068f));

            var projectile = caster.CreateSpellProjectile(Spell, target);
            // TODO: check
            // Physics description bitfield
            // State
            // Weenie description
            Assert.AreEqual(20973, projectile.WeenieClassId);
        }
    }
}
