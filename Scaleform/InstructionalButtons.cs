using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.Scaleform
{
    public class InstructionalButtons : BaseScaleform
    {
        #region Properties

        public List<Dictionary<Control, string>> Pool { get; set; }

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
                    return true;
                }
            }

            return false;
        }

        public void Draw()
        {
            CallFunction("CLEAR_ALL");
            CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
            CallFunction("CREATE_CONTAINER");
            if (Pool != null)
            {
                if (Pool.Count > 0)
                {
                    for (int i = 0; i < Pool.Count; i++)
                    {
                        var pair = Pool[i].First();
                        CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)pair.Key, 0), pair.Value); 
                    }
                }
            }
            CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
            DrawFullscreen();
        }

        #endregion
    }
}