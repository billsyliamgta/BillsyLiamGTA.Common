using GTA.Native;

namespace BillsyLiamGTA.Common.Elements
{
    public class Tools
    {
        #region Functions

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        public static int joaat(string hash) => Function.Call<int>(Hash.GET_HASH_KEY, hash);

        #endregion
    }
}