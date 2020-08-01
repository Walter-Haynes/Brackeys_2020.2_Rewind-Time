using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

namespace Core.Player.Movement.Abilities
{
    using UnityEngine.InputSystem;

    public class JumpAbility : PlayerAbility
    {
        [BoxGroup("Box")] 
        [SerializeField] private float jumpSpeed = 10;

        public override void OnAbilityEnable()
        {
            base.OnAbilityEnable();

            _inputAction.started += StartJump;
            _inputAction.canceled += EndJump;
        }

        public override void AbilityFixedUpdate()
        {
            base.AbilityFixedUpdate();

            //SystemCore.PlayerRigidbody2D
        }

        private void StartJump(InputAction.CallbackContext context) 
        {
            SystemCore.PlayerRigidbody2D.AddForce(new Vector2(0f, jumpSpeed));

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

    }
}