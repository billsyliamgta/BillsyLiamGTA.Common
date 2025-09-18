using System.Collections.Generic;
using GTA;
using GTA.Native;
using static BillsyLiamGTA.Common.SHVDN.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendLobbyMenuSliderItem<T> : FrontendLobbyMenuBaseItem
    {
        #region Properties

        public List<T> Items { get; set; }

        public T Value
        {
            get
            {
                if (Items != null)
                {
                    if (Items.Count > 0 && Index >= 0 && Index < Items.Count)
                    {
                        return Items[Index];
                    }
                }

                return default;
            }
        }

        public int Index { get; set; } = 0;

        public FrontendMenuSliderItemValueChangedEventHandler<T> ValueChanged { get; set; }

        #endregion

        #region Constructors

        public FrontendLobbyMenuSliderItem(string title, string description, List<T> list, int startingIndex = 0) : base(title, description)
        {
            Items = list;
            Index = startingIndex;
        }

        #endregion

        #region Functions

        public override void Add(int index)
        {
            base.Add(index);
            CallFunctionFrontend("SET_DATA_SLOT", 0 /* columnId */, index /* uniqueId */, 0, index /* uniqueId */, 0, 0, true, Text, string.Empty, 0, Items[Index].ToString(), 0, false);
        }

        public override void Update(int index)
        {
            base.Update(index);
            CallFunctionFrontend("UPDATE_SLOT", 0 /* columnId */, index /* uniqueId */, 0, index /* uniqueId */, 0, 0, true, Text, string.Empty, 0, Items[Index].ToString(), 0, false);
        }

        public override void Process(int index)
        {
            bool shouldUpdate = false;

            if (Game.IsControlJustPressed(Control.FrontendRight) && Index < Items.Count - 1)
            {
                Index++;
                shouldUpdate = true;
            }

            if (Game.IsControlJustPressed(Control.FrontendLeft) && Index > 0)
            {
                Index--;
                shouldUpdate = true;
            }

            if (shouldUpdate)
            {
                ValueChanged?.Invoke(this, new FrontendMenuSliderItemValueChangedArgs<T>(Value, Index, this));
                Update(index);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        #endregion
    }
}