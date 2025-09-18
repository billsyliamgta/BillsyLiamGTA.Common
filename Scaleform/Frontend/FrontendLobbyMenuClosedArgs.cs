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