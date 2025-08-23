using System;
using GTA;
using BillsyLiamGTA.Common.Elements;
using static BillsyLiamGTA.Common.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.Graphics.TimerBars
{
    public class CountdownTimerBar : BaseTimerBar
    {
        #region Properties

        public VariableTimer VariableTimer { get; set; }

        #endregion

        #region Constructor

        public CountdownTimerBar(string text, int interval) : base(text, false)
        {
            VariableTimer = new VariableTimer(interval);
            VariableTimer.Start();
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += TextOffset;
            VariableTimer.Update(Game.TimeScale);
            var time = TimeSpan.FromMilliseconds(VariableTimer.Counter);
            VariableTimer.OnTimerExpired += (sender) =>
            {
                TimerBarPool.Remove(this);
            };
            DrawText(time.ToString(@"mm\:ss"), InitialX, y + 0.001f, 0, TextScale, TitleColour, TitleColour.A, 2, TextWrap);
        }

        #endregion
    }
}