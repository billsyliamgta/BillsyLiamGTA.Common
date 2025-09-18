using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Native;
using static BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars.TimerBarHelpers;

namespace BillsyLiamGTA.Common.SHVDN.Graphics.TimerBars
{
    public class TimerBarPool : Script
    {
        #region Properties

        private static List<BaseTimerBar> _bars = new List<BaseTimerBar>();

        public static bool ShouldMoveUp { get; set; } = false;

        #endregion

        #region Constructor

        public TimerBarPool()
        {
            Tick += OnTick;
        }

        #endregion

        #region Functions

        public static void Add(BaseTimerBar bar)
        {
            if (!Contains(bar))
            {
                _bars.Add(bar);
            }
        }

        public static void Remove(BaseTimerBar bar)
        {
            if (Contains(bar))
            {
                _bars.Remove(bar);
            }
        }

        public static void Clear()
        {
            if (_bars?.Count > 0)
            {
                _bars.Clear();
                _bars = null;
            }    
        }

        public static bool Contains(BaseTimerBar bar)
        {
            if (_bars != null)
            {
                if (_bars.Contains(bar))
                {
                    return true;
                }
            }

            return false;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_bars?.Count > 0 && Function.Call<bool>(Hash.IS_HUD_PREFERENCE_SWITCHED_ON) && !Function.Call<bool>(Hash.IS_HUD_HIDDEN) && !Function.Call<bool>(Hash.IS_RADAR_HIDDEN) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.Player.Character.IsAlive)
            {
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
                if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
                {
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 6);
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 7);
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 8);
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 9);
                    Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 82, 66);
                    Function.Call(Hash.SET_SCRIPT_GFX_ALIGN_PARAMS, 0, 0, GfxAlignWidth, GfxAlignHeight);
                    float drawY = LoadingPrompt.IsActive || ShouldMoveUp ? InitialBusySpinnerY : InitialY;
                    for (int i = 0; i < _bars.Count; i++)
                    {
                        BaseTimerBar bar = _bars[i];
                        bar.Draw(drawY);
                        drawY -= bar.Thin ? TimerBarThinMargin : TimerBarMargin;
                    }
                    Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
                }
            }
        }

        #endregion
    }
}