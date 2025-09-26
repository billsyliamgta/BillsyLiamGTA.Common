/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System;
using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using static GTA.Game;
using BaseScaleform = BillsyLiamGTA.Common.SHVDN.Scaleform.BaseScaleform;
using BillsyLiamGTA.Common.SHVDN.Scaleform;
using BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars;
using static BillsyLiamGTA.Common.SHVDN.Elements.Extensions;

namespace BillsyLiamGTA.Common.SHVDN.Minigames.Fingerprint
{
    public class FingerprintMinigame
    {
        #region Fields

        readonly string[] TextureDicts = new[]
        {
            "MPFClone_Common",
            "MPFClone_Decor",
            "MPFClone_GridDetails",
            "MPFClone_Grid",
            "MPFClone_Print0",
            "MPFClone_Print1",
            "MPFClone_Print2",
            "MPFClone_Print3",
            "mphackinggame",
            "mphackinggamebg",
            "mphackinggamewin",
            "mphackinggamewin2",
            "mphackinggamewin3",
            "mphackinggameoverlay",
            "mphackinggameoverlay1"
        };

        #endregion

        #region Properties

        public enum FingerprintMinigameState
        {
            Aborted,
            Failed,
            Successful
        }

        public FingerprintMinigameState State { get; private set; }

        private float fLocal_1604 = 0f;
        
        private float fLocal_1605 = 0f;
        
        private int iVar0 = 0;
        
        private int iVar13 = 0;
        
        private int Bink = 0;
        
        private int TargetPrint = 1;

        public int Index { get; set; } = 0;
        
        private int _selection = 0;

        public int Selection
        {
            get
            {
                return _selection;
            }
            set
            {
                if (value < 0 || value > 7)
                {
                    throw new ArgumentOutOfRangeException("ERROR: Selection must be between 0 and 7.");
                }

                _selection = value;
            }
        }

        private int _lives = 3;

        /// <summary>
        /// The total number of lives left before the minigame fails.
        /// </summary>
        public int Lives
        {
            get
            {
                return _lives;
            }
            set
            {
                if (value < 0 || value > 6)
                {
                    throw new ArgumentOutOfRangeException("ERROR: Lives must be between 0 and 6.");
                }
                _lives = value;
            }
        }

        private int _printsToHack = 2;
        
        /// <summary>
        /// The number of prints that must be hacked before the minigame is considered complete.
        /// </summary>
        public int PrintsToHack
        {
            get
            {
                return _printsToHack;
            }
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new ArgumentOutOfRangeException("ERROR: PrintsToHack must be between 1 and 4.");
                }

                _printsToHack = value;
            }
        }

        public int ScramblerSegments { get; set; } = 31;

        public int StartedAbortingGameTime = 0;

        public int BeganProcessingGameTime = 0;

        private int FinishedProcessingGameTime = 0;

        public int[] Time;

        /// <summary>
        /// Whether or not the minigame is currently in the process of aborting.
        /// </summary>
        public bool IsAborting = false;

        /// <summary>
        /// Whether or not the minigame is currently processing the hack outcome.
        /// </summary>
        public bool IsProcessing = false;

        public int ProcessingIndex { get; set; } = 0;

        private bool HasProcessedOutcome = false;

        /// <summary>
        /// The outcome of the hack, true if successful, false if failed.
        /// </summary>
        public bool HackOutcome { get; private set; } = false;

        private List<int> ShuffledPrints;

        private List<int> SelectedPrints;

        public bool IsScrambling { get; set; } = false;

        public bool InitScrambling = false;

        private int StartedScramblingGameTime = 0;

        private int UpdateScramblerSegmentsGameTime = 0;

        private int ScramblerLastShuffledGameTime = 0;

        public string[] fpCompTextureName = new string[8];

        public int[] fpCompAlpha = new int[8];

        public int[] targetFgAlpha = new int[8];

        public int UpdateProcessFgGameTime = 0;

        public int GridNoiseSet = 0;

        public int UpdateGridNoiseSetGameTime = 0;

        public int GridDetailsSet = 0;

        public int UpdateGridDetailsSetGameTime = 0;

        public int TechSet = 0;

        public int UpdateTechSetGameTime = 0;

        public int DiscSet = 0;

        public int UpdateDiscSetGameTime = 0;

        private readonly Dictionary<int, HashSet<int>> CorrectPrintsByTarget = new Dictionary<int, HashSet<int>>
        {
            { 1, new HashSet<int> { 0, 3, 5, 6 } },
            { 2, new HashSet<int> { 0, 1, 2, 3 } },
            { 3, new HashSet<int> { 0, 1, 2, 3 } },
            { 4, new HashSet<int> { 0, 1, 2, 3 } }
        };

        public InstructionalButtons InstructionalButtons;

        private bool HasDrawScreenSoundPlayed = false;

        private int SoundId = 0;
        
        private int SoundId2 = 0;

        public bool IsInactive => Index == 4;

        #endregion

        #region Constructor

        public FingerprintMinigame(int printsToHack = 3)
        {
            PrintsToHack = printsToHack;
        }

        #endregion

        #region Functions

        void func_14091(string sParam0, string sParam1, float fParam2, float fParam3, float fParam4, float fParam5, float fParam6, int iParam7, int iParam8, int iParam9, int iParam10, bool bParam11)//Position - 0x46C46F
        {
            Function.Call(Hash.DRAW_SPRITE, sParam0, sParam1, func_14094(fParam2), fParam3, (func_14093(fParam4) * fLocal_1605), func_14092(fParam5), fParam6, iParam7, iParam8, iParam9, iParam10, bParam11, 0);
        }

        float func_14092(float fParam0)//Position - 0x46C4A4
        {
            float fVar0;

            fVar0 = (fParam0 / 1080f);
            return fVar0;
        }

        float func_14093(float fParam0)//Position - 0x46C4B8
        {
            float fVar0;

            fVar0 = (fParam0 / 1920f);
            return fVar0;
        }

        float func_14094(float fParam0)//Position - 0x46C4CC
        {
            fParam0 = (0.5f - ((0.5f - fParam0) / fLocal_1604));
            return fParam0;
        }

        void func_14127()//Position - 0x46DB0D
        {
            fLocal_1605 = (1.778f / fLocal_1604);
        }

        void func_14128()//Position - 0x46DB27
        {
            fLocal_1604 = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
            if (Function.Call<bool>(Hash.IS_PC_VERSION))
            {
                if (fLocal_1604 >= 4f)
                {
                    fLocal_1604 = (fLocal_1604 / 3f);
                }
            }
        }

        unsafe void func_14175(int iParam0, float* uParam1, float* uParam2)//Position - 0x46FC44
        {
            switch (iParam0)
            {
                case 0:
                    *uParam1 = 0.105f;
                    *uParam2 = 0.306f;
                    break;

                case 1:
                    *uParam1 = 0.239f;
                    *uParam2 = 0.306f;
                    break;

                case 2:
                    *uParam1 = 0.105f;
                    *uParam2 = 0.439f;
                    break;

                case 3:
                    *uParam1 = 0.239f;
                    *uParam2 = 0.439f;
                    break;

                case 4:
                    *uParam1 = 0.105f;
                    *uParam2 = 0.572f;
                    break;

                case 5:
                    *uParam1 = 0.239f;
                    *uParam2 = 0.572f;
                    break;

                case 6:
                    *uParam1 = 0.105f;
                    *uParam2 = 0.706f;
                    break;

                case 7:
                    *uParam1 = 0.239f;
                    *uParam2 = 0.706f;
                    break;
            }
        }

        float func_14172(int iParam0)//Position - 0x46F8FF
        {
            float fVar0 = 0f;

            switch (iParam0)
            {
                case 1:
                    fVar0 = 0.536f;
                    break;

                case 2:
                    fVar0 = 0.662f;
                    break;

                case 3:
                    fVar0 = 0.782f;
                    break;

                case 4:
                    fVar0 = 0.905f;
                    break;
            }
            return fVar0;
        }

        private void DisableControls(bool toggle)
        {
            if (toggle)
            {
                DisableControlThisFrame(Control.FrontendPause);
                DisableControlThisFrame(Control.FrontendPauseAlternate);
                DisableControlThisFrame(Control.Phone);
                DisableControlThisFrame(Control.SelectWeapon);
            }
        }

        private void UpdateTimer()
        {
            Time[5]--;

            if (Time[5] < 0)
            {
                Time[5] = 9;
                Time[4]--;

                if (Time[4] < 0)
                {
                    Time[4] = 9;
                    Time[3]--;

                    if (Time[3] < 0)
                    {
                        Time[3] = 9;
                        Time[2]--;

                        if (Time[2] < 0)
                        {
                            Time[2] = 5;
                            Time[1]--;

                            if (Time[1] < 0)
                            {
                                Time[1] = 9;
                                Time[0]--;

                                if (Time[0] < 0)
                                {
                                    Time[0] = 0;
                                    Time[1] = 0;
                                    Time[2] = 0;
                                    Time[3] = 0;
                                    Time[4] = 0;
                                    Time[5] = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Controls()
        {
            bool canRunCheck = SelectedPrints?.Count == 4;
            if (!IsProcessing && !IsScrambling)
            {
                if (IsControlJustPressed(Control.FrontendRight) && Selection < 7)
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    Selection++;
                }

                if (IsControlJustPressed(Control.FrontendLeft) && Selection > 0)
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    Selection--;
                }

                if (IsControlJustPressed(Control.FrontendDown) && Selection != 6 && Selection != 7)
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    Selection += 2;
                }

                if (IsControlJustPressed(Control.FrontendUp) && Selection != 0 && Selection != 1)
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    Selection -= 2;
                }

                if (IsControlJustPressed(Control.FrontendAccept))
                {
                    bool contains = SelectedPrints.Contains(Selection);
                    if (SelectedPrints.Count != 4)
                    {
                        if (!contains)
                        {
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                            SelectedPrints.Add(Selection);
                        }
                    }

                    if (contains)
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Deselect_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                        SelectedPrints.Remove(Selection);
                    }
                }

                if (IsControlJustPressed(Control.FrontendRup) && canRunCheck)
                {
                    ScramblerSegments = 31;
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Window_Draw", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    SoundId2 = Function.Call<int>(Hash.GET_SOUND_ID);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId2, "Processing", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                    BeganProcessingGameTime = GameTime;
                    IsProcessing = true;
                }
            }

            var defaultPool = new List<Dictionary<Control, string>>
            {
                new Dictionary<Control, string> { { Control.FrontendCancel, "Abort Hack (Hold)" } },
                new Dictionary<Control, string> { { Control.FrontendAccept, "Select" } },
                new Dictionary<Control, string> { { Control.FrontendRight, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendLeft, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendDown, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendUp, "Move Selector" } }
            };

            var canRunCheckPool = new List<Dictionary<Control, string>>
            {
                new Dictionary<Control, string> { { Control.FrontendCancel, "Abort Hack (Hold)" } },
                new Dictionary<Control, string> { { Control.FrontendAccept, "Select" } },
                new Dictionary<Control, string> { { Control.FrontendRup, "Run Check" } },
                new Dictionary<Control, string> { { Control.FrontendRight, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendLeft, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendDown, string.Empty } },
                new Dictionary<Control, string> { { Control.FrontendUp, "Move Selector" } }
            };

            InstructionalButtons?.SetPool(canRunCheck ? canRunCheckPool : defaultPool);
            InstructionalButtons?.Draw();
        }

        private void ShufflePrints()
        {
            if (ShuffledPrints?.Count > 0)
                ShuffledPrints.Clear();

            ShuffledPrints = Enumerable.Range(0, 8).ToList();
            Random rng = new Random();
            ShuffledPrints = ShuffledPrints.OrderBy(x => rng.Next()).ToList();
        }

        private bool IsSelectionCorrect()
        {
            HashSet<int> correctSet;
            if (!CorrectPrintsByTarget.TryGetValue(TargetPrint, out correctSet))
                return false;

            if (SelectedPrints.Count != correctSet.Count)
                return false;

            foreach (int visualIndex in SelectedPrints)
            {
                int originalIndex = ShuffledPrints[visualIndex];
                if (!correctSet.Contains(originalIndex))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Request's the streamed texture dicts and returns true if they are all loaded.
        /// </summary>
        /// <returns></returns>
        private bool LoadStreamedTextureDicts()
        {
            foreach (var dict in TextureDicts)
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, dict, false);

            return TextureDicts.All(dict =>
                Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, dict));
        }

        /// <summary>
        /// Unload's the streamed texture dicts.
        /// </summary>
        private void UnloadStreamedTextureDicts()
        {
            foreach (var dict in TextureDicts)
                Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, dict);
        }

        /// <summary>
        /// Request's the script audio banks and returns true if they are all loaded.
        /// </summary>
        /// <returns></returns>
        private bool RequestScriptAudioBanks()
        {
            if (Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1) && Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1))
                return true;

            return false;
        }

        /// <summary>
        /// Release's the script audio banks.
        /// </summary>
        private void ReleaseScriptAudioBanks()
        {
            Function.Call(Hash.RELEASE_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
            Function.Call(Hash.RELEASE_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
        }

        public unsafe void Update()
        {
            DisableControls(Index > 0);
            switch (Index)
            {
                case 0:
                    {
                        if (LoadStreamedTextureDicts() && RequestScriptAudioBanks())
                        {
                            InstructionalButtons = new InstructionalButtons();
                            InstructionalButtons.Load();                           
                            StartAudioScene("DLC_H3_Fingerprint_Hack_Scene");
                            Bink = Function.Call<int>(Hash.SET_BINK_MOVIE, "INTRO_FC");
                            Function.Call(Hash.PLAY_BINK_MOVIE, Bink);
                            Function.Call(Hash.SET_BINK_MOVIE_TIME, Bink, 0f);
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Startup_Sequence", "DLC_H3_Cas_Door_Minigame_Sounds", true);
                            func_14128();
                            func_14127();
                            Time = new int[6];
                            Time[0] = 0;
                            Time[1] = 4;
                            Time[2] = 0;
                            Time[3] = 0;
                            Time[4] = 0;
                            Time[5] = 0;
                            UpdateScramblerSegmentsGameTime = GameTime + 3000;
                            Index++;
                        }
                    }
                    break;
                case 1:
                    {
                        Function.Call(Hash.DRAW_BINK_MOVIE, Bink, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);

                        float time = Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, Bink);

                        if (time > 75f && !HasDrawScreenSoundPlayed)
                        {
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Draw_Screen", "DLC_H3_Cas_Door_Minigame_Sounds", true);
                            HasDrawScreenSoundPlayed = true;
                        }

                        if (time > 99f)
                        {
                            Function.Call(Hash.STOP_BINK_MOVIE, Bink);  
                            Function.Call(Hash.RELEASE_BINK_MOVIE, Bink);
                            Bink = 0;
                            SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId, "Background_Hum", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                            ShufflePrints();
                            SelectedPrints = new List<int>();
                            TimerBarPool.ShouldMoveUp = true;
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (IsControlJustPressed(Control.Reload) && !IsAborting)
                        {
                            StartedAbortingGameTime = GameTime;
                            IsAborting = true;
                        }

                        if (IsControlJustReleased(Control.Reload) && IsAborting)
                        {
                            StartedAbortingGameTime = 0;
                            IsAborting = false;
                        }

                        UpdateTimer();

                        if (Lives == 0 || Time.All(p => p == 0) || IsAborting && GameTime - StartedAbortingGameTime > 3000)
                        {
                            State = IsAborting ? FingerprintMinigameState.Aborted : FingerprintMinigameState.Failed;
                            Function.Call(Hash.STOP_SOUND, SoundId);
                            Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                            SoundId = 0;
                            Bink = Function.Call<int>(Hash.SET_BINK_MOVIE, "FAIL_FC");
                            Function.Call(Hash.PLAY_BINK_MOVIE, Bink);
                            Function.Call(Hash.SET_BINK_MOVIE_TIME, Bink, 0f);
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Failed", "DLC_H3_Cas_Door_Minigame_Sounds", true);
                            Index++;
                        }
                        else
                        {
                            bool processing = IsProcessing;
                            bool scrambling = IsScrambling;

                            func_14091("mphackinggamebg", "bg", 0.5f, 0.5f, 1920f, 1080f, 0f, 255, 255, 255, 255, false);

                            if (GameTime - UpdateTechSetGameTime > 250)
                            {
                                if (TechSet == 4)
                                    TechSet = 0;
                                else
                                    TechSet++;
                                UpdateTechSetGameTime = GameTime;
                            }

                            func_14091("mphackinggamewin", $"tech_1_{TechSet}", 1f, 0.358f, 780f, 512f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggamewin2", $"tech_2_0", 0.993f, 0.642f, 840f, 580f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggamewin", "tech_3_0", 0.073f, 0.489f, 980f, 780f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggamewin3", "tech_4_0", 0.044f, 0.672f, 980f, 580f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", "Black_BG", 0.5f, 0.5f, 1920f, 1080f, 0f, 5, 5, 5, 100, false);
                            func_14091("MPFClone_Common", "background_layout", 0.5f, 0.5f, 1264f, 940f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggameoverlay", "grid_rgb_pixels", 0.5f, 0.5f, 1264f, 940f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggameoverlay1", "ScreenGrid", 0.5f, 0.5f, 1264f, 940f, 0f, 255, 255, 255, 255, false);                          

                            if (GameTime - UpdateDiscSetGameTime > 500)
                            {
                                if (DiscSet == 2)
                                    DiscSet = 0;
                                else
                                    DiscSet++;
                                UpdateDiscSetGameTime = GameTime;
                            }

                            func_14091("MPFClone_Common", $"disc_a{DiscSet}", 0.983f, 0.669f, 100f, 100f, 0f, 255, 255, 255, 255, false);
                            func_14091("MPFClone_Common", $"disc_b{DiscSet}", 0.983f, 0.669f, 100f, 100f, 0f, 255, 255, 255, 255, false);
                            func_14091("MPFClone_Common", $"disc_c{DiscSet}", 0.983f, 0.669f, 100f, 100f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", "Scrambler_BG", 0.169f, 0.845f, 400f, 64f, 0f, 255, 255, 255, 255, false);                         

                            for (int i = 1; i < ScramblerSegments; i++)
                            {
                                float fillSegmentX = ((0.169f - 0.178f) + (0.0115f * Function.Call<float>(Hash.TO_FLOAT, i)));
                                func_14091("mphackinggame", "Scrambler_Fill_Segment", fillSegmentX, (0.845f + 0.005f), 12f, 80f, 0f, 255, 255, 255, 255, false);
                            }

                            float numberX = 0.122f;
                            float numberXOffset = 0.031f;
                            float numberY = 0.144f;

                            func_14091("mphackinggame", $"Numbers_{Time[0]}", (numberX - numberXOffset * 2f), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", $"Numbers_{Time[1]}", (numberX - numberXOffset), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", $"Numbers_{Time[2]}", (numberX + numberXOffset * 1f), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", $"Numbers_{Time[3]}", (numberX + numberXOffset * 2f), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", "Numbers_Colon", numberX, numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", "Numbers_Colon", numberX + numberXOffset * 3f, numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", $"Numbers_{Time[4]}", (numberX + numberXOffset * 4f), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggame", $"Numbers_{Time[5]}", (numberX + numberXOffset * 5f), numberY, 40f, 60f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggameoverlay", "grid_rgb_pixels", 0.5f, 0.5f, 1920f, 1080f, 0f, 255, 255, 255, 255, false);
                            func_14091("mphackinggameoverlay1", "ScreenGrid", 0.5f, 0.5f, 1920f, 1080f, 0f, 255, 255, 255, 255, false);

                            for (int i = 1; i < Lives + 1; i++)
                            {
                                float lifeX = 0.983f;
                                float lifeY = (0.19f + (0.055f * i));
                                func_14091("mphackinggame", "Life", lifeX, lifeY, 64f, 64f, 0f, 255, 255, 255, 127, false);
                            }

                            for (int i = 0; i <= 7; i++)
                            {
                                if (processing)
                                {
                                    if (GameTime - UpdateProcessFgGameTime > 500)
                                    {
                                        targetFgAlpha[i] = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 127, 255);

                                        if (i == 7)
                                            UpdateProcessFgGameTime = GameTime;
                                    }
                                }
                                else
                                    targetFgAlpha[i] = 127;

                                func_14091($"MPFClone_Print{TargetPrint - 1}", $"fp{TargetPrint}_{i + 1}", 0.674f, 0.379f, 400f, 512f, 0f, 255, 255, 255, targetFgAlpha[i], false);
                                float fpX = 0f;
                                float fpY = 0f;                           
                                func_14175(i, &fpX, &fpY);                              
                                if (scrambling)
                                {
                                    if (GameTime - ScramblerLastShuffledGameTime > 300)
                                    {
                                        if (!InitScrambling)
                                        {
                                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Print_Shuffle", "DLC_H3_Cas_Finger_Minigame_Sounds", true);                                            
                                            InitScrambling = true;
                                        }
                                        else
                                        {
                                            if (GameTime - StartedScramblingGameTime > 5000)
                                            {
                                                InitScrambling = false;
                                                ScramblerLastShuffledGameTime = 0;
                                                StartedScramblingGameTime = 0;
                                                Selection = 0;
                                                IsScrambling = false;
                                            }
                                            ShufflePrints();
                                            switch (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2))
                                            {
                                                case 0:
                                                    {
                                                        fpCompAlpha[i] = 125;
                                                        fpCompTextureName[i] = $"fp{TargetPrint}_comp_{ShuffledPrints[i] + 1}";
                                                    }
                                                    break;
                                                case 1:
                                                    {
                                                        fpCompAlpha[i] = 255;
                                                        fpCompTextureName[i] = "comp_blank";
                                                    }
                                                    break;
                                            }

                                            if (i == 7)
                                                ScramblerLastShuffledGameTime = GameTime;
                                        }
                                    }
                                }
                                else
                                {
                                    fpCompAlpha[i] = 127;
                                    if (SelectedPrints.Contains(i))
                                        fpCompAlpha[i] = 255;
                                    fpCompTextureName[i] = $"fp{TargetPrint}_comp_{ShuffledPrints[i] + 1}";
                                }
                                func_14091($"MPFClone_Print{TargetPrint - 1}", fpCompTextureName[i], fpX, fpY, 128f, 128f, 0f, 255, 255, 255, fpCompAlpha[i], false);
                            }

                            if (processing)
                            {
                                if (GameTime - UpdateGridNoiseSetGameTime > 250)
                                {
                                    if (GridNoiseSet == 2)
                                        GridNoiseSet = 0;
                                    else
                                        GridNoiseSet++;
                                    UpdateGridNoiseSetGameTime = GameTime;
                                }

                                func_14091("MPFClone_Grid", $"Grid_Noise_{GridNoiseSet}", 0.653f, 0.379f, 800f, 800f, 0f, 255, 255, 255, 255, false);
                            }

                            int forward = (GridDetailsSet + 1) % 4;
                            int reverse = 3 - forward;
                            func_14091("MPFClone_Decor", $"techaration_{forward % 2}", 0.5f, 0.5f, 1264f, 940f, 0f, 255, 255, 255, 255, false);
                            func_14091("MPFClone_GridDetails", $"griddetails_{forward}", 0.439f, 0.379f, 16f, 512f, 0f, 255, 255, 255, 255, false);
                            func_14091("MPFClone_GridDetails", $"griddetails_{reverse}", 0.902f, 0.379f, 16f, 512f, 0f, 255, 255, 255, 255, false);

                            if (GameTime - UpdateGridDetailsSetGameTime > 500)
                            {
                                if (GridDetailsSet == 3)
                                    GridDetailsSet = 0;
                                else
                                    GridDetailsSet++;
                                UpdateGridDetailsSetGameTime = GameTime;
                            }

                            float selectorX = 0f;
                            float selectorY = 0f;
                            func_14175(Selection, &selectorX, &selectorY);
                            if (!scrambling)
                                func_14091("MPFClone_Common", "selectorFrame", selectorX, selectorY, 180f, 180f, 0f, 255, 255, 255, 255, false);
                            for (int i = 0; i <= 4; i++)
                            {
                                if (i <= PrintsToHack)
                                    func_14091("MPFClone_Common", $"Decypher_{i + 1}", func_14172(i + 1), 0.832f, 128f, 128f, 0f, 255, 255, 255, 255, false);
                                else
                                    func_14091("MPFClone_Common", "Disabled_Signal", func_14172(i), 0.832f, 128f, 128f, 0f, 255, 255, 255, 255, false);
                            }
                            func_14091("MPFClone_Common", "Decyphered_Selector", func_14172(TargetPrint), 0.832f, 180f, 180f, 0f, 255, 255, 255, 255, false);

                            if (processing)
                            {
                                switch (ProcessingIndex)
                                {
                                    case 0:
                                        {
                                            func_14091("mphackinggame", "Loading_Window", 0.5f, 0.5f, 480f, 128f, 0f, 255, 255, 255, 255, false);
                                            iVar13 = GameTime - BeganProcessingGameTime;
                                            if (iVar13 >= 2000)
                                            {
                                                Function.Call(Hash.STOP_SOUND, SoundId2);
                                                Function.Call(Hash.RELEASE_SOUND_ID, SoundId2);
                                                SoundId2 = 0;
                                                BeganProcessingGameTime = 0;
                                                ProcessingIndex++;
                                            }
                                            float fVar14 = (Function.Call<float>(Hash.TO_FLOAT, iVar13) / Function.Call<float>(Hash.TO_FLOAT, 2000));
                                            fVar14 = (fVar14 * 100f);
                                            float fVar15 = ((Function.Call<float>(Hash.TO_FLOAT, 35) / 100f) * fVar14);
                                            iVar0 = 1;
                                            while (iVar0 <= Function.Call<int>(Hash.FLOOR, fVar15))
                                            {
                                                float fVar16 = (0.32f + (0.01f * Function.Call<float>(Hash.TO_FLOAT, iVar0)));
                                                func_14091("mphackinggame", "Loading_Bar_Segment", fVar16, 0.516f, 12f, 40f, 0f, 255, 255, 255, 255, false);
                                                iVar0++;
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            HackOutcome = IsSelectionCorrect();
                                            string textureName = HackOutcome ? "Correct_" : "Incorrect_";
                                            func_14091("mphackinggame", ((GameTime / 500) % 2) == 0 ? textureName + "0" : textureName + "1", 0.5f, 0.5f, 600f, 140f, 0f, 255, 255, 255, 255, false);

                                            if (HasProcessedOutcome)
                                            {
                                                if (GameTime - FinishedProcessingGameTime > 3000)
                                                {
                                                    FinishedProcessingGameTime = 0;
                                                    HasProcessedOutcome = false;
                                                    ProcessingIndex++;
                                                }
                                            }
                                            else
                                            {
                                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, HackOutcome ? "Target_Match" : "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
                                                FinishedProcessingGameTime = GameTime;
                                                HasProcessedOutcome = true;
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            ShufflePrints();
                                            SelectedPrints.Clear();
                                            if (HackOutcome)
                                            {
                                                if (TargetPrint == PrintsToHack)
                                                {
                                                    State = FingerprintMinigameState.Successful;
                                                    Function.Call(Hash.STOP_SOUND, SoundId);
                                                    Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                                                    SoundId = 0;
                                                    Bink = Function.Call<int>(Hash.SET_BINK_MOVIE, "SUCCESS_FC");
                                                    Function.Call(Hash.PLAY_BINK_MOVIE, Bink);
                                                    Function.Call(Hash.SET_BINK_MOVIE_TIME, Bink, 0f);
                                                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_H3_Cas_Door_Minigame_Sounds", true);
                                                    Index++;
                                                }
                                                TargetPrint++;
                                            }
                                            else
                                            {
                                                if (Lives > 0)
                                                    Lives--;
                                            }

                                            IsProcessing = false;
                                            ProcessingIndex = 0;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (ScramblerSegments == 0)
                                {
                                    ScramblerSegments = 31;
                                    StartedScramblingGameTime = GameTime;
                                    IsScrambling = true;
                                }
                                else
                                {
                                    if (GameTime > UpdateScramblerSegmentsGameTime)
                                    {
                                        UpdateScramblerSegmentsGameTime = GameTime + 3000;

                                        if (ScramblerSegments > 0)
                                        {
                                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Scramble_Countdown_High", "DLC_H3_Cas_Door_Minigame_Sounds", true);
                                            ScramblerSegments--;
                                        }
                                    }
                                }
                            }

                            Controls();
                        }
                    }
                    break;
                case 3:
                    {
                        Function.Call(Hash.DRAW_BINK_MOVIE, Bink, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
                        if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, Bink) > 99f)
                        {
                            Function.Call(Hash.STOP_BINK_MOVIE, Bink);
                            Function.Call(Hash.RELEASE_BINK_MOVIE, Bink);
                            Bink = 0;
                            StartAudioScene("DLC_H3_Fingerprint_Hack_Scene");
                            GameplayCamera.RelativeHeading = 0f;
                            GameplayCamera.RelativePitch = 1f;
                            Index++;
                        }
                    }
                    break;
            }
        }

        public void Dispose()
        {
            fLocal_1604 = 0f;
            fLocal_1605 = 0f;
            iVar0 = 0;
            iVar13 = 0;
            if (Bink != 0)
            {
                Function.Call(Hash.STOP_BINK_MOVIE, Bink);
                Function.Call(Hash.RELEASE_BINK_MOVIE, Bink);
                Bink = 0;
            }
            Index = 0;
            Selection = 0;
            Lives = 3;
            PrintsToHack = 2;
            ScramblerSegments = 31;
            StartedAbortingGameTime = 0;
            BeganProcessingGameTime = 0;
            FinishedProcessingGameTime = 0;
            Time = null;
            IsAborting = false;
            IsProcessing = false;
            ProcessingIndex = 0;
            HasProcessedOutcome = false;
            HackOutcome = false;
            if (ShuffledPrints?.Count > 0)
            {
                ShuffledPrints.Clear();
                ShuffledPrints = null;
            }
            if (SelectedPrints?.Count > 0)
            {
                SelectedPrints.Clear();
                SelectedPrints = null;
            }
            IsScrambling = false;
            InitScrambling = false;
            StartedScramblingGameTime = 0;
            UpdateScramblerSegmentsGameTime = 0;
            ScramblerLastShuffledGameTime = 0;
            UpdateProcessFgGameTime = 0;
            GridNoiseSet = 0;
            UpdateGridNoiseSetGameTime = 0;
            GridNoiseSet = 0;
            UpdateGridDetailsSetGameTime = 0;
            TechSet = 0;
            UpdateTechSetGameTime = 0;
            DiscSet = 0;
            UpdateDiscSetGameTime = 0;
            InstructionalButtons?.Dispose();
            InstructionalButtons = null;
            TimerBarPool.ShouldMoveUp = false;
            if (IsAudioSceneActive("DLC_H3_Fingerprint_Hack_Scene"))
                StopAudioScene("DLC_H3_Fingerprint_Hack_Scene");
            if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, SoundId))
            {
                Function.Call(Hash.STOP_SOUND, SoundId);
                Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                SoundId = 0;
            }
            if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, SoundId2))
            {
                Function.Call(Hash.STOP_SOUND, SoundId2);
                Function.Call(Hash.RELEASE_SOUND_ID, SoundId2);
                SoundId2 = 0;
            }
            HasDrawScreenSoundPlayed = false;
            UnloadStreamedTextureDicts();
            ReleaseScriptAudioBanks();
        }

        #endregion
    }
}