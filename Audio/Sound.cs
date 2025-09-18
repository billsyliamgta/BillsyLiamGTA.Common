using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.SHVDN.Audio
{
    public class Sound
    {
        #region Properties

        /// <summary>
        /// The name of the soundset.
        /// </summary>
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the sound.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Whether or not the sound should be played back in the Rockstar Editor.
        /// </summary>
        public bool ReplayEditorFlag { get; set; } = false;

        #endregion

        #region Constructor

        public Sound(string setName, string name, bool replayEditorFlag = false)
        {
            SetName = setName;
            Name = name;
            ReplayEditorFlag = replayEditorFlag;
        }

        #endregion

        #region Functions

        public void PlayFrontend(int soundId = -1) => Function.Call(Hash.PLAY_SOUND_FRONTEND, soundId, Name, SetName, ReplayEditorFlag);

        public void PlayFromCoord(int soundId, Vector3 position, int range = 0) => Function.Call(Hash.PLAY_SOUND_FROM_COORD, soundId, Name, position.X, position.Y, position.Z, SetName, false, range, ReplayEditorFlag);

        #endregion
    }
}