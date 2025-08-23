using System;
using GTA;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.Ped;

namespace BillsyLiamGTA.Common.Minigames
{
    public class Artwork : BaseMinigame
    {
        #region Properties

        private Prop Cabinet;

        private Prop Painting;

        private Prop Bag;

        private Prop Knife;

        private SynchronizedScene PlayerScene;

        private SynchronizedScene PaintingScene;

        private string ResumeClip = string.Empty;

        private string ExitClip = string.Empty;

        private int ResumeIndex = 0;

        private int FinishedExitSceneGameTime = 0;

        private int Version = 1;

        private bool _hasSetPaintingUp = false;

        private bool _exitedWithPainting = false;

        #endregion

        #region Constructor

        public Artwork(Prop cabinet)
        {
            Cabinet = cabinet;
            string paintingModel = "ch_prop_vault_painting_01a";
            switch (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 10))
            {
                case 0:
                    {
                        paintingModel = "ch_prop_vault_painting_01a";
                    }
                    break;
                case 1:
                    {
                        paintingModel = "ch_prop_vault_painting_01b";
                    }
                    break;
                case 2:
                    {
                        paintingModel = "ch_prop_vault_painting_01c";
                    }
                    break;
                case 3:
                    {
                        paintingModel = "ch_prop_vault_painting_01d";
                    }
                    break;
                case 4:
                    {
                        paintingModel = "ch_prop_vault_painting_01e";
                    }
                    break;
                case 5:
                    {
                        paintingModel = "ch_prop_vault_painting_01f";
                    }
                    break;
                case 6:
                    {
                        paintingModel = "ch_prop_vault_painting_01g";
                    }
                    break;
                case 7:
                    {
                        paintingModel = "ch_prop_vault_painting_01h";
                    }
                    break;
                case 8:
                    {
                        paintingModel = "ch_prop_vault_painting_01i";
                    }
                    break;
                case 9:
                    {
                        paintingModel = "ch_prop_vault_painting_01j";
                    }
                    break;
            }
            Painting = World.CreatePropNoOffset(paintingModel, cabinet.Position, cabinet.Rotation, false);
            Version = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1, 3);
        }

        #endregion

        #region Functions

        public void SetPaintingUp()
        {
            if (!_hasSetPaintingUp)
            {
                Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                int start = Game.GameTime;
                while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig11_steal_painting@male@"))
                {
                    if (Game.GameTime - start > 2000)
                    {
                        throw new TimeoutException("ERROR: Timed out while setting up painting.");
                    }
                    Script.Wait(0);
                }
                Painting.Position = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_top_left_enter_ch_prop_vault_painting_01a", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                Painting.Rotation = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_top_left_enter_ch_prop_vault_painting_01a", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                Painting.IsPositionFrozen = true;
                _hasSetPaintingUp = true;
            }
        }

        public override void Update()
        {
            base.Update();
            switch (Index)
            {
                case 0:
                    {
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "FMMC", 2);
                        if (Game.Player.Character.Position.DistanceTo(Cabinet.GetOffsetPosition(new Vector3(0f, 0.5f, 0f))) < 1.5f && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "FMMC", 2) && !IsLooted)
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "FMMC_INT_P7" /* GXT: Press ~INPUT_CONTEXT~ to cut the painting. */, true);
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
                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig11_steal_painting@male@") && Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS", false, -1))
                        {
                            Vector3 pos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig11_steal_painting@male@", string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                            Vector3 rot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig11_steal_painting@male@", string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
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
                            string clip = string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip;
                            Bag = World.CreateProp("hei_p_m_bag_var22_arm_s", Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_hei_p_m_bag_var22_arm_s", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_hei_p_m_bag_var22_arm_s", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), false, false);
                            Bag.IsCollisionEnabled = false;
                            Knife = World.CreateProp("w_me_switchblade", Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_w_me_switchblade", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_w_me_switchblade", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2), false, false);
                            Knife.IsCollisionEnabled = false;
                            PlayerScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", clip, 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_hei_p_m_bag_var22_arm_s");
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@",  clip + "_w_me_switchblade");
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Knife);
                            PlayerScene.PlayEntity(Cabinet, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_ch_prop_ch_sec_cabinet_02a");
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Cabinet);
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", clip + "_cam_ble");                    
                            if (clip == $"ver_0{Version}_top_left_enter")
                            {
                                PaintingScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_ch_prop_vault_painting_01a");
                                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Painting);
                            }
                            Index++;
                        }
                    }
                    break;
                case 3:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            if (ResumeIndex == 0)
                            {
                                Index++;
                            }
                            else
                            {
                                Index = ResumeIndex;
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 3))
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_cam");
                            PlayerScene.SetLooped(true);
                            PaintingScene.Generate();
                            PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_ch_prop_vault_painting_01a");
                            PaintingScene.SetLooped(true);
                        }
                        else
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_INTOBJ_CP_R" /* GXT: ~s~~INPUT_MOVE_RIGHT_ONLY~ to cut right ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */, false);
                            if (Game.IsControlJustPressed(Control.MoveRightOnly))
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_cam");
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_ch_prop_vault_painting_01a");
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_left_re-enter";
                                ExitClip = $"ver_0{Version}_top_left_exit";
                                ResumeIndex = 4;
                                Index = 14;
                            }
                        }
                    }
                    break;
                case 5:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Index++;
                        }
                    }
                    break;
                case 6:
                    {
                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_right_idle", 3))
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_right_idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_right_idle_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_right_idle_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_right_idle_cam");
                            PlayerScene.SetLooped(true);
                        }
                        else
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_INTOBJ_CP_D" /* GXT: ~s~~INPUT_MOVE_DOWN_ONLY~ to cut down ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */, false);
                            if (Game.IsControlJustPressed(Control.MoveDownOnly))
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_cam");
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_ch_prop_vault_painting_01a");
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_right_enter";
                                ExitClip = $"ver_0{Version}_top_right_exit";
                                ResumeIndex = 6;
                                Index = 14;
                            }
                        }
                    }
                    break;
                case 7:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Index++;
                        }
                    }
                    break;
                case 8:
                    {
                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_idle", 3))
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_idle_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_idle_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_idle_cam");
                            PlayerScene.SetLooped(true);
                        }
                        else
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_INTOBJ_CP_L" /* GXT: ~s~~INPUT_MOVE_LEFT_ONLY~ to cut left ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */, false);
                            if (Game.IsControlJustPressed(Control.MoveLeftOnly))
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_cam");
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_ch_prop_vault_painting_01a");
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_bottom_right_enter";
                                ExitClip = $"ver_0{Version}_bottom_right_exit";
                                ResumeIndex = 8;
                                Index = 14;
                            }
                        }
                    }
                    break;
                case 9:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Index++;
                        }
                    }
                    break;
                case 10:
                    {
                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 3))
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_cam");
                            PlayerScene.SetLooped(true);
                        }
                        else
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_INTOBJ_CP_D" /* GXT: ~s~~INPUT_MOVE_DOWN_ONLY~ to cut down ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */, false);
                            if (Game.IsControlJustPressed(Control.MoveDownOnly))
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_cam");
                                PaintingScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_ch_prop_vault_painting_01a");                           
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_left_re-enter";
                                ExitClip = $"ver_0{Version}_top_left_exit";
                                ResumeIndex = 10;
                                Index = 14;
                            }
                        }
                    }
                    break;
                case 11:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Function.Call(Hash.CLEAR_HELP, true);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", Version == 2 ? "ver_02_with_painting_exit_cam" : "ver_01_with_painting_exit_cam_re1");
                            PaintingScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                            PaintingScene.Generate();
                            PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_ch_prop_vault_painting_01a");
                            _exitedWithPainting = true;
                            Index++;
                        }
                    }
                    break;
                case 12:
                    {
                        if (PlayerScene.Phase >= 0.8f)
                        {
                            if (_exitedWithPainting)
                            {
                                ValueAdded.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 400000, 600000), true));
                                IsLooted = true;
                            }
                            Game.Player.SetControlState(false);
                            GameplayCamera.RelativeHeading = 0f;
                            GameplayCamera.RelativePitch = 1f;
                            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, 1, 1500, 1, 0);
                            FinishedExitSceneGameTime = Game.GameTime;
                            Index++;
                        }
                    }
                    break;
                case 13:
                    {
                        if (Game.GameTime - FinishedExitSceneGameTime > 1500 && PlayerScene.IsFinished)
                        {
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
                            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
                            Bag?.Delete();
                            Bag = null;
                            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 45, 0, 0);
                            Knife?.Delete();
                            Knife = null;
                            Game.Player.SetControlState(true);
                            Game.Player.Character.Task.ClearAll();
                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS");
                            Index = 0;
                        }
                    }
                    break;
                case 14:
                    {
                        Function.Call(Hash.CLEAR_HELP, true);
                        PlayerScene.Generate();
                        PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip, 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                        PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_hei_p_m_bag_var22_arm_s");
                        PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_w_me_switchblade");
                        PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_cam");
                        Index = 12;
                    }
                    break;
            }
        }

        public override void PushDeathResetFunc()
        {
            base.PushDeathResetFunc();
            Cabinet?.Delete();
            Cabinet = null;
            Painting?.Delete();
            Painting = null;
            Bag?.Delete();
            Bag = null;
            Knife?.Delete();
            Knife = null;
            PlayerScene?.Dispose();
            PlayerScene = null;
            PaintingScene?.Dispose();
            PaintingScene = null;
            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 2);
            Game.Player.SetControlState(true);
            Game.Player.Character.Task.ClearAllImmediately();
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS");
        }

        public override void Dispose()
        {
            base.Dispose();
            Cabinet?.Delete();
            Cabinet = null;
            Painting?.Delete();
            Painting = null;
            Bag?.Delete();
            Bag = null;
            Knife?.Delete();
            Knife = null;
            PlayerScene?.Dispose();
            PlayerScene = null;
            PaintingScene?.Dispose();
            PaintingScene = null;
            ResumeClip = string.Empty;
            ExitClip = string.Empty;
            ResumeIndex = 0;
            FinishedExitSceneGameTime = 0;
            Version = 1;
            _hasSetPaintingUp = false;
            _exitedWithPainting = false;
            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 2);
            Game.Player.SetControlState(true);
            Game.Player.Character.Task.ClearAllImmediately();
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS");
        }

        #endregion
    }
}