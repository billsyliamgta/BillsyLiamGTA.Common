using GTA;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.Ped;

namespace BillsyLiamGTA.Common.Minigames
{
    public class Lockbox : BaseMinigame
    {
        #region Properties

        private Prop Cabinet;

        private Prop Bag;

        private Prop Drill;

        private Prop Moneybag;

        private SynchronizedScene PlayerScene;

        private SynchronizedScene CabinetScene;

        public int Pattern { get; set; } = 1;

        public int Stage { get; private set; } = 1;

        private int PtfxEffect = 0;

        private int SoundId = 0;

        private int FinishedExitSceneGameTime = 0;

        #endregion

        #region Constructor

        public Lockbox(Prop cabinet)
        {
            Cabinet = cabinet;
        }

        #endregion

        public override unsafe void Update()
        {
            base.Update();
            if (Index > 0 && Index != 5 && Index != 6 && Index != 7)
            {
                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "CHI_DRIL_HELP2" /* GXT: Hold ~INPUT_ATTACK~ to drill into the safety deposit boxes. ~n~Press ~INPUT_RELOAD~ to stop drilling the safety deposit boxes. */, true);
            }
            switch (Index)
            {
                case 0:
                    {
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "MC_PLAY", 0);
                        if (Game.Player.Character.Position.DistanceTo(Cabinet.GetOffsetPosition(new Vector3(0f, 0.5f, 0f))) < 1.5f && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "MC_PLAY", 0) && !IsLooted)
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "CAS_DRIL_HELP1" /* GXT: Press ~INPUT_CONTEXT~ to begin drilling the lockers. */, true);
                            if (Game.IsControlJustPressed(Control.Context))
                            {
                                Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, true);
                                Function.Call(Hash.LOCK_MINIMAP_ANGLE, 0);
                                Index++;
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        Function.Call(Hash.REQUEST_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_01@male@");
                        Function.Call(Hash.REQUEST_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_02@male@");
                        Function.Call(Hash.REQUEST_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_03@male@");
                        Function.Call(Hash.REQUEST_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_04@male@");
                        Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ch_finale");
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_01@male@") && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_02@male@") && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_03@male@") && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_04@male@") && Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_ch_finale") && Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/CASINO_HEIST_FINALE_GENERAL_01", false, -1))
                        {
                            Vector3 pos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                            Vector3 rot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                            Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, pos.X, pos.Y, pos.Z, 1f, 5000, rot.Z, 0.75f);
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                        {
                            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 0);
                            Bag = World.CreateProp("hei_p_m_bag_var22_arm_s", Game.Player.Character.Position, false, false);
                            Bag.IsCollisionEnabled = false;
                            Drill = World.CreateProp("ch_prop_vault_drill_01a", Game.Player.Character.Position, false, false);
                            Drill.IsCollisionEnabled = false;
                            PlayerScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter_p_m_bag_var22_arm_s");
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                            PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter_ch_prop_vault_drill_01a");
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Drill);
                            PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "enter_cam");
                            Index++;
                        }
                    }
                    break;
                case 3:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Index++;
                        }
                    }
                    break;
                case 4:
                    {
                        if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "action", 3))
                        {
                            Local_29575.f_13 = PlayerScene.Phase;

                            if (Local_29575.f_13 > 0.999f)
                            {
                                string clip = string.Empty;
                                string cabinet = string.Empty;
                                switch (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 5))
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                        {
                                            clip = "no_reward";
                                        }
                                        break;
                                    case 3:
                                    case 4:
                                        {
                                            clip = "reward";
                                        }
                                        break;
                                }
                                switch (Pattern)
                                {
                                    case 1:
                                        {
                                            cabinet = "ch_prop_ch_sec_cabinet_01abc";
                                        }
                                        break;
                                    case 2:
                                        {
                                            cabinet = "ch_prop_ch_sec_cabinet_01def";
                                        }
                                        break;
                                    case 3:
                                        {
                                            cabinet = "ch_prop_ch_sec_cabinet_01ghi";
                                        }
                                        break;
                                }
                                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PtfxEffect);
                                PtfxEffect = 0;
                                Function.Call(Hash.STOP_SOUND, SoundId);
                                Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                                SoundId = 0;
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", clip, 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", clip + "_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", clip + "_ch_prop_vault_drill_01a");
                                if (clip == "reward")
                                {
                                    Moneybag = World.CreateProp("ch_prop_ch_moneybag_01a", Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "reward_ch_prop_ch_moneybag_01a", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "reward_ch_prop_ch_moneybag_01a", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), false, false);
                                    PlayerScene.PlayEntity(Moneybag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "reward_ch_prop_ch_moneybag_01a");
                                }
                                PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", clip + "_cam");
                                CabinetScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                                CabinetScene.Generate();
                                CabinetScene.PlayEntity(Cabinet, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", clip + "_" + cabinet);
                                Index++;
                            }

                            if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PtfxEffect))
                            {
                                Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, PtfxEffect, "power", 1f, false);
                            }

                            if (Game.IsControlJustReleased(Control.Attack))
                            {
                                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PtfxEffect);
                                PtfxEffect = 0;
                                Function.Call(Hash.STOP_SOUND, SoundId);
                                Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                                SoundId = 0;
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_ch_prop_vault_drill_01a");
                                PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_cam");
                                PlayerScene.SetLooped(true);
                            }
                        }
                        else if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle", 3))
                        {
                            if (Game.IsControlJustPressed(Control.Attack))
                            {
                                Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ch_finale");
                                PtfxEffect = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, "scr_ch_finale_drill_sparks_nodecal", Drill, 0f, 0f, 0f, 0f, 90f, 0f, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, Drill, "Tip_01"), 1f, false, false, false);
                                SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                                Function.Call(Hash.PLAY_SOUND_FROM_COORD, SoundId, "drill", Drill.Position.X, Drill.Position.Y, Drill.Position.Z, "dlc_ch_heist_finale_lockbox_drill_sounds", false, 0, false);
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "action", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "action_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "action_ch_prop_vault_drill_01a");
                                PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "action_cam");
                                PlayerScene.Phase = Local_29575.f_13;
                            }

                            if (Game.IsControlJustPressed(Control.Reload))
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "exit", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "exit_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "exit_ch_prop_vault_drill_01a");
                                PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "exit_cam");
                                Index = 6;
                            }
                        }
                        else
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Drill, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_ch_prop_vault_drill_01a");
                            PlayerScene.PlayCam($"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_0{Stage}@male@", "idle_cam");
                            PlayerScene.SetLooped(true);
                        }
                    }
                    break;
                case 5:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Function.Call(Hash.CLEAR_HELP, true);

                            if (Moneybag != null)
                            {
                                ValueAdded?.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 50000), true));
                                Moneybag.Delete();
                                Moneybag = null;
                            }

                            Local_29575.f_13 = 0f;
                            switch (Stage)
                            {
                                case 1:
                                case 2:
                                case 3:
                                    {
                                        Stage++;
                                        Index--;
                                    }
                                    break;
                                case 4:
                                    {
                                        IsLooted = true;
                                        Index++;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case 6:
                    {
                        if (PlayerScene.Phase >= 0.8f)
                        {
                            Bag?.Delete();
                            Bag = null;
                            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 45, 0, 0);
                            Drill?.Delete();
                            Drill = null;
                            Game.Player.SetControlState(false);
                            GameplayCamera.RelativeHeading = 0f;
                            GameplayCamera.RelativePitch = 1f;
                            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, 1500, true, false, 0);
                            FinishedExitSceneGameTime = Game.GameTime;
                            Index++;
                        }
                    }
                    break;
                case 7:
                    {
                        if (Game.GameTime - FinishedExitSceneGameTime > 1500)
                        {
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
                            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
                            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_01@male@");
                            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_02@male@");
                            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_03@male@");
                            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_04@male@");
                            Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ch_finale");
                            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/CASINO_HEIST_FINALE_GENERAL_01");
                            Game.Player.SetControlState(true);
                            Game.Player.Character.Task.ClearAll();
                            Index = 0;
                        }
                    }
                    break;
            }
        }

        public override void PushDeathResetFunc()
        {
            base.PushDeathResetFunc();
            Dispose();
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 45, 0, 0);
        }

        public override void Dispose()
        {
            base.Dispose();
            Bag?.Delete();
            Bag = null;
            Drill?.Delete();
            Drill = null;
            PlayerScene?.Dispose();
            PlayerScene = null;
            CabinetScene?.Dispose();
            CabinetScene = null;

            if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, PtfxEffect))
            {
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PtfxEffect);
                PtfxEffect = 0;
            }

            if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, SoundId))
            {
                Function.Call(Hash.STOP_SOUND, SoundId);
                Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                SoundId = 0;
            }       

            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_01@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_02@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_03@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, $"anim_heist@hs3f@ig10_lockbox_drill@pattern_0{Pattern}@lockbox_04@male@");
            
            if (Function.Call<bool>(Hash.HAS_PTFX_ASSET_LOADED, "scr_ch_finale"))
            {
                Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ch_finale");
            }

            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            Game.Player.SetControlState(true);
            Game.Player.Character.Task.ClearAllImmediately();
        }
    }
}