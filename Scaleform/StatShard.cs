using GTA.Native;
using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform
{
    public class StatShard
    {
        #region Fields

        private const string ScriptAudioBank = "HUD_321_GO";

        private const int BackgroundAlpha = 100;

        #endregion

        #region Properties

        private MpBigMessageFreemode Main;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        #endregion

        #region Constructors

        public StatShard(string title, string description)
        {
            Main = new MpBigMessageFreemode();
            Title = title;
            Description = description;
        }

        #endregion

        #region Functions

        public void LoadAll(int timeout = 2000)
        {
            int start = Game.GameTime;
            while (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, ScriptAudioBank, false, -1))
            {
                if (Game.GameTime - start > timeout)
                    throw new TimeoutException($"ERROR: Timed out while loading StatShard script audio bank '{ScriptAudioBank}'.");
                Script.Wait(0);
            }
            Main.Load();
        }

        public void DisposeAll()
        {
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, ScriptAudioBank);
            Main.Dispose();
        }

        public void Show()
        {
            LoadAll();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "WINNER", "CELEBRATION_SOUNDSET", true);
            Function.Call(Hash.ANIMPOSTFX_PLAY, "SuccessNeutral", 1000, false);
            int start = Game.GameTime;
            Main.CallFunction("SHOW_MISSION_PASSED_MESSAGE", Title, Description, BackgroundAlpha, true, 10, false, 2);
            while (Game.GameTime - start < 1000)
            {
                Main.Render2D();
                Script.Wait(0);
            }
            Main.CallFunction("TRANSITION_UP", 0.15f, true);
            start = Game.GameTime;
            while (Game.GameTime - start < 600)
            {
                Main.Render2D();
                Script.Wait(0);
            }
            Main.CallFunction("ROLL_DOWN_BACKGROUND");
            start = Game.GameTime;
            while (Game.GameTime - start < 14000)
            {
                Main.Render2D();
                Script.Wait(0);
            }
            DisposeAll();
        }

        #endregion
    }
}