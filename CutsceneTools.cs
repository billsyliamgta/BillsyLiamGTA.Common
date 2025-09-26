/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GTA;
using GTA.Native;
using Ped = GTA.Ped;

namespace BillsyLiamGTA.Common.SHVDN
{
    public class CutsceneTools
    {
        #region Properties

        public static bool ScriptIsFreemodeMale { get; protected set; }

        public static bool ScriptIsFreemodeFemale { get; protected set; }

        public static PedHash ScriptPlayerPreviousModel { get; protected set; }

        private static string[] cutscenePed1Comp;
        
        private static string[] cutscenePed2Comp;

        private static string[] cutscenePed3Comp;

        private static string[] cutscenePed4Comp;

        #endregion

        #region Functions
        
        public unsafe static void PlayerPedModelSet()
        {
            GTA.Ped ped = Game.Player.Character;

            if (ped.Model == PedHash.FreemodeMale01)
            {
                ScriptIsFreemodeMale = true;
                ScriptIsFreemodeFemale = false;
            }
            else if (ped.Model == PedHash.FreemodeFemale01)
            {
                ScriptIsFreemodeMale = false;
                ScriptIsFreemodeFemale = true;
            }
            else
            {
                ScriptIsFreemodeMale = false;
                ScriptIsFreemodeFemale = false;
                ScriptPlayerPreviousModel = ped.Model;
            }

            ulong num = (ulong)(long)ped.MemoryAddress;
            ulong num2 = *(ulong*)(num + 32);
            *(long*)(num2 + 24) = 3214308084L;
        }

        public unsafe static void PlayerPedModelSetBack()
        {
            GTA.Ped ped = Game.Player.Character;

            if (ScriptIsFreemodeMale)
            {
                ulong num = (ulong)(long)ped.MemoryAddress;
                ulong num2 = *(ulong*)(num + 32);
                *(long*)(num2 + 24) = 1885233650L;
            }
            else if (ScriptIsFreemodeFemale)
            {
                ulong num3 = (ulong)(long)ped.MemoryAddress;
                ulong num4 = *(ulong*)(num3 + 32);
                *(long*)(num4 + 24) = 2627665880L;
            }
            else
            {
                ulong num5 = (ulong)(long)ped.MemoryAddress;
                ulong num6 = *(ulong*)(num5 + 32);
                *(long*)(num6 + 24) = (long)ScriptPlayerPreviousModel;
            }
        }

        public static void InitCutscenePedCompArrays()
        {
            cutscenePed1Comp = new string[12];
            cutscenePed2Comp = new string[12];
            cutscenePed3Comp = new string[12];
            cutscenePed4Comp = new string[12];
        }

        public static void ResetCutscenePedCompArrays()
        {
            cutscenePed1Comp = null;
            cutscenePed2Comp = null;
            cutscenePed3Comp = null;
            cutscenePed4Comp = null;
        }


        public static string[] GetCutscenePedCompArray(string mp)
        {
            string[] targetCompArray = null;

            switch (mp)
            {
                case "MP_1":
                    targetCompArray = cutscenePed1Comp;
                    break;
                case "MP_2":
                    targetCompArray = cutscenePed2Comp;
                    break;
                case "MP_3":
                    targetCompArray = cutscenePed3Comp;
                    break;
                case "MP_4":
                    targetCompArray = cutscenePed4Comp;
                    break;
            }

            return targetCompArray;
        }

        public static void SetPedOutfitCutscene(string mp, GTA.Ped ped)
        {
            string[] targetCompArray = GetCutscenePedCompArray(mp);

            if (targetCompArray == null)
                return;

            for (int i = 0; i < 12; i++)
            {
                int drawable = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped.Handle, i);
                int texture = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, ped.Handle, i);
                targetCompArray[i] = i + "_" + drawable + "_" + texture;
            }
        }

        public static void GetPedOutfitCutscene(string mp, GTA.Ped ped)
        {
            string[] targetCompArray = GetCutscenePedCompArray(mp);

            if (targetCompArray == null)
                return;

            for (int i = 0; i < 12; i++)
            {
                string[] part = targetCompArray[i].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped.Handle, i, int.Parse(part[1]), int.Parse(part[2]), 0);
            }
        }

        #endregion
    }
}