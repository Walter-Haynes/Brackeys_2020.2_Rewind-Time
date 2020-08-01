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

        private Rigidbody2D _rigidbody2D;

        private List<RecordingFrame> _recordingFrames = new List<RecordingFrame>();

        private bool
            _isRecording = false,
            _isReplaying = false;

        #endregion
        
        #region ReplayObject

        //[ShowIf(nameof(_usePrefab))]
        [AssetsOnly]
        [SerializeField] private GameObject replayPrefab;
        
        private Rigidbody2D _replayObject;
        
        private bool _usePrefab = false;

        [ContextMenu(itemName: "Toggle Prefab Usage")]
        private void ToggleUsePrefab()
            => _usePrefab = !_usePrefab;

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

            RecordingManager.Instance.StartRecording_Event      += StartRecording;
            RecordingManager.Instance.StopRecording_Event       += StopRecording;
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

                CreateReplayObject();

                PlayFrame();
            }
        }

        private void RecordFrame()
        {
            if(_replayObject)
            {
                _replayObject.isKinematic = false;   
            }

            Transform __transform = transform;
            _recordingFrames.Add(item: new RecordingFrame(position: __transform.position, rotation: __transform.rotation));
        }

        private int _currentReplayFrameIndex = 0;

        private void PlayFrame()
        {
            _replayObject.isKinematic = true;

            if(_currentReplayFrameIndex >= _recordingFrames.Count)
            {
                StopPlayRecording();
                return;
            }

            RecordingFrame __frame = _recordingFrames[index: _currentReplayFrameIndex];

            //foreach(RecordingFrame __frame in _recordingFrames)

            Transform __transform = _replayObject.transform;

            __transform.position = __frame.Position;
            __transform.rotation = __frame.Rotation;
            
            _currentReplayFrameIndex++;

        }

        private void CreateReplayObject()
        {
            //Use this object to replay 
            if(!replayPrefab)
            {
                _replayObject = this._rigidbody2D;
            }
            else if(_replayObject == null || _replayObject == this._rigidbody2D)
            {
                if(Instantiate(original: replayPrefab).TryGetComponent(out Rigidbody2D __spawnedRigidbody))
                {
                    _replayObject = __spawnedRigidbody;
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