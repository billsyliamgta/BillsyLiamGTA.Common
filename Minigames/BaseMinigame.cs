using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Minigames
{
    public abstract class BaseMinigame
    {
        #region Properties

        public struct MinigameStruct
        {
            public int f_1;

            public float Phase;

            public float f_14;

            public float f_15;
        }

        public unsafe MinigameStruct Data;

        public virtual float MinRate { get; set; } = 0.75f;

        public virtual float MaxRate { get; set; } = 1.5f;

        public int Index { get; set; } = 0;

        public bool NativeInProgress
        {
            get => Function.Call<bool>(Hash.IS_MINIGAME_IN_PROGRESS);
            set => Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, value);
        }

        public bool ScriptIsInProgress { get; set; } = false;

        public bool IsLooted { get; set; } = false;

        public bool EnableCasinoBlackjackCamera { get; set; } = false;

        public MinigameValueAddedEventHandler ValueAdded { get; set; }

        public BagManager.BagVariantType PreviousBagType;

        #endregion

        #region Functions     

        public float func_350(float fParam0, float fParam1, float fParam2)//Position - 0xD36B
        {
            if (fParam0 > fParam2)
            {
                return fParam2;
            }
            else if (fParam0 < fParam1)
            {
                return fParam1;
            }
            return fParam0;
        }

        public unsafe void func_7677(MinigameStruct* uParam0, float fParam1, float fParam2)//Position - 0x23375B
        {
            if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
            {
                return;
            }
            if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/))
            {
                if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2 /*FRONTEND_CONTROL*/, 237 /*INPUT_CURSOR_ACCEPT*/))
                {
                    switch (uParam0->f_1)
                    {
                        case 0:
                            uParam0->f_15 = func_350((uParam0->f_15 + 0.1f), fParam1, fParam2);
                            break;

                        case 1:
                            uParam0->f_15 = func_350((uParam0->f_15 + 0.09f), fParam1, fParam2);
                            break;

                        case 2:
                            uParam0->f_15 = func_350((uParam0->f_15 + 0.08f), fParam1, fParam2);
                            break;
                    }
                }
            }
            else if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2 /*FRONTEND_CONTROL*/, 201 /*INPUT_FRONTEND_ACCEPT*/))
            {
                switch (uParam0->f_1)
                {
                    case 0:
                        uParam0->f_15 = func_350((uParam0->f_15 + 0.1f), fParam1, fParam2);
                        break;

                    case 1:
                        uParam0->f_15 = func_350((uParam0->f_15 + 0.09f), fParam1, fParam2);
                        break;

                    case 2:
                        uParam0->f_15 = func_350((uParam0->f_15 + 0.08f), fParam1, fParam2);
                        break;
                }
            }
            switch (uParam0->f_1)
            {
                case 0:
                    uParam0->f_15 = func_350((uParam0->f_15 - ((0.01f * 30f) * Function.Call<float>(Hash.TIMESTEP))), fParam1, fParam2);
                    break;

                case 1:
                    uParam0->f_15 = func_350((uParam0->f_15 - ((0.0125f * 30f) * Function.Call<float>(Hash.TIMESTEP))), fParam1, fParam2);
                    break;

                case 2:
                    uParam0->f_15 = func_350((uParam0->f_15 - ((0.0135f * 30f) * Function.Call<float>(Hash.TIMESTEP))), fParam1, fParam2);
                    break;
            }
            func_7678(&(uParam0->f_14), uParam0->f_15, 0.02f, true);
            uParam0->f_14 = func_350(uParam0->f_14, fParam1, fParam2);
        }

        public unsafe void func_7678(float* uParam0, float fParam1, float fParam2, bool bParam3)//Position - 0x233901
        {
            float fVar0;

            if (*uParam0 != fParam1)
            {
                fVar0 = fParam2;
                if (bParam3)
                {
                    fVar0 = (0f + ((fParam2 * 30f) * Function.Call<float>(Hash.TIMESTEP)));
                }
                if ((*uParam0 - fParam1) > fVar0)
                {
                    *uParam0 = (*uParam0 - fVar0);
                }
                else if ((*uParam0 - fParam1) < -fVar0)
                {
                    *uParam0 = (*uParam0 + fVar0);
                }
                else
                {
                    *uParam0 = fParam1;
                }
            }
        }

        public void SetInProgress(bool toggle)
        {
            NativeInProgress = toggle;
            ScriptIsInProgress = toggle;
        }

        /// <summary>
        /// A virtual method that updates the minigame.
        /// </summary>
        public virtual unsafe void Update()
        {
            if (EnableCasinoBlackjackCamera)
            {
                Function.Call(Hash.INVALIDATE_IDLE_CAM);
                Function.Call((Hash)0x79C0E43EB9B944E2, Function.Call<Hash>(Hash.GET_HASH_KEY, "CASINO_BLACKJACK_CAMERA"));
            }

            if (Game.Player.Character.IsDead && ScriptIsInProgress) 
                PushDeathResetFunction();

            fixed (MinigameStruct* pData = &Data) 
                func_7677(pData, MinRate, MaxRate);
        }

        /// <summary>
        /// If the minigame is in progress, and the player dies this function will be called.
        /// </summary>
        public virtual void PushDeathResetFunction()
        {
            Index = 0;
            SetInProgress(false);
            Game.Player.Character.CanSwitchWeapons = true;
            Game.Player.SetControlState(true);
            Game.Player.Character.Task.ClearAllImmediately();
            BagManager.SetBagFromVariantType(Game.Player.Character, PreviousBagType);
        }

        public void ResetGlobals()
        {
            Data.f_1 = 0;
            Data.Phase = 0f;
            Data.f_14 = 0f;
            Data.f_15 = 0f;
            MinRate = 0.75f;
            MaxRate = 1.5f;
            Index = 0;
            SetInProgress(false);
            IsLooted = false;
            PreviousBagType = BagManager.BagVariantType.Invalid;
        }

        public virtual void Dispose() => ResetGlobals();

        #endregion
    }
}