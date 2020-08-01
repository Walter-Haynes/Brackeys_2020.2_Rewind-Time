using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

namespace Core.Player.Movement.Abilities
{
    public class MoveAbility : PlayerAbility
    {
        [BoxGroup("Box")] 
        [SerializeField] private float movementSpeed = 10;
        
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        
        private Vector3 _velocity = Vector3.zero;

        [ValueDropdown(nameof(_moveDirections))]
        [BoxGroup("Box")] 
        [SerializeField] private Vector3 moveDirection = Vector3.zero;

        private static IEnumerable _moveDirections = new ValueDropdownList<Vector3>()
        {
            {"None", Vector3.zero},
            {"X (Horizontal)", Vector3.forward},
            {"Y (Vertical)", Vector3.up},
            {"Z (Depth)", Vector3.right},
        };

        private float _input = 0;

        public override void AbilityUpdate()
        {
            base.AbilityUpdate();

            _input = _inputAction.ReadValue<float>();
        }

        public override void AbilityFixedUpdate()
        {
            base.AbilityFixedUpdate();

            //SystemCore.PlayerRigidbody2D

            Rigidbody2D __rigidbody = SystemCore.PlayerRigidbody2D;

            Vector2 __currVelocity = _velocity = __rigidbody.velocity;
            Vector3 __targetVelocity = new Vector2(_input * (movementSpeed), __currVelocity.y);
            
            // And then smoothing it out and applying it to the character
            __rigidbody.velocity = Vector3.SmoothDamp(current: __currVelocity, __targetVelocity, ref _velocity , movementSmoothing);
        }
    }
}