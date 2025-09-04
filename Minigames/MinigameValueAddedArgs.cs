using GTA.Native;

namespace BillsyLiamGTA.Common.Minigames
{
    /// <summary>
    /// Represents the arguments of a minigame value being added.
    /// </summary>
    public class MinigameValueAddedArgs
    {
        public int Value { get; set; }

        public bool PlayFrontendSound { get; set; }

        public MinigameValueAddedArgs(int value, bool playFrontendSound)
        {
            Value = value;
            PlayFrontendSound = playFrontendSound;

            if (playFrontendSound)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);
            }
        }
    }
}