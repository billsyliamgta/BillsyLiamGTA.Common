/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendLobbyMenuClosedArgs
    {
        #region Properties

        public FrontendLobbyMenu Menu { get; }

        #endregion

        #region Constructors

        internal FrontendLobbyMenuClosedArgs(FrontendLobbyMenu menu)
        {
            Menu = menu;
        }

        #endregion
    }
}