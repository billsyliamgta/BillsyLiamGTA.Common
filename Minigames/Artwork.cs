/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.SHVDN.Ped;

namespace BillsyLiamGTA.Common.SHVDN.Minigames
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

        private float CamBlendOutPhase = 0.999f;

        private int CamBlendOutDuration = 1750;

        private int Version = 1;

        private bool HasSetPaintingUp = false;

        private bool ExitedWithPainting = false;

        private const int ExitWithCamBlendOutIndex = 13;

        private string[] HelpTextEntryStrings =
        {
            "FMMC_INT_P7" /* GXT: Press ~INPUT_CONTEXT~ to cut the painting. */,
            "MC_INTOBJ_CP_R" /* GXT: ~s~~INPUT_MOVE_RIGHT_ONLY~ to cut right ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */,
            "MC_INTOBJ_CP_D" /* GXT: ~s~~INPUT_MOVE_DOWN_ONLY~ to cut down ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */,
            "MC_INTOBJ_CP_L" /* GXT: ~s~~INPUT_MOVE_LEFT_ONLY~ to cut left ~n~Press ~INPUT_SCRIPT_RRIGHT~ to exit. */
        };

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
            if (!HasSetPaintingUp)
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
                HasSetPaintingUp = true;
            }
        }

        public override unsafe void Update()
        {
            base.Update();
            switch (Index)
            {
                case 0:
                    {
                        if (IsLooted) return;
                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "FMMC", 2);
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig11_steal_painting@male@") && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "FMMC", 2) && Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS", false, -1))
                        {
                            Vector3 offsetPos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig11_steal_painting@male@", string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                            Vector3 offsetRot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig11_steal_painting@male@", string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z, 0f, 2);
                            if (Game.Player.Character.Position.DistanceTo(offsetPos) < 1f)
                            {
                                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, HelpTextEntryStrings[0], true);
                                if (Game.IsControlJustPressed(Control.Context))
                                {
                                    Screen.ClearHelpText();
                                    Game.Player.Character.CanSwitchWeapons = false;
                                    Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                                    SetInProgress(true);
                                    Function.Call(Hash.LOCK_MINIMAP_ANGLE, 0);
                                    Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, offsetPos.X, offsetPos.Y, offsetPos.Z, 1f, 20000, offsetRot.Z, 0.75f);
                                    Index++;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                        {
                            string clip = string.IsNullOrEmpty(ResumeClip) ? $"ver_0{Version}_top_left_enter" : ResumeClip;
                            PreviousBagType = BagManager.GetBagVariantTypeFromPed(Game.Player.Character);
                            Bag = BagManager.CreateBagPropFromPed(Game.Player.Character);
                            if (Bag != null && Bag.Exists())
                            {
                                Bag.IsCollisionEnabled = false;
                                Bag.IsVisible = false;
                            }
                            Knife = World.CreateProp("w_me_switchblade", Game.Player.Character.Position, false, false);
                            if (Knife != null && Knife.Exists())
                            {
                                Knife.IsCollisionEnabled = false;
                                Knife.IsVisible = false;
                            }
                            PlayerScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", clip, 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_w_me_switchblade");
                            PlayerScene.PlayEntity(Cabinet, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_ch_prop_ch_sec_cabinet_02a");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", clip + "_cam_ble");
                            if (clip == $"ver_0{Version}_top_left_enter" && Painting != null && Painting.Exists())
                            {
                                PaintingScene = new SynchronizedScene(Cabinet.Position, Cabinet.Rotation);
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", clip + "_ch_prop_vault_painting_01a");;
                            }
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (Bag != null && Bag.Exists() && !Bag.IsVisible && PlayerScene != null && PlayerScene.Phase > 0f)
                        {
                            BagManager.RemoveBag(Game.Player.Character);
                            Bag.IsVisible = true;
                        }

                        if (Knife != null && Knife.Exists() && !Knife.IsVisible && PlayerScene != null && PlayerScene.Phase > 0f)
                            Knife.IsVisible = true;

                        if (PlayerScene != null && PlayerScene.IsFinished)
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
                case 3:
                    {
                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 3))
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_w_me_switchblade");
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_cam");
                            PlayerScene.SetLooped(true);
                            if (PaintingScene != null)
                            {
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_idle_ch_prop_vault_painting_01a");
                                PaintingScene.SetLooped(true);
                            }
                        }
                        else
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, HelpTextEntryStrings[1], false);
                            if (Game.IsControlJustPressed(Control.MoveRightOnly))
                            {
                                Screen.ClearHelpText();
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_cam");
                                if (PaintingScene != null)
                                {
                                    PaintingScene.Generate();
                                    PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_top_left_to_right_ch_prop_vault_painting_01a");
                                }
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_left_re-enter";
                                ExitClip = $"ver_0{Version}_top_left_exit";
                                ResumeIndex = 3;
                                Index = ExitWithCamBlendOutIndex;
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished) Index++;
                    }
                    break;
                case 5:
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
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, HelpTextEntryStrings[2], false);
                            if (Game.IsControlJustPressed(Control.MoveDownOnly))
                            {
                                Screen.ClearHelpText();
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_cam");
                                if (PaintingScene != null)
                                {
                                    PaintingScene.Generate();
                                    PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_right_top_to_bottom_ch_prop_vault_painting_01a");
                                }
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_right_enter";
                                ExitClip = $"ver_0{Version}_top_right_exit";
                                ResumeIndex = 5;
                                Index = ExitWithCamBlendOutIndex;
                            }
                        }
                    }
                    break;
                case 6:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished) Index++;
                    }
                    break;
                case 7:
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
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, HelpTextEntryStrings[3], false);
                            if (Game.IsControlJustPressed(Control.MoveLeftOnly))
                            {
                                Screen.ClearHelpText();
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_cam");
                                if (PaintingScene != null)
                                {
                                    PaintingScene.Generate();
                                    PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_bottom_right_to_left_ch_prop_vault_painting_01a");
                                }
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_bottom_right_enter";
                                ExitClip = $"ver_0{Version}_bottom_right_exit";
                                ResumeIndex = 7;
                                Index = ExitWithCamBlendOutIndex;
                            }
                        }
                    }
                    break;
                case 8:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished) Index++;
                    }
                    break;
                case 9:
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
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, HelpTextEntryStrings[2], false);
                            if (Game.IsControlJustPressed(Control.MoveDownOnly))
                            {
                                Screen.ClearHelpText();
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_hei_p_m_bag_var22_arm_s");
                                PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_w_me_switchblade");
                                PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_cam");
                                if (PaintingScene != null)
                                {
                                    PaintingScene.Generate();
                                    PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_cutting_left_top_to_bottom_ch_prop_vault_painting_01a");
                                }
                                Index++;
                            }
                            if (Game.IsControlJustPressed(Control.ScriptRRight))
                            {
                                ResumeClip = $"ver_0{Version}_top_left_re-enter";
                                ExitClip = $"ver_0{Version}_top_left_exit";
                                ResumeIndex = 9;
                                Index = ExitWithCamBlendOutIndex;
                            }
                        }
                    }
                    break;
                case 10:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished)
                        {
                            Screen.ClearHelpText();
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_hei_p_m_bag_var22_arm_s");
                            PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_w_me_switchblade");
                            string camClip = Version == 2 ? "ver_02_with_painting_exit_cam" : "ver_01_with_painting_exit_cam_re1";
                            PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", camClip);
                            if (PaintingScene != null)
                            {
                                PaintingScene.Generate();
                                PaintingScene.PlayEntity(Painting, "anim_heist@hs3f@ig11_steal_painting@male@", $"ver_0{Version}_with_painting_exit_ch_prop_vault_painting_01a");
                            }
                            float camBlendOutPhase = 0f, unknownParam = 0f;
                            if (Function.Call<bool>(Hash.FIND_ANIM_EVENT_PHASE, "anim_heist@hs3f@ig11_steal_painting@male@", camClip, "CamBlendOut", &camBlendOutPhase, &unknownParam))
                            {
                            }
                            CamBlendOutPhase = camBlendOutPhase;
                            CamBlendOutDuration = 1750;
                            ExitedWithPainting = true;
                            Index++;
                        }
                    }
                    break;
                case 11:
                    {
                        if (PlayerScene != null && PlayerScene.Phase > CamBlendOutPhase)
                        {
                            Game.Player.SetControlState(false);
                            Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f, 1f);
                            Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
                            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, true, CamBlendOutDuration, true, false, 0);
                            FinishedExitSceneGameTime = Game.GameTime;
                            Index++;
                        }
                    }
                    break;
                case 12:
                    {
                        if (Game.GameTime - FinishedExitSceneGameTime > CamBlendOutDuration && PlayerScene != null && PlayerScene.IsFinished)
                        {
                            if (ExitedWithPainting)
                            {
                                ValueAdded.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 400000, 600000), true));
                                IsLooted = true;
                            }
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            Game.Player.Character.CanSwitchWeapons = true;
                            SetInProgress(false);
                            Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);
                            if (Bag != null && Bag.Exists())
                            {
                                Bag.Delete();
                                Bag = null;
                            }
                            BagManager.SetBagFromVariantType(Game.Player.Character, PreviousBagType);
                            if (Knife != null && Knife.Exists())
                            {
                                Knife.Delete();
                                Knife = null;
                            }
                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig11_steal_painting@male@");
                            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/HEIST_FINALE_STEAL_PAINTINGS");
                            Game.Player.SetControlState(true);
                            Game.Player.Character.Task.ClearAll();
                            Index = 0;
                        }
                    }
                    break;
                case 13:
                    {
                        Screen.ClearHelpText();
                        PlayerScene.Generate();
                        PlayerScene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip, 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                        PlayerScene.PlayEntity(Bag, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_hei_p_m_bag_var22_arm_s");
                        PlayerScene.PlayEntity(Knife, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_w_me_switchblade");
                        PlayerScene.PlayCam("anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_cam");
                        float camBlendOutPhase = 0f, unknownParam = 0f;
                        if (Function.Call<bool>(Hash.FIND_ANIM_EVENT_PHASE, "anim_heist@hs3f@ig11_steal_painting@male@", ExitClip + "_cam", "CamBlendOut", &camBlendOutPhase, &unknownParam))
                        {
                        }
                        CamBlendOutPhase = camBlendOutPhase;
                        CamBlendOutDuration = 1750;
                        Index = 11;
                    }
                    break;
            }
        }

        public override void PushDeathResetFunction()
        {
            base.PushDeathResetFunction();
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            if (Knife != null && Knife.Exists())
            {
                Knife.Delete();
                Knife = null;
            }
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
            if (Cabinet != null && Cabinet.Exists())
            {
                Cabinet.Delete();
                Cabinet = null;
            }
            if (Painting != null && Painting.Exists())
            {
                Painting.Delete();
                Painting = null;
            }          
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            if (Knife != null && Knife.Exists())
            {
                Knife.Delete();
                Knife = null;
            }
            PlayerScene?.Dispose();
            PlayerScene = null;
            PaintingScene?.Dispose();
            PaintingScene = null;;
            ResumeClip = string.Empty;
            ExitClip = string.Empty;
            ResumeIndex = 0;
            FinishedExitSceneGameTime = 0;
            Version = 1;
            HasSetPaintingUp = false;
            ExitedWithPainting = false;
            Game.Player.Character.CanSwitchWeapons = true;
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