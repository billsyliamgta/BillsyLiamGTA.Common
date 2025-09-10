using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;

namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public class FrontendLobbyMenuItem : FrontendLobbyMenuBaseItem
    {
        #region Constructor

        public FrontendLobbyMenuItem(string text, string description) : base(text, description)
        {
        }

        #endregion

        #region Functions

        public override void Add(int index)
        {
            base.Add(index);
            CallFunctionFrontend("SET_DATA_SLOT", 0 /* columnId */, index /* uniqueId */, 0, index /* uniqueId */, 1, 0, true, Text, false, -1, 0, 0, false);
        }

        #endregion
    }
}