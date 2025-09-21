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
    public abstract class FrontendLobbyMenuBaseItem
    {
        #region Properties

        private string _text = string.Empty;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                PromptUpdate = true;
            }
        }

        private string _description = string.Empty;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                PromptUpdate = true;
            }
        }

        public bool PromptUpdate { get; set; }

        public FrontendMenuItemActivatedEventHandler Activated { get; set; }

        #endregion

        #region Constructor

        public FrontendLobbyMenuBaseItem(string text, string description)
        {
            Text = text;
            Description = description;
        }

        #endregion

        #region Functions

        public virtual void Add(int index)
        {
        }

        public virtual void Update(int index)
        {
        }

        public virtual void Process(int index)
        {
        }

        #endregion
    }
}