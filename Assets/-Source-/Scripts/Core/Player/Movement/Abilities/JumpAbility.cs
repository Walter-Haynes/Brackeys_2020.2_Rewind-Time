using UnityEngine;
using UnityEngine.InputSystem;

using Sirenix.OdinInspector;

namespace Core.Player.Movement.Abilities
{
    using CommonGames.Utilities.Extensions;

    public class JumpAbility : PlayerAbility
    {
        #region Variables

        [BoxGroup("Box")] 
        [HorizontalGroup("Box/JumpHeights")]
        [SerializeField] private float minHeight = 1f, maxHeight = 3.5f;

        [BoxGroup("Box")]
        [SerializeField] private float timeToJump = .4f;

        private PlayerGravity _Internal_PlayerGravity;
        private PlayerGravity PlayerGravity 
            => _Internal_PlayerGravity = _Internal_PlayerGravity.TryGetIfNull(context: this);
        private float Gravity
        {
            get => PlayerGravity.Gravity;
            set => PlayerGravity.Gravity = value;
        }


        private float _gravity, _maxJumpSpeed, _minJumpSpeed;
        
        #endregion

        #region Methods

        public override void OnAbilityEnable()
        {
            base.OnAbilityEnable();
            
            SetupGravity();

            _inputAction.started += StartJump;
            _inputAction.canceled += EndJump;
        }

        private void SetupGravity()
        {
            // S = V0 * t + a * t^2 * 0.5
            // h = V0 * t + g * t^2 * 0.5
            // h = g * t^2 * 0.5
            // g = h / (t^2*0.5)

            //SystemCore.GetBehaviour<PlayerGravity>().Gravity
            
            Gravity = _gravity = -maxHeight / (timeToJump * timeToJump * 0.5f);
            _maxJumpSpeed = (Mathf.Abs(_gravity) * timeToJump) / Time.deltaTime;
            _minJumpSpeed = (Mathf.Sqrt(2 * Mathf.Abs(_gravity) * minHeight)) / Time.deltaTime;
        }

        private void StartJump(InputAction.CallbackContext context) 
        {
            SystemCore.PlayerRigidbody2D.AddForce(new Vector2(0f, _minJumpSpeed));

            /*
            if (axis.y < 0) 
            {
                character.JumpDown();
            }
            else 
            {
                character.Jump();
            }
            */
        }

        private void EndJump(InputAction.CallbackContext context) 
        {
            //character.EndJump();
        }

        #endregion

    }
}