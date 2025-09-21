/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System.Drawing;
using static BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars
{
    public class NumberTimerBar : BaseTimerBar
    {
        #region Properties

        public int Value { get; set; } = 0;

        public bool Dollars { get; set; } = false;

        public Color NumberColor { get; set; } = Color.White;

        #endregion

        #region Constructors

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