using GTA;
using GTA.Native;
using System;

namespace BillsyLiamGTA.Common.Scaleform
{
    public abstract class BaseScaleform
    {
        #region Properties

        public int Handle { get; protected set; } = 0;

        public string Name { get; protected set; } = string.Empty;

        public bool HasLoaded => Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);

        public BaseScaleform(string name)
        {
            Name = name;
        }

        #endregion

        #region Functions

        public void CallFunction(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            }
        }

        public static void CallFunctionFrontend(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            }
        }

        public static void CallFunctionFrontendHeader(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
            }
        }

        private static void PushArgsInternal(params object[] args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case int i:
                        {
                            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, i);
                        }
                        break;
                    case float f:
                        {
                            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, f);
                        }
                        break;
                    case bool b:
                        {
                            Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, b);
                        }
                        break;
                    case string s:
                        {
                            Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, s);
                            Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                        }
                        break;
                }
            }
        }

        public void Load(int timeout = 2000)
        {
            int start = Game.GameTime;
            Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, Name);
            while (!HasLoaded)
            {
                if (Game.GameTime - start > timeout)
                {
                    throw new TimeoutException($"ERROR: Scaleform '{Name}' failed to load within {timeout}ms.");
                }
                Script.Wait(0);
            }
        }

        public unsafe void Dispose()
        {
            if (HasLoaded)
            {
                int handle = Handle;
                Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
                Handle = 0;
            }
        }

        public void DrawFullscreen() => Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Handle, 255, 255, 255, 255, 0);

        public void DrawFullscreenMasked(BaseScaleform scaleform) => Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, Handle, scaleform.Handle, 255, 255, 255, 255);

        #endregion
    }
}