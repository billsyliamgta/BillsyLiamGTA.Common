using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common
{
    public class Respawn : Script
    {
        #region Properties

        public static bool UseCustomPlayerSpawnPoint { get; private set; } = false;

        public static Vector3 CustomSpawnPointPosition { get; private set; } = Vector3.Zero;

        public static int ReturnWantedLevel = 0;

        public static int ReturnHour = Function.Call<int>(Hash.GET_CLOCK_HOURS);

        public static int ReturnMinute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);

        public static int ReturnSecond = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

        #endregion

        #region Constructor

        public Respawn()
        {
            Tick += OnTick;
            Aborted += OnAborted;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (GlobalVariable.Get(5).Read<int>() == 1)
                DeathControl();
        }

        private void OnAborted(object sender, EventArgs e)
        {
            if (GlobalVariable.Get(5).Read<int>() == 1)
                SetRespawnStat(true);
        }

        #endregion

        #region Functions

        public static void DisableHospitalRestart(int hospitalIndex, bool toggle) => Function.Call(Hash.DISABLE_HOSPITAL_RESTART, hospitalIndex, toggle);

        public static void DisablePoliceRestart(int policeIndex, bool toggle) => Function.Call(Hash.DISABLE_POLICE_RESTART, policeIndex, toggle);

        public static void TerminateAllScriptsWithThisName(string scriptName) => Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, scriptName);

        public static void RequestScript(string scriptName) => Function.Call(Hash.REQUEST_SCRIPT, scriptName);

        public static void StartNewScript(string scriptName, int stackSize) => Function.Call(Hash.START_NEW_SCRIPT, scriptName, stackSize);

        public static void SetScriptAsNoLongerNeeded(string scriptName) => Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);

        public static bool HasScriptLoaded(string scriptName) => Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName);

        public static void SetClockTime(int hour, int minute, int second) => Function.Call(Hash.SET_CLOCK_TIME, hour, minute, second);

        public static void SetCustomRespawnPoint(Vector3 position, bool toggle)
        {
            CustomSpawnPointPosition = position;
            UseCustomPlayerSpawnPoint = toggle;
        }

        public static void ClearCustomRespawnPoint()
        {
            CustomSpawnPointPosition = Vector3.Zero;
            UseCustomPlayerSpawnPoint = false;
        }

        public static void SetRespawnStat(bool toggle)
        {
            if (toggle)
            {
                GlobalVariable.Get(5).Write(0);
                RequestScript("respawn_controller");
                while (!HasScriptLoaded("respawn_controller")) 
                    Wait(0);
                StartNewScript("respawn_controller", 128);
                SetScriptAsNoLongerNeeded("respawn_controller");
                DisableHospitalRestart(0, false);
                DisableHospitalRestart(1, false);
                DisableHospitalRestart(2, false);
                DisableHospitalRestart(3, false);
                DisableHospitalRestart(4, false);
                DisablePoliceRestart(0, false);
                DisablePoliceRestart(1, false);
                DisablePoliceRestart(2, false);
                DisablePoliceRestart(3, false);
                DisablePoliceRestart(4, false);
                DisablePoliceRestart(5, false);
                DisablePoliceRestart(6, false);
                ClearCustomRespawnPoint();
            }
            else
            {
                GlobalVariable.Get(5).Write(1);
                TerminateAllScriptsWithThisName("respawn_controller");
                DisableHospitalRestart(0, true);
                DisableHospitalRestart(1, true);
                DisableHospitalRestart(2, true);
                DisableHospitalRestart(3, true);
                DisableHospitalRestart(4, true);
                DisablePoliceRestart(0, true);
                DisablePoliceRestart(1, true);
                DisablePoliceRestart(2, true);
                DisablePoliceRestart(3, true);
                DisablePoliceRestart(4, true);
                DisablePoliceRestart(5, true);
                DisablePoliceRestart(6, true);
            }
        }

        public unsafe static void DeathControl()
        {
            if (!Game.Player.Character.IsDead) 
                return;
            int start = Game.GameTime;
            bool flag = false;
            if (Screen.IsHelpTextDisplayed) 
                Screen.ClearHelpText();
            while (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED", false, -1)) 
                Wait(0);
            Function.Call(Hash.START_AUDIO_SCENE, "DEATH_SCENE");
            GTA.Scaleform scaleform = GTA.Scaleform.RequestMovie("MP_BIG_MESSAGE_FREEMODE");
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "MP_Flash", "WastedSounds", true);
            GameplayCamera.Shake(CameraShake.DeathFail, 1.5f);
            Function.Call(Hash.ANIMPOSTFX_PLAY, "DeathFailMPIn", 0, false);
            while (!Screen.IsFadedOut && Game.Player.Character.IsDead) // If the player is resurrected by a trainer for example, it'll move on.
            {
                if (Game.GameTime - start > 2000)
                {
                    scaleform.Render2D();
                    if (!flag)
                    {
                        scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", Game.GetLocalizedString("RESPAWN_W_MP") /* GXT: WASTED */, "", 27);
                        Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "NG_deathfail_BW_base", 10f);
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TextHit", "WastedSounds", true);
                        flag = true;
                    }
                }
                Wait(0);
            }
            scaleform.Dispose();
            scaleform = null;
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "OFFMISSION_WASTED");
            Function.Call(Hash.STOP_AUDIO_SCENE, "DEATH_SCENE");
            SetClockTime(ReturnHour, ReturnMinute, ReturnSecond);
            Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
            Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
            if (GameplayCamera.IsShaking) 
                GameplayCamera.StopShaking();
            Vector3 position = Vector3.Zero;
            if (UseCustomPlayerSpawnPoint)
                position = CustomSpawnPointPosition;
            else
            {
                position = Game.Player.Character.Position;
                int interiorFlag = 2;
                if (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character) != 0) 
                    interiorFlag = 1;
                Function.Call(Hash.SPAWNPOINTS_START_SEARCH, position.X, position.Y, position.Z, 150f, 5f, 0x18 | interiorFlag | 0x20, -1f, 20000);
                int spawnPoints = Function.Call<int>(Hash.SPAWNPOINTS_GET_NUM_SEARCH_RESULTS);
                Function.Call(Hash.SPAWNPOINTS_GET_SEARCH_RESULT, Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, spawnPoints), &position.X, &position.Y, &position.Z);
            }
            Game.Player.Character.Position = position;
            Function.Call(Hash.DISPLAY_HUD, true);
            Function.Call(Hash.DISPLAY_RADAR, true);
            GameplayCamera.RelativeHeading = 0f;
            GameplayCamera.RelativePitch = 1f;
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Function.Call<int>(Hash.PLAYER_ID), ReturnWantedLevel, false);
            Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, Function.Call<int>(Hash.PLAYER_ID), false);
        }

        #endregion
    }
}