using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RecordingObject : MonoBehaviour
{

    #region Variables

    private Rigidbody2D _rigidbody2D;
    
    private List<PointInTime> pointsInTime = new List<PointInTime>();
    
    private readonly struct PointInTime
    {
        [PublicAPI]
        public readonly Vector2 Position;
        
        [PublicAPI]
        public readonly Quaternion Rotation;

        public PointInTime(in Vector2 position, in Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }
    
    #endregion

    #region Methods

    private void Start()
    {
        pointsInTime = default;

        if(!RecordingManager.InstanceExists) return;
        
        RecordingManager.Instance.StartRecording_Event += StartRecording;
        RecordingManager.Instance.StopRecording_Event += StopRecording;
        RecordingManager.Instance.PlayRecording_Event += PlayRecording;
    }

    private void StartRecording()
    {
        
    }
    
    private void StopRecording()
    {
        
    }

    private void PlayRecording()
    {
        
    }
    
    #endregion
}
