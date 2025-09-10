using GTA.UI;
using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.Scaleform.Frontend
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