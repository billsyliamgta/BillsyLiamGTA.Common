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
using System.Collections.Generic;
using GTA;
using GTA.Native;
using BillsyLiamGTA.Common.SHVDN.Weapon;

namespace BillsyLiamGTA.Common.SHVDN.Player
{
    public class PlayerManager
    {
        #region Properties

        private static int PreviousPlayerModel;

        private static int[] PreviousPlayerPedDrawables;

        private static int[] PreviousPlayerPedTextures;

        private static bool HasPreviousPlayerPedOutfitSet = false;

        private static List<WeaponData> PreviousWeapons;

        #endregion

        #region Functions

        [Obsolete("Use SwapModel(Model model) instead.")]
        public static bool ChangeModel(Model model) => SwapModel(model);

        public static bool SwapModel(Model model)
        {
            if (Game.Player.Character.Model == model)
                return true;

            if (model.IsInCdImage && model.IsValid)
            {
                if (model.Request(4000))
                {
                    GTA.Ped oldPed = Game.Player.Character;

                    GTA.Ped newPed = World.CreatePed(model, Game.Player.Character.Position);

                    Function.Call(Hash.CHANGE_PLAYER_PED, Function.Call<int>(Hash.PLAYER_ID), newPed, false, true);

                    oldPed.Delete();

                    oldPed = null;

                    model.MarkAsNoLongerNeeded();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Set's the previous player ped outfit.
        /// </summary>
        public static void SetPreviousPlayerPedOutfit()
        {
            PreviousPlayerModel = Function.Call<int>(Hash.GET_ENTITY_MODEL, Function.Call<int>(Hash.PLAYER_PED_ID));
            PreviousPlayerPedDrawables = new int[12];
            PreviousPlayerPedTextures = new int[12];

            for (int i = 0; i < 12; i++)
            {
                PreviousPlayerPedDrawables[i] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Game.Player.Character, i);
                PreviousPlayerPedTextures[i] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, Game.Player.Character, i);
            }

            HasPreviousPlayerPedOutfitSet = true;
        }

        /// <summary>
        /// Get's the previous player ped outfit, if it was set.
        /// </summary>
        public static void GetPreviousPlayerPedOutfit()
        {
            if (Function.Call<int>(Hash.GET_ENTITY_MODEL, Function.Call<int>(Hash.PLAYER_PED_ID)) != PreviousPlayerModel)
                SwapModel(PreviousPlayerModel);

            if (PreviousPlayerPedDrawables != null && PreviousPlayerPedDrawables.Length == 12 && PreviousPlayerPedTextures != null && PreviousPlayerPedTextures.Length == 12)
            {
                for (int i = 0; i < 12; i++)
                    Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, i, PreviousPlayerPedDrawables[i], PreviousPlayerPedTextures[i], 0);
            }

            PreviousPlayerModel = 0;
            PreviousPlayerPedDrawables = null;
            PreviousPlayerPedTextures = null;
            HasPreviousPlayerPedOutfitSet = false;
        }

        /// <summary>
        /// Set's the previous player's weapon data.
        /// </summary>
        public static void SetPreviousPlayerPedWeapons()
        {
            PreviousWeapons = WeaponData.GetData(Game.Player.Character);
            Script.Wait(0);
            Game.Player.Character.Weapons.RemoveAll();
        }

        /// <summary>
        /// Get's the previous player weapon data, if it was set.
        /// </summary>
        public static void GetPreviousPlayerPedWeapons() => WeaponData.SetFromData(Game.Player.Character, PreviousWeapons);

        #endregion
    }
}