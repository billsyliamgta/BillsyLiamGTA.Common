using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Elements
{
    public class BaseScript : Script
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

        public static void TriggerMusicEvent(string musicEvent) => Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicEvent);

        public static bool PrepareMusicEvent(string musicEvent) => Function.Call<bool>(Hash.PREPARE_MUSIC_EVENT, musicEvent);

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