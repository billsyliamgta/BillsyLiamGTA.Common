using System.Drawing;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars
{
    public static class TimerBarHelpers
    {
        #region Fields

        public const float GfxAlignWidth = 0.952f;

        public const float GfxAlignHeight = 0.949f;

        public const float InitialX = 0.795f;

        public const float InitialY = 0.925f;

        public const float InitialBusySpinnerY = 0.887f;

        public const float BgBaseX = 0.874f;

        public const float ProgressBaseX = 0.913f;

        public const float CheckpointBaseX = 0.9445f;

        public const float BgOffset = 0.008f;

        public const float BgThinOffset = 0.012f;

        public const float TextOffset = -0.0113f;

        public const float PlayerTitleOffset = -0.005f;

        public const float BarOffset = 0.012f;

        public const float CheckpointOffsetX = 0.0094f;

        public const float CheckpointOffsetY = 0.012f;

        public const float TimerBarWidth = 0.165f;

        public const float TimerBarHeight = 0.035f;

        public const float TimerBarThinHeight = 0.028f;

        public const float TimerBarMargin = 0.0399f;

        public const float TimerBarThinMargin = 0.0319f;

        public const float ProgressWidth = 0.069f;

        public const float ProgressHeight = 0.011f;

        public const float CheckpointWidth = 0.012f;

        public const float CheckpointHeight = 0.023f;

        public const float TitleScale = 0.202f;

        public const float TitleWrap = 0.867f;

        public const float TextScale = 0.494f;

        public const float TextWrap = 0.95f;

        public const float PlayerTitleScale = 0.447f;

        #endregion

        #region Functions

        public static void DrawText(string text, float x, float y, int font, float scale, Color colour, int alpha, int justification, float wrap, bool shadow = false, bool outline = false)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, justification);
            Function.Call(Hash.SET_TEXT_WRAP, 0, wrap);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_SCALE, 0, scale);
            Function.Call(Hash.SET_TEXT_COLOUR, colour.R, colour.G, colour.B, alpha);
            if (outline) Function.Call(Hash.SET_TEXT_OUTLINE);
            if (shadow) Function.Call(Hash.SET_TEXT_DROP_SHADOW);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
        }

        public static void DrawInteger(int value, float x, float y, int font, float scale, Color colour, int justification, float wrap, bool shadow = false, bool outline = false, bool dollar = false)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, dollar ? "ESDOLLA" : "NUMBER");
            Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, value);
            if (dollar) Function.Call(Hash.ADD_TEXT_COMPONENT_FORMATTED_INTEGER, value, 1);
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, justification);
            Function.Call(Hash.SET_TEXT_WRAP, 0, wrap);
            Function.Call(Hash.SET_TEXT_FONT, font);
            Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
            Function.Call(Hash.SET_TEXT_COLOUR, colour.R, colour.G, colour.B, colour.A);
            if (outline) Function.Call(Hash.SET_TEXT_OUTLINE);
            if (shadow) Function.Call(Hash.SET_TEXT_DROP_SHADOW);         
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
        }

        #endregion
    }
}