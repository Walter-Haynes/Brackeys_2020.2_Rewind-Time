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

        [SerializeField] private InputAction toggleRecord, toggleReplay;

        [PublicAPI]
        public event Action ToggleRecord_Event, ToggleReplay_Event;

        #endregion

        private void Awake()
        {
            toggleRecord.started += OnToggleRecord;
            toggleReplay.started += OnToggleReplay;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            toggleRecord.Enable();
            toggleReplay.Enable();
        }

        protected override void OnDisable()
        {
            toggleRecord.Disable();
            toggleReplay.Disable();
        }

        private void OnToggleRecord(InputAction.CallbackContext ctx)
            => ToggleRecord_Event?.Invoke();

        private void OnToggleReplay(InputAction.CallbackContext ctx)
            => ToggleReplay_Event?.Invoke();
    }
}