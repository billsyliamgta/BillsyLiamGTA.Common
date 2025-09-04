using System;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.Scaleform
{
    public class Celebration
    {
        #region Fields

        public const string ScriptAudioBank = "HUD_321_GO";

        public const string WallId = "intro";

        public const int ForegroundAlpha = 40;

        public const int BackgroundAlpha = 75;

        public const int Transition = 333;

        #endregion

        #region Properties

        public CelebrationMain Main;

        public CelebrationBackground Background;

        public CelebrationForeground Foreground;

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

        #region Constructor

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
                {
                    throw new TimeoutException($"ERROR: Timed out while loading Celebration script audio bank '{ScriptAudioBank}'.");
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