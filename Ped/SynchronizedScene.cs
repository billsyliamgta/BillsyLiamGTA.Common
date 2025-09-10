using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.Ped
{
    /// <summary>
    /// A class for creating and managing synchronized scenes.
    /// </summary>
    public class SynchronizedScene : IDisposable
    {
        #region Properties
        
        /// <summary>
        /// The handle of the Synchronized Scene.
        /// </summary>
        public int Handle { get; protected set; } = -1;

        /// <summary>
        /// Whether or not the Synchronized Scene is valid (i.e., has been generated successfully).
        /// </summary>
        public bool IsValid => Handle != -1;

        /// <summary>
        /// The phase of the Synchronized Scene, which represents its progress from 0.0 to 1.0.
        /// </summary>
        public float Phase
        {
            get
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Handle);
            }
            set
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Handle, value);
            }
        }

        /// <summary>
        /// The rate of the Synchronized Scene, which affects its playback speed.
        /// </summary>
        public float Rate
        {
            get
            {
                return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, Handle);
            }
            set
            {
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Handle, value);
            }
        }

        /// <summary>
        /// Whether or not the Synchronized Scene is finished.
        /// </summary>
        public bool IsFinished => Phase > 0.999f;

        /// <summary>
        /// Position of the Synchronized Scene.
        /// </summary>
        public Vector3 Position { get; set; } = Vector3.Zero;

        /// <summary>
        /// Rotation of the Synchronized Scene.
        /// </summary>
        public Vector3 Rotation { get; set; } = Vector3.Zero;

        /// <summary>
        /// The Camera object of the Synchronized Scene.
        /// </summary>
        public Camera Camera { get; protected set; } = null;

        /// <summary>
        /// A enum containing Playback Flags for Synchronized Scenes.
        /// </summary>
        public enum PlaybackFlags
        {
            NONE = 0,
            USE_PHYSICS = 1,
            TAG_SYNC_OUT = 2,
            DONT_INTERRUPT = 4,
            ON_ABORT_STOP_SCENE = 8,
            UNKNOWN = 13,
            ABORT_ON_WEAPON_DAMAGE = 16,
            BLOCK_MOVER_UPDATE = 32,
            LOOP_WITHIN_SCENE = 64,
            PRESERVE_VELOCITY = 128,
            EXPAND_PED_CAPSULE_FROM_SKELETON = 256,
            ACTIVATE_RAGDOLL_ON_COLLISION = 512,
            HIDE_WEAPON = 1024,
            ABORT_ON_DEATH = 2048,
            VEHICLE_ABORT_ON_LARGE_IMPACT = 4096,
            VEHICLE_ALLOW_PLAYER_ENTRY = 8192,
            PROCESS_ATTACHMENTS_ON_START = 16384,
            NET_ON_EARLY_NON_PED_STOP_RETURN_TO_START = 32768,
            SET_PED_OUT_OF_VEHICLE_AT_START = 65536,
            NET_DISREGARD_ATTACHMENT_CHECKS = 131072
        }

        /// <summary>
        /// A enum containing Ragdoll Blocking Flags for Synchronized Scenes.
        /// </summary>
        public enum RagdollBlockingFlags
        {
            NONE = 0,
            BULLET_IMPACT = 1,
            VEHICLE_IMPACT = 2,
            FIRE = 4,
            ELECTROCUTION = 8,
            PLAYER_IMPACT = 16,
            UNKNOWN = 18,
            EXPLOSION = 32,
            IMPACT_OBJECT = 64,
            MELEE = 128,
            RUBBER_BULLET = 256,
            FALLING = 512,
            WATER_JET = 1024,
            DROWNING = 2048,
            ALLOW_BLOCK_DEAD_PED = 4096,
            PLAYER_BUMP = 8192,
            PLAYER_RAGDOLL_BUMP = 16384,
            PED_RAGDOLL_BUMP = 32768,
            VEHICLE_GRAB = 65536
        }

        /// <summary>
        /// A enum containing Ik Control Flags for Synchronized Scenes.
        /// </summary>
        public enum IkControlFlags
        {
            NONE = 0,
            DISABLE_LEG_IK = 1,
            DISABLE_ARM_IK = 2,
            DISABLE_HEAD_IK = 4,
            DISABLE_TORSO_IK = 8,
            DISABLE_TORSO_REACT_IK = 16,
            USE_LEG_ALLOW_TAGS = 32,
            USE_LEG_BLOCK_TAGS = 64,
            USE_ARM_ALLOW_TAGS = 128,
            USE_ARM_BLOCK_TAGS = 256,
            PROCESS_WEAPON_HAND_GRIP = 512,
            USE_FP_ARM_LEFT = 1024,
            USE_FP_ARM_RIGHT = 2048,
            DISABLE_TORSO_VEHICLE_IK = 4096,
            LINKED_FACIAL = 8192
        }

        #endregion

        #region Constructor

        public SynchronizedScene(Vector3 position, float heading = 0f)
        {
            Position = position;
            Rotation = new Vector3(0f, 0f, heading);
        }

        public SynchronizedScene(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Generates the Synchronized Scene with a timeout.
        /// </summary>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public void Generate(int timeout = 2000)
        {
            Handle = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, 2);
            int start = Game.GameTime;
            while (!IsValid) // Wait for the Synchronized Scene to validate.
            {
                if (Game.GameTime - start > timeout) // And timeout if not.
                {
                    throw new TimeoutException("ERROR: Timed out while validating Synchronized Scene.");
                }
                Script.Wait(0);
            }
        }

        /// <summary>
        /// Play's the Synchronized Scene on a ped with the specified animation dictionary and name.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="animDict"></param>
        /// <param name="animName"></param>
        /// <param name="blendIn"></param>
        /// <param name="blendOut"></param>
        /// <param name="flag"></param>
        /// <param name="ragdollFlag"></param>
        /// <param name="moveBlend"></param>
        /// <param name="ikFlag"></param>
        public void PlayPed(GTA.Ped ped, string animDict, string animName, float blendIn = 8f, float blendOut = 8f, PlaybackFlags flag = PlaybackFlags.NONE, RagdollBlockingFlags ragdollFlag = RagdollBlockingFlags.NONE, float moveBlend = 0x447a0000, IkControlFlags ikFlag = IkControlFlags.NONE)
        {
            if (IsValid && ped != null && ped.Exists())
            {
                Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped, Handle, animDict, animName, blendIn, blendOut, (int)flag, (int)ragdollFlag, moveBlend, (int)ikFlag);
                Function.Call(Hash.FORCE_PED_AI_AND_ANIMATION_UPDATE, ped, false, false);
            }
        }

        /// <summary>
        /// Play's the Synchronized Scene on a entity with the specified animation dictionary and name.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="animDict"></param>
        /// <param name="animName"></param>
        /// <param name="blendIn"></param>
        /// <param name="blendOut"></param>
        /// <param name="flags"></param>
        /// <param name="moveBlend"></param>
        public void PlayEntity(Entity entity, string animDict, string animName, float blendIn = 8f, float blendOut = 8f, PlaybackFlags flags = PlaybackFlags.NONE, float moveBlend = 0x447a0000)
        {
            if (IsValid && entity != null && entity.Exists())
            {
                Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, entity, Handle, animName, animDict, blendIn, blendOut, (int)flags, moveBlend);
                Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, entity);
            }
        }

        /// <summary>
        /// Play's the Synchronized Scene on a camera with the specified animation dictionary and name.
        /// </summary>
        /// <param name="animDict"></param>
        /// <param name="animName"></param>

        public void PlayCam(string animDict, string animName)
        {
            if (IsValid)
            {
                if (Camera == null)
                {
                    Camera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", true);
                }

                Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, Camera, Handle, animName, animDict);
                ScriptCameraDirector.StartRendering();
            }
        }

        public bool PlayAudioEvent()
        {
            if (IsValid)
            {
                if (Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_AUDIO_EVENT, Handle))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Makes the Synchronized Scene looped.
        /// </summary>
        /// <param name="toggle"></param>
        public void SetLooped(bool toggle)
        {
            if (IsValid)
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, Handle, toggle);
        }

        /// <summary>
        /// Sets the Synchronized Scene to hold the last frame when it finishes.
        /// </summary>
        /// <param name="toggle"></param>

        public void SetHoldLastFrame(bool toggle)
        {
            if (IsValid)
                Function.Call(Hash.SET_SYNCHRONIZED_SCENE_HOLD_LAST_FRAME, Handle, toggle);
        }

        /// <summary>
        /// Disposes the Synchronized Scene's camera if it exists.
        /// </summary>
        /// <param name="shouldStopRendering"></param>
        public void DisposeCamera(bool shouldStopRendering)
        {
            if (Camera != null && Camera.Exists())
            {
                if (shouldStopRendering) ScriptCameraDirector.StopRendering();
                Camera.Delete();
                Camera = null;
            }
        }

        /// <summary>
        /// Dispose the Synchronized Scene if its valid.
        /// </summary>
        public void Dispose()
        {
            if (IsValid)
            {
                Function.Call(Hash.TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE, Handle);
                Handle = 0;
                DisposeCamera(true);
            }
        }

        #endregion
    }
}