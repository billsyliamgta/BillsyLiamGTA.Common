using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.Elements
{
    public class BaseMission : Script
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

        public static int Clamp(int value, int min, int max)
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

        public static int GetHashKey(string hash) => Function.Call<int>(Hash.GET_HASH_KEY, hash);

        public static unsafe void SetBit(int* address, int offset) => Function.Call(Hash.SET_BIT, &address, offset);

        public static bool IsBitSet(int address, int offset) => Function.Call<bool>((Hash)0xA921AA820C25702F, address, offset);

        public static unsafe void ClearBit(int* address, int offset) => Function.Call(Hash.CLEAR_BIT, &address, offset);

        public static void SetPedLocation(GTA.Ped ped, Vector3 position, float heading = 0f, bool noOffset = false)
        {
            if (noOffset)
                ped.PositionNoOffset = position;
            else
                ped.Position = position;
            ped.Heading = heading;
        }

        public static void SetPlayerPedLocation(Vector3 position, float heading = 0f, bool noOffset = false) => SetPedLocation(Game.Player.Character, position, heading, noOffset);

        #endregion
    }
}