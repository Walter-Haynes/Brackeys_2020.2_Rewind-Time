using System.Collections.Generic;

using UnityEngine;

namespace Core.Replay
{
    using CommonGames.Utilities.CGTK;
    using CommonGames.Utilities.Extensions;
    using Sirenix.OdinInspector;

    public class RecordingObject : MonoBehaviour
    {

        #region Variables
        
        //[ShowIf(nameof(_usePrefab))]
        [AssetsOnly]
        [SerializeField] private GameObject replayPrefab;
        
        private Rigidbody2D 
            _recordingRigidbody, 
            _replayRigidbody;

        private List<RecordingFrame> _recordingFrames = new List<RecordingFrame>();
        private int _currentReplayFrameIndex = 0;

        private bool
            _isRecording = false,
            _isReplaying = false;

        #endregion

        #region Methods

        private void Reset()
        {
            _recordingRigidbody = _recordingRigidbody.TryGetIfNull(context: this);
        }

        private void Start()
        {
            _recordingRigidbody = _recordingRigidbody.TryGetIfNull(context: this);

            if(!RecordingManager.InstanceExists) return;

            RecordingManager.Instance.ToggleRecord_Event += ToggleRecord;
            RecordingManager.Instance.ToggleReplay_Event += ToggleReplay;
        }
        
        private void FixedUpdate()
        {
            if(_isRecording && !_isReplaying)
            {
                RecordFrame();
            }

            if(_isReplaying && !_isRecording)
            {
                CreateReplayObject();

                PlayFrame();
            }
        }

        #region Recording

        private void ToggleRecord()
        {
            if(!_isRecording)
            {
                StartRecording();   
            }
            else
            {
                StopRecording();   
            }
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

        
        
        private void RecordFrame()
        {
            if(_replayRigidbody)
            {
                _replayRigidbody.isKinematic = false;   
            }

            Transform __transform = transform;
            _recordingFrames.Add(item: new RecordingFrame(position: __transform.position, rotation: __transform.rotation));
        }


        #endregion

        #region Replaying

        private void ToggleReplay()
        {
            if(!_isReplaying)
            {
                StartReplaying();
            }
            else
            {
                StopReplaying();
            }
        }

        private void StartReplaying()
        {
            CGDebug.PlayDing();

            _isRecording = false;

            _isReplaying = true;
        }
        private void StopReplaying()
        {
            CGDebug.PlayDing();

            _isRecording = false;

            _isReplaying = false;

            _currentReplayFrameIndex = 0;
        }
        
        
        
        private void PlayFrame()
        {
            _replayRigidbody.isKinematic = true;

            if(_currentReplayFrameIndex >= _recordingFrames.Count)
            {
                StopReplaying();
                return;
            }

            RecordingFrame __frame = _recordingFrames[index: _currentReplayFrameIndex];

            Transform __transform = _replayRigidbody.transform;

            __transform.position = __frame.Position;
            __transform.rotation = __frame.Rotation;
            
            _currentReplayFrameIndex++;

        }

        #endregion

        private void CreateReplayObject()
        {
            //Use this object to replay 
            if(!replayPrefab)
            {
                _replayRigidbody = this._recordingRigidbody;
            }
            else if(!_replayRigidbody || _replayRigidbody == this._recordingRigidbody)
            {
                if(Instantiate(original: replayPrefab).TryGetComponent(out Rigidbody2D __spawnedRigidbody))
                {
                    _replayRigidbody = __spawnedRigidbody;
                }
                else
                {
                    Debug.LogError(message: "Prefab doesn't have a Rigidbody2D!!");
                }
            }
        }

        #endregion
        
    }
}