/*
* BillsyLiamGTA.Common - A ScripthookV .NET framework for Grand Theft Auto V
* Copyright (C) 2025 BillsyLiamGTA
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*/
using System;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Scaleform
{
    /// <summary>
    /// A base for creating Scaleform classes.
    /// </summary>
    public abstract class BaseScaleform
    {
        #region Properties

        /// <summary>
        /// The handle of the Scaleform.
        /// </summary>
        public int Handle { get; protected set; } = 0;

        /// <summary>
        /// The name of the Scaleform.
        /// </summary>
        public string Name { get; protected set; } = string.Empty;

        /// <summary>
        /// Whether or not the Scaleform has loaded.
        /// </summary>
        public bool HasLoaded => Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);

        #endregion

        #region Constructors
        
        public BaseScaleform(string name)
        {
            Name = name;
        }

        #endregion

        #region Functions

        public bool CallFunction(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                return true;
            }

            return false;
        }

        public static bool CallFunctionFrontend(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                return true;
            }

            return false;
        }

        public static bool CallFunctionFrontendHeader(string function, params object[] args)
        {
            if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, function))
            {
                PushArgsInternal(args);
                Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
                return true;
            }

            return false;
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
                            if (s.Length > 99)
                                Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_LITERAL_STRING, s);
                            else
                            {
                                Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, s);
                                Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                            }
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

