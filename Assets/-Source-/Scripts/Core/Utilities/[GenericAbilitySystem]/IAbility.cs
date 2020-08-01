using JetBrains.Annotations;

namespace Core.Player.GenericAbilitySystem
{
    public interface IAbility
    {
        #region Methods

        /// <summary> Call when Enabling the Ability</summary>
        [UsedImplicitly]
        void OnAbilityEnable();
        /// <summary> Call when Disabling the Ability</summary>
        [UsedImplicitly]
        void OnAbilityDisable();
        
        /// <summary> Call when the Ability is Enabled, similar to Start() as here you should be able to assume everything is ready. </summary>
        [UsedImplicitly]
        void AbilityStart();

        /// <summary> Call when updating the Ability. </summary>
        ///<remarks> If you're going to use this call this from a MonoBehaviour's Update() function. </remarks>
        [UsedImplicitly] 
        void AbilityUpdate();
        
        /// <summary> Call when updating the Ability in a fixed time loop. </summary>
        ///<remarks> If you're going to use this call this from a MonoBehaviour's FixedUpdate() function. </remarks>
        [UsedImplicitly]
        void AbilityFixedUpdate();
        
        [PublicAPI]
        float DeltaTime { get; set; }
        [PublicAPI]
        float FixedDeltaTime { get; set; }

        #endregion
    }
}