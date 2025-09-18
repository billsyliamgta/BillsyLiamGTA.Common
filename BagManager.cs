using GTA;
using GTA.Native;
using System.Linq;
using System.Collections.Generic;

namespace BillsyLiamGTA.Common.SHVDN
{
    public class BagManager
    {
        public const int BAG_VARIANT_COUNT = 11;

        public enum BagVariantType
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

        private static string GetBagModelString(BagVariantType bagVariantType)
        {
            switch (bagVariantType)
            {
                case BagVariantType.Invalid:
                case BagVariantType.OriginalHeists:
                    {
                        return "hei_p_m_bag_var22_arm_s";
                    }
                case BagVariantType.CasinoYungAncestor:
                    {
                        return "ch_p_m_bag_var01_arm_s";
                    }
                case BagVariantType.CasinoRegular:
                    {
                        return "ch_p_m_bag_var02_arm_s";
                    }
                case BagVariantType.CasinoMaintenance:
                    {
                        return "ch_p_m_bag_var03_arm_s";
                    }
                case BagVariantType.CasinoBugstars:
                    {
                        return "ch_p_m_bag_var04_arm_s";
                    }
                case BagVariantType.CasinoGeometric:
                    {
                        return "ch_p_m_bag_var05_arm_s";
                    }
                case BagVariantType.CasinoPattern:
                    {
                        return "ch_p_m_bag_var06_arm_s";
                    }
                case BagVariantType.CasinoGeometric2:
                    {
                        return "ch_p_m_bag_var07_arm_s";
                    }
                case BagVariantType.CasinoAggressive1:
                    {
                        return "ch_p_m_bag_var08_arm_s";
                    }
                case BagVariantType.CasinoAggressive2:
                    {
                        return "ch_p_m_bag_var09_arm_s";
                    }
                case BagVariantType.CasinoAggressive3:
                    {
                        return "ch_p_m_bag_var10_arm_s";
                    }
            }

            return "NULL";
        }

        public static Dictionary<int, int> GetBagDrawableAndTexture(BagVariantType bagVariantType)
        {
            switch (bagVariantType)
            {
                case BagVariantType.Invalid:
                case BagVariantType.OriginalHeists:
                    {
                        return new Dictionary<int, int>() { { 45, 0 } };
                    }
                case BagVariantType.CasinoYungAncestor:
                    {
                        return new Dictionary<int, int>() { { 82, 9 } };
                    }
                case BagVariantType.CasinoRegular:
                    {
                        return new Dictionary<int, int>() { { 82, 0 } };
                    }
                case BagVariantType.CasinoMaintenance:
                    {
                        return new Dictionary<int, int>() { { 82, 1 } };
                    }
                case BagVariantType.CasinoBugstars:
                    {
                        return new Dictionary<int, int>() { { 82, 8 } };
                    }
                case BagVariantType.CasinoGeometric:
                    {
                        return new Dictionary<int, int>() { { 82, 13 } };
                    }
                case BagVariantType.CasinoPattern:
                    {
                        return new Dictionary<int, int>() { { 82, 12 } };
                    }
                case BagVariantType.CasinoGeometric2:
                    {
                        return new Dictionary<int, int>() { { 82, 15 } };
                    }
                case BagVariantType.CasinoAggressive1:
                    {
                        return new Dictionary<int, int>() { { 82, 10 } };
                    }
                case BagVariantType.CasinoAggressive2:
                    {
                        return new Dictionary<int, int>() { { 82, 11 } };
                    }
                case BagVariantType.CasinoAggressive3:
                    {
                        return new Dictionary<int, int>() { { 82, 14 } };
                    }
            }

            return null;
        }

        public static BagVariantType GetBagVariantTypeFromPed(GTA.Ped ped)
        {
            for (int i = 0; i < BAG_VARIANT_COUNT; i++)
            {
                BagVariantType type = (BagVariantType)i;
                var bag = GetBagDrawableAndTexture(type).First();

                if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped, 5) == bag.Key && Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, ped, 5) == bag.Value)
                {
                    return type;
                }
            }

            return BagVariantType.Invalid;
        }

        public static Prop CreateBagPropFromPed(GTA.Ped ped)
        {
            BagVariantType type = GetBagVariantTypeFromPed(ped);
            string bagModel = GetBagModelString(type);
            // Notification.PostTicker("Bag Variant Type: " + type.ToString() + " Bag Model: " + bagModel, true);
            return World.CreateProp(GetBagModelString(type), ped.Position, false, false);
        }

        public static void SetBagFromVariantType(GTA.Ped ped, BagVariantType bagVariantType)
        {
            var bag = GetBagDrawableAndTexture(bagVariantType).First();
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, bag.Key, bag.Value, 0);
        }

        public static void RemoveBag(GTA.Ped ped) => Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 0);
    }
}