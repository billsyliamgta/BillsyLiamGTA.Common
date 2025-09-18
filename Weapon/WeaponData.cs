using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace BillsyLiamGTA.Common.SHVDN.Weapon
{
    /// <summary>
    /// A class for getting and setting weapon data for loadouts.
    /// </summary>
    public class WeaponData
    {
        #region Properties

        public WeaponHash Hash;

        public WeaponComponentCollection Components;

        public WeaponTint Tint;

        public int Ammo;

        public int AmmoInClip;

        #endregion

        #region Constructor

        public WeaponData(WeaponHash hash, WeaponComponentCollection components, WeaponTint tint, int ammo, int ammoInClip)
        {
            Hash = hash;
            Components = components;
            Tint = tint;
            Ammo = ammo;
            AmmoInClip = ammoInClip;
        }

        #endregion

        #region Functions

        public static List<WeaponData> GetData(GTA.Ped ped)
        {
            List<WeaponData> data = new List<WeaponData>();

            foreach (GTA.Weapon weapon in ped.Weapons)
            {
                data.Add(new WeaponData(weapon.Hash, weapon.Components, weapon.Tint, weapon.Ammo, weapon.AmmoInClip));
            }

            return data;
        }

        public static void SetFromData(GTA.Ped ped, List<WeaponData> lData)
        {
            if (ped != null && ped.Exists() && lData?.Count > 0)
            {
                foreach (WeaponData weapon in lData)
                {
                    ped.Weapons.Give(weapon.Hash, weapon.Ammo, false, true);
                    ped.Weapons[weapon.Hash].AmmoInClip = weapon.AmmoInClip;
                    ped.Weapons[weapon.Hash].Tint = weapon.Tint;
                    if (weapon.Components?.Count > 0)
                    {
                        foreach (WeaponComponent component in weapon.Components)
                        {
                            Function.Call(GTA.Native.Hash.GIVE_WEAPON_COMPONENT_TO_PED, ped.Handle, weapon.Hash, component.ComponentHash);
                        }
                    }
                }
            }
        }

        #endregion
    }
}