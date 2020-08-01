using UnityEngine;
using UnityEngine.InputSystem;

using JetBrains.Annotations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Core.Player.GenericAbilitySystem.Example.RAB
{
    using CommonGames.Utilities.Helpers;
    
    using GenericAbilitySystem;

    public abstract class BaseRABAbility : RABBehaviour, IAbility
    {
        #region Variables

        #region Shown

        [BoxGroup("Box", showLabel: false)]

        [BoxGroup("Box/Input", showLabel: false)]
        //[ValueDropdown(nameof(InputNames))]
        [LabelText("Input")]
        [SerializeField] protected InputAction _inputAction;

        #endregion

        #region Hidden
        
        public float DeltaTime { get; set; }
        
        public float FixedDeltaTime { get; set; }

        #endregion
        
        #endregion

        #region Methods

        #region Unity Event Functions

        private void OnEnable() => OnAbilityEnable();
        private void OnDisable() => OnAbilityDisable();

        private void Start() => AbilityStart();
        
        private void Update() => AbilityUpdate();
        
        private void FixedUpdate() => AbilityFixedUpdate();

        #endregion

        #region Ability Unity Functions

        /// <summary> Replacement for the default Enable function. (implements input event binding per default)</summary>
        /// <remarks> YOU HAVE TO CALL BASE.ONABILITYENABLE IF YOU DON'T MAKE CHANGE IT.</remarks>
        public virtual void OnAbilityEnable()
            => _inputAction.Enable();

        /// <summary> Replacement for the default Disable function. (implements input event un-binding per default)</summary>
        public virtual void OnAbilityDisable()
            => _inputAction.Disable();

        public virtual void AbilityStart()
            => IsInitialized = true;

        /// <remarks> Automatic saving of Time.deltaTime each frame. </remarks>
        public virtual void AbilityUpdate() 
            => DeltaTime = Time.deltaTime;
        
        /// <remarks> Automatic saving of Time.fixedDeltaTime and Time.deltaTime each frame. </remarks>
        public virtual void AbilityFixedUpdate() 
            => DeltaTime = FixedDeltaTime = Time.fixedDeltaTime;

        #endregion

        /// <summary> This function is called when there's Input for the inheriting Ability. </summary>
        protected virtual void OnAbilityInput(InputAction.CallbackContext context)
        {
            if(this.IsPrefab()) return;
            if(!Application.isPlaying) return;
            
            if(!SystemCore.MayMove) return;
            
        }

        /// <summary> Implement your actual ability code in an override of this. </summary>
        /// <remarks> Meant as a shared way of calling the ability via external code.</remarks>
        /// <remarks> Can't fake input obviously so many implementations probably won't do anything at all, for that you'll need to call <see cref="OnAbilityInput"/> </remarks>
        public virtual void DoAbility()
        {
            if(this.IsPrefab()) return;
            if(!Application.isPlaying) return;
            
            if(!SystemCore.MayMove) return;
            
        }

        #endregion

    }
}