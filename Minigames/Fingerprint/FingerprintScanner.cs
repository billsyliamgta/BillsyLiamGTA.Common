/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.SHVDN.Ped;
using static BillsyLiamGTA.Common.SHVDN.Elements.Extensions;

namespace BillsyLiamGTA.Common.SHVDN.Minigames.Fingerprint
{
    public enum FingerprintScannerType
    {
        /// <summary>
        /// Not interactive.
        /// </summary>
        Dummy = -1,
        /// <summary>
        /// Must hack the fingerprint scanner.
        /// </summary>
        Minigame = 0,
        /// <summary>
        /// Instantly swipes the fingerprint scanner.
        /// </summary>
        InstantSwipe = 1,
        /// <summary>
        /// Plant's a thermal charge on the fingerprint scanner.
        /// </summary>
        PlaceThermal = 2
    }

    /// <summary>
    /// A class for creating fingerprint scanners.
    /// </summary>
    public class FingerprintScanner
    {
        #region Properties

        public FingerprintScannerType Type { get; set; } = FingerprintScannerType.Minigame;

        public int Index { get; private set; } = 0;

        public int PrintsToHack { get; set; } = 3;

        public bool CanBeUsed { get; set; } = true;

        private Prop Keypad;

        private Prop USB;

        private Prop Phone;

        private Prop Card;

        private Prop Thermite;

        private int ThermalBurnPtfxHandle = 0;

        private int ThermalBurnPtfxStartedGameTime = 0;

        private BagManager.BagVariantTypes PreviousBagType;

        private Prop Bag;

        private SynchronizedScene Scene;

        private FingerprintMinigame Minigame;

        #endregion

        #region Constructor

        public FingerprintScanner(Vector3 position, float heading = 0f, FingerprintScannerType type = FingerprintScannerType.Minigame)
        {
            Keypad = World.CreatePropNoOffset("ch_prop_fingerprint_scanner_01c", position, new Vector3(0f, 0f, heading), false);
            if (Keypad != null && Keypad.Exists())
            {
                Keypad.IsInvincible = true;
                Keypad.IsPositionFrozen = true;
                Keypad.IsCollisionProof = true;
                Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, Keypad, false);
                Function.Call(Hash.SET_ENTITY_DYNAMIC, Keypad, false);
                Function.Call(Hash.SET_ENTITY_COLLISION, Keypad, false, 0);
                Function.Call(Hash.SET_USE_KINEMATIC_PHYSICS, Keypad, false);
            }
            Type = type;
        }

        #endregion

        #region Functions

        public unsafe void Update()
        {
            bool isDead = Game.Player.Character.IsDead;

            switch (Type)
            {
                case FingerprintScannerType.Dummy:
                    {
                    }
                    break;
                case FingerprintScannerType.Minigame:
                    {
                        switch (Index)
                        {
                            case 0:
                                {
                                    if (CanBeUsed)
                                    {
                                        RequestAnimDict("anim_heist@hs3f@ig1_hack_keypad@male@");
                                        if (HasAnimDictLoaded("anim_heist@hs3f@ig1_hack_keypad@male@"))
                                        {
                                            Vector3 posOffset = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            Vector3 rotOffset = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            if (!isDead && Game.Player.Character.Position.DistanceTo(posOffset) < 1.5f)
                                            {
                                                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to start hacking.");
                                                if (Game.IsControlJustPressed(Control.Context))
                                                {
                                                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), true);
                                                    Function.Call(Hash.SET_PED_USING_ACTION_MODE, Game.Player.Character, false, "DEFAULT_ACTION");
                                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, posOffset.X, posOffset.Y, posOffset.Z, 1f, 5000, rotOffset.Z, 0.1f);
                                                    Index++;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                                    {
                                        USB = World.CreateProp("ch_prop_ch_usb_drive01x", Game.Player.Character.Position, false, false);
                                        if (USB != null && USB.Exists())
                                            USB.IsCollisionEnabled = false;
                                        Phone = World.CreateProp("prop_phone_ing", Game.Player.Character.Position, false, false);
                                        if (Phone != null && Phone.Exists())
                                            Phone.IsCollisionEnabled = false;
                                        Scene = new SynchronizedScene(Keypad.Position, Keypad.Rotation);
                                        Scene.Generate();
                                        Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                        Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01_ch_prop_ch_usb_drive01x");
                                        Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01_prop_phone_ing");
                                        Index++;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (Scene != null && Scene.IsFinished)
                                    {
                                        Scene.Generate();
                                        Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                        Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01_ch_prop_ch_usb_drive01x");
                                        Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01_prop_phone_ing");
                                        Scene.IsLooped = true;
                                        Index++;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (Minigame == null)
                                        Minigame = new FingerprintMinigame(PrintsToHack);
                                    else
                                    {
                                        Minigame.Update();

                                        if (Minigame.IsInactive || isDead)
                                        {
                                            Scene.Generate();

                                            if (!isDead && USB != null && USB.Exists() && Phone != null && Phone.Exists())
                                            {
                                                switch (Minigame.State)
                                                {
                                                    case FingerprintMinigame.FingerprintMinigameState.Aborted:
                                                        {
                                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit_ch_prop_ch_usb_drive01x");
                                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit_prop_phone_ing");
                                                            CanBeUsed = true;
                                                        }
                                                        break;
                                                    case FingerprintMinigame.FingerprintMinigameState.Failed:
                                                        {
                                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react_ch_prop_ch_usb_drive01x");
                                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react_prop_phone_ing");
                                                            CanBeUsed = true;
                                                        }
                                                        break;
                                                    case FingerprintMinigame.FingerprintMinigameState.Successful:
                                                        {
                                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01_ch_prop_ch_usb_drive01x");
                                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01_prop_phone_ing");
                                                            OpenNearbyDoor();
                                                            CanBeUsed = false;
                                                        }
                                                        break;
                                                }
                                            }

                                            Minigame.Dispose();
                                            Minigame = null;
                                            Index++;
                                        }
                                    }  
                                }
                                break;
                            case 4:
                                {
                                    if (Scene != null && Scene.IsFinished || isDead)
                                    {
                                        if (USB != null && USB.Exists())
                                        {
                                            USB.Delete();
                                            USB = null;
                                        }
                                        if (Phone != null && Phone.Exists())
                                        {
                                            Phone.Delete();
                                            Phone = null;
                                        }
                                        Game.Player.Character.Task.ClearAll();
                                        Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig1_hack_keypad@male@");
                                        Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), false);
                                        Index = 0;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case FingerprintScannerType.InstantSwipe:
                    {
                        switch (Index)
                        {
                            case 0:
                                {
                                    if (CanBeUsed)
                                    {
                                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig3_cardswipe@male@");
                                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig3_cardswipe@male@"))
                                        {
                                            Vector3 offsetPos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig3_cardswipe@male@", "success_var01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            Vector3 offsetRot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig3_cardswipe@male@", "success_var01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            if (!isDead && Game.Player.Character.Position.DistanceTo(offsetRot) < 1f)
                                            {
                                                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to swipe your keycard.");
                                                if (Game.IsControlJustPressed(Control.Context))
                                                {
                                                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), true);
                                                    Function.Call(Hash.SET_PED_USING_ACTION_MODE, Game.Player.Character, false, "DEFAULT_ACTION");
                                                    Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, offsetPos.X, offsetPos.Y, offsetPos.Z, 1f, 20000, offsetRot.Z, 0.1f);
                                                    Index++;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                                    {
                                        Card = World.CreateProp("ch_prop_swipe_card_01b", Game.Player.Character.Position, false, false);
                                        if (Card != null && Card.Exists())
                                            Card.IsCollisionEnabled = false;
                                        int rng = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1, 4);
                                        Scene = new SynchronizedScene(Keypad.Position, Keypad.Rotation);
                                        Scene.Generate();
                                        Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig3_cardswipe@male@", $"success_var0{rng}", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                        Scene.PlayEntity(Card, "anim_heist@hs3f@ig3_cardswipe@male@", $"success_var0{rng}_card");
                                        Index++;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (Scene != null)
                                    {
                                        if (Scene.IsFinished)
                                        {
                                            if (Card != null && Card.Exists())
                                            {
                                                Card.Delete();
                                                Card = null;
                                            }
                                            Scene.Dispose();
                                            Scene = null;
                                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig3_cardswipe@male@");
                                            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), false);
                                            Game.Player.SetControlState(true);
                                            Game.Player.Character.Task.ClearAll();
                                            OpenNearbyDoor();
                                            CanBeUsed = false;
                                            Index = 0;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case FingerprintScannerType.PlaceThermal:
                    {
                        switch (Index)
                        {
                            case 0:
                                {
                                    if (CanBeUsed)
                                    {
                                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@");
                                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@thermal_charge");
                                        Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ch_finale");
                                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@") && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@thermal_charge") && Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_ch_finale"))
                                        {
                                            Vector3 offsetPos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@", "thermal_charge_male_male", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            Vector3 offsetRot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@", "thermal_charge_male_male", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                                            if (!isDead && Game.Player.Character.Position.DistanceTo(offsetPos) < 1f)
                                            {
                                                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to place the thermal charge.");
                                                if (Game.IsControlJustPressed(Control.Context))
                                                {
                                                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), true);
                                                    Function.Call(Hash.SET_PED_USING_ACTION_MODE, Game.Player.Character, false, "DEFAULT_ACTION");
                                                    Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, offsetPos.X, offsetPos.Y, offsetPos.Z, 1f, 20000, offsetRot.Z, 0.1f);
                                                    Index++;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                                    {
                                        Thermite = World.CreateProp("hei_prop_heist_thermite", Game.Player.Character.Position, false, false);
                                        if (Thermite != null && Thermite.Exists())
                                        {
                                            Thermite.IsCollisionEnabled = false;
                                            Thermite.IsVisible = false;
                                        }
                                        PreviousBagType = BagManager.GetBagVariantTypeFromPed(Game.Player.Character);
                                        Bag = BagManager.CreateBagPropFromPed(Game.Player.Character);
                                        if (Bag != null && Bag.Exists())
                                        {
                                            Bag.IsCollisionEnabled = false;
                                            Bag.IsVisible = false;
                                        }
                                        Scene = new SynchronizedScene(Keypad.Position, Keypad.Rotation);
                                        Scene.Generate();
                                        Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@", "thermal_charge_male_male", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                        Scene.PlayEntity(Thermite, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@", "thermal_charge_male_hei_prop_heist_thermite");
                                        Scene.PlayEntity(Bag, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@", "thermal_charge_male_p_m_bag_var22_arm_s");
                                        Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                                        Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Thermite);
                                        Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                                        Index++;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (Thermite != null && Thermite.Exists() && !Thermite.IsVisible && Scene != null && Scene.Phase > 0f)
                                        Thermite.IsVisible = true;

                                    if (Bag != null && Bag.Exists() && !Bag.IsVisible && Scene != null && Scene.Phase > 0f)
                                    {
                                        BagManager.RemoveBag(Game.Player.Character);
                                        Bag.IsVisible = true;
                                    }

                                    if (Scene.IsFinished)
                                    {
                                        if (Bag != null && Bag.Exists())
                                        {
                                            Bag.Delete();
                                            Bag = null;
                                        }
                                        BagManager.SetBagFromVariantType(Game.Player.Character, PreviousBagType);
                                        Scene.Dispose();
                                        Scene = null;
                                        Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), false);
                                        Game.Player.SetControlState(true);
                                        Game.Player.Character.Task.ClearAll();
                                        Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ch_finale");
                                        ThermalBurnPtfxHandle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "scr_ch_finale_thermal_burn", Thermite, 0f, 0f, 0f, 0f, 0f, 0f, 1f, false, false, false);
                                        ThermalBurnPtfxStartedGameTime = Game.GameTime;
                                        CanBeUsed = false;
                                        Index++;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    bool shouldPlayExit = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_intro", 3) || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_loop", 3);
                                    if (Game.Player.Character.Position.DistanceTo(Thermite.Position) < 2f)
                                    {
                                        if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_intro", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_loop", 3))
                                        {
                                            int sequence = 0;
                                            Function.Call(Hash.OPEN_SEQUENCE_TASK, &sequence);
                                            Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_intro", 8f, 8f, -1, 48, 0f, false, false, false);
                                            Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_loop", 8f, 8f, -1, 49, 0f, false, false, false);
                                            Function.Call(Hash.CLOSE_SEQUENCE_TASK, sequence);
                                            Function.Call(Hash.TASK_PERFORM_SEQUENCE, Game.Player.Character, sequence);
                                            Function.Call(Hash.CLEAR_SEQUENCE_TASK, &sequence);
                                        }
                                    }
                                    else
                                    {
                                        if (shouldPlayExit) 
                                            Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_exit", 8f, 8f, -1, 48, 0f, false, false, false);
                                    }

                                    if (Game.GameTime - ThermalBurnPtfxStartedGameTime > 15000)
                                    {
                                        if (shouldPlayExit) Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@heists@ornate_bank@thermal_charge", "cover_eyes_exit", 8f, 8f, -1, 48, 0f, false, false, false);
                                        if (Thermite != null && Thermite.Exists())
                                        {
                                            Thermite.Delete();
                                            Thermite = null;
                                        }
                                        if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ThermalBurnPtfxHandle))
                                        {
                                            Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ThermalBurnPtfxHandle);
                                            ThermalBurnPtfxHandle = 0;
                                        }
                                        ThermalBurnPtfxStartedGameTime = 0;
                                        Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@");
                                        Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@thermal_charge");
                                        Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ch_finale");
                                        OpenNearbyDoor();                                       
                                        Index = 0;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        public void OpenNearbyDoor()
        {
            Prop[] searchDoor = World.GetNearbyProps(Keypad.Position, 5f, new Model("ch_prop_ch_vault_slide_door_lrg"), new Model("ch_prop_ch_vault_slide_door_sm"));

            if (searchDoor.Length > 0)
            {
                foreach (Prop door in searchDoor)
                {
                    door.IsPositionFrozen = false;
                }
            }
        }

        public void Dispose()
        {
            Index = 0;
            PrintsToHack = 3;
            CanBeUsed = true;

            if (Keypad != null && Keypad.Exists())
            {
                Keypad?.Delete();
                Keypad = null;
            }

            if (USB != null && USB.Exists())
            {
                USB.Delete();
                USB = null;
            }

            if (Phone != null && Phone.Exists())
            {
                Phone.Delete();
                Phone = null;
            }

            if (Card != null && Card.Exists())
            {
                Card.Delete();
                Card = null;
            }

            if (Thermite != null && Thermite.Exists())
            {
                Thermite.Delete();
                Thermite = null;
            }

            PreviousBagType = BagManager.BagVariantTypes.Invalid;

            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }

            if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ThermalBurnPtfxHandle))
            {
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ThermalBurnPtfxHandle);
                ThermalBurnPtfxHandle = 0;
            }
            ThermalBurnPtfxStartedGameTime = 0;

            Scene?.Dispose();
            Scene = null;
            Minigame?.Dispose();
            Minigame = null;
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig1_hack_keypad@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig3_cardswipe@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@thermal_charge");
            Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ch_finale");
            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), false);
        }

        #endregion
    }
}