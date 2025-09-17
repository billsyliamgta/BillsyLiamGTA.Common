using System.Drawing;
using GTA.Native;
using BillsyLiamGTA.Common.Elements;
using static BillsyLiamGTA.Common.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.Graphics.TimerBars
{
    public class ProgressTimerBar : BaseTimerBar
    {
        #region Properties

        private float _progress = 0;

        public float Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = BaseMission.Clamp(value, 0f, 1f);
            }
        }

        public Color FgColour = Color.FromArgb(255, 140, 140, 140);

        public Color Colour = Color.FromArgb(255, 240, 240, 240);

        #endregion

        public ProgressTimerBar(string text, float progress = 0f) : base(text, true)
        {
            Progress = progress;
        }

        public override void Draw(float y)
        {
            base.Draw(y);
            y += BarOffset;
            Function.Call(Hash.DRAW_RECT, ProgressBaseX, y, ProgressWidth, ProgressHeight, FgColour.R, FgColour.G, FgColour.B, FgColour.A, false);
            float fgWidth = ProgressWidth * Progress;
            float fgX = (ProgressBaseX - ProgressWidth * 0.5f) + (fgWidth * 0.5f);
            Function.Call(Hash.DRAW_RECT, fgX, y, fgWidth, ProgressHeight, Colour.R, Colour.G, Colour.B, Colour.A, false);
        }
    }
}