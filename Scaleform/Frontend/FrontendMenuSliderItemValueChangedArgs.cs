namespace BillsyLiamGTA.Common.SHVDN.Scaleform.Frontend
{
    public class FrontendMenuSliderItemValueChangedArgs<T>
    {
        #region Properties

        public T Value { get; }

        public int Index { get; }

        public FrontendLobbyMenuSliderItem<T> Item { get; }

        #endregion

        #region Constructors

        internal FrontendMenuSliderItemValueChangedArgs(T value, int index, FrontendLobbyMenuSliderItem<T> item)
        {
            Value = value;
            Index = index;
            Item = item;
        }

        #endregion
    }
}