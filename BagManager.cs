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
using GTA.Native;
using System.Linq;
using System.Collections.Generic;

namespace BillsyLiamGTA.Common.SHVDN
{
    public class BagManager
    {
        #region Fields

        public const int BAG_VARIANT_COUNT = 11;

        #endregion

        #region Properties

        public enum BagVariantTypes
        {
            Invalid = -1,
            OriginalHeists = 0,
            CasinoYungAncestor = 1,
            CasinoRegular = 2,
            CasinoMaintenance = 3,
            CasinoBugstars = 4,
            CasinoGeometric = 5,
            CasinoPattern = 6,
            CasinoGeometric2 = 7,
            CasinoAggressive1 = 8,
            CasinoAggressive2 = 9,
            CasinoAggressive3 = 10,
            CasinoAggressive4 = 11,
        }

        #endregion

        #region Functions

        private static string GetBagModelString(BagVariantTypes bagVariantType)
        {
            switch (bagVariantType)
            {
                case BagVariantTypes.Invalid:
                case BagVariantTypes.OriginalHeists:
                    return "hei_p_m_bag_var22_arm_s";
                case BagVariantTypes.CasinoYungAncestor:
                    return "ch_p_m_bag_var01_arm_s";
                case BagVariantTypes.CasinoRegular:
                    return "ch_p_m_bag_var02_arm_s";
                case BagVariantTypes.CasinoMaintenance:
                    return "ch_p_m_bag_var03_arm_s";
                case BagVariantTypes.CasinoBugstars:
                    return "ch_p_m_bag_var04_arm_s";
                case BagVariantTypes.CasinoGeometric:
                    return "ch_p_m_bag_var05_arm_s";
                case BagVariantTypes.CasinoPattern:
                    return "ch_p_m_bag_var06_arm_s";
                case BagVariantTypes.CasinoGeometric2:
                    return "ch_p_m_bag_var07_arm_s";
                case BagVariantTypes.CasinoAggressive1:
                    return "ch_p_m_bag_var08_arm_s";
                case BagVariantTypes.CasinoAggressive2:
                    return "ch_p_m_bag_var09_arm_s";
                case BagVariantTypes.CasinoAggressive3:
                    return "ch_p_m_bag_var10_arm_s";
            }

            return "NULL";
        }

        public static Dictionary<int, int> GetBagDrawableAndTexture(GTA.Ped ped, BagVariantTypes bagVariantType)
        {
            switch (bagVariantType)
            {
                case BagVariantTypes.Invalid:
                case BagVariantTypes.OriginalHeists:
                    {
                        if (ped.Model == PedHash.FreemodeMale01 || ped.Model == PedHash.FreemodeFemale01)
                            return new Dictionary<int, int>() { { 45, 0 } };
                        else
                            return new Dictionary<int, int>() { { 1, 0 } };
                    }
                case BagVariantTypes.CasinoYungAncestor:
                    return new Dictionary<int, int>() { { 82, 9 } };
                case BagVariantTypes.CasinoRegular:
                    return new Dictionary<int, int>() { { 82, 0 } };
                case BagVariantTypes.CasinoMaintenance:
                    return new Dictionary<int, int>() { { 82, 1 } };
                case BagVariantTypes.CasinoBugstars:
                    return new Dictionary<int, int>() { { 82, 8 } };
                case BagVariantTypes.CasinoGeometric:
                    return new Dictionary<int, int>() { { 82, 13 } };
                case BagVariantTypes.CasinoPattern:
                    return new Dictionary<int, int>() { { 82, 12 } };
                case BagVariantTypes.CasinoGeometric2:
                    return new Dictionary<int, int>() { { 82, 15 } };
                case BagVariantTypes.CasinoAggressive1:
                    return new Dictionary<int, int>() { { 82, 10 } };
                case BagVariantTypes.CasinoAggressive2:
                    return new Dictionary<int, int>() { { 82, 11 } };
                case BagVariantTypes.CasinoAggressive3:
                    return new Dictionary<int, int>() { { 82, 14 } };
            }

            return null;
        }

        public static BagVariantTypes GetBagVariantTypeFromPed(GTA.Ped ped)
        {
            for (int i = 0; i < BAG_VARIANT_COUNT; i++)
            {
                BagVariantTypes type = (BagVariantTypes)i;
                var bag = GetBagDrawableAndTexture(ped, type).First();

                if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped, 5) == bag.Key && Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, ped, 5) == bag.Value)
                    return type;
            }

            return BagVariantTypes.Invalid;
        }

        public static Prop CreateBagPropFromPed(GTA.Ped ped)
        {
            BagVariantTypes type = GetBagVariantTypeFromPed(ped);
            string bagModel = GetBagModelString(type);
            // Notification.PostTicker("Bag Variant Type: " + type.ToString() + " Bag Model: " + bagModel, true);
            return World.CreateProp(GetBagModelString(type), ped.Position, false, false);
        }

        public static void SetBagFromVariantType(GTA.Ped ped, BagVariantTypes bagVariantType)
        {
            var bag = GetBagDrawableAndTexture(ped, bagVariantType).First();
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, ped.Model == PedHash.FreemodeMale01 || ped.Model == PedHash.FreemodeFemale01 ? 5 : 9, bag.Key, bag.Value, 0);
        }

        public static void RemoveBag(GTA.Ped ped) => Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, ped.Model == PedHash.FreemodeMale01 || ped.Model == PedHash.FreemodeFemale01 ? 5 : 9, 0, 0, 0);

        #endregion
    }
}