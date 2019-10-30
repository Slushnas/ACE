using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Tests.ACLogview_Imports.Enums
{
    // TODO: Figure out what bitfield(s) these values map to and replace with OR's
    // Note: These IDs are from the last version of the client. Earlier versions of the client had different values for some of the enums.
    public enum MotionCommand : uint
    {
        Command_Invalid = 0,
        Motion_Invalid = 0x80000000,
        Motion_HoldRun = 0x85000001,
        Motion_HoldSidestep = 0x85000002,
        Motion_Ready = 0x41000003,
        Motion_Stop = 0x40000004,
        Motion_WalkForward = 0x45000005,
        Motion_WalkBackwards = 0x45000006,
        Motion_RunForward = 0x44000007,
        Motion_Fallen = 0x40000008,
        Motion_Interpolating = 0x40000009,
        Motion_Hover = 0x4000000A,
        Motion_On = 0x4000000B,
        Motion_Off = 0x4000000C,
        Motion_TurnRight = 0x6500000D,
        Motion_TurnLeft = 0x6500000E,
        Motion_SideStepRight = 0x6500000F,
        Motion_SideStepLeft = 0x65000010,
        Motion_Dead = 0x40000011,
        Motion_Crouch = 0x41000012,
        Motion_Sitting = 0x41000013,
        Motion_Sleeping = 0x41000014,
        Motion_Falling = 0x40000015,
        Motion_Reload = 0x40000016,
        Motion_Unload = 0x40000017,
        Motion_Pickup = 0x40000018,
        Motion_StoreInBackpack = 0x40000019,
        Motion_Eat = 0x4000001A,
        Motion_Drink = 0x4000001B,
        Motion_Reading = 0x4000001C,
        Motion_JumpCharging = 0x4000001D,
        Motion_AimLevel = 0x4000001E,
        Motion_AimHigh15 = 0x4000001F,
        Motion_AimHigh30 = 0x40000020,
        Motion_AimHigh45 = 0x40000021,
        Motion_AimHigh60 = 0x40000022,
        Motion_AimHigh75 = 0x40000023,
        Motion_AimHigh90 = 0x40000024,
        Motion_AimLow15 = 0x40000025,
        Motion_AimLow30 = 0x40000026,
        Motion_AimLow45 = 0x40000027,
        Motion_AimLow60 = 0x40000028,
        Motion_AimLow75 = 0x40000029,
        Motion_AimLow90 = 0x4000002A,
        Motion_MagicBlast = 0x4000002B,
        Motion_MagicSelfHead = 0x4000002C,
        Motion_MagicSelfHeart = 0x4000002D,
        Motion_MagicBonus = 0x4000002E,
        Motion_MagicClap = 0x4000002F,
        Motion_MagicHarm = 0x40000030,
        Motion_MagicHeal = 0x40000031,
        Motion_MagicThrowMissile = 0x40000032,
        Motion_MagicRecoilMissile = 0x40000033,
        Motion_MagicPenalty = 0x40000034,
        Motion_MagicTransfer = 0x40000035,
        Motion_MagicVision = 0x40000036,
        Motion_MagicEnchantItem = 0x40000037,
        Motion_MagicPortal = 0x40000038,
        Motion_MagicPray = 0x40000039,
        Motion_StopTurning = 0x2000003A,
        Motion_Jump = 0x2500003B,
        Motion_HandCombat = 0x8000003C,
        Motion_NonCombat = 0x8000003D,
        Motion_SwordCombat = 0x8000003E,
        Motion_BowCombat = 0x8000003F,
        Motion_SwordShieldCombat = 0x80000040,
        Motion_CrossbowCombat = 0x80000041,
        Motion_UnusedCombat = 0x80000042,
        Motion_SlingCombat = 0x80000043,
        Motion_2HandedSwordCombat = 0x80000044,
        Motion_2HandedStaffCombat = 0x80000045,
        Motion_DualWieldCombat = 0x80000046,
        Motion_ThrownWeaponCombat = 0x80000047,
        Motion_Graze = 0x80000048,
        Motion_Magic = 0x80000049,
        Motion_Hop = 0x1000004A,
        Motion_Jumpup = 0x1000004B,
        Motion_Cheer = 0x1300004C,
        Motion_ChestBeat = 0x1000004D,
        Motion_TippedLeft = 0x1000004E,
        Motion_TippedRight = 0x1000004F,
        Motion_FallDown = 0x10000050,
        Motion_Twitch1 = 0x10000051,
        Motion_Twitch2 = 0x10000052,
        Motion_Twitch3 = 0x10000053,
        Motion_Twitch4 = 0x10000054,
        Motion_StaggerBackward = 0x10000055,
        Motion_StaggerForward = 0x10000056,
        Motion_Sanctuary = 0x10000057,
        Motion_ThrustMed = 0x10000058,
        Motion_ThrustLow = 0x10000059,
        Motion_ThrustHigh = 0x1000005A,
        Motion_SlashHigh = 0x1000005B,
        Motion_SlashMed = 0x1000005C,
        Motion_SlashLow = 0x1000005D,
        Motion_BackhandHigh = 0x1000005E,
        Motion_BackhandMed = 0x1000005F,
        Motion_BackhandLow = 0x10000060,
        Motion_Shoot = 0x10000061,
        Motion_AttackHigh1 = 0x10000062,
        Motion_AttackMed1 = 0x10000063,
        Motion_AttackLow1 = 0x10000064,
        Motion_AttackHigh2 = 0x10000065,
        Motion_AttackMed2 = 0x10000066,
        Motion_AttackLow2 = 0x10000067,
        Motion_AttackHigh3 = 0x10000068,
        Motion_AttackMed3 = 0x10000069,
        Motion_AttackLow3 = 0x1000006A,
        Motion_HeadThrow = 0x1000006B,
        Motion_FistSlam = 0x1000006C,
        Motion_BreatheFlame_ = 0x1000006D,
        Motion_SpinAttack = 0x1000006E,
        Motion_MagicPowerUp01 = 0x1000006F,
        Motion_MagicPowerUp02 = 0x10000070,
        Motion_MagicPowerUp03 = 0x10000071,
        Motion_MagicPowerUp04 = 0x10000072,
        Motion_MagicPowerUp05 = 0x10000073,
        Motion_MagicPowerUp06 = 0x10000074,
        Motion_MagicPowerUp07 = 0x10000075,
        Motion_MagicPowerUp08 = 0x10000076,
        Motion_MagicPowerUp09 = 0x10000077,
        Motion_MagicPowerUp10 = 0x10000078,
        Motion_ShakeFist = 0x13000079,
        Motion_Beckon = 0x1300007A,
        Motion_BeSeeingYou = 0x1300007B,
        Motion_BlowKiss = 0x1300007C,
        Motion_BowDeep = 0x1300007D,
        Motion_ClapHands = 0x1300007E,
        Motion_Cry = 0x1300007F,
        Motion_Laugh = 0x13000080,
        Motion_MimeEat = 0x13000081,
        Motion_MimeDrink = 0x13000082,
        Motion_Nod = 0x13000083,
        Motion_Point = 0x13000084,
        Motion_ShakeHead = 0x13000085,
        Motion_Shrug = 0x13000086,
        Motion_Wave = 0x13000087,
        Motion_Akimbo = 0x13000088,
        Motion_HeartyLaugh = 0x13000089,
        Motion_Salute = 0x1300008A,
        Motion_ScratchHead = 0x1300008B,
        Motion_SmackHead = 0x1300008C,
        Motion_TapFoot = 0x1300008D,
        Motion_WaveHigh = 0x1300008E,
        Motion_WaveLow = 0x1300008F,
        Motion_YawnStretch = 0x13000090,
        Motion_Cringe = 0x13000091,
        Motion_Kneel = 0x13000092,
        Motion_Plead = 0x13000093,
        Motion_Shiver = 0x13000094,
        Motion_Shoo = 0x13000095,
        Motion_Slouch = 0x13000096,
        Motion_Spit = 0x13000097,
        Motion_Surrender = 0x13000098,
        Motion_Woah = 0x13000099,
        Motion_Winded = 0x1300009A,
        Motion_YMCA = 0x1200009B,
        Motion_EnterGame = 0x1000009C,
        Motion_ExitGame = 0x1000009D,
        Motion_OnCreation = 0x1000009E,
        Motion_OnDestruction = 0x1000009F,
        Motion_EnterPortal = 0x100000A0,
        Motion_ExitPortal = 0x100000A1,
        Command_Cancel = 0x080000A2,
        Command_UseSelected = 0x090000A3,
        Command_AutosortSelected = 0x090000A4,
        Command_DropSelected = 0x090000A5,
        Command_GiveSelected = 0x090000A6,
        Command_SplitSelected = 0x090000A7,
        Command_ExamineSelected = 0x090000A8,
        Command_CreateShortcutToSelected = 0x080000A9,
        Command_PreviousCompassItem = 0x090000AA,
        Command_NextCompassItem = 0x090000AB,
        Command_ClosestCompassItem = 0x090000AC,
        Command_PreviousSelection = 0x090000AD,
        Command_LastAttacker = 0x090000AE,
        Command_PreviousFellow = 0x090000AF,
        Command_NextFellow = 0x090000B0,
        Command_ToggleCombat = 0x090000B1,
        Command_HighAttack = 0x0D0000B2,
        Command_MediumAttack = 0x0D0000B3,
        Command_LowAttack = 0x0D0000B4,
        Command_EnterChat = 0x080000B5,
        Command_ToggleChat = 0x080000B6,
        Command_SavePosition = 0x080000B7,
        Command_OptionsPanel = 0x090000B8,
        Command_ResetView = 0x090000B9,
        Command_CameraLeftRotate = 0x0D0000BA,
        Command_CameraRightRotate = 0x0D0000BB,
        Command_CameraRaise = 0x0D0000BC,
        Command_CameraLower = 0x0D0000BD,
        Command_CameraCloser = 0x0D0000BE,
        Command_CameraFarther = 0x0D0000BF,
        Command_FloorView = 0x090000C0,
        Command_MouseLook = 0x0C0000C1,
        Command_PreviousItem = 0x090000C2,
        Command_NextItem = 0x090000C3,
        Command_ClosestItem = 0x090000C4,
        Command_ShiftView = 0x0D0000C5,
        Command_MapView = 0x090000C6,
        Command_AutoRun = 0x090000C7,
        Command_DecreasePowerSetting = 0x090000C8,
        Command_IncreasePowerSetting = 0x090000C9,
        Motion_Pray = 0x130000CA,
        Motion_Mock = 0x130000CB,
        Motion_Teapot = 0x130000CC,
        Motion_SpecialAttack1 = 0x100000CD,
        Motion_SpecialAttack2 = 0x100000CE,
        Motion_SpecialAttack3 = 0x100000CF,
        Motion_MissileAttack1 = 0x100000D0,
        Motion_MissileAttack2 = 0x100000D1,
        Motion_MissileAttack3 = 0x100000D2,
        Motion_CastSpell = 0x400000D3,
        Motion_Flatulence = 0x120000D4,
        Command_FirstPersonView = 0x090000D5,
        Command_AllegiancePanel = 0x090000D6,
        Command_FellowshipPanel = 0x090000D7,
        Command_SpellbookPanel = 0x090000D8,
        Command_SpellComponentsPanel = 0x090000D9,
        Command_HousePanel = 0x090000DA,
        Command_AttributesPanel = 0x090000DB,
        Command_SkillsPanel = 0x090000DC,
        Command_MapPanel = 0x090000DD,
        Command_InventoryPanel = 0x090000DE,
        Motion_Demonet = 0x120000DF,
        Motion_UseMagicStaff = 0x400000E0,
        Motion_UseMagicWand = 0x400000E1,
        Motion_Blink = 0x100000E2,
        Motion_Bite = 0x100000E3,
        Motion_TwitchSubstate1 = 0x400000E4,
        Motion_TwitchSubstate2 = 0x400000E5,
        Motion_TwitchSubstate3 = 0x400000E6,
        Command_CaptureScreenshotToFile = 0x090000E7,
        Motion_BowNoAmmo = 0x800000E8,
        Motion_CrossBowNoAmmo = 0x800000E9,
        Motion_ShakeFistState = 0x430000EA,
        Motion_PrayState = 0x430000EB,
        Motion_BowDeepState = 0x430000EC,
        Motion_ClapHandsState = 0x430000ED,
        Motion_CrossArmsState = 0x430000EE,
        Motion_ShiverState = 0x430000EF,
        Motion_PointState = 0x430000F0,
        Motion_WaveState = 0x430000F1,
        Motion_AkimboState = 0x430000F2,
        Motion_SaluteState = 0x430000F3,
        Motion_ScratchHeadState = 0x430000F4,
        Motion_TapFootState = 0x430000F5,
        Motion_LeanState = 0x430000F6,
        Motion_KneelState = 0x430000F7,
        Motion_PleadState = 0x430000F8,
        Motion_ATOYOT = 0x420000F9,
        Motion_SlouchState = 0x430000FA,
        Motion_SurrenderState = 0x430000FB,
        Motion_WoahState = 0x430000FC,
        Motion_WindedState = 0x430000FD,
        Command_AutoCreateShortcuts = 0x090000FE,
        Command_AutoRepeatAttacks = 0x090000FF,
        Command_AutoTarget = 0x09000100,
        Command_AdvancedCombatInterface = 0x09000101,
        Command_IgnoreAllegianceRequests = 0x09000102,
        Command_IgnoreFellowshipRequests = 0x09000103,
        Command_InvertMouseLook = 0x09000104,
        Command_LetPlayersGiveYouItems = 0x09000105,
        Command_AutoTrackCombatTargets = 0x09000106,
        Command_DisplayTooltips = 0x09000107,
        Command_AttemptToDeceivePlayers = 0x09000108,
        Command_RunAsDefaultMovement = 0x09000109,
        Command_StayInChatModeAfterSend = 0x0900010A,
        Command_RightClickToMouseLook = 0x0900010B,
        Command_VividTargetIndicator = 0x0900010C,
        Command_SelectSelf = 0x0900010D,
        Motion_SkillHealSelf = 0x1000010E,
        Motion_Woah_Duplicate1 = 0x1000010F, // Appears to be the same as Motion_Woah except it starts with 0x10 instead of 0x13
        Motion_MimeDrink_Duplicate1 = 0x10000110, // Appears to be the same as Motion_MimeDrink except it starts with 0x10 instead of 0x13
        Motion_MimeDrink_Duplicate2 = 0x10000111, // Appears to be the same as Motion_MimeDrink except it starts with 0x10 instead of 0x13
        Command_NextMonster = 0x09000112,
        Command_PreviousMonster = 0x09000113,
        Command_ClosestMonster = 0x09000114,
        Command_NextPlayer = 0x09000115,
        Command_PreviousPlayer = 0x09000116,
        Command_ClosestPlayer = 0x09000117,
        Motion_SnowAngelState = 0x43000118,
        Motion_WarmHands = 0x13000119,
        Motion_CurtseyState = 0x4300011A,
        Motion_AFKState = 0x4300011B,
        Motion_MeditateState = 0x4300011C,
        Command_TradePanel = 0x0900011D,
        Motion_LogOut = 0x1000011E,
        Motion_DoubleSlashLow = 0x1000011F,
        Motion_DoubleSlashMed = 0x10000120,
        Motion_DoubleSlashHigh = 0x10000121,
        Motion_TripleSlashLow = 0x10000122,
        Motion_TripleSlashMed = 0x10000123,
        Motion_TripleSlashHigh = 0x10000124,
        Motion_DoubleThrustLow = 0x10000125,
        Motion_DoubleThrustMed = 0x10000126,
        Motion_DoubleThrustHigh = 0x10000127,
        Motion_TripleThrustLow = 0x10000128,
        Motion_TripleThrustMed = 0x10000129,
        Motion_TripleThrustHigh = 0x1000012A,
        Motion_MagicPowerUp01Purple = 0x1000012B,
        Motion_MagicPowerUp02Purple = 0x1000012C,
        Motion_MagicPowerUp03Purple = 0x1000012D,
        Motion_MagicPowerUp04Purple = 0x1000012E,
        Motion_MagicPowerUp05Purple = 0x1000012F,
        Motion_MagicPowerUp06Purple = 0x10000130,
        Motion_MagicPowerUp07Purple = 0x10000131,
        Motion_MagicPowerUp08Purple = 0x10000132,
        Motion_MagicPowerUp09Purple = 0x10000133,
        Motion_MagicPowerUp10Purple = 0x10000134,
        Motion_Helper = 0x13000135,
        Motion_Pickup5 = 0x40000136,
        Motion_Pickup10 = 0x40000137,
        Motion_Pickup15 = 0x40000138,
        Motion_Pickup20 = 0x40000139,
        Motion_HouseRecall = 0x1000013A,
        Motion_AtlatlCombat = 0x8000013B,
        Motion_ThrownShieldCombat = 0x8000013C,
        Motion_SitState = 0x4300013D,
        Motion_SitCrossleggedState = 0x4300013E,
        Motion_SitBackState = 0x4300013F,
        Motion_PointLeftState = 0x43000140,
        Motion_PointRightState = 0x43000141,
        Motion_TalktotheHandState = 0x43000142,
        Motion_PointDownState = 0x43000143,
        Motion_DrudgeDanceState = 0x43000144,
        Motion_PossumState = 0x43000145,
        Motion_ReadState = 0x43000146,
        Motion_ThinkerState = 0x43000147,
        Motion_HaveASeatState = 0x43000148,
        Motion_AtEaseState = 0x43000149,
        Motion_NudgeLeft = 0x1300014A,
        Motion_NudgeRight = 0x1300014B,
        Motion_PointLeft = 0x1300014C,
        Motion_PointRight = 0x1300014D,
        Motion_PointDown = 0x1300014E,
        Motion_Knock = 0x1300014F,
        Motion_ScanHorizon = 0x13000150,
        Motion_DrudgeDance = 0x13000151,
        Motion_HaveASeat = 0x13000152,
        Motion_LifestoneRecall = 0x10000153,
        Command_CharacterOptionsPanel = 0x09000154,
        Command_SoundAndGraphicsPanel = 0x09000155,
        Command_HelpfulSpellsPanel = 0x09000156,
        Command_HarmfulSpellsPanel = 0x09000157,
        Command_CharacterInformationPanel = 0x09000158,
        Command_LinkStatusPanel = 0x09000159,
        Command_VitaePanel = 0x0900015A,
        Command_ShareFellowshipXP = 0x0900015B,
        Command_ShareFellowshipLoot = 0x0900015C,
        Command_AcceptCorpseLooting = 0x0900015D,
        Command_IgnoreTradeRequests = 0x0900015E,
        Command_DisableWeather = 0x0900015F,
        Command_DisableHouseEffect = 0x09000160,
        Command_SideBySideVitals = 0x09000161,
        Command_ShowRadarCoordinates = 0x09000162,
        Command_ShowSpellDurations = 0x09000163,
        Command_MuteOnLosingFocus = 0x09000164,
        Motion_Fishing = 0x10000165,
        Motion_MarketplaceRecall = 0x10000166,
        Motion_EnterPKLite = 0x10000167,
        Command_AllegianceChat = 0x09000168,
        Command_AutomaticallyAcceptFellowshipRequests = 0x09000169,
        Command_Reply = 0x0900016A,
        Command_MonarchReply = 0x0900016B,
        Command_PatronReply = 0x0900016C,
        Command_ToggleCraftingChanceOfSuccessDialog = 0x0900016D,
        Command_UseClosestUnopenedCorpse = 0x0900016E,
        Command_UseNextUnopenedCorpse = 0x0900016F,
        Command_IssueSlashCommand = 0x09000170,
        Motion_AllegianceHometownRecall = 0x10000171,
        Motion_PKArenaRecall = 0x10000172,
        Motion_OffhandSlashHigh = 0x10000173,
        Motion_OffhandSlashMed = 0x10000174,
        Motion_OffhandSlashLow = 0x10000175,
        Motion_OffhandThrustHigh = 0x10000176,
        Motion_OffhandThrustMed = 0x10000177,
        Motion_OffhandThrustLow = 0x10000178,
        Motion_OffhandDoubleSlashLow = 0x10000179,
        Motion_OffhandDoubleSlashMed = 0x1000017A,
        Motion_OffhandDoubleSlashHigh = 0x1000017B,
        Motion_OffhandTripleSlashLow = 0x1000017C,
        Motion_OffhandTripleSlashMed = 0x1000017D,
        Motion_OffhandTripleSlashHigh = 0x1000017E,
        Motion_OffhandDoubleThrustLow = 0x1000017F,
        Motion_OffhandDoubleThrustMed = 0x10000180,
        Motion_OffhandDoubleThrustHigh = 0x10000181,
        Motion_OffhandTripleThrustLow = 0x10000182,
        Motion_OffhandTripleThrustMed = 0x10000183,
        Motion_OffhandTripleThrustHigh = 0x10000184,
        Motion_OffhandKick = 0x10000185,
        Motion_AttackHigh4 = 0x10000186,
        Motion_AttackMed4 = 0x10000187,
        Motion_AttackLow4 = 0x10000188,
        Motion_AttackHigh5 = 0x10000189,
        Motion_AttackMed5 = 0x1000018A,
        Motion_AttackLow5 = 0x1000018B,
        Motion_AttackHigh6 = 0x1000018C,
        Motion_AttackMed6 = 0x1000018D,
        Motion_AttackLow6 = 0x1000018E,
        Motion_PunchFastHigh = 0x1000018F,
        Motion_PunchFastMed = 0x10000190,
        Motion_PunchFastLow = 0x10000191,
        Motion_PunchSlowHigh = 0x10000192,
        Motion_PunchSlowMed = 0x10000193,
        Motion_PunchSlowLow = 0x10000194,
        Motion_OffhandPunchFastHigh = 0x10000195,
        Motion_OffhandPunchFastMed = 0x10000196,
        Motion_OffhandPunchFastLow = 0x10000197,
        Motion_OffhandPunchSlowHigh = 0x10000198,
        Motion_OffhandPunchSlowMed = 0x10000199,
        Motion_OffhandPunchSlowLow = 0x1000019A,
        Motion_Woah_Duplicate2 = 0x1000019B, // Appears to be the same as Motion_Woah except it starts with 0x10 instead of 0x13
    }

    namespace MovementTypes
    {
        public enum Type
        {
            Invalid,
            RawCommand,
            InterpretedCommand,
            StopRawCommand,
            StopInterpretedCommand,
            StopCompletely,
            MoveToObject,
            MoveToPosition,
            TurnToObject,
            TurnToHeading
        }
    }

    public enum HoldKey
    {
        HoldKey_Invalid,
        HoldKey_None,
        HoldKey_Run,
        Num_HoldKeys
    }

}
