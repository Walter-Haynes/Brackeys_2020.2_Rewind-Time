using System;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#endif

using JetBrains.Annotations;

namespace Core.Player.GenericAbilitySystem.Example.RAB
{
    using CommonGames.Utilities.CustomTypes;
    using CommonGames.Utilities.Extensions;
    
    using GenericAbilitySystem;

    /// <summary> Contains references to shared <see cref="RABAbility"/>s and manages it's <see cref="RABBehaviour"/>s. </summary>
    public sealed partial class RABCore : AbilitySystemCore<RABCore>
    {
        #region Variables

        #region Shown

        #region References

        [BoxGroup("Core")] 
        [SerializeField] private UnityEngine.Camera _playerCamera = null;
        [PublicAPI] public UnityEngine.Camera PlayerCamera 
            => _playerCamera;
        
        [BoxGroup("Core")] 
        [SerializeField] private Rigidbody _rigidbody = null;
        [PublicAPI]
        public Rigidbody PlayerRigidbody
            => _rigidbody = _rigidbody.TryGetIfNull(context: this);

        #endregion

        #endregion

        #region Hidden
        
        //[NonSerialized] public bool CanMove = true;
        
        [PublicAPI]
        public CGLock MayMove { get; set; } = new CGLock(0);

        /// <summary> Is this Character Grounded? </summary>
        [PublicAPI]
        public bool IsGrounded 
            => throw new NotImplementedException();

        #endregion

        #endregion

        #region Methods
        
        #endregion
    }
}