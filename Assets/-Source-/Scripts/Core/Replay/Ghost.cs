using System;
using System.Collections;
using System.Collections.Generic;

using CommonGames.Utilities.CGTK;
using CommonGames.Utilities.Extensions;

using JetBrains.Annotations;
using UnityEngine;

namespace Core.Replay
{
    public class Ghost : MonoBehaviour
    {
        #region Variables

        private Rigidbody2D _rigidbody2D;

        private List<RecordingFrame> _recordingFrames = new List<RecordingFrame>();

        private bool
            _isRecording = false,
            _isReplaying = false;


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
            RecordingManager.Instance.TogglePlayRecording_Event += TogglePlayRecording;
        }

        private void StartRecording()
        {
            CGDebug.PlayDing();

            _recordingFrames = new List<RecordingFrame>();

            _isReplaying = false;
            _isRecording = true;
        }

        private void StopRecording()
        {
            CGDebug.PlayDing();

            _isRecording = false;
        }

        private void TogglePlayRecording()
        {
            if(!_isReplaying)
            {
                StartPlayRecording();
            }
            else
            {
                StopPlayRecording();
            }
        }

        private void StartPlayRecording()
        {
            CGDebug.PlayDing();

            _isRecording = false;

            _isReplaying = true;
        }

        private void StopPlayRecording()
        {
            CGDebug.PlayDing();

            _isRecording = false;

            _isReplaying = false;

            _currentReplayFrameIndex = 0;
        }


        private void FixedUpdate()
        {
            if(_isRecording && !_isReplaying)
            {
                CGDebug.Log("Record");

                RecordFrame();
            }

            if(_isReplaying && !_isRecording)
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

        private int _currentReplayFrameIndex = 0;

        private void PlayFrame()
        {
            _rigidbody2D.isKinematic = true;

            if(_currentReplayFrameIndex > _recordingFrames.Count)
            {
                StopPlayRecording();
                return;
            }

            RecordingFrame __frame = _recordingFrames[index: _currentReplayFrameIndex];

            //foreach(RecordingFrame __frame in _recordingFrames)

            Transform __transform = transform;

            __transform.position = __frame.Position;
            __transform.rotation = __frame.Rotation;


            _currentReplayFrameIndex++;

        }

        #endregion
    }
}