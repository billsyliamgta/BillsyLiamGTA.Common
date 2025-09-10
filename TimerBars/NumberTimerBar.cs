using System.Drawing;
using static BillsyLiamGTA.Common.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.Graphics.TimerBars
{
    public class NumberTimerBar : BaseTimerBar
    {
        #region Properties

        public int Value { get; set; } = 0;

        public bool Dollars { get; set; } = false;

        public Color NumberColor { get; set; } = Color.White;

        #endregion

        #region Constructor

        public NumberTimerBar(string text, bool dollars) : base(text, false)
        {
            Dollars = dollars;
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += TextOffset;
            DrawInteger(Value, InitialX, y + 0.001f, 0, TextScale, NumberColor, 2, TextWrap, false, false, Dollars);
        }

        #endregion
    }
}