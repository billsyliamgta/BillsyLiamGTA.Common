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
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform
{
    public class Celebration
    {
        #region Fields

        private const string ScriptAudioBank = "HUD_321_GO";

        private const string WallId = "intro";

        private const int ForegroundAlpha = 40;

        private const int BackgroundAlpha = 75;

        private const int Transition = 333;

        #endregion

        #region Properties

        private CelebrationMain Main;

        private CelebrationBackground Background;

        private CelebrationForeground Foreground;

        public enum Textures
        {
            BLANK = 0,
            SHARD_TEXTURE = 1,
            SHARD_RACE_FLAG = 3,
            VERTICAL_RACE_FLAG = 4,
            TRANSFORM_RACE_FLAG = 5,
            SHARD_GRID = 6,
            VERTICAL_GRID = 7,
            SHARD_TARGET_ASSAULT = 8,
            VERTICAL_TARGET_ASSAULT = 9,
            SHARD_REMIX_1 = 10,
            VERTICAL_REMIX_1 = 11,
            SHARD_REMIX_2 = 12,
            VERTICAL_REMIX_2 = 13,
            SHARD_REMIX_3 = 14,
            VERTICAL_REMIX_3 = 15,
            SHARD_REMIX_4 = 16,
            VERTICAL_REMIX_4 = 17,
            SHARD_REMIX_5 = 18,
            VERTICAL_REMIX_5 = 19,
            SHARD_ARENA_WARS = 20,
            VERTICAL_BANNER_ARENA_WARS = 21
        }

        #endregion

        #region Constructors

        public Celebration()
        {
            Main = new CelebrationMain();
            Background = new CelebrationBackground();
            Foreground = new CelebrationForeground();
        }

        #endregion

        #region Functions

        private void CallFunctionAll(string function, params object[] args)
        {
            Main.CallFunction(function, args);
            Background.CallFunction(function, args);
            Foreground.CallFunction(function, args);
        }

        private void LoadAll(int timeout = 2000)
        {
            int start = Game.GameTime;
            while (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, ScriptAudioBank, false, -1))
            {
                if (Game.GameTime - start > timeout)
                    throw new TimeoutException($"ERROR: Timed out while loading Celebration script audio bank '{ScriptAudioBank}'.");
                Script.Wait(0);
            }
            Main.Load();
            Background.Load();
            Foreground.Load();
        }

        public void DisposeAll()
        {
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, ScriptAudioBank);
            Main.Dispose();
            Background.Dispose();
            Foreground.Dispose();
        }

        private void DrawAll()
        {
            Background.Render2DMasked(Foreground);
            Main.Render2D();
        }

        /// <summary>
        /// Display's the intro with parameters.
        /// </summary>
        /// <param name="mode">The raw text of the mode.</param>
        /// <param name="job">The raw text of the job.</param>
        /// <param name="challenge">The raw text of the challenge.</param>
        /// <param name="duration">How long in seconds, should the intro be displayed for once transitioned.</param>
        /// <param name="colour">The background colour of the intro. E.g. 'HUD_COLOUR_BLACK' </param>
        /// <param name="textColourName">The text colour of the intro. E.g. 'HUD_COLOUR_PURE_WHITE' </param>
        public void ShowIntro(string mode, string job, string challenge, int duration = 2, string colour = "HUD_COLOUR_BLACK", string textColourName = "HUD_COLOUR_PURE_WHITE", Textures texture = Textures.BLANK)
        {
            LoadAll();
            CallFunctionAll("CLEANUP", WallId);
            CallFunctionAll("CREATE_STAT_WALL", WallId, colour, 40);
            CallFunctionAll("SET_PAUSE_DURATION", duration);
            CallFunctionAll("ADD_INTRO_TO_WALL", WallId, mode, job, true, challenge, string.Empty, challenge, duration, string.Empty, true, textColourName);
            CallFunctionAll("ADD_BACKGROUND_TO_WALL", WallId, 75, (int)texture);
            CallFunctionAll("SHOW_STAT_WALL", WallId);
            int start = Game.GameTime;
            while (Game.GameTime - start < (Transition * 2) + (duration * 1000))
            {
                Game.DisableControlThisFrame(Control.SkipCutscene);
                Game.DisableControlThisFrame(Control.FrontendPause);
                Game.DisableControlThisFrame(Control.FrontendPauseAlternate);
                Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                DrawAll();
                Script.Wait(0);
            }
            DisposeAll();
        }

        #endregion
    }
}