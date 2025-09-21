/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA.UI;
using static BillsyLiamGTA.Common.SHVDN.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendLobbyMenuItem : FrontendLobbyMenuBaseItem
    {
        #region Properties

        public HudColor Color { get; set; } = HudColor.Invalid;

        #endregion

        #region Constructor

        public FrontendLobbyMenuItem(string text, string description) : base(text, description)
        {
        }

        #endregion

        #region Functions

        public override void Add(int index)
        {
            base.Add(index);
            CallFunctionFrontend("SET_DATA_SLOT", 0 /* columnId */, index /* uniqueId */, 0, index /* uniqueId */, Color == HudColor.Invalid ? 1 : 2, 0, true, Text, false, -1, 0, (int)Color, false);
        }

        #endregion
    }
}