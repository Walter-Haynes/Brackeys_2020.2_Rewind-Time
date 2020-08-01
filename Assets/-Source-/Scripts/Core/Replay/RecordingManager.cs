using System;

using UnityEngine;
using UnityEngine.InputSystem;

using JetBrains.Annotations;

namespace Core.Replay
{
    using CommonGames.Utilities;
    
    public sealed class RecordingManager : Singleton<RecordingManager>
    {
        #region Variables

        [SerializeField] private InputAction startRecording, stopRecording, playRecording;

        [PublicAPI]
        public event Action StartRecording_Event, StopRecording_Event, TogglePlayRecording_Event;

        #endregion

        private void Awake()
        {
            startRecording.started += OnStartRecordingEvent;
            stopRecording.started += OnStopRecordingEvent;
            playRecording.started += OnPlayRecordingEvent;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            startRecording.Enable();
            stopRecording.Enable();
            playRecording.Enable();
        }

        protected override void OnDisable()
        {
            startRecording.Disable();
            stopRecording.Disable();
            playRecording.Disable();
        }

        private void OnStartRecordingEvent(InputAction.CallbackContext ctx)
            => StartRecording_Event?.Invoke();

        private void OnStopRecordingEvent(InputAction.CallbackContext ctx)
            => StopRecording_Event?.Invoke();

        private void OnPlayRecordingEvent(InputAction.CallbackContext ctx)
            => TogglePlayRecording_Event?.Invoke();
    }
}