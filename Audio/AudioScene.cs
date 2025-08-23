using GTA.Native;

namespace BillsyLiamGTA.Common.Audio
{
    public class AudioScene
    {
        public static void Start(string name) => Function.Call(Hash.START_AUDIO_SCENE, name);

        public static void Stop(string name) => Function.Call(Hash.STOP_AUDIO_SCENE, name);

        public static void StopAll() => Function.Call(Hash.STOP_AUDIO_SCENES);

        public static bool IsActive(string name) => Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, name);
    }
}