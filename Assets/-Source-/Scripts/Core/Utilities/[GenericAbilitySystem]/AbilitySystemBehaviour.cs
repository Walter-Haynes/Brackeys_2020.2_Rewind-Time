using UnityEngine;

using JetBrains.Annotations;

namespace Core.Player.GenericAbilitySystem
{
    using CommonGames.Utilities.Extensions;

    /// <summary> The SystemBehaviour is where ALL Behaviours that *aren't* the <see cref="AbilitySystemCore{T}"/> have to inherit from. </summary>
    /// <typeparam name="TCore"> Inheritors have to define which Type of <see cref="TCore"/> they are a part of...</typeparam>
    public abstract class AbilitySystemBehaviour<TCore> : MonoBehaviour
        where TCore : AbilitySystemCore<TCore>
    {
        #region Variables

        private TCore _systemCore = null;

        [PublicAPI]
        public virtual TCore SystemCore
        {
            get => _systemCore = _systemCore.TryGetInParentIfNull(context: this);
            protected set => _systemCore = value;
        }

        [UsedImplicitly]
        public virtual bool IsInitialized { get; protected set; } = false;

        #endregion

        #region Methods

        [PublicAPI]
        public virtual void SetSystemCoreReference(in TCore systemCore)
        {
            SystemCore = systemCore;
            IsInitialized = true;
        }

        #endregion
    }
}
