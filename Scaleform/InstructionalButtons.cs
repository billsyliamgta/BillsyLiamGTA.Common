/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform
{
    public class InstructionalButtons : BaseScaleform
    {
        #region Properties

        public List<Dictionary<Control, string>> Pool { get; set; }

        public bool MouseEnabled { get; set; } = true;

        private bool ShouldUpdate { get; set; } = true;

        #endregion

        #region Constructor

        public InstructionalButtons() : base("INSTRUCTIONAL_BUTTONS")
        {
            Pool = new List<Dictionary<Control, string>>();
        }

        #endregion

        #region Functions

        public void SetPool(List<Dictionary<Control, string>> pool) => Pool = pool;

        public bool AddButton(Dictionary<Control, string> button)
        {
            if (Pool != null)
            {
                if (!Pool.Contains(button))
                {
                    Pool.Add(button);
                    ShouldUpdate = true;
                    return true;
                }
            }

            return false;
        }

        public bool RemoveButton(Dictionary<Control, string> button)
        {
            if (Pool != null)
            {
                if (Pool.Contains(button))
                {
                    Pool.Remove(button);
                    ShouldUpdate = true;
                    return true;
                }
            }

            return false;
        }

        public void Draw()
        {
            if (ShouldUpdate)
            {
                CallFunction("CLEAR_ALL");
                CallFunction("TOGGLE_MOUSE_BUTTONS", MouseEnabled ? 1 : 0);
                // CallFunction("CREATE_CONTAINER");
                if (Pool != null)
                {
                    if (Pool.Count > 0)
                    {
                        for (int i = 0; i < Pool.Count; i++)
                        {
                            var pair = Pool[i].First();
                            if (MouseEnabled)
                            {
                                CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)pair.Key, 0), pair.Value, true, (int)pair.Key);
                            }
                            else
                            {
                                CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)pair.Key, 0), pair.Value);
                            }
                        }
                    }
                }
                CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
                ShouldUpdate = false;
            }
            DrawFullscreen();
        }

        #endregion
    }
}