using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.Ped
{
    public class Enemy
    {
        #region Properties

        public GTA.Ped Handle { get; set; }

        public bool ProvokedMode { get; set; } = false;

        private int SpottedGameTime = 0;

        private bool Spotted = false;

        public int SpottedReactionTime { get; set; } = 2000;

        private bool _isAlerted = false;

        public bool IsAlerted
        {
            get
            {
                return _isAlerted;
            }
            set
            {
                if (Handle.AttachedBlip != null && Handle.AttachedBlip.Exists())
                {
                    Function.Call(Hash.SET_BLIP_SHOW_CONE, Handle.AttachedBlip, !value, 11);
                }

                _isAlerted = value;
            }
        }

        #endregion

        public Enemy(GTA.Ped handle)
        {
            Handle = handle;                 
            EnemyHandler.Add(this);
        }

        #region Functions

        public void Update()
        {
            if (Handle != null && Handle.Exists())
            {
                bool isAlive = Handle.IsAlive;
                bool sameInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Handle) == Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character);
                bool blipExists = false;

                if (Handle.AttachedBlip != null && Handle.AttachedBlip.Exists())
                {
                    if (isAlive)
                    {
                        Handle.AttachedBlip.Alpha = sameInterior ? 255 : 0;
                    }
                    else
                    {
                        Handle.AttachedBlip.Delete();
                    }

                    blipExists = true;
                }

                if (isAlive && sameInterior && IsAlerted && Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_COMBAT_PED")) != 1)
                {
                    if (blipExists)
                    {
                        if (Handle.AttachedBlip.Color != BlipColor.Red)
                        {
                            Handle.AttachedBlip.Color = BlipColor.Red;
                        }
                    }
                    Function.Call(Hash.TASK_COMBAT_PED, Handle, Game.Player.Character, 0, 16);
                }

                if (isAlive && sameInterior)
                {
                    if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, Handle, Game.Player.Character) || Handle.IsInCombatAgainst(Game.Player.Character) || Function.Call<int>(Hash.GET_PED_ALERTNESS, Handle) == 1 || Function.Call<int>(Hash.GET_PED_ALERTNESS, Handle) == 3 || Function.Call<bool>(Hash.CAN_PED_HEAR_PLAYER, Function.Call<int>(Hash.PLAYER_ID), Handle))
                    {
                        if (!Spotted)
                        {
                            bool shouldReact = ProvokedMode && Game.Player.Character.Weapons.Current != WeaponHash.Unarmed || !ProvokedMode;
                            if (shouldReact)
                            {
                                Handle.Task.ClearAllImmediately();
                                Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, Handle, Game.Player.Character, -1, true);
                                SpottedGameTime = Game.GameTime;
                                Spotted = true;
                            }                            
                        }
                        else
                        {
                            if (Game.GameTime - SpottedGameTime > SpottedReactionTime)
                            {
                                EnemyHandler.SetAlertedStat(true);
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (Handle.AttachedBlip != null && Handle.AttachedBlip.Exists()) Handle.AttachedBlip.Delete();
            if (Handle != null && Handle.Exists()) Handle.Delete();
            _isAlerted = false;
            SpottedGameTime = 0;
            Spotted = false;
            SpottedReactionTime = 2000;
        }

        #endregion
    }
}