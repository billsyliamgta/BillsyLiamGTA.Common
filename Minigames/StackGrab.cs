using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.SHVDN.Ped;
using BillsyLiamGTA.Common.SHVDN.Audio;
using static BillsyLiamGTA.Common.SHVDN.Minigames.TrollyGrab;

/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
namespace BillsyLiamGTA.Common.SHVDN.Minigames
{
    public class StackGrab : BaseMinigame
    {
        #region Properties

        private Prop Stack;

        /// <summary>
        /// A enum containing the different types of stack grabs.
        /// </summary>
        public enum StackGrabTypes
        {
            Invalid = -1,
            Cash,
            Gold,
            Coke
        }

        public StackGrabTypes Type { get; protected set; } = StackGrabTypes.Invalid;

        private Prop Bag;

        private SynchronizedScene PlayerScene;

        private SynchronizedScene StackScene;

        #endregion

        #region Constructors

        public StackGrab(Prop stack, StackGrabTypes type)
        {
            Stack = stack;
            Type = type;
            MaxRate = (type == StackGrabTypes.Gold ? 1.2f : 1.5f);
        }

        #endregion

        #region Functions

        public static StackGrab CreateStack(StackGrabTypes type, Vector3 position, Vector3 rotation)
        {
            Prop stack = World.CreateProp(GetModelName(type), position, rotation, false, false);
            if (stack != null && stack.Exists())
                stack.IsPositionFrozen = true;
            return new StackGrab(stack, type);
        }

        /// <summary>
        /// Sets <see cref="BaseMinigame.EnableCasinoBlackjackCamera"/> to false and plays the exit sequence.
        /// </summary>
        private void PlayExitSequence()
        {
            EnableCasinoBlackjackCamera = false;
            string animDict = GetAnimDict();
            PlayerScene = new SynchronizedScene(Stack);
            PlayerScene.Generate();
            PlayerScene.PlayPed(Game.Player.Character, animDict, "exit", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
            PlayerScene.PlayEntity(Bag, animDict, "exit_bag", 1f, -1f);
            StackScene.Rate = 0f;
            Index = 4;
        }

        /// <summary>
        /// Gets the appropriate GXT entry for the help text based on if the player is grabbing or not.
        /// </summary>
        /// <param name="isGrabbing"></param>
        /// <returns></returns>
        private string GetHelpTextGXTEntry(bool isGrabbing)
        {
            if (isGrabbing)
            {
                if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/))
                {
                    switch (Type)
                    {
                        case StackGrabTypes.Cash:
                            return "MC_GRAB_3_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the cash. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the cash. */
                        case StackGrabTypes.Coke:
                            return "MC_GRAB_5_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the drugs. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the drugs. */
                        case StackGrabTypes.Gold:
                            return "MC_GRAB_4B"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the gold. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the gold. */
                    }
                }
                else
                {
                    switch (Type)
                    {
                        case StackGrabTypes.Cash:
                            return "MC_GRAB_3"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the cash. */
                        case StackGrabTypes.Coke:
                            return "MC_GRAB_5"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the drugs. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the drugs. */
                        case StackGrabTypes.Gold:
                            return "MC_GRAB_4"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the gold. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the gold. */
                    }
                }
            }
            else
            {
                switch (Type)
                {
                    case StackGrabTypes.Cash:
                        return "MC_GRAB_1"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the cash. */
                    case StackGrabTypes.Coke:
                        return "MC_GRAB_6"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the drugs. */
                    case StackGrabTypes.Gold:
                        return "MC_GRAB_2"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the gold. */
                }
            }

            return "INVALID";
        }

        /// <summary>
        /// Gets the appropriate model name for the <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetModelName(StackGrabTypes type)
        {
            switch (type)
            {
                case StackGrabTypes.Cash:
                    return "h4_prop_h4_cash_stack_01a";
                case StackGrabTypes.Gold:
                    return "h4_prop_h4_gold_stack_01a";
                case StackGrabTypes.Coke:
                    return "h4_prop_h4_coke_stack_01a";
            }

            return "INVALID";
        }

        /// <summary>
        /// Gets the appropriate anim dict for the <see cref="Type"/>
        /// </summary>
        /// <returns></returns>
        private string GetAnimDict()
        {
            switch (Type)
            {
                case StackGrabTypes.Cash:
                    return "anim@scripted@heist@ig1_table_grab@cash@male@";
                case StackGrabTypes.Gold:
                case StackGrabTypes.Coke:
                    return "anim@scripted@heist@ig1_table_grab@gold@male@";
            }

            return "INVALID";
        }

        /// <summary>
        /// A override method that updates the minigame.
        /// </summary>
        public override void Update()
        {
            base.Update();
            string animDict = GetAnimDict();
            if (EnableCasinoBlackjackCamera)
                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, GetHelpTextGXTEntry(true), true);
            switch (Index)
            {
                case 0:
                    {
                        if (IsLooted)
                            return;
                        Function.Call(Hash.REQUEST_ANIM_DICT, animDict);
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "MC_PLAY", 0);
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict) && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "MC_PLAY", 0) && Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3) && Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", false, -1))
                        {
                            Vector3 offsetPos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, animDict, "enter", Stack.Position.X, Stack.Position.Y, Stack.Position.Z, Stack.Rotation.X, Stack.Rotation.Y, Stack.Rotation.Z, 0f, 2);
                            Vector3 offsetRot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, animDict, "enter", Stack.Position.X, Stack.Position.Y, Stack.Position.Z, Stack.Rotation.X, Stack.Rotation.Y, Stack.Rotation.Z, 0f, 2);
                            if (Game.Player.Character.Position.DistanceTo(offsetPos) < 1f)
                            {
                                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, GetHelpTextGXTEntry(false), true);
                                if (Game.IsControlJustPressed(Control.Context))
                                {
                                    Screen.ClearHelpText();
                                    Game.Player.Character.CanSwitchWeapons = false;
                                    Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                                    SetInProgress(true);
                                    AudioScene.Start("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                                    PreviousBagType = BagManager.GetBagVariantTypeFromPed(Game.Player.Character);
                                    Bag = BagManager.CreateBagPropFromPed(Game.Player.Character);
                                    if (Bag != null && Bag.Exists())
                                    {
                                        Bag.IsCollisionEnabled = false;
                                        Bag.IsVisible = false;
                                    }
                                    EnableCasinoBlackjackCamera = true;
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
                            PlayerScene = new SynchronizedScene(Stack);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, animDict, "enter", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, animDict, "enter_bag", 1000f, -8f);
                            Function.Call(Hash.PLAY_FACIAL_ANIM, Game.Player.Character, "enter_facial", animDict);
                            StackScene = new SynchronizedScene(Stack);
                            StackScene.Generate();
                            StackScene.PlayEntity(Stack, animDict, Type == StackGrabTypes.Gold ? "grab_gold" : "grab_cash", 1000f, -4f, SynchronizedScene.PlaybackFlags.USE_PHYSICS, 0);
                            StackScene.Rate = 0f;
                            StackScene.Phase = Data.Phase;
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

                        if (PlayerScene != null && PlayerScene.IsFinished)
                            Index++;
                    }
                    break;
                case 3:
                    {
                        if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, animDict, "grab", 3))
                        {
                            PlayerScene.Rate = Data.f_14;
                            StackScene.Rate = Data.f_14;
                            Data.Phase = PlayerScene.Phase;

                            if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "LOOTED")))
                            {
                                int min = 0, max = 0;
                                switch (Type)
                                {
                                    case StackGrabTypes.Cash:
                                        min = 10000;
                                        max = 15000;
                                        break;

                                    case StackGrabTypes.Coke:
                                        min = 25000;
                                        max = 30000;
                                        break;

                                    case StackGrabTypes.Gold:
                                        min = 40000;
                                        max = 45000;
                                        break;
                                }
                                ValueAdded?.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max), true));
                                if (Data.f_14 == 0.75f)
                                {
                                    PlayerScene.Generate();
                                    PlayerScene.PlayPed(Game.Player.Character, animDict, "grab_idle", 2f, -8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                    PlayerScene.PlayEntity(Bag, animDict, "grab_idle_bag", 2f, -4f);
                                    PlayerScene.SetLooped(true);
                                    StackScene.Rate = 0f;
                                }
                            }

                            if (Data.Phase > 0.999f)
                            {
                                IsLooted = true;
                                PlayExitSequence();
                            }
                        }
                        else if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, animDict, "grab_idle", 3))
                        {
                            if (Data.f_14 > 0.75f)
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, animDict, "grab", 4f, -4f, SynchronizedScene.PlaybackFlags.UNKNOWN, SynchronizedScene.RagdollBlockingFlags.UNKNOWN, 1000f, SynchronizedScene.IkControlFlags.LINKED_FACIAL);
                                PlayerScene.PlayEntity(Bag, animDict, "grab_bag", 2f, -8f);
                                PlayerScene.Phase = Data.Phase;
                            }

                            if (Game.IsControlJustPressed(Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/) ? Control.CursorCancel : Control.FrontendCancel))
                                PlayExitSequence();
                        }
                        else
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, animDict, "grab_idle", 2f, -8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, animDict, "grab_idle_bag", 2f, -4f);
                            PlayerScene.SetLooped(true);
                        }
                    }
                    break;
                case 4:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished)
                        {
                            if (Bag != null && Bag.Exists())
                            {
                                Bag?.Delete();
                                Bag = null;
                            }
                            BagManager.SetBagFromVariantType(Game.Player.Character, PreviousBagType);
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            StackScene?.Dispose();
                            StackScene = null;
                            Game.Player.Character.CanSwitchWeapons = true;
                            SetInProgress(false);
                            AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01");
                            Function.Call(Hash.REMOVE_ANIM_DICT, animDict);
                            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
                            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
                            Game.Player.SetControlState(true);
                            Game.Player.Character.Task.ClearAll();
                            Index = 0;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// A override method that is triggered if the player dies during the minigame.
        /// </summary>
        public override void PushDeathResetFunction()
        {
            base.PushDeathResetFunction();
            EnableCasinoBlackjackCamera = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            PlayerScene?.Dispose();
            PlayerScene = null;
            StackScene?.Dispose();
            StackScene = null;
            if (AudioScene.IsActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE"))
                AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01");
            Function.Call(Hash.REMOVE_ANIM_DICT, GetAnimDict());
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 0))
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3))
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        /// <summary>
        /// Disposes the minigame. It's recommend to use this function in case the script is aborted.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (Stack != null && Stack.Exists())
            {
                Stack.Delete();
                Stack = null;
            }
            EnableCasinoBlackjackCamera = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            PlayerScene?.Dispose();
            PlayerScene = null;
            StackScene?.Dispose();
            StackScene = null;
            if (AudioScene.IsActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE"))
                AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01");
            Function.Call(Hash.REMOVE_ANIM_DICT, GetAnimDict());
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 0))
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3))
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        #endregion
    }
}