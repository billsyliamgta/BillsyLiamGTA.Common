using System.Drawing;
using GTA.Native;
using static BillsyLiamGTA.Common.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.Graphics.TimerBars
{
    public abstract class BaseTimerBar
    {
        #region Properties

        public string Title { get; set; }

        public bool Thin { get; set; } = false;

        public bool IsFlashing { get; set; } = false;

        public int FlashInterval { get; set; } = 500;

        public Color TitleColour { get; set; } = Color.White;

        public Color OverlayColour { get; set; }

        #endregion

        #region Constructor

        public BaseTimerBar(string title, bool thin)
        {
            Title = title;
            Thin = thin;
        }

        #endregion

        #region Functions

        public virtual void Draw(float y)
        {
            y += Thin ? BgThinOffset : BgOffset;
            Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_black_bg", BgBaseX, y, TimerBarWidth, Thin ? TimerBarThinHeight : TimerBarHeight, 0, 255, 255, 255, 140, false, 0);
            if (OverlayColour != null)
            {
                Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_white_bg", BgBaseX, y, TimerBarWidth, Thin ? TimerBarThinHeight : TimerBarHeight, 0, OverlayColour.R, OverlayColour.G, OverlayColour.B, 140, false, 0);
            }
            DrawText(Title, InitialX, y - 0.011f, 0, TitleScale + 0.1f, TitleColour, TitleColour.A, 2, TitleWrap);
        }

        #endregion
    }
}