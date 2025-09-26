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
using GTA.Native;
using BillsyLiamGTA.Common.SHVDN.Elements;
using static BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars
{
    public class ProgressTimerBar : BaseTimerBar
    {
        #region Properties

        private float _progress = 0;

        public float Progress
        {
            get => _progress;
            set => _progress = Extensions.Clamp(value, 0f, 1f);
        }

        public Color ForegroundColour = Color.FromArgb(155, 240, 240, 240);

        public Color Colour = Color.FromArgb(255, 240, 240, 240);

        #endregion

        #region Contructors

        public ProgressTimerBar(string text, float progress = 0f) : base(text, true)
        {
            Progress = progress;
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += BarOffset;
            Function.Call(Hash.DRAW_RECT, ProgressBaseX, y, ProgressWidth, ProgressHeight, ForegroundColour.R, ForegroundColour.G, ForegroundColour.B, ForegroundColour.A, false);
            float fgWidth = ProgressWidth * Progress;
            float fgX = (ProgressBaseX - ProgressWidth * 0.5f) + (fgWidth * 0.5f);
            Function.Call(Hash.DRAW_RECT, fgX, y, fgWidth, ProgressHeight, Colour.R, Colour.G, Colour.B, Colour.A, false);
        }

        #endregion
    }
}