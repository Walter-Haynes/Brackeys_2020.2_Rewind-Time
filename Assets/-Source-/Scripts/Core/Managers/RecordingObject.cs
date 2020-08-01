using System;
using System.Collections;
using System.Collections.Generic;
using CommonGames.Utilities.CGTK;
using CommonGames.Utilities.Extensions;
using JetBrains.Annotations;
using UnityEngine;

public class RecordingObject : MonoBehaviour
{

    #region Variables

    private Rigidbody2D _rigidbody2D;
    
    private List<RecordingFrame> _recordingFrames = new List<RecordingFrame>();

    private bool 
        _isRecording = false, 
        _isReplaying = false;
    
    private readonly struct RecordingFrame
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
    
    #endregion

    #region Methods

    private void Reset()
    {
        _rigidbody2D = _rigidbody2D.TryGetIfNull(context: this);
    }

    private void Start()
    {
        //_recordingFrames = default;
        
        _rigidbody2D = _rigidbody2D.TryGetIfNull(context: this);

        if(!RecordingManager.InstanceExists) return;
        
        RecordingManager.Instance.StartRecording_Event += StartRecording;
        RecordingManager.Instance.StopRecording_Event += StopRecording;
        RecordingManager.Instance.PlayRecording_Event += PlayRecording;
    }

    private void StartRecording()
    {
        CGDebug.PlayDing();
        
        _isReplaying = false;
        _isRecording = true;
    }
    private void StopRecording()
    {
        CGDebug.PlayDing();
        
        _isRecording = false;
    }
    private void PlayRecording()
    {
        CGDebug.PlayDing();
        
        _isRecording = false;
        
        _isReplaying = !_isReplaying;
    }

    private void FixedUpdate()
    {
        if(_isRecording && !_isReplaying)
        {
            CGDebug.Log("Record");
            
            RecordFrame();   
        }
        else if(_isReplaying)
        {
            CGDebug.Log("Replay");
            
            PlayFrame();
        }
    }
    
    private void RecordFrame()
    {
        _rigidbody2D.isKinematic = false;
        
        Transform __transform = transform;
        _recordingFrames.Add(item: new RecordingFrame(__transform.position, __transform.rotation));
    }
    
    private void PlayFrame()
    {
        _rigidbody2D.isKinematic = true;
        
        foreach(RecordingFrame __frame in _recordingFrames)
        {
            Transform __transform = transform;
            
            __transform.position = __frame.Position;
            __transform.rotation = __frame.Rotation;
        }
    }
    
    #endregion
}
