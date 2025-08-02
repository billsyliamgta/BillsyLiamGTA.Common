using System;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.Graphics.Scaleform
{
    public class Celebration
    { 
        #region Fields

        private const string audioBank = "HUD_321_GO";

        private const string bgColourId = "HUD_COLOUR_BLACK";

        private const string textColourName = "HUD_COLOUR_WHITE";

        private const string wallId = "intro";

        private const int transitionIn = 333;

        private const int transitionOut = 333;

        private const int bgAlpha = 75;

        private const int fgAlpha = 40;

        #endregion

        #region Properties

        public Scaleform Main;

        public Scaleform Background;

        public Scaleform Foreground;

        public string BackgroundColour { get; set; } = string.Empty;

        public string Mode { get; set; } = string.Empty;

        public string Job { get; set; } = string.Empty;

        public string Challenge { get; set; } = string.Empty;

        public int Duration { get; set; } = 2;

        #endregion

        #region Constructor

        public Celebration()
        {
            Main = new Scaleform("MP_CELEBRATION");
            Background = new Scaleform("MP_CELEBRATION_BG");
            Foreground = new Scaleform("MP_CELEBRATION_FG");
        }

        #endregion

        #region Functions

        public void CallFunctionAll(string function, params object[] args)
        {
            Main.CallFunction(function, args);
            Background.CallFunction(function, args);
            Foreground.CallFunction(function, args);
        }

        public void LoadAll(int timeout = 2000)
        {
            int start = Game.GameTime;
            while (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, audioBank, -1, false))
            {
                if (Game.GameTime - start > timeout)
                {
                    throw new TimeoutException($"Script audio bank '{audioBank}' failed to load within {timeout}ms.");
                }
                Script.Wait(0);
            }
            Main.Load(timeout);
            Background.Load(timeout);
            Foreground.Load(timeout);
        }

        public void DisposeAll()
        {
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, audioBank);
            Main.Dispose();
            Background.Dispose();
            Foreground.Dispose();
        }

        public void DrawAll()
        {
            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
            Background.DrawFullscreenMasked(Foreground);
            Main.DrawFullscreen();
        }

        public void Show()
        {
            LoadAll();
            CallFunctionAll("CLEANUP", wallId);
            CallFunctionAll("CREATE_STAT_WALL", wallId, string.IsNullOrEmpty(BackgroundColour) ? bgColourId : BackgroundColour, fgAlpha);
            CallFunctionAll("SET_PAUSE_DURATION", Duration);
            CallFunctionAll("ADD_INTRO_TO_WALL", wallId, Mode, Job, true, Challenge, string.Empty, Challenge, Duration, string.Empty, true, textColourName);
            CallFunctionAll("ADD_BACKGROUND_TO_WALL", wallId, bgAlpha, 0);
            CallFunctionAll("SHOW_STAT_WALL", wallId);
            int start = Game.GameTime;
            while (Game.GameTime - start < transitionIn + transitionOut + (Duration * 1000))
            {
                DrawAll();
                Script.Wait(0);
            }
            DisposeAll();
        }

        #endregion
    }
}