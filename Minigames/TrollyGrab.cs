using System;
using GTA;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.Ped;

namespace BillsyLiamGTA.Common.Minigames
{
    public class TrollyGrab : BaseMinigame
    {
        #region Enum

        public enum TrollyVariants
        {
            Invalid = -1,
            Cash,
            Coke,
            Gold,
            Diamond
        }

        #endregion

        #region Properties

        private Prop Cart;

        private Prop Hand;

        private bool EnableCasinoBlackjackCamera = false;

        public TrollyVariants Variant { get; set; }

        private Prop Bag;

        private SynchronizedScene PlayerScene;

        private SynchronizedScene CartScene;

        #endregion

        #region Constructor

        public TrollyGrab(Prop cart, TrollyVariants variant)
        {
            Cart = cart;
            Variant = variant;
            if (variant == TrollyVariants.Invalid) throw new ArgumentException("ERROR: Trolly Grab object created with the hand variant 'Invalid'.");
            MaxRate = variant == TrollyVariants.Gold ? 1.2f : 1.5f;
        }

        #endregion

        #region Functions

        public void GiveBlip(BlipSprite sprite, BlipColor color, string name, float scale = 1f, bool shortRange = true)
        {
            if (Cart != null && Cart.Exists())
            {
                Blip blip = Cart.AddBlip();
                blip.Sprite = sprite;
                blip.Scale = scale;
                blip.Color = color;
                blip.IsShortRange = shortRange;
                blip.Name = name;
            }
        }

        public void RemoveBlip()
        {
            if (Cart != null && Cart.Exists() && Cart.AttachedBlip != null && Cart.AttachedBlip.Exists())
                Cart.AttachedBlip.Delete();
        }

        private void PlayExitSequence()
        {
            EnableCasinoBlackjackCamera = false;
            Hand?.Delete();
            Hand = null;
            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
            PlayerScene.Generate();
            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "exit", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
            Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_exit", 1f, -1f);
            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
            CartScene.Rate = 0f;
            Index = 5;
        }

        private string GetHelpTextGXTEntry(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        switch (Variant)
                        {
                            case TrollyVariants.Cash:
                                {
                                    return "MC_GRAB_1"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the cash. */
                                }
                            case TrollyVariants.Coke:
                                {
                                    return "MC_GRAB_6"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the drugs. */
                                }
                            case TrollyVariants.Gold:
                                {
                                    return "MC_GRAB_2"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the gold. */
                                }
                            case TrollyVariants.Diamond:
                                {
                                    return "MC_GRAB_7"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the diamonds. */
                                }
                        }
                    }
                    break;
                case 1:
                    {
                        switch (Variant)
                        {
                            case TrollyVariants.Cash:
                                {
                                    return "MC_GRAB_3_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the cash. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the cash. */
                                }
                            case TrollyVariants.Coke:
                                {
                                    return "MC_GRAB_5_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the drugs. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the drugs. */
                                }
                            case TrollyVariants.Gold:
                                {
                                    return "MC_GRAB_4B"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the gold. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the gold. */
                                }
                            case TrollyVariants.Diamond:
                                {
                                    return "MC_GRAB_7_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the diamonds. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the diamonds. */
                                }
                        }
                    }
                    break;
                case 2:
                    {
                        switch (Variant)
                        {
                            case TrollyVariants.Cash:
                                {
                                    return "MC_GRAB_3"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the cash. */
                                }
                            case TrollyVariants.Coke:
                                {
                                    return "MC_GRAB_5"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the drugs. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the drugs. */
                                }
                            case TrollyVariants.Gold:
                                {
                                    return "MC_GRAB_4"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the gold. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the gold. */
                                }
                            case TrollyVariants.Diamond:
                                {
                                    return "MC_GRAB_7_TP"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the diamonds. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the diamonds. */
                                }
                        }
                    }
                    break;
            }

            return "INVALID";
        }

        private string GetHandModel()
        {
            switch (Variant)
            {
                case TrollyVariants.Cash:
                    {
                        return "hei_prop_heist_cash_pile";
                    }
                case TrollyVariants.Coke:
                    {
                        return "imp_prop_impexp_coke_pile";
                    }
                case TrollyVariants.Gold:
                    {
                        return "ch_prop_gold_bar_01a";
                    }
                case TrollyVariants.Diamond:
                    {
                        return "ch_prop_vault_dimaondbox_01a";
                    }
            }

            return "INVALID";
        }

        public override unsafe void Update()
        {
            base.Update();
            if (EnableCasinoBlackjackCamera)
            {
                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, GetHelpTextGXTEntry(Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/) ? 1 : 2), true);
                Function.Call(Hash.INVALIDATE_IDLE_CAM);
                Function.Call((Hash)0x79C0E43EB9B944E2, Function.Call<Hash>(Hash.GET_HASH_KEY, "CASINO_BLACKJACK_CAMERA"));
            }
            switch (Index)
            {
                case 0:
                    {
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "MC_PLAY", 0);
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
                        if (Game.Player.Character.Position.DistanceTo(Cart.GetOffsetPosition(new Vector3(0f, 0.5f, 0f))) < 1.5f && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "MC_PLAY", 0) && Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3) && !IsLooted)
                        {
                            Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, GetHelpTextGXTEntry(0), true);
                            if (Game.IsControlJustPressed(Control.Context))
                            {
                                Game.Player.Character.CanSwitchWeapons = false;
                                Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                                Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, true);
                                Function.Call(Hash.START_AUDIO_SCENE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                                Hand = World.CreateProp(GetHandModel(), Game.Player.Character.Position, false, false);
                                Hand.IsCollisionEnabled = false;
                                Hand.IsVisible = false;
                                Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Hand, Game.Player.Character, Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character, 60309), 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
                                Bag = World.CreateProp("hei_p_m_bag_var22_arm_s", Game.Player.Character.Position - new Vector3(0f, 0f, 5f), false, false);
                                Bag.IsCollisionEnabled = false;
                                EnableCasinoBlackjackCamera = true;
                                Index++;
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@grab_cash"))
                        {
                            Vector3 pos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2);
                            Vector3 rot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2);
                            Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, pos.X, pos.Y, pos.Z, 1f, 5000, rot.Z, 0.75f);                  
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                        {
                            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 0);   
                            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "intro", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1000f, -8f);
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                            CartScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                            CartScene.Generate();
                            CartScene.PlayEntity(Cart, "anim@heists@ornate_bank@grab_cash", "cart_cash_dissapear", 1000f, -4f, SynchronizedScene.PlaybackFlags.USE_PHYSICS, 0);
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Cart);
                            CartScene.Rate = 0f;
                            CartScene.Phase = Local_29575.f_13;
                            Index++;
                        }
                    }
                    break;
                case 3:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Index++;
                        }
                    }
                    break;
                case 4:
                    {
                        if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab", 3))
                        {
                            PlayerScene.Rate = Local_29575.f_14;
                            CartScene.Rate = Local_29575.f_14;
                            Local_29575.f_13 = PlayerScene.Phase;

                            if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "CASH_APPEAR")))
                            {
                                Hand.IsVisible = true;
                            }

                            if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "RELEASE_CASH_DESTROY")))
                            {
                                int min = 0, max = 0;
                                switch (Variant)
                                {
                                    case TrollyVariants.Cash:
                                        {
                                            min = 2000;
                                            max = 3000;
                                        }
                                        break;
                                    case TrollyVariants.Gold:
                                        {
                                            min = 9000;
                                            max = 10000;
                                        }
                                        break;
                                    case TrollyVariants.Diamond:
                                        {
                                            min = 12000;
                                            max = 13000;
                                        }
                                        break;
                                }
                                ValueAdded?.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max), true));
                                Hand.IsVisible = false;
                                if (Local_29575.f_14 == 0.75f)
                                {
                                    PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                                    PlayerScene.Generate();
                                    PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                    Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                                    PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab_idle", 1f, -1f);
                                    Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                                    PlayerScene.SetLooped(true);
                                    CartScene.Rate = 0f;
                                }
                            }

                            if (Local_29575.f_13 > 0.999f)
                            {
                                RemoveBlip();
                                IsLooted = true;
                                PlayExitSequence();
                            }
                        }
                        else if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 3))
                        {
                            if (Local_29575.f_14 > 0.75f)
                            {
                                PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab", 4f, -4f, SynchronizedScene.PlaybackFlags.UNKNOWN, SynchronizedScene.RagdollBlockingFlags.UNKNOWN, 1000f, SynchronizedScene.IkControlFlags.LINKED_FACIAL);
                                Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                                PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab", 2f, -8f);
                                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                                PlayerScene.Phase = Local_29575.f_13;
                            }

                            if (Game.IsControlJustPressed(Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/) ? Control.CursorCancel : Control.FrontendCancel))
                            {
                                PlayExitSequence();
                            }
                        }
                        else
                        {
                            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 2f, -8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab_idle", 2f, -4f);
                            Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                            PlayerScene.SetLooped(true);
                        }
                    }
                    break;
                case 5:
                    {
                        if (PlayerScene.IsFinished)
                        {
                            Bag?.Delete();
                            Bag = null;
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            CartScene?.Dispose();
                            CartScene = null;
                            Game.Player.Character.CanSwitchWeapons = true;
                            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
                            Function.Call(Hash.STOP_AUDIO_SCENE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
                            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 45, 0, 0);
                            Game.Player.Character.Task.ClearAll();
                            Index = 0;
                        }
                    }
                    break;
            }
        }

        public override void PushDeathResetFunc()
        {
            base.PushDeathResetFunc();
            Hand?.Delete();
            Hand = null;
            EnableCasinoBlackjackCamera = false;
            Bag?.Delete();
            Bag = null;
            PlayerScene?.Dispose();
            PlayerScene = null;
            CartScene?.Dispose();
            CartScene = null;
            Game.Player.Character.CanSwitchWeapons = true;
            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
            if (Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE")) Function.Call(Hash.STOP_AUDIO_SCENE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 45, 0, 0);
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        public override void Dispose()
        {
            base.Dispose();
            Cart?.AttachedBlip?.Delete();
            Cart?.Delete();
            Cart = null;
            Hand?.Delete();
            Hand = null;
            EnableCasinoBlackjackCamera = false;
            Bag?.Delete();
            Bag = null;
            PlayerScene?.Dispose();
            PlayerScene = null;
            CartScene?.Dispose();
            CartScene = null;
            Game.Player.Character.CanSwitchWeapons = true;
            Function.Call(Hash.SET_MINIGAME_IN_PROGRESS, false);
            if (Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE")) Function.Call(Hash.STOP_AUDIO_SCENE, "DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        #endregion
    }
}