/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Player
{
    /// <summary>
    /// A enum containing the different types of player switching.
    /// </summary>
    public enum PlayerSwitchTypes
    {
        SWITCH_TYPE_AUTO = 0,
        SWITCH_TYPE_LONG = 1,
        SWITCH_TYPE_MEDIUM = 2,
        SWITCH_TYPE_SHORT = 3
    };

    /// <summary>
    /// A class for managing player switching.
    /// </summary>
    public class PlayerSwitch
    {
        #region Properties

        /// <summary>
        /// Gets the state of player switch currently in progress.
        /// </summary>
        public static int State => Function.Call<int>(Hash.GET_PLAYER_SWITCH_STATE);

        /// <summary>
        /// Gets the type of player switch currently in progress.
        /// </summary>
        public static int Type => Function.Call<int>(Hash.GET_PLAYER_SWITCH_TYPE);

        /// <summary>
        /// Whether or not player switch is in progress.
        /// </summary>
        public static bool IsInProgress => Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS);

        /// <summary>
        /// A enum containing the different flags for player switching.
        /// </summary>
        public enum SwitchFlags
        {
            SKIP_INTRO = 1,
            SKIP_OUTRO = 2,
            PAUSE_BEFORE_PAN = 4,
            PAUSE_BEFORE_OUTRO = 8,
            SKIP_PAN = 16,
            UNKNOWN_DEST = 32,
            DESCENT_ONLY = 64,
            START_FROM_CAMPOS = 128,
            PAUSE_BEFORE_ASCENT = 256,
            PAUSE_BEFORE_DESCENT = 512,
            ALLOW_SNIPER_AIM_INTRO = 1024,
            ALLOW_SNIPER_AIM_OUTRO = 2048,
            SKIP_TOP_DESCENT = 4096,
            SUPPRESS_OUTRO_FX = 8192,
            SUPPRESS_INTRO_FX = 16384,
            DELAY_ASCENT_FX = 32768
        }

        #endregion

        #region Functions

        /// <summary>
        /// Switches out the player. Previously known as '_SWITCH_OUT_PLAYER'.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="flags"></param>
        /// <param name="switchType"></param>
        public static void SwitchToMultiFirstPart(GTA.Ped ped, int flags, int switchType) => Function.Call(Hash.SWITCH_TO_MULTI_FIRSTPART, ped.Handle, flags, switchType);

        /// <summary>
        /// Switches in the player. Previously known as '_SWITCH_IN_PLAYER'.
        /// </summary>
        /// <param name="ped"></param>
        public static void SwitchToMultiSecondPart(GTA.Ped ped) => Function.Call(Hash.SWITCH_TO_MULTI_SECONDPART, ped.Handle);

        /// <summary>
        /// Completes a player switch from one ped to another.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="flags"></param>
        /// <param name="switchType"></param>
        public static void Start(GTA.Ped from, GTA.Ped to, SwitchFlags flags, PlayerSwitchTypes switchType) => Function.Call(Hash.START_PLAYER_SWITCH, from.Handle, to.Handle, (int)flags, (int)switchType);

        /// <summary>
        /// Stop's a player switch if one is in progress.
        /// </summary>
        public static void Stop() => Function.Call(Hash.STOP_PLAYER_SWITCH);

        #endregion
    }
}