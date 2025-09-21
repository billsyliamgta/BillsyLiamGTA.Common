/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System;
using GTA;
using BillsyLiamGTA.Common.SHVDN.Elements;
using static BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars
{
    public class CountdownTimerBar : BaseTimerBar
    {
        #region Properties

        public VariableTimer VariableTimer { get; set; }

        #endregion

        #region Constructors

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