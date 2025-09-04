using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Security;

namespace BillsyLiamGTA.Common.Scaleform
{
    public class HeistCelebration
    {
        #region Fields

        public const string ScriptAudioBank = "HUD_321_GO";

        public const string WallId = "CELEB_HEIST";

        public const string StepId = "CELEB_PSCORE";

        public const int BackgroundAlpha = 75;

        public const int Transition = 333;

        #endregion

        #region Properties

        public HeistCelebrationMain Main;

        public HeistCelebrationBackground Background;

        public HeistCelebrationForeground Foreground;

        public struct BigDollarsStep
        {
            public int Value;

            public string TopText;

            public string BottomText;

            public BigDollarsStep(int value, string topText, string bottomText)
            {
                Value = value;
                TopText = topText;
                BottomText = bottomText;
            }
        }

        #endregion

        #region Constructor

        public HeistCelebration()
        {
            Main = new HeistCelebrationMain();
            Background = new HeistCelebrationBackground();
            Foreground = new HeistCelebrationForeground();
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
                {
                    throw new TimeoutException($"ERROR: Timed out while loading Heist Celebration script audio bank '{ScriptAudioBank}'.");
                }
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
            Background.DrawFullscreenMasked(Foreground);
            Main.DrawFullscreen();
        }

        public void ShowResult(string missionText, string completeText, string messageText, int duration = 5, List<BigDollarsStep> bigDollarsSteps = null)
        {
            LoadAll();
            CallFunctionAll("CLEANUP", WallId);
            CallFunctionAll("CREATE_STAT_WALL", WallId, 1 /* sfxId*/);
            CallFunctionAll("SET_PAUSE_DURATION", duration);
            CallFunctionAll("ADD_MISSION_RESULT_TO_WALL", WallId, missionText, completeText, messageText, true, true, true);
            CallFunctionAll("CREATE_STAT_TABLE", WallId, StepId);
            CallFunctionAll("ADD_STAT_TABLE_TO_WALL", WallId, StepId);

            #region Big Dollars Step

            int totalBigDollarSteps = 0;
            if (bigDollarsSteps?.Count > 0)
            {
                CallFunctionAll("CREATE_INCREMENTAL_CASH_ANIMATION", WallId, "SUMMARY");
                for (totalBigDollarSteps = 0; totalBigDollarSteps < bigDollarsSteps.Count; totalBigDollarSteps++)
                {
                    int previousValue = 0;
                    if (totalBigDollarSteps > 0)
                    {
                        previousValue = bigDollarsSteps[totalBigDollarSteps - 1].Value;
                    }
                    CallFunctionAll("ADD_INCREMENTAL_CASH_WON_STEP", WallId, "SUMMARY", previousValue, bigDollarsSteps[totalBigDollarSteps].Value, bigDollarsSteps[totalBigDollarSteps].TopText, bigDollarsSteps[totalBigDollarSteps].BottomText, "", "", 7);
                }
                CallFunctionAll("ADD_INCREMENTAL_CASH_ANIMATION_TO_WAL", WallId, "SUMMARY");
            }

            #endregion

            CallFunctionAll("ADD_BACKGROUND_TO_WALL", WallId, BackgroundAlpha, 1);
            CallFunctionAll("SHOW_STAT_WALL", WallId);
            int start = Game.GameTime;
            while (Game.GameTime - start < (Transition * 2) + (duration * 1000) + (totalBigDollarSteps * 500))
            {
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