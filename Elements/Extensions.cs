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

        #region Misc

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

        public static int GetGameTimer() => Function.Call<int>(Hash.GET_GAME_TIMER);

        #endregion

        #region Audio

        public static void TriggerMusicEvent(string musicEvent) => Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicEvent);

        public static bool PrepareMusicEvent(string musicEvent) => Function.Call<bool>(Hash.PREPARE_MUSIC_EVENT, musicEvent);

        public static void StartAudioScene(string audioScene) => Function.Call(Hash.START_AUDIO_SCENE, audioScene);

        public static bool IsAudioSceneActive(string audioScene) => Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, audioScene);

        public static void StopAudioScene(string audioScene) => Function.Call(Hash.STOP_AUDIO_SCENE, audioScene);

        public static void StopAudioScenes() => Function.Call(Hash.STOP_AUDIO_SCENES);

        public static bool RequestMissionAudioBank(string bankName, bool bOverNetwork) => Function.Call<bool>(Hash.REQUEST_MISSION_AUDIO_BANK, bankName, bOverNetwork);

        public static bool RequestScriptAudioBank(string bankName, bool bOverNetwork) => Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, bankName, bOverNetwork);

        public static void ReleaseNamedScriptAudioBank(string bankName) => Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, bankName);

        public static void ReleaseMissionAudioBank() => Function.Call(Hash.RELEASE_MISSION_AUDIO_BANK);

        public static void ReleaseScriptAudioBank() => Function.Call(Hash.RELEASE_SCRIPT_AUDIO_BANK);

        #endregion

        #region Ped

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

        #region Animations

        public static void RequestAnimDict(string animDict) => Function.Call(Hash.REQUEST_ANIM_DICT, animDict);

        public static bool HasAnimDictLoaded(string animDict) => Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict);

        public static void RemoveAnimDict(string animDict) => Function.Call(Hash.REMOVE_ANIM_DICT, animDict);

        public static Vector3 GetAnimInitialOffsetPosition(string animDict, string animName, Vector3 position, Vector3 rotation, float phase) => Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, animDict, animName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, phase, 2);

        public static Vector3 GetAnimInitialOffsetRotation(string animDict, string animName, Vector3 position, Vector3 rotation, float phase) => Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, animDict, animName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, phase, 2);

        #endregion

        #endregion
    }
}