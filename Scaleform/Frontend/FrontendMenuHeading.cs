namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public struct FrontendMenuHeading
    {
        #region Properties

        public string Text { get; set; }

        public float Width { get; set; }

        #endregion

        #region Constructor

        public FrontendMenuHeading(string text, float width)
        {
            Text = text;
            Width = width;
        }

        #endregion
    }
}