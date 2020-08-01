using UnityEngine;

using JetBrains.Annotations;

namespace Core.Replay
{

    public readonly struct RecordingFrame
    {
        [PublicAPI]
        public readonly Vector2 Position;

        [PublicAPI]
        public readonly Quaternion Rotation;

        public RecordingFrame(in Vector2 position, in Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }
}