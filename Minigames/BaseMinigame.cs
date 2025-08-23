using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.Minigames
{
    public abstract class BaseMinigame
    {
        #region Properties

        public struct MinigameStruct
        {
            public int f_1;

            public float f_13;

            public float f_14;

            public float f_15;
        }

        public unsafe MinigameStruct Local_29575;

        public virtual float MinRate { get; set; } = 0.75f;

        public virtual float MaxRate { get; set; } = 1.5f;

        public int Index { get; set; } = 0;

        public bool IsLooted { get; set; } = false;

        public MinigameValueAddedEventHandler ValueAdded { get; set; }

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

        public virtual unsafe void Update()
        {
            if (Game.Player.Character.IsDead && Index > 0)
            {
                PushDeathResetFunc();
            }

            fixed (MinigameStruct* pData = &Local_29575)
            {
                func_7677(pData, MinRate, MaxRate);
            }
        }

        /// <summary>
        /// If the minigame is in progress, and the player dies, this function will be called.
        /// </summary>
        public virtual void PushDeathResetFunc()
        {
            Index = 0;
            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
        }

        public virtual void Dispose()
        {
            Local_29575.f_1 = 0;
            Local_29575.f_13 = 0f;
            Local_29575.f_14 = 0f;
            Local_29575.f_15 = 0f;
            MinRate = 0.75f;
            MaxRate = 1.5f;
            Index = 0;
            IsLooted = false;
        }

        #endregion
    }
}