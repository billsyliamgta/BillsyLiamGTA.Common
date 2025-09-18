using System.Collections.Generic;
using GTA;
using GTA.Native;
using Ped = GTA.Ped;

namespace BillsyLiamGTA.Common.SHVDN
{
    public class CutsceneTools
    {
        public static bool IsFreemodeMale;

        public static bool IsFreemodeFemale;

        public static PedHash PlayerPreviousModel;

        public static List<string> CutscenePed1Comp = new List<string>()
        {
            { "0_0_0" },
            { "1_0_0" },
            { "2_0_0" },
            { "3_0_0" },
            { "4_0_0" },
            { "5_0_0" },
            { "6_0_0" },
            { "7_0_0" },
            { "8_0_0" },
            { "9_0_0" },
            { "10_0_0" },
            { "11_0_0" }
        };

        public static List<string> CutscenePed2Comp = new List<string>()
        {
            { "0_0_0" },
            { "1_0_0" },
            { "2_0_0" },
            { "3_0_0" },
            { "4_0_0" },
            { "5_0_0" },
            { "6_0_0" },
            { "7_0_0" },
            { "8_0_0" },
            { "9_0_0" },
            { "10_0_0" },
            { "11_0_0" }
        };

        public static List<string> CutscenePed3Comp = new List<string>()
        {
            { "0_0_0" },
            { "1_0_0" },
            { "2_0_0" },
            { "3_0_0" },
            { "4_0_0" },
            { "5_0_0" },
            { "6_0_0" },
            { "7_0_0" },
            { "8_0_0" },
            { "9_0_0" },
            { "10_0_0" },
            { "11_0_0" }
        };

        public static List<string> CutscenePed4Comp = new List<string>()
        {
            { "0_0_0" },
            { "1_0_0" },
            { "2_0_0" },
            { "3_0_0" },
            { "4_0_0" },
            { "5_0_0" },
            { "6_0_0" },
            { "7_0_0" },
            { "8_0_0" },
            { "9_0_0" },
            { "10_0_0" },
            { "11_0_0" }
        };

        public unsafe static void PlayerPedModelSet()
        {
            GTA.Ped ped = Game.Player.Character;

            if (ped.Model == PedHash.FreemodeMale01)
            {
                IsFreemodeMale = true;
                IsFreemodeFemale = false;
            }
            else if (ped.Model == PedHash.FreemodeFemale01)
            {
                IsFreemodeMale = false;
                IsFreemodeFemale = true;
            }
            else
            {
                IsFreemodeMale = false;
                IsFreemodeFemale = false;
                PlayerPreviousModel = ped.Model;
            }

            ulong num = (ulong)(long)ped.MemoryAddress;
            ulong num2 = *(ulong*)(num + 32);
            *(long*)(num2 + 24) = 3214308084L;
        }

        public unsafe static void PlayerPedModelSetBack()
        {
            GTA.Ped ped = Game.Player.Character;

            if (IsFreemodeMale)
            {
                ulong num = (ulong)(long)ped.MemoryAddress;
                ulong num2 = *(ulong*)(num + 32);
                *(long*)(num2 + 24) = 1885233650L;
            }
            else if (IsFreemodeFemale)
            {
                ulong num3 = (ulong)(long)ped.MemoryAddress;
                ulong num4 = *(ulong*)(num3 + 32);
                *(long*)(num4 + 24) = 2627665880L;
            }
            else
            {
                ulong num5 = (ulong)(long)ped.MemoryAddress;
                ulong num6 = *(ulong*)(num5 + 32);
                *(long*)(num6 + 24) = (long)PlayerPreviousModel;
            }
        }

        public static void SetPedOutfitCutscene(string MP, GTA.Ped NonCutscene)
        {
            if (MP.Equals("MP_1"))
            {
                CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 0);
                CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 1);
                CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 2);
                CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 3);
                CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 4);
                CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 5);
                CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 6);
                CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 7);
                CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 8);
                CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 9);
                CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 10);
                CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 11);
            }

            if (MP.Equals("MP_2"))
            {
                CutscenePed2Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 0);
                CutscenePed2Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 1);
                CutscenePed2Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 2);
                CutscenePed2Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 3);
                CutscenePed2Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 4);
                CutscenePed2Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 5);
                CutscenePed2Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 6);
                CutscenePed2Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 7);
                CutscenePed2Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 8);
                CutscenePed2Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 9);
                CutscenePed2Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 10);
                CutscenePed2Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 11);
            }

            if (MP.Equals("MP_3"))
            {
                CutscenePed3Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 0);
                CutscenePed3Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 1);
                CutscenePed3Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 2);
                CutscenePed3Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 3);
                CutscenePed3Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 4);
                CutscenePed3Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 5);
                CutscenePed3Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 6);
                CutscenePed3Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 7);
                CutscenePed3Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 8);
                CutscenePed3Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 9);
                CutscenePed3Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 10);
                CutscenePed3Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 11);
            }

            if (MP.Equals("MP_4"))
            {
                CutscenePed4Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 0);
                CutscenePed4Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 1);
                CutscenePed4Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 2);
                CutscenePed4Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 3);
                CutscenePed4Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 4);
                CutscenePed4Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 5);
                CutscenePed4Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 6);
                CutscenePed4Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 7);
                CutscenePed4Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 8);
                CutscenePed4Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 9);
                CutscenePed4Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 10);
                CutscenePed4Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene.Handle, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene.Handle, 11);
            }
        }
        public static void GetPedOutfitCutscene(string MP, GTA.Ped NonCutscene)
        {
            if (MP.Equals("MP_1"))
            {
                string[] part = CutscenePed1Comp[0].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 0, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[1].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 1, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[2].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 2, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[3].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 3, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[4].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 4, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[5].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 5, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[6].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 6, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[7].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 7, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[8].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 8, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[9].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 9, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[10].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 10, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed1Comp[11].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 11, int.Parse(part[1]), int.Parse(part[2]), 1);
            }

            if (MP.Equals("MP_2"))
            {
                string[] part = CutscenePed2Comp[0].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 0, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[1].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 1, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[2].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 2, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[3].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 3, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[4].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 4, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[5].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 5, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[6].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 6, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[7].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 7, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[8].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 8, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[9].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 9, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[10].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 10, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed2Comp[11].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 11, int.Parse(part[1]), int.Parse(part[2]), 1);
            }

            if (MP.Equals("MP_3"))
            {
                string[] part = CutscenePed2Comp[0].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 0, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[1].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 1, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[2].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 2, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[3].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 3, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[4].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 4, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[5].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 5, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[6].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 6, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[7].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 7, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[8].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 8, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[9].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 9, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[10].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 10, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed3Comp[11].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 11, int.Parse(part[1]), int.Parse(part[2]), 1);
            }

            if (MP.Equals("MP_4"))
            {
                string[] part = CutscenePed1Comp[0].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 0, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[1].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 1, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[2].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 2, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[3].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 3, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[4].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 4, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[5].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 5, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[6].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 6, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[7].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 7, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[8].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 8, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[9].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 9, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[10].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 10, int.Parse(part[1]), int.Parse(part[2]), 1);
                part = CutscenePed4Comp[11].Split('_');
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene.Handle, 11, int.Parse(part[1]), int.Parse(part[2]), 1);
            }
        }
    }
}