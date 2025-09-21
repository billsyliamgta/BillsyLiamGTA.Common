/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA;

namespace BillsyLiamGTA.Common.SHVDN.Elements
{
    public class VariableTimer
    {
        #region Properties

        public delegate void TimerExpired(object sender);

        private int TimerMax;

        private decimal TimerCounter;

        private bool IsRunning;

        public bool AutoReset;

        public int Counter => (int)TimerCounter;

        public event TimerExpired OnTimerExpired;

        #endregion

        #region Constructors

        public VariableTimer(int interval)
        {
            TimerCounter = interval;
            TimerMax = interval;
        }

        #endregion

        #region Functions

        public void AddTime(decimal amount)
        {
            TimerCounter += amount;
        }

        public void RemoveTime(decimal amount)
        {
            TimerCounter -= amount;
            if (TimerCounter < 0m)
            {
                TimerCounter = default(decimal);
            }
        }

        public void Update(float timescale)
        {
            if (!IsRunning)
            {
                return;
            }
            float num = Game.LastFrameTime * 1000f;
            TimerCounter -= (decimal)(num * timescale);
            if (TimerCounter <= 0m)
            {
                OnTimerExpired?.Invoke(this);
                if (AutoReset)
                {
                    TimerCounter += (decimal)TimerMax;
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Reset()
        {
            TimerCounter = TimerMax;
        }

        #endregion
    }
}