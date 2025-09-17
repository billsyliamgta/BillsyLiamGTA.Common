using System.Linq;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using BillsyLiamGTA.Common.Elements;
using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;


namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public class FrontendLobbyMenu
    {
        #region Properties

        public const string MenuHash = "FE_MENU_VERSION_CORONA";

        /// <summary>
        /// The title of the menu.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The description of the menu.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public Dictionary<int, int> Highlights { get; set; } = null;

        public bool PauseMenuHeaderDetailsVisible { get; set; } = false;

        public bool PauseMenuHeaderTabsSelectable { get; set; } = false;

        public List<FrontendMenuHeading> Headings { get; private set; }

        public InstructionalButtons InstructionalButtons { get; private set; }

        public List<FrontendLobbyMenuBaseItem> Items {  get; private set; }

        public FrontendLobbyMenuBaseItem CurrentItem
        {
            get
            {
                if (Items != null)
                {
                    if (Items.Count > 0 && Selection >= 0 && Selection < Items.Count)
                    {
                        return Items[Selection];
                    }
                }

                return null;
            }
        }

        public FrontendLobbyMenuMissionDetails MissionDetails { get; set; } = null;

        public bool IsActive
        {
            get => Function.Call<int>(Hash.GET_CURRENT_FRONTEND_MENU_VERSION) == BaseMission.GetHashKey(MenuHash);
        }

        public bool IsReadyForControl
        {
            get => Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL);
        }

        /// <summary>
        /// The selection of the menu.
        /// </summary>
        public int Selection { get; private set; } = 0;

        /// <summary>
        /// Whether or not the <see cref="FrontendLobbyMenu"/> object has been inited.
        /// </summary>
        public bool Inited { get; private set; } = false;

        /// <summary>
        /// The game time of when the <see cref="FrontendLobbyMenu"/> object was inited.
        /// </summary>
        public int InitedGameTime { get; private set; } = 0;

        /// <summary>
        /// Whether or not the menu can be closed by pressing <see cref="Control.FrontendCancel"/>.
        /// </summary>
        public bool CanCloseMenu { get; set; } = true;

        /// <summary>
        /// Invokes when the menu is closed.
        /// </summary>
        public FrontendLobbyMenuClosedEventHandler Closed { get; set; }

        /// <summary>
        /// Makes the description icon flash when the description is updated.
        /// </summary>
        public bool FlashDescriptionIcon { get; set; } = true;

        /// <summary>
        /// Makes the description text flash when the description is updated.
        /// </summary>
        public bool FlashDescriptionText { get; set; } = false;

        #endregion

        #region Constructor

        public FrontendLobbyMenu(string title, string description, Dictionary<int, int> highlights = null)
        {
            Title = title;
            Description = description;
            Highlights = highlights;
            Headings = new List<FrontendMenuHeading>();
            InstructionalButtons = new InstructionalButtons();
            InstructionalButtons.AddButton(new Dictionary<Control, string>() { { Control.FrontendAccept, "Select" } });
            InstructionalButtons.AddButton(new Dictionary<Control, string>() { { Control.FrontendCancel, "Cancel" } });
            Items = new List<FrontendLobbyMenuBaseItem>();
        }

        #endregion

        #region Functions

        private void TakeControl() => Function.Call(Hash.TAKE_CONTROL_OF_FRONTEND);

        private void ReleaseControl() => Function.Call(Hash.RELEASE_CONTROL_OF_FRONTEND);

        private void Activate() => Function.Call(Hash.ACTIVATE_FRONTEND_MENU, BaseMission.GetHashKey(MenuHash), false, -1);

        private void Restart() => Function.Call(Hash.RESTART_FRONTEND_MENU, BaseMission.GetHashKey(MenuHash), -1);

        public bool AddHeading(FrontendMenuHeading heading)
        {
            if (Headings != null && !Headings.Contains(heading))
            {
                Headings.Add(heading);
                return true;
            }

            return false;
        }

        public bool RemoveHeading(FrontendMenuHeading heading)
        {
            if (Headings != null && Headings.Contains(heading))
            {
                Headings.Remove(heading);
                return true;
            }

            return false;
        }

        public bool AddItem(FrontendLobbyMenuBaseItem item)
        {
            if (Items != null && !Items.Contains(item))
            {
                Items.Add(item);
                return true;
            }

            return false;
        }

        public bool RemoveItem(FrontendLobbyMenuBaseItem item)
        {
            if (Items != null && Items.Contains(item))
            {
                Items.Remove(item);
                return true;
            }

            return false;
        }

        public void Show()
        {
            InstructionalButtons?.Load();
            if (!IsActive)
            {
                TakeControl();
                Activate();
            }
        }

        public void Release()
        {
            InstructionalButtons?.Dispose();          
            if (IsActive)
            {
                ReleaseControl();
                CallFunctionFrontend("KILL_PAGE");
                Activate();
            }
            Inited = false;
            Closed?.Invoke(this, new FrontendLobbyMenuClosedArgs(this));
        }

        public virtual unsafe void Process()
        {
            if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !Function.Call<bool>(Hash.IS_PAUSE_MENU_RESTARTING))
            {
                if (IsReadyForControl)
                {
                    if (!Inited)
                    {
                        CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 0);
                        CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 1);
                        CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 3);
                        Script.Wait(0);
                        if (Highlights != null)
                        {
                            var highlights = Highlights.First();
                            CallFunctionFrontendHeader("SET_ALL_HIGHLIGHTS", highlights.Key, highlights.Value);
                        }
                        CallFunctionFrontendHeader("SHOW_HEADING_DETAILS", PauseMenuHeaderDetailsVisible); // Whether or not the player card should be shown.
                        CallFunctionFrontendHeader("SHOW_MENU", !PauseMenuHeaderTabsSelectable); // Disables the pause menu header tabs from being able to be selected.
                        CallFunctionFrontendHeader("SHIFT_CORONA_DESC", !string.IsNullOrEmpty(Description), false);
                        Script.Wait(0);
                        CallFunctionFrontendHeader("SET_HEADER_TITLE", Title ?? string.Empty, false /* Some challenge boolean */, Description ?? string.Empty, false);
                        Script.Wait(0);
                        if (Headings?.Count > 0)
                        {
                            for (int i = 0; i < Headings.Count; i++)
                                CallFunctionFrontend("SET_MENU_HEADER_TEXT_BY_INDEX", i, Headings[i].Text, Headings[i].Width);
                        }
                        Script.Wait(0);
                        if (Items?.Count > 0)
                        {
                            for (int i = 0; i < Items.Count; i++)
                                Items[i].Add(i);
                        }
                        Script.Wait(0);
                        CallFunctionFrontend("DISPLAY_DATA_SLOT", 0);
                        CallFunctionFrontend("DISPLAY_DATA_SLOT", 3);
                        Script.Wait(0);
                        MissionDetails?.Show();
                        Script.Wait(0);
                        CallFunctionFrontend("SET_COLUMN_FOCUS", 0, false, false, false);
                        CallFunctionFrontend("LOCK_MOUSE_SUPPORT", true, true);
                        CallFunctionFrontend("PAGE_FADE_IN");
                        Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2 /*FRONTEND_CONTROL*/, (int)Control.FrontendAccept, 1f);
                        Script.Wait(0);
                        Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2 /*FRONTEND_CONTROL*/, (int)Control.FrontendDown, 1f);
                        Script.Wait(0);
                        Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2 /*FRONTEND_CONTROL*/, (int)Control.FrontendUp, 1f);
                        Script.Wait(0);
                        Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2 /*FRONTEND_CONTROL*/, (int)Control.FrontendAccept, 1f);
                        Script.Wait(0);
                        InitedGameTime = Game.GameTime;
                        Inited = true;
                    }
                    else
                    {
                        // Draw the instructional buttons if they exist.
                        if (InstructionalButtons != null)
                        {
                            Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
                            InstructionalButtons.Draw();
                            Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
                        }
                        
                        // If there's no items, this will return null, so its best to check its not.
                        if (CurrentItem != null)
                        {
                            CurrentItem.Process(Selection);
                            bool update = CurrentItem.PromptUpdate;
                            CurrentItem.PromptUpdate = false;

                            if (Function.Call<bool>(Hash.HAS_MENU_TRIGGER_EVENT_OCCURRED))
                            {
                                int lastItemMenuId = 0;
                                int selectedItemUniqueId = 0;
                                Function.Call(Hash.GET_MENU_TRIGGER_EVENT_DETAILS, &lastItemMenuId, &selectedItemUniqueId);
                                Selection = selectedItemUniqueId;
                                if (Game.GameTime - InitedGameTime > 100)
                                    CurrentItem?.Activated?.Invoke(this, new FrontendMenuItemActivatedArgs(CurrentItem));
                                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                                update = true;
                            }

                            if (Function.Call<bool>(Hash.HAS_MENU_LAYOUT_CHANGED_EVENT_OCCURRED))
                            {
                                int lastItemMenuId = 0;
                                int selectedItemMenuId = 0;
                                int selectedItemUniqueId = 0;
                                Function.Call(Hash.GET_MENU_LAYOUT_CHANGED_EVENT_DETAILS, &lastItemMenuId, &selectedItemMenuId, &selectedItemUniqueId);
                                Selection = selectedItemUniqueId;
                                update = true;
                            }

                            if (update)
                            {
                                CurrentItem.Update(Selection);
                                CallFunctionFrontend("SET_DESCRIPTION", 0, CurrentItem?.Description, FlashDescriptionIcon, FlashDescriptionText);
                            }
                        }

                        if (CanCloseMenu && Game.IsControlJustPressed(Control.FrontendCancel))
                        {
                            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CANCEL", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                            Release();
                        }
                    }
                }
            }          
        }

        #endregion
    }
}