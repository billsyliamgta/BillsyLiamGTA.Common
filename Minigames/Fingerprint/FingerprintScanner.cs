using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using BillsyLiamGTA.Common.Ped;

namespace BillsyLiamGTA.Common.Minigames.Fingerprint
{
    public class FingerprintScanner
    {
        #region Properties

        public int Index { get; set; } = 0;

        public Prop Keypad { get; set; }

        public Prop USB { get; set; }

        public Prop Phone { get; set; }

        public SynchronizedScene Scene { get; set; }

        public FingerprintMinigame Minigame;

        #endregion

        #region Constructor

        public FingerprintScanner(Vector3 position, float heading = 0f, int printsToHack = 3)
        {
            Keypad = World.CreatePropNoOffset("ch_prop_fingerprint_scanner_01c", position, new Vector3(0f, 0f, heading), false);
            Keypad.IsInvincible = true;
            Keypad.IsPositionFrozen = true;
            Minigame = new FingerprintMinigame(printsToHack);
        }

        #endregion

        #region Functions

        public void Update()
        {
            bool isDead = Game.Player.Character.IsDead;

            switch (Index)
            {
                case 0:
                    {
                        Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig1_hack_keypad@male@");
                        if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim_heist@hs3f@ig1_hack_keypad@male@"))
                        {
                            Vector3 posOffset = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                            Vector3 rotOffset = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", Keypad.Position.X, Keypad.Position.Y, Keypad.Position.Z, Keypad.Rotation.X, Keypad.Rotation.Y, Keypad.Rotation.Z, 0f, 2);
                            if (!isDead && Game.Player.Character.Position.DistanceTo(posOffset) < 1.5f && Minigame.State != FingerprintMinigame.FingerprintMinigameState.Successful)
                            {
                                Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to start hacking.");
                                if (Game.IsControlJustPressed(Control.Context))
                                {
                                    Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), true);
                                    Function.Call(Hash.SET_PED_USING_ACTION_MODE, Game.Player.Character, false, "DEFAULT_ACTION");
                                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Game.Player.Character, posOffset.X, posOffset.Y, posOffset.Z, 1f, 5000, rotOffset.Z, 0.1f);
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
                            Model usbModel = new Model("ch_prop_ch_usb_drive01x");
                            usbModel.Request(5000);
                            USB = World.CreateProp(usbModel, Game.Player.Character.Position, false, false);
                            usbModel.MarkAsNoLongerNeeded();
                            USB.IsCollisionEnabled = false;
                            Model phoneModel = new Model("prop_phone_ing");
                            phoneModel.Request(5000);
                            Phone = World.CreateProp(phoneModel, Game.Player.Character.Position, false, false);
                            phoneModel.MarkAsNoLongerNeeded();
                            Phone.IsCollisionEnabled = false;
                            Scene = new SynchronizedScene(Keypad.Position, Keypad.Rotation);
                            Scene.Generate();
                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01_ch_prop_ch_usb_drive01x");
                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01_prop_phone_ing");
                            Index++;
                        }
                    }
                    break;
                case 2:
                    {
                        if (Scene.Phase > 0.999f)
                        {
                            Scene.Generate();
                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01_ch_prop_ch_usb_drive01x");
                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "hack_loop_var_01_prop_phone_ing");
                            Scene.SetLooped(true);
                            Index++;
                        }
                    }
                    break;
                case 3:
                    {
                        Minigame.Update();

                        if (Minigame.IsInactive || isDead)
                        {
                            Scene.Generate();

                            if (!isDead && USB != null && USB.Exists() && Phone != null && Phone.Exists())
                            {
                                switch (Minigame.State)
                                {
                                    case FingerprintMinigame.FingerprintMinigameState.Aborted:
                                        {
                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit_ch_prop_ch_usb_drive01x");
                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit_prop_phone_ing");
                                        }
                                        break;
                                    case FingerprintMinigame.FingerprintMinigameState.Failed:
                                        {
                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react_ch_prop_ch_usb_drive01x");
                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "fail_react_prop_phone_ing");
                                        }
                                        break;
                                    case FingerprintMinigame.FingerprintMinigameState.Successful:
                                        {
                                            Scene.PlayPed(Game.Player.Character, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01", 8f, 8f, SynchronizedScene.PlaybackFlags.HIDE_WEAPON);
                                            Scene.PlayEntity(USB, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01_ch_prop_ch_usb_drive01x");
                                            Scene.PlayEntity(Phone, "anim_heist@hs3f@ig1_hack_keypad@male@", "success_react_exit_var_01_prop_phone_ing");
                                        }
                                        break;
                                }
                            }

                            Minigame.Dispose();
                            Index++;
                        }
                    }
                    break;
                case 4:
                    {
                        if (Scene.Phase > 0.999f || isDead)
                        {
                            if (USB != null && USB.Exists())
                            {
                                USB.Delete();
                                USB = null;
                            }

                            if (Phone != null && Phone.Exists())
                            {
                                Phone.Delete();
                                Phone = null;
                            }

                            Game.Player.Character.Task.ClearAll();
                            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig1_hack_keypad@male@");
                            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Function.Call<int>(Hash.PLAYER_ID), false);
                            Index = 0;
                        }
                    }
                    break;
            }
        }

        public void Dispose()
        {
            Index = 0;

            if (Keypad != null && Keypad.Exists())
            {
                Keypad?.Delete();
                Keypad = null;
            }

            if (USB != null && USB.Exists())
            {
                USB.Delete();
                USB = null;
            }

            if (Phone != null && Phone.Exists())
            {
                Phone.Delete();
                Phone = null;
            }

            Scene?.Dispose();
            Scene = null;
            Minigame?.Dispose();
            Minigame = null;
            Function.Call(Hash.REMOVE_ANIM_DICT, "anim_heist@hs3f@ig1_hack_keypad@male@");
            Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);
        }

        #endregion
    }
}