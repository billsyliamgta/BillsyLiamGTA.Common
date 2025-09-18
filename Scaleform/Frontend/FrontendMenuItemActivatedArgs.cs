namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendMenuItemActivatedArgs
    {
        #region Properties

        public FrontendLobbyMenuBaseItem Item { get; }

        #endregion

        #region Constructors

        internal FrontendMenuItemActivatedArgs(FrontendLobbyMenuBaseItem item)
        {
            Item = item;
        }

        #endregion
    }
}