using GTA.Native;
using BillsyLiamGTA.Common.Elements;
using static BillsyLiamGTA.Common.Scaleform.BaseScaleform;
using GTA;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BillsyLiamGTA.Common.Scaleform.Frontend
{
    public class FrontendLobbyMenu
    {
        #region Properties

        public const string MenuHash = "FE_MENU_VERSION_CORONA";

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Dictionary<int, int> Highlights;

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

        public FrontendLobbyMenuMissionDetails MissionDetails { get; private set; }

        public bool IsActive
        {
            get => Function.Call<int>(Hash.GET_CURRENT_FRONTEND_MENU_VERSION) == Tools.joaat(MenuHash);
        }

        public bool IsReadyForControl
        {
            get => Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL);
        }

        public int Selection { get; private set; } = 0;

        public bool Inited { get; private set; } = false;

        public int InitedGameTime { get; private set; } = 0;

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

        private void Activate() => Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Tools.joaat(MenuHash), false, -1);

        private void Restart() => Function.Call(Hash.RESTART_FRONTEND_MENU, Tools.joaat(MenuHash), -1);

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

        public void AddMissionDetails(FrontendLobbyMenuMissionDetails missionDetails) => MissionDetails = missionDetails;

        public void RemoveMissionDetails(FrontendLobbyMenuMissionDetails missionDetails) => MissionDetails = null;

        public void Show()
        {
            InstructionalButtons?.Load();
            TakeControl();
            Activate();
        }

        public void Release()
        {
            InstructionalButtons?.Dispose();
            ReleaseControl();
            Activate();
            Inited = false;
        }

        public virtual unsafe void Update()
        {
            if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) && !Function.Call<bool>(Hash.IS_PAUSE_MENU_RESTARTING))
            {
                if (IsReadyForControl)
                {
                    if (!Inited)
                    {
                        CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 0);
                        CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 1);
                        Script.Wait(0);
                        if (Highlights != null)
                        {
                            var highlights = Highlights.First();
                            CallFunctionFrontendHeader("SET_ALL_HIGHLIGHTS", highlights.Key, highlights.Value);
                        }
                        CallFunctionFrontendHeader("SHOW_HEADING_DETAILS", false);
                        CallFunctionFrontendHeader("SHIFT_CORONA_DESC", true, false);
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
                        Script.Wait(0);
                        MissionDetails?.Show();
                        Script.Wait(0);
                        CallFunctionFrontend("SET_COLUMN_FOCUS", 0, false, false, false);
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
                        if (InstructionalButtons != null)
                        {
                            Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
                            InstructionalButtons.Draw();
                            Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
                        }
                        
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
                                if (Game.GameTime - InitedGameTime > 500)
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
                                CallFunctionFrontend("SET_DESCRIPTION", 0, CurrentItem?.Description ?? string.Empty, true, false);
                            }
                        }                     
                    }
                }
            }          
        }

        #endregion
    }
}