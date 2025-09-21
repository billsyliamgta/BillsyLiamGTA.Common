/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Audio
{
    public class AudioScene
    {
        #region Functions

        public static void Start(string name) => Function.Call(Hash.START_AUDIO_SCENE, name);

        public static void Stop(string name) => Function.Call(Hash.STOP_AUDIO_SCENE, name);

        public static void StopAll() => Function.Call(Hash.STOP_AUDIO_SCENES);

        public static bool IsActive(string name) => Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, name);

        #endregion
    }
}