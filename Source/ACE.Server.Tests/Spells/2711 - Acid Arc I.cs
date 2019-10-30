using System;
using System.IO;
using System.Linq;
using System.Numerics;
using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Tests.Message_Deserializers;
using ACE.Server.WorldObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Spells
{
    [TestClass]
    public class _2711___Acid_Arc_I
    {
        public static Entity.Spell Spell;
        public static WorldObject Caster;
        public static WorldObject Target;
        public static SpellProjectile Projectile;
        public static int SpellId = 2711;
        public static uint ExpectedWCID = 20973;
        public static float ExpectedScriptIntensity = 0;

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
        public static void ClassInit(TestContext context)
        {
            Spell = new ACE.Server.Entity.Spell(SpellId);

            Caster = WorldObjectFactory.CreateNewWorldObject(1);
            Caster.Name = "Player";
            Caster.InitPhysicsObj();
            // Location = 0x9A220035 [160.126114 102.768822 108.005005] -0.679814 0.000000 0.000000 -0.733385 - Qalaba'r Festival Grounds
            Caster.Location = new Position(0x9A220035, new Vector3(160.126114f, 102.768822f, 108.005005f), new Quaternion(0f, 0f, -0.7333848f, -0.6798138f));

            Target = WorldObjectFactory.CreateNewWorldObject(6077);
            Target.Name = "Oak Target Drudge";
            Target.InitPhysicsObj();
            // Location = 0x9A220035 [150 102 108.0033] 0.7071068 0 0 -0.7071068 - Qalaba'r Festival Grounds
            Target.Location = new Position(0x9A220035, new Vector3(150f, 102f, 108.0033f), new Quaternion(0f, 0f, -0.7071068f, 0.7071068f));

            Projectile = Caster.CreateSpellProjectile(Spell, Target);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            
        }

        [TestInitialize()]
        public void TestInit()
        {
            
        }

        [TestMethod]
        public void GetProjectileSpellType_Returns_Correct_ProjectileType()
        {
            var projectileType = SpellProjectile.GetProjectileSpellType(Spell.Id);
            Assert.AreEqual(SpellProjectile.ProjectileSpellType.Arc, projectileType);
        }

        [TestMethod]
        public void GetProjectileScriptIntensity_Returns_Correct_Value()
        {
            var scriptIntensity = Projectile.GetProjectileScriptIntensity(Projectile.SpellType);
            Assert.AreEqual(ExpectedScriptIntensity, scriptIntensity);
        }

        [TestMethod]
        public void CreateObject_Returns_Correct_WCID()
        {
            var createObjectMessage = new GameMessageCreateObject(Projectile);
            CM_Physics.CreateObject parsed;
            using (BinaryReader binaryReader = new BinaryReader(createObjectMessage.Data))
            {
                binaryReader.BaseStream.Position = 0;
                var opcode = binaryReader.ReadUInt32();
                parsed = CM_Physics.CreateObject.read(binaryReader);
            }
            // TODO: check
            // Physics description bitfield
            // State
            // Weenie description
            Assert.AreEqual(ExpectedWCID, parsed.wdesc._wcid);
        }
    }
}
