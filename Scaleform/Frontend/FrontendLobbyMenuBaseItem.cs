namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public abstract class FrontendLobbyMenuBaseItem
    {
        #region Properties

        public string Text { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

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

        #endregion
    }
}