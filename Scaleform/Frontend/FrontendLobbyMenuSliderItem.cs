using System.Collections.Generic;
using GTA;
using GTA.Native;
using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public class FrontendLobbyMenuSliderItem<T> : FrontendLobbyMenuBaseItem
    {
        #region Properties

        public List<T> Items { get; set; }

        public int CurrentSelection { get; set; } = 0;

        #endregion

        #region Constructors

        public FrontendLobbyMenuSliderItem(string title, string description, List<T> list, int startingIndex = 0) : base(title, description)
        {
            Items = list;
            CurrentSelection = startingIndex;
        }

        #endregion

        #region Functions

        public override void Add(int index)
        {
            base.Add(index);
            CallFunctionFrontend("SET_DATA_SLOT", 0 /* columnId */, index /* uniqueId */, 0, index /* uniqueId */, 0, 0, true, Text, "", 0, Items[CurrentSelection].ToString(), 0, false);
        }

        public override void Update(int index)
        {
            base.Update(index);
            bool shouldUpdate = false;

            if (Game.IsControlJustPressed(Control.FrontendRight) && CurrentSelection < Items.Count - 1)
            {
                CurrentSelection++;
                shouldUpdate = true;
            }

            if (Game.IsControlJustPressed(Control.FrontendLeft) && CurrentSelection > 0)
            {
                CurrentSelection--;
                shouldUpdate = true;
            }

            if (shouldUpdate)
            {
                CallFunctionFrontend("UPDATE_SLOT", 0, index /* uniqueId */, 0, index /* uniqueId */, 0, 0, true, Text, "", 0, Items[CurrentSelection].ToString(), 0, false);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        #endregion
    }
}