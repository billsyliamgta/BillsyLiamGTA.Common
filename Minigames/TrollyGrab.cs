using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.Ped;
using BillsyLiamGTA.Common.Audio;

namespace BillsyLiamGTA.Common.Minigames
{
    public class TrollyGrab : BaseMinigame
    {
        #region Properties

        private Prop Cart;

        private Prop Hand;

        private bool EnableCasinoBlackjackCamera = false;

        public enum Variants
        {
            Invalid = -1,
            Cash,
            Coke,
            Gold,
            Diamond
        }

        public Variants Variant { get; set; }

        private Prop Bag;

        private SynchronizedScene PlayerScene;

        private SynchronizedScene CartScene;

        #endregion

        #region Constructor

        public TrollyGrab(Prop cart, Variants variant)
        {
            Cart = cart;
            Variant = variant;
            if (variant == Variants.Invalid) throw new ArgumentException("ERROR: TrollyGrab object created with the variant type 'Invalid'.");
            MaxRate = variant == Variants.Gold ? 1.2f : 1.5f;
        }

        #endregion

        #region Functions

        public void GiveBlip(BlipSprite sprite, BlipColor colour, string name, float scale = 1f, bool isShortRange = true)
        {
            if (Cart != null && Cart.Exists())
            {
                Blip blip = Cart.AddBlip();
                blip.Sprite = sprite;
                blip.Scale = scale;
                blip.Color = colour;
                blip.IsShortRange = isShortRange;
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
            if (Hand != null && Hand.Exists())
            {
                Hand.Delete();
                Hand = null;
            }
            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
            PlayerScene.Generate();
            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "exit", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_exit", 1f, -1f);
            CartScene.Rate = 0f;
            Index = 4;
        }

        private string GetHelpTextGXTEntry(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        switch (Variant)
                        {
                            case Variants.Cash:
                                {
                                    return "MC_GRAB_1"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the cash. */
                                }
                            case Variants.Coke:
                                {
                                    return "MC_GRAB_6"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the drugs. */
                                }
                            case Variants.Gold:
                                {
                                    return "MC_GRAB_2"; /* GXT: Press ~INPUT_CONTEXT~ to begin grabbing the gold. */
                                }
                            case Variants.Diamond:
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
                            case Variants.Cash:
                                {
                                    return "MC_GRAB_3_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the cash. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the cash. */
                                }
                            case Variants.Coke:
                                {
                                    return "MC_GRAB_5_MK"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the drugs. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the drugs. */
                                }
                            case Variants.Gold:
                                {
                                    return "MC_GRAB_4B"; /* GXT: Repeatedly tap ~INPUT_CURSOR_ACCEPT~ to quickly grab the gold. Press ~INPUT_CURSOR_CANCEL~ to stop grabbing the gold. */
                                }
                            case Variants.Diamond:
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
                            case Variants.Cash:
                                {
                                    return "MC_GRAB_3"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the cash. */
                                }
                            case Variants.Coke:
                                {
                                    return "MC_GRAB_5"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the drugs. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the drugs. */
                                }
                            case Variants.Gold:
                                {
                                    return "MC_GRAB_4"; /* GXT: Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the gold. Press ~INPUT_FRONTEND_CANCEL~ to stop grabbing the gold. */
                                }
                            case Variants.Diamond:
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
                case Variants.Cash:
                    {
                        return "hei_prop_heist_cash_pile";
                    }
                case Variants.Coke:
                    {
                        return "imp_prop_impexp_coke_pile";
                    }
                case Variants.Gold:
                    {
                        return "ch_prop_gold_bar_01a";
                    }
                case Variants.Diamond:
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
                        if (IsLooted) return;
                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "MC_PLAY", 0);
                        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@grab_cash") && Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "MC_PLAY", 0) && Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3) && !IsLooted)
                        {
                            Vector3 offsetPos = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2);
                            Vector3 offsetRot = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@heists@ornate_bank@grab_cash", "intro", Cart.Position.X, Cart.Position.Y, Cart.Position.Z, Cart.Rotation.X, Cart.Rotation.Y, Cart.Rotation.Z, 0f, 2);
                            if (Game.Player.Character.Position.DistanceTo(offsetPos) < 1f)
                            {
                                Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, GetHelpTextGXTEntry(0), true);
                                if (Game.IsControlJustPressed(Control.Context))
                                {
                                    Screen.ClearHelpText();
                                    Game.Player.Character.CanSwitchWeapons = false;
                                    Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                                    SetInProgress(true);
                                    AudioScene.Start("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                                    Hand = World.CreateProp(GetHandModel(), Game.Player.Character.Position, false, false);
                                    if (Hand != null && Hand.Exists())
                                    {
                                        Hand.IsCollisionEnabled = false;
                                        Hand.IsVisible = false;
                                        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Hand, Game.Player.Character, Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character, 60309), 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
                                    }
                                    PreviousBagType = BagManager.GetBagVariantTypeFromPed(Game.Player.Character);
                                    Bag = BagManager.CreateBagPropFromPed(Game.Player.Character);
                                    if (Bag != null && Bag.Exists())
                                    {
                                        Bag.IsCollisionEnabled = false;
                                        Bag.IsVisible = false;
                                    }
                                    EnableCasinoBlackjackCamera = true;
                                    Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, offsetPos.X, offsetPos.Y, offsetPos.Z, 1f, 20000, offsetRot.Z, 0.75f);
                                    Index++;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD")) == 7)
                        {
                            PlayerScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "intro", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                            if (Bag != null && Bag.Exists())
                            {
                                PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1000f, -8f);
                                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                            }
                            CartScene = new SynchronizedScene(Cart.Position, Cart.Rotation);
                            CartScene.Generate();
                            if (Cart != null && Cart.Exists())
                            {
                                CartScene.PlayEntity(Cart, "anim@heists@ornate_bank@grab_cash", "cart_cash_dissapear", 1000f, -4f, SynchronizedScene.PlaybackFlags.USE_PHYSICS, 0);
                                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Cart);
                            }
                            CartScene.Rate = 0f;
                            CartScene.Phase = Data.Phase;
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (Bag != null && Bag.Exists() && !Bag.IsVisible && PlayerScene != null && PlayerScene.Phase > 0f)
                        {
                            BagManager.RemoveBag(Game.Player.Character);
                            Bag.IsVisible = true;
                        }

                        if (PlayerScene != null && PlayerScene.IsFinished) Index++;
                    }
                    break;
                case 3:
                    {
                        if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab", 3))
                        {
                            PlayerScene.Rate = Data.f_14;
                            CartScene.Rate = Data.f_14;
                            Data.Phase = PlayerScene.Phase;

                            if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "CASH_APPEAR")))
                            {
                                if (Hand != null && Hand.Exists() && !Hand.IsVisible) 
                                    Hand.IsVisible = true;
                            }

                            if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "RELEASE_CASH_DESTROY")))
                            {
                                int min = 0, max = 0;
                                switch (Variant)
                                {
                                    case Variants.Cash:
                                        min = 10000;
                                        max = 15000;
                                        break;

                                    case Variants.Coke:
                                        min = 25000;
                                        max = 30000;
                                        break;

                                    case Variants.Gold:
                                        min = 40000;
                                        max = 45000;
                                        break;

                                    case Variants.Diamond:
                                        min = 60000;
                                        max = 70000;
                                        break;
                                }
                                ValueAdded?.Invoke(this, new MinigameValueAddedArgs(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, min, max), true));
                                if (Hand != null && Hand.Exists() && Hand.IsVisible) Hand.IsVisible = false;
                                if (Data.f_14 == 0.75f)
                                {
                                    PlayerScene.Generate();
                                    PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 1.5f, -8.0f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                    Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, Game.Player.Character, false, false);
                                    if (Bag != null && Bag.Exists())
                                    {
                                        PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab_idle", 1f, -1f);
                                        Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Bag);
                                    }
                                    PlayerScene.SetLooped(true);
                                    CartScene.Rate = 0f;
                                }
                            }

                            if (Data.Phase > 0.999f)
                            {
                                RemoveBlip();
                                IsLooted = true;
                                PlayExitSequence();
                            }
                        }
                        else if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 3))
                        {
                            if (Data.f_14 > 0.75f)
                            {
                                PlayerScene.Generate();
                                PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab", 4f, -4f, SynchronizedScene.PlaybackFlags.UNKNOWN, SynchronizedScene.RagdollBlockingFlags.UNKNOWN, 1000f, SynchronizedScene.IkControlFlags.LINKED_FACIAL);
                                PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab", 2f, -8f);
                                PlayerScene.Phase = Data.Phase;
                            }

                            if (Game.IsControlJustPressed(Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2 /*FRONTEND_CONTROL*/) ? Control.CursorCancel : Control.FrontendCancel)) PlayExitSequence();
                        }
                        else
                        {
                            PlayerScene.Generate();
                            PlayerScene.PlayPed(Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "grab_idle", 2f, -8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            PlayerScene.PlayEntity(Bag, "anim@heists@ornate_bank@grab_cash", "bag_grab_idle", 2f, -4f);
                            PlayerScene.SetLooped(true);
                        }
                    }
                    break;
                case 4:
                    {
                        if (PlayerScene != null && PlayerScene.IsFinished)
                        {
                            if (Bag != null && Bag.Exists())
                            {
                                Bag?.Delete();
                                Bag = null;
                            }
                            BagManager.SetBagFromVariantType(Game.Player.Character, PreviousBagType);
                            PlayerScene?.Dispose();
                            PlayerScene = null;
                            CartScene?.Dispose();
                            CartScene = null;
                            Game.Player.Character.CanSwitchWeapons = true;
                            SetInProgress(false);
                            AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
                            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
                            Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);                        
                            Game.Player.SetControlState(true);
                            Game.Player.Character.Task.ClearAll();
                            Index = 0;
                        }
                    }
                    break;
            }
        }

        public override void PushDeathResetFunction()
        {
            base.PushDeathResetFunction();
            if (Hand != null && Hand.Exists())
            {
                Hand.Delete();
                Hand = null;
            }
            EnableCasinoBlackjackCamera = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            PlayerScene?.Dispose();
            PlayerScene = null;
            CartScene?.Dispose();
            CartScene = null;
            if (AudioScene.IsActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE")) 
                AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 0)) 
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3)) 
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Cart != null && Cart.Exists())
            {
                if (Cart.AttachedBlip != null && Cart.AttachedBlip.Exists())
                {
                    Cart.AttachedBlip.Delete();
                }

                Cart.Delete();
                Cart = null;
            }
            if (Hand != null && Hand.Exists())
            {
                Hand.Delete();
                Hand = null;
            }
            EnableCasinoBlackjackCamera = false;
            if (Bag != null && Bag.Exists())
            {
                Bag.Delete();
                Bag = null;
            }
            PlayerScene?.Dispose();
            PlayerScene = null;
            CartScene?.Dispose();
            CartScene = null;
            if (AudioScene.IsActive("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE")) 
                AudioScene.Stop("DLC_HEIST_MINIGAME_PAC_CASH_GRAB_SCENE");
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim@heists@ornate_bank@grab_cash");
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 0)) 
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 0);
            if (Function.Call<bool>(Hash.HAS_ADDITIONAL_TEXT_LOADED, 3)) 
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 3);
        }

        #endregion
    }
}