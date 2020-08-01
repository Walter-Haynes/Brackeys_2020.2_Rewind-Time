using System;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using JetBrains.Annotations;

namespace Core.Player
{
    using CommonGames.Utilities.CustomTypes;
    using CommonGames.Utilities.Extensions;
    
    using GenericAbilitySystem;

    /// <summary> Contains references to shared <see cref="PlayerAbility"/>s and manages it's <see cref="PlayerBehaviour"/>s. </summary>
    public sealed partial class PlayerCore : AbilitySystemCore<PlayerCore>
    {
        #region Variables

        #region Shown

        #region References

        [BoxGroup("Core")] 
        [SerializeField] private Camera _playerCamera = null;
        [PublicAPI] 
        public Camera PlayerCamera 
            => _playerCamera;
        
        [BoxGroup("Core")] 
        [SerializeField] private Rigidbody2D _rigidbody = null;
        [PublicAPI]
        public Rigidbody2D PlayerRigidbody2D
            => _rigidbody = _rigidbody.TryGetIfNull(context: this);

        #endregion

        #endregion

        #region Hidden
        
        [PublicAPI]
        public CGLock MayMove { get; set; } = new CGLock(0);

        /// <summary> Is this Character Grounded? </summary>
        [PublicAPI]
        public bool IsGrounded 
            => throw new NotImplementedException();

        #endregion

        #endregion

        #region Methods

        private void Reset()
        {
            _playerCamera = Camera.main;

            _rigidbody = PlayerRigidbody2D;
        }
        
        #endregion
    }
}