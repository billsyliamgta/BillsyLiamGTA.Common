/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Elements
{
    /// <summary>
    /// A class containing useful functions.
    /// </summary>
    public class Extensions
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