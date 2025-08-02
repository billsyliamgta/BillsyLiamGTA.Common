using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BillsyLiamGTA.Common.Ped
{
    public class SyncScene : IDisposable
    {
        #region Enums

        public enum PlaybackFlags
        {
            NONE = 0,
            USE_PHYSICS = 1,
            TAG_SYNC_OUT = 2,
            DONT_INTERRUPT = 4,
            ON_ABORT_STOP_SCENE = 8,
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

        public enum RagdollBlockingFlags
        {
            NONE = 0,
            BULLET_IMPACT = 1,
            VEHICLE_IMPACT = 2,
            FIRE = 4,
            ELECTROCUTION = 8,
            PLAYER_IMPACT = 16,
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

        #region Properties

        public int Handle { get; protected set; } = -1;

        public bool IsValid => Handle != -1;

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

        public Vector3 Position { get; set; } = Vector3.Zero;

        public Vector3 Rotation { get; set; } = Vector3.Zero;

        #endregion

        #region Functions

        public void Generate(int timeout = 2000)
        {
            Handle = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Position.X, Position.Y, Position.Z, Rotation.X, Rotation.Y, Rotation.Z, 2);
            int start = Game.GameTime;
            while (!IsValid)
            {
                if (Game.GameTime - start > timeout)
                {
                    throw new TimeoutException($"Sync scene failed to validate within {timeout}ms.");
                }
                Script.Wait(0);
            }
        }

        public void PlayPed(GTA.Ped ped, string animDict, string animName, float blendIn = 8f, float blendOut = 8f, PlaybackFlags playbackFlags = PlaybackFlags.NONE, RagdollBlockingFlags ragdollFlags = RagdollBlockingFlags.NONE, float moveBlend = 0x447a0000, IkControlFlags ikFlags = IkControlFlags.NONE)
        {
            if (IsValid)
                Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped.Handle, animDict, animName, blendIn, blendOut, (int)playbackFlags, (int)ragdollFlags, moveBlend, (int)ikFlags);
        }

        public void Dispose()
        {
            if (IsValid)
            {
                Function.Call(Hash.TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE, Handle);
                Handle = -1;
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}