using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MotionCommand = ACE.Server.Tests.ACLogview_Imports.Enums.MotionCommand;
using MovementTypes = ACE.Server.Tests.ACLogview_Imports.Enums.MovementTypes;
using Util = ACE.Server.Tests.ACLogview_Imports.Packets;

namespace ACE.Server.Tests.ACLogview_Imports.Message_Deserializers
{
    class CM_Movement
    {

        // A class that mimics MovementManager::unpack_movement
        public class MovementDataUnpack
        {
            public MovementTypes.Type movement_type;
            public byte movement_options;
            public MovementParameters movement_params = new MovementParameters();
            public MotionCommand style;
            public InterpretedMotionState interpretedMotionState = new InterpretedMotionState();
            public uint stickToObject;
            public bool standing_longjump = false;
            public uint moveToObject;
            public Position moveToPos = new Position();
            public float my_run_rate;
            public uint turnToObject;
            public float desiredHeading;

            public static MovementDataUnpack read(BinaryReader binaryReader)
            {
                MovementDataUnpack newObj = new MovementDataUnpack();
                newObj.movement_type = (MovementTypes.Type)binaryReader.ReadByte();
                newObj.movement_options = binaryReader.ReadByte();
                newObj.style = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                switch (newObj.movement_type)
                {
                    case MovementTypes.Type.Invalid:
                        {
                            newObj.interpretedMotionState = InterpretedMotionState.read(binaryReader);
                            if ((newObj.movement_options & 0x1) != 0)
                            {
                                newObj.stickToObject = binaryReader.ReadUInt32();
                            }
                            if ((newObj.movement_options & 0x2) != 0)
                            {
                                newObj.standing_longjump = true;
                            }
                            break;
                        }
                    case MovementTypes.Type.MoveToObject:
                        {
                            newObj.moveToObject = binaryReader.ReadUInt32();
                            newObj.moveToPos = Position.readOrigin(binaryReader);
                            newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                            newObj.my_run_rate = binaryReader.ReadSingle();
                            break;
                        }
                    case MovementTypes.Type.MoveToPosition:
                        {
                            newObj.moveToPos = Position.readOrigin(binaryReader);
                            newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                            newObj.my_run_rate = binaryReader.ReadSingle();
                            break;
                        }
                    case MovementTypes.Type.TurnToObject:
                        {
                            newObj.turnToObject = binaryReader.ReadUInt32();
                            newObj.desiredHeading = binaryReader.ReadSingle();
                            newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                            break;
                        }
                    case MovementTypes.Type.TurnToHeading:
                        {
                            newObj.movement_params = MovementParameters.read(newObj.movement_type, binaryReader);
                            break;
                        }
                }

                return newObj;
            }
        }

        public class MovementParameters
        {
            public enum PackBitfield
            {
                can_walk = (1 << 0),
                can_run = (1 << 1),
                can_sidestep = (1 << 2),
                can_walk_backwards = (1 << 3),
                can_charge = (1 << 4),
                fail_walk = (1 << 5),
                use_final_heading = (1 << 6),
                sticky = (1 << 7),
                move_away = (1 << 8),
                move_towards = (1 << 9),
                use_spheres = (1 << 10),
                set_hold_key = (1 << 11),
                autonomous = (1 << 12),
                modify_raw_state = (1 << 13),
                modify_interpreted_state = (1 << 14),
                cancel_moveto = (1 << 15),
                stop_completely = (1 << 16),
                disable_jump_during_link = (1 << 17),
            }

            public MovementTypes.Type type;
            public uint bitfield;
            public float distance_to_object;
            public float min_distance;
            public float fail_distance;
            public float speed;
            public float walk_run_threshhold;
            public float desired_heading;
            public int Length;

            public static MovementParameters read(MovementTypes.Type type, BinaryReader binaryReader)
            {
                MovementParameters newObj = new MovementParameters();
                newObj.type = type;
                var startPosition = binaryReader.BaseStream.Position;
                switch (type)
                {
                    case MovementTypes.Type.MoveToObject:
                    case MovementTypes.Type.MoveToPosition:
                        {
                            newObj.bitfield = binaryReader.ReadUInt32();
                            newObj.distance_to_object = binaryReader.ReadSingle();
                            newObj.min_distance = binaryReader.ReadSingle();
                            newObj.fail_distance = binaryReader.ReadSingle();
                            newObj.speed = binaryReader.ReadSingle();
                            newObj.walk_run_threshhold = binaryReader.ReadSingle();
                            newObj.desired_heading = binaryReader.ReadSingle();
                            break;
                        }
                    case MovementTypes.Type.TurnToObject:
                    case MovementTypes.Type.TurnToHeading:
                        {
                            newObj.bitfield = binaryReader.ReadUInt32();
                            newObj.speed = binaryReader.ReadSingle();
                            newObj.desired_heading = binaryReader.ReadSingle();
                            break;
                        }
                }
                newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
                return newObj;
            }
        }

        public class InterpretedMotionState
        {
            public enum PackBitfield
            {
                current_style = (1 << 0),
                forward_command = (1 << 1),
                forward_speed = (1 << 2),
                sidestep_command = (1 << 3),
                sidestep_speed = (1 << 4),
                turn_command = (1 << 5),
                turn_speed = (1 << 6)
            }

            public uint bitfield;
            public MotionCommand current_style = MotionCommand.Motion_NonCombat;
            public MotionCommand forward_command = MotionCommand.Motion_Ready;
            public MotionCommand sidestep_command;
            public MotionCommand turn_command;
            public float forward_speed = 1.0f;
            public float sidestep_speed = 1.0f;
            public float turn_speed = 1.0f;
            public List<ActionNode> actions = new List<ActionNode>();
            public List<string> packedItems = new List<string>(); // For display purposes
            public int Length;
            public byte endPadding;

            public static InterpretedMotionState read(BinaryReader binaryReader)
            {
                InterpretedMotionState newObj = new InterpretedMotionState();
                var startPosition = binaryReader.BaseStream.Position;
                newObj.bitfield = binaryReader.ReadUInt32();
                if ((newObj.bitfield & (uint)PackBitfield.current_style) != 0)
                {
                    newObj.current_style = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                    newObj.packedItems.Add(PackBitfield.current_style.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.forward_command) != 0)
                {
                    newObj.forward_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                    newObj.packedItems.Add(PackBitfield.forward_command.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.sidestep_command) != 0)
                {
                    newObj.sidestep_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                    newObj.packedItems.Add(PackBitfield.sidestep_command.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.turn_command) != 0)
                {
                    newObj.turn_command = (MotionCommand)command_ids[binaryReader.ReadUInt16()];
                    newObj.packedItems.Add(PackBitfield.turn_command.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.forward_speed) != 0)
                {
                    newObj.forward_speed = binaryReader.ReadSingle();
                    newObj.packedItems.Add(PackBitfield.forward_speed.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.sidestep_speed) != 0)
                {
                    newObj.sidestep_speed = binaryReader.ReadSingle();
                    newObj.packedItems.Add(PackBitfield.sidestep_speed.ToString());
                }
                if ((newObj.bitfield & (uint)PackBitfield.turn_speed) != 0)
                {
                    newObj.turn_speed = binaryReader.ReadSingle();
                    newObj.packedItems.Add(PackBitfield.turn_speed.ToString());
                }

                uint num_actions = (newObj.bitfield >> 7) & 0x1F;
                newObj.packedItems.Add("num_actions = " + num_actions);
                for (int i = 0; i < num_actions; ++i)
                {
                    newObj.actions.Add(ActionNode.read(binaryReader));
                }

                newObj.endPadding = Util.readToAlign(binaryReader);
                newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);

                return newObj;
            }
        }

        public class ActionNode
        {
            public uint action;
            public uint stamp;
            public int autonomous;
            public float speed;
            public int Length;

            public static ActionNode read(BinaryReader binaryReader)
            {
                ActionNode newObj = new ActionNode();
                var startPosition = binaryReader.BaseStream.Position;
                newObj.action = command_ids[binaryReader.ReadUInt16()];
                uint packedSequence = binaryReader.ReadUInt16();
                newObj.stamp = packedSequence & 0x7FFF;
                newObj.autonomous = (int)((packedSequence >> 15) & 1);
                newObj.speed = binaryReader.ReadSingle();
                newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
                return newObj;
            }
        }

        // Note: These IDs are from the last version of the client. Earlier versions of the client had a different array order and values.
        static uint[] command_ids = {
        (uint)MotionCommand.Motion_Invalid,
        (uint)MotionCommand.Motion_HoldRun,
        (uint)MotionCommand.Motion_HoldSidestep,
        (uint)MotionCommand.Motion_Ready,
        (uint)MotionCommand.Motion_Stop,
        (uint)MotionCommand.Motion_WalkForward,
        (uint)MotionCommand.Motion_WalkBackwards,
        (uint)MotionCommand.Motion_RunForward,
        (uint)MotionCommand.Motion_Fallen,
        (uint)MotionCommand.Motion_Interpolating,
        (uint)MotionCommand.Motion_Hover,
        (uint)MotionCommand.Motion_On,
        (uint)MotionCommand.Motion_Off,
        (uint)MotionCommand.Motion_TurnRight,
        (uint)MotionCommand.Motion_TurnLeft,
        (uint)MotionCommand.Motion_SideStepRight,
        (uint)MotionCommand.Motion_SideStepLeft,
        (uint)MotionCommand.Motion_Dead,
        (uint)MotionCommand.Motion_Crouch,
        (uint)MotionCommand.Motion_Sitting,
        (uint)MotionCommand.Motion_Sleeping,
        (uint)MotionCommand.Motion_Falling,
        (uint)MotionCommand.Motion_Reload,
        (uint)MotionCommand.Motion_Unload,
        (uint)MotionCommand.Motion_Pickup,
        (uint)MotionCommand.Motion_StoreInBackpack,
        (uint)MotionCommand.Motion_Eat,
        (uint)MotionCommand.Motion_Drink,
        (uint)MotionCommand.Motion_Reading,
        (uint)MotionCommand.Motion_JumpCharging,
        (uint)MotionCommand.Motion_AimLevel,
        (uint)MotionCommand.Motion_AimHigh15,
        (uint)MotionCommand.Motion_AimHigh30,
        (uint)MotionCommand.Motion_AimHigh45,
        (uint)MotionCommand.Motion_AimHigh60,
        (uint)MotionCommand.Motion_AimHigh75,
        (uint)MotionCommand.Motion_AimHigh90,
        (uint)MotionCommand.Motion_AimLow15,
        (uint)MotionCommand.Motion_AimLow30,
        (uint)MotionCommand.Motion_AimLow45,
        (uint)MotionCommand.Motion_AimLow60,
        (uint)MotionCommand.Motion_AimLow75,
        (uint)MotionCommand.Motion_AimLow90,
        (uint)MotionCommand.Motion_MagicBlast,
        (uint)MotionCommand.Motion_MagicSelfHead,
        (uint)MotionCommand.Motion_MagicSelfHeart,
        (uint)MotionCommand.Motion_MagicBonus,
        (uint)MotionCommand.Motion_MagicClap,
        (uint)MotionCommand.Motion_MagicHarm,
        (uint)MotionCommand.Motion_MagicHeal,
        (uint)MotionCommand.Motion_MagicThrowMissile,
        (uint)MotionCommand.Motion_MagicRecoilMissile,
        (uint)MotionCommand.Motion_MagicPenalty,
        (uint)MotionCommand.Motion_MagicTransfer,
        (uint)MotionCommand.Motion_MagicVision,
        (uint)MotionCommand.Motion_MagicEnchantItem,
        (uint)MotionCommand.Motion_MagicPortal,
        (uint)MotionCommand.Motion_MagicPray,
        (uint)MotionCommand.Motion_StopTurning,
        (uint)MotionCommand.Motion_Jump,
        (uint)MotionCommand.Motion_HandCombat,
        (uint)MotionCommand.Motion_NonCombat,
        (uint)MotionCommand.Motion_SwordCombat,
        (uint)MotionCommand.Motion_BowCombat,
        (uint)MotionCommand.Motion_SwordShieldCombat,
        (uint)MotionCommand.Motion_CrossbowCombat,
        (uint)MotionCommand.Motion_UnusedCombat,
        (uint)MotionCommand.Motion_SlingCombat,
        (uint)MotionCommand.Motion_2HandedSwordCombat,
        (uint)MotionCommand.Motion_2HandedStaffCombat,
        (uint)MotionCommand.Motion_DualWieldCombat,
        (uint)MotionCommand.Motion_ThrownWeaponCombat,
        (uint)MotionCommand.Motion_Graze,
        (uint)MotionCommand.Motion_Magic,
        (uint)MotionCommand.Motion_Hop,
        (uint)MotionCommand.Motion_Jumpup,
        (uint)MotionCommand.Motion_Cheer,
        (uint)MotionCommand.Motion_ChestBeat,
        (uint)MotionCommand.Motion_TippedLeft,
        (uint)MotionCommand.Motion_TippedRight,
        (uint)MotionCommand.Motion_FallDown,
        (uint)MotionCommand.Motion_Twitch1,
        (uint)MotionCommand.Motion_Twitch2,
        (uint)MotionCommand.Motion_Twitch3,
        (uint)MotionCommand.Motion_Twitch4,
        (uint)MotionCommand.Motion_StaggerBackward,
        (uint)MotionCommand.Motion_StaggerForward,
        (uint)MotionCommand.Motion_Sanctuary,
        (uint)MotionCommand.Motion_ThrustMed,
        (uint)MotionCommand.Motion_ThrustLow,
        (uint)MotionCommand.Motion_ThrustHigh,
        (uint)MotionCommand.Motion_SlashHigh,
        (uint)MotionCommand.Motion_SlashMed,
        (uint)MotionCommand.Motion_SlashLow,
        (uint)MotionCommand.Motion_BackhandHigh,
        (uint)MotionCommand.Motion_BackhandMed,
        (uint)MotionCommand.Motion_BackhandLow,
        (uint)MotionCommand.Motion_Shoot,
        (uint)MotionCommand.Motion_AttackHigh1,
        (uint)MotionCommand.Motion_AttackMed1,
        (uint)MotionCommand.Motion_AttackLow1,
        (uint)MotionCommand.Motion_AttackHigh2,
        (uint)MotionCommand.Motion_AttackMed2,
        (uint)MotionCommand.Motion_AttackLow2,
        (uint)MotionCommand.Motion_AttackHigh3,
        (uint)MotionCommand.Motion_AttackMed3,
        (uint)MotionCommand.Motion_AttackLow3,
        (uint)MotionCommand.Motion_HeadThrow,
        (uint)MotionCommand.Motion_FistSlam,
        (uint)MotionCommand.Motion_BreatheFlame_,
        (uint)MotionCommand.Motion_SpinAttack,
        (uint)MotionCommand.Motion_MagicPowerUp01,
        (uint)MotionCommand.Motion_MagicPowerUp02,
        (uint)MotionCommand.Motion_MagicPowerUp03,
        (uint)MotionCommand.Motion_MagicPowerUp04,
        (uint)MotionCommand.Motion_MagicPowerUp05,
        (uint)MotionCommand.Motion_MagicPowerUp06,
        (uint)MotionCommand.Motion_MagicPowerUp07,
        (uint)MotionCommand.Motion_MagicPowerUp08,
        (uint)MotionCommand.Motion_MagicPowerUp09,
        (uint)MotionCommand.Motion_MagicPowerUp10,
        (uint)MotionCommand.Motion_ShakeFist,
        (uint)MotionCommand.Motion_Beckon,
        (uint)MotionCommand.Motion_BeSeeingYou,
        (uint)MotionCommand.Motion_BlowKiss,
        (uint)MotionCommand.Motion_BowDeep,
        (uint)MotionCommand.Motion_ClapHands,
        (uint)MotionCommand.Motion_Cry,
        (uint)MotionCommand.Motion_Laugh,
        (uint)MotionCommand.Motion_MimeEat,
        (uint)MotionCommand.Motion_MimeDrink,
        (uint)MotionCommand.Motion_Nod,
        (uint)MotionCommand.Motion_Point,
        (uint)MotionCommand.Motion_ShakeHead,
        (uint)MotionCommand.Motion_Shrug,
        (uint)MotionCommand.Motion_Wave,
        (uint)MotionCommand.Motion_Akimbo,
        (uint)MotionCommand.Motion_HeartyLaugh,
        (uint)MotionCommand.Motion_Salute,
        (uint)MotionCommand.Motion_ScratchHead,
        (uint)MotionCommand.Motion_SmackHead,
        (uint)MotionCommand.Motion_TapFoot,
        (uint)MotionCommand.Motion_WaveHigh,
        (uint)MotionCommand.Motion_WaveLow,
        (uint)MotionCommand.Motion_YawnStretch,
        (uint)MotionCommand.Motion_Cringe,
        (uint)MotionCommand.Motion_Kneel,
        (uint)MotionCommand.Motion_Plead,
        (uint)MotionCommand.Motion_Shiver,
        (uint)MotionCommand.Motion_Shoo,
        (uint)MotionCommand.Motion_Slouch,
        (uint)MotionCommand.Motion_Spit,
        (uint)MotionCommand.Motion_Surrender,
        (uint)MotionCommand.Motion_Woah,
        (uint)MotionCommand.Motion_Winded,
        (uint)MotionCommand.Motion_YMCA,
        (uint)MotionCommand.Motion_EnterGame,
        (uint)MotionCommand.Motion_ExitGame,
        (uint)MotionCommand.Motion_OnCreation,
        (uint)MotionCommand.Motion_OnDestruction,
        (uint)MotionCommand.Motion_EnterPortal,
        (uint)MotionCommand.Motion_ExitPortal,
        (uint)MotionCommand.Command_Cancel,
        (uint)MotionCommand.Command_UseSelected,
        (uint)MotionCommand.Command_AutosortSelected,
        (uint)MotionCommand.Command_DropSelected,
        (uint)MotionCommand.Command_GiveSelected,
        (uint)MotionCommand.Command_SplitSelected,
        (uint)MotionCommand.Command_ExamineSelected,
        (uint)MotionCommand.Command_CreateShortcutToSelected,
        (uint)MotionCommand.Command_PreviousCompassItem,
        (uint)MotionCommand.Command_NextCompassItem,
        (uint)MotionCommand.Command_ClosestCompassItem,
        (uint)MotionCommand.Command_PreviousSelection,
        (uint)MotionCommand.Command_LastAttacker,
        (uint)MotionCommand.Command_PreviousFellow,
        (uint)MotionCommand.Command_NextFellow,
        (uint)MotionCommand.Command_ToggleCombat,
        (uint)MotionCommand.Command_HighAttack,
        (uint)MotionCommand.Command_MediumAttack,
        (uint)MotionCommand.Command_LowAttack,
        (uint)MotionCommand.Command_EnterChat,
        (uint)MotionCommand.Command_ToggleChat,
        (uint)MotionCommand.Command_SavePosition,
        (uint)MotionCommand.Command_OptionsPanel,
        (uint)MotionCommand.Command_ResetView,
        (uint)MotionCommand.Command_CameraLeftRotate,
        (uint)MotionCommand.Command_CameraRightRotate,
        (uint)MotionCommand.Command_CameraRaise,
        (uint)MotionCommand.Command_CameraLower,
        (uint)MotionCommand.Command_CameraCloser,
        (uint)MotionCommand.Command_CameraFarther,
        (uint)MotionCommand.Command_FloorView,
        (uint)MotionCommand.Command_MouseLook,
        (uint)MotionCommand.Command_PreviousItem,
        (uint)MotionCommand.Command_NextItem,
        (uint)MotionCommand.Command_ClosestItem,
        (uint)MotionCommand.Command_ShiftView,
        (uint)MotionCommand.Command_MapView,
        (uint)MotionCommand.Command_AutoRun,
        (uint)MotionCommand.Command_DecreasePowerSetting,
        (uint)MotionCommand.Command_IncreasePowerSetting,
        (uint)MotionCommand.Motion_Pray,
        (uint)MotionCommand.Motion_Mock,
        (uint)MotionCommand.Motion_Teapot,
        (uint)MotionCommand.Motion_SpecialAttack1,
        (uint)MotionCommand.Motion_SpecialAttack2,
        (uint)MotionCommand.Motion_SpecialAttack3,
        (uint)MotionCommand.Motion_MissileAttack1,
        (uint)MotionCommand.Motion_MissileAttack2,
        (uint)MotionCommand.Motion_MissileAttack3,
        (uint)MotionCommand.Motion_CastSpell,
        (uint)MotionCommand.Motion_Flatulence,
        (uint)MotionCommand.Command_FirstPersonView,
        (uint)MotionCommand.Command_AllegiancePanel,
        (uint)MotionCommand.Command_FellowshipPanel,
        (uint)MotionCommand.Command_SpellbookPanel,
        (uint)MotionCommand.Command_SpellComponentsPanel,
        (uint)MotionCommand.Command_HousePanel,
        (uint)MotionCommand.Command_AttributesPanel,
        (uint)MotionCommand.Command_SkillsPanel,
        (uint)MotionCommand.Command_MapPanel,
        (uint)MotionCommand.Command_InventoryPanel,
        (uint)MotionCommand.Motion_Demonet,
        (uint)MotionCommand.Motion_UseMagicStaff,
        (uint)MotionCommand.Motion_UseMagicWand,
        (uint)MotionCommand.Motion_Blink,
        (uint)MotionCommand.Motion_Bite,
        (uint)MotionCommand.Motion_TwitchSubstate1,
        (uint)MotionCommand.Motion_TwitchSubstate2,
        (uint)MotionCommand.Motion_TwitchSubstate3,
        (uint)MotionCommand.Command_CaptureScreenshotToFile,
        (uint)MotionCommand.Motion_BowNoAmmo,
        (uint)MotionCommand.Motion_CrossBowNoAmmo,
        (uint)MotionCommand.Motion_ShakeFistState,
        (uint)MotionCommand.Motion_PrayState,
        (uint)MotionCommand.Motion_BowDeepState,
        (uint)MotionCommand.Motion_ClapHandsState,
        (uint)MotionCommand.Motion_CrossArmsState,
        (uint)MotionCommand.Motion_ShiverState,
        (uint)MotionCommand.Motion_PointState,
        (uint)MotionCommand.Motion_WaveState,
        (uint)MotionCommand.Motion_AkimboState,
        (uint)MotionCommand.Motion_SaluteState,
        (uint)MotionCommand.Motion_ScratchHeadState,
        (uint)MotionCommand.Motion_TapFootState,
        (uint)MotionCommand.Motion_LeanState,
        (uint)MotionCommand.Motion_KneelState,
        (uint)MotionCommand.Motion_PleadState,
        (uint)MotionCommand.Motion_ATOYOT,
        (uint)MotionCommand.Motion_SlouchState,
        (uint)MotionCommand.Motion_SurrenderState,
        (uint)MotionCommand.Motion_WoahState,
        (uint)MotionCommand.Motion_WindedState,
        (uint)MotionCommand.Command_AutoCreateShortcuts,
        (uint)MotionCommand.Command_AutoRepeatAttacks,
        (uint)MotionCommand.Command_AutoTarget,
        (uint)MotionCommand.Command_AdvancedCombatInterface,
        (uint)MotionCommand.Command_IgnoreAllegianceRequests,
        (uint)MotionCommand.Command_IgnoreFellowshipRequests,
        (uint)MotionCommand.Command_InvertMouseLook,
        (uint)MotionCommand.Command_LetPlayersGiveYouItems,
        (uint)MotionCommand.Command_AutoTrackCombatTargets,
        (uint)MotionCommand.Command_DisplayTooltips,
        (uint)MotionCommand.Command_AttemptToDeceivePlayers,
        (uint)MotionCommand.Command_RunAsDefaultMovement,
        (uint)MotionCommand.Command_StayInChatModeAfterSend,
        (uint)MotionCommand.Command_RightClickToMouseLook,
        (uint)MotionCommand.Command_VividTargetIndicator,
        (uint)MotionCommand.Command_SelectSelf,
        (uint)MotionCommand.Motion_SkillHealSelf,
        (uint)MotionCommand.Motion_Woah_Duplicate1,
        (uint)MotionCommand.Motion_MimeDrink_Duplicate1,
        (uint)MotionCommand.Motion_MimeDrink_Duplicate2,
        (uint)MotionCommand.Command_NextMonster,
        (uint)MotionCommand.Command_PreviousMonster,
        (uint)MotionCommand.Command_ClosestMonster,
        (uint)MotionCommand.Command_NextPlayer,
        (uint)MotionCommand.Command_PreviousPlayer,
        (uint)MotionCommand.Command_ClosestPlayer,
        (uint)MotionCommand.Motion_SnowAngelState,
        (uint)MotionCommand.Motion_WarmHands,
        (uint)MotionCommand.Motion_CurtseyState,
        (uint)MotionCommand.Motion_AFKState,
        (uint)MotionCommand.Motion_MeditateState,
        (uint)MotionCommand.Command_TradePanel,
        (uint)MotionCommand.Motion_LogOut,
        (uint)MotionCommand.Motion_DoubleSlashLow,
        (uint)MotionCommand.Motion_DoubleSlashMed,
        (uint)MotionCommand.Motion_DoubleSlashHigh,
        (uint)MotionCommand.Motion_TripleSlashLow,
        (uint)MotionCommand.Motion_TripleSlashMed,
        (uint)MotionCommand.Motion_TripleSlashHigh,
        (uint)MotionCommand.Motion_DoubleThrustLow,
        (uint)MotionCommand.Motion_DoubleThrustMed,
        (uint)MotionCommand.Motion_DoubleThrustHigh,
        (uint)MotionCommand.Motion_TripleThrustLow,
        (uint)MotionCommand.Motion_TripleThrustMed,
        (uint)MotionCommand.Motion_TripleThrustHigh,
        (uint)MotionCommand.Motion_MagicPowerUp01Purple,
        (uint)MotionCommand.Motion_MagicPowerUp02Purple,
        (uint)MotionCommand.Motion_MagicPowerUp03Purple,
        (uint)MotionCommand.Motion_MagicPowerUp04Purple,
        (uint)MotionCommand.Motion_MagicPowerUp05Purple,
        (uint)MotionCommand.Motion_MagicPowerUp06Purple,
        (uint)MotionCommand.Motion_MagicPowerUp07Purple,
        (uint)MotionCommand.Motion_MagicPowerUp08Purple,
        (uint)MotionCommand.Motion_MagicPowerUp09Purple,
        (uint)MotionCommand.Motion_MagicPowerUp10Purple,
        (uint)MotionCommand.Motion_Helper,
        (uint)MotionCommand.Motion_Pickup5,
        (uint)MotionCommand.Motion_Pickup10,
        (uint)MotionCommand.Motion_Pickup15,
        (uint)MotionCommand.Motion_Pickup20,
        (uint)MotionCommand.Motion_HouseRecall,
        (uint)MotionCommand.Motion_AtlatlCombat,
        (uint)MotionCommand.Motion_ThrownShieldCombat,
        (uint)MotionCommand.Motion_SitState,
        (uint)MotionCommand.Motion_SitCrossleggedState,
        (uint)MotionCommand.Motion_SitBackState,
        (uint)MotionCommand.Motion_PointLeftState,
        (uint)MotionCommand.Motion_PointRightState,
        (uint)MotionCommand.Motion_TalktotheHandState,
        (uint)MotionCommand.Motion_PointDownState,
        (uint)MotionCommand.Motion_DrudgeDanceState,
        (uint)MotionCommand.Motion_PossumState,
        (uint)MotionCommand.Motion_ReadState,
        (uint)MotionCommand.Motion_ThinkerState,
        (uint)MotionCommand.Motion_HaveASeatState,
        (uint)MotionCommand.Motion_AtEaseState,
        (uint)MotionCommand.Motion_NudgeLeft,
        (uint)MotionCommand.Motion_NudgeRight,
        (uint)MotionCommand.Motion_PointLeft,
        (uint)MotionCommand.Motion_PointRight,
        (uint)MotionCommand.Motion_PointDown,
        (uint)MotionCommand.Motion_Knock,
        (uint)MotionCommand.Motion_ScanHorizon,
        (uint)MotionCommand.Motion_DrudgeDance,
        (uint)MotionCommand.Motion_HaveASeat,
        (uint)MotionCommand.Motion_LifestoneRecall,
        (uint)MotionCommand.Command_CharacterOptionsPanel,
        (uint)MotionCommand.Command_SoundAndGraphicsPanel,
        (uint)MotionCommand.Command_HelpfulSpellsPanel,
        (uint)MotionCommand.Command_HarmfulSpellsPanel,
        (uint)MotionCommand.Command_CharacterInformationPanel,
        (uint)MotionCommand.Command_LinkStatusPanel,
        (uint)MotionCommand.Command_VitaePanel,
        (uint)MotionCommand.Command_ShareFellowshipXP,
        (uint)MotionCommand.Command_ShareFellowshipLoot,
        (uint)MotionCommand.Command_AcceptCorpseLooting,
        (uint)MotionCommand.Command_IgnoreTradeRequests,
        (uint)MotionCommand.Command_DisableWeather,
        (uint)MotionCommand.Command_DisableHouseEffect,
        (uint)MotionCommand.Command_SideBySideVitals,
        (uint)MotionCommand.Command_ShowRadarCoordinates,
        (uint)MotionCommand.Command_ShowSpellDurations,
        (uint)MotionCommand.Command_MuteOnLosingFocus,
        (uint)MotionCommand.Motion_Fishing,
        (uint)MotionCommand.Motion_MarketplaceRecall,
        (uint)MotionCommand.Motion_EnterPKLite,
        (uint)MotionCommand.Command_AllegianceChat,
        (uint)MotionCommand.Command_AutomaticallyAcceptFellowshipRequests,
        (uint)MotionCommand.Command_Reply,
        (uint)MotionCommand.Command_MonarchReply,
        (uint)MotionCommand.Command_PatronReply,
        (uint)MotionCommand.Command_ToggleCraftingChanceOfSuccessDialog,
        (uint)MotionCommand.Command_UseClosestUnopenedCorpse,
        (uint)MotionCommand.Command_UseNextUnopenedCorpse,
        (uint)MotionCommand.Command_IssueSlashCommand,
        (uint)MotionCommand.Motion_AllegianceHometownRecall,
        (uint)MotionCommand.Motion_PKArenaRecall,
        (uint)MotionCommand.Motion_OffhandSlashHigh,
        (uint)MotionCommand.Motion_OffhandSlashMed,
        (uint)MotionCommand.Motion_OffhandSlashLow,
        (uint)MotionCommand.Motion_OffhandThrustHigh,
        (uint)MotionCommand.Motion_OffhandThrustMed,
        (uint)MotionCommand.Motion_OffhandThrustLow,
        (uint)MotionCommand.Motion_OffhandDoubleSlashLow,
        (uint)MotionCommand.Motion_OffhandDoubleSlashMed,
        (uint)MotionCommand.Motion_OffhandDoubleSlashHigh,
        (uint)MotionCommand.Motion_OffhandTripleSlashLow,
        (uint)MotionCommand.Motion_OffhandTripleSlashMed,
        (uint)MotionCommand.Motion_OffhandTripleSlashHigh,
        (uint)MotionCommand.Motion_OffhandDoubleThrustLow,
        (uint)MotionCommand.Motion_OffhandDoubleThrustMed,
        (uint)MotionCommand.Motion_OffhandDoubleThrustHigh,
        (uint)MotionCommand.Motion_OffhandTripleThrustLow,
        (uint)MotionCommand.Motion_OffhandTripleThrustMed,
        (uint)MotionCommand.Motion_OffhandTripleThrustHigh,
        (uint)MotionCommand.Motion_OffhandKick,
        (uint)MotionCommand.Motion_AttackHigh4,
        (uint)MotionCommand.Motion_AttackMed4,
        (uint)MotionCommand.Motion_AttackLow4,
        (uint)MotionCommand.Motion_AttackHigh5,
        (uint)MotionCommand.Motion_AttackMed5,
        (uint)MotionCommand.Motion_AttackLow5,
        (uint)MotionCommand.Motion_AttackHigh6,
        (uint)MotionCommand.Motion_AttackMed6,
        (uint)MotionCommand.Motion_AttackLow6,
        (uint)MotionCommand.Motion_PunchFastHigh,
        (uint)MotionCommand.Motion_PunchFastMed,
        (uint)MotionCommand.Motion_PunchFastLow,
        (uint)MotionCommand.Motion_PunchSlowHigh,
        (uint)MotionCommand.Motion_PunchSlowMed,
        (uint)MotionCommand.Motion_PunchSlowLow,
        (uint)MotionCommand.Motion_OffhandPunchFastHigh,
        (uint)MotionCommand.Motion_OffhandPunchFastMed,
        (uint)MotionCommand.Motion_OffhandPunchFastLow,
        (uint)MotionCommand.Motion_OffhandPunchSlowHigh,
        (uint)MotionCommand.Motion_OffhandPunchSlowMed,
        (uint)MotionCommand.Motion_OffhandPunchSlowLow,
        (uint)MotionCommand.Motion_Woah_Duplicate2
    };
    }
}
