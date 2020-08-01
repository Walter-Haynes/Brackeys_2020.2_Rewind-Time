using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

namespace Core.Player.GenericAbilitySystem.Example.RAB
{

    public class RABMovementAbility : BaseRABAbility
    {
        [BoxGroup("Box")] [SerializeField] private float ballSpeed = 10;

        [ValueDropdown(nameof(_moveDirections))]
        [BoxGroup("Box")] [SerializeField] private Vector3 moveDirection = Vector3.zero;

        private static IEnumerable _moveDirections = new ValueDropdownList<Vector3>()
        {
            {"None", Vector3.zero},
            {"X (Horizontal)", Vector3.forward},
            {"Y (Vertical)", Vector3.up},
            {"Z (Depth)", Vector3.right},
        };

        private float _input = 0;

        public override void AbilityStart()
        {
            base.AbilityStart();

            SystemCore.PlayerRigidbody.maxAngularVelocity = float.MaxValue;
        }

        public override void AbilityUpdate()
        {
            base.AbilityUpdate();

            _input = _inputAction.ReadValue<float>();
        }

        public override void AbilityFixedUpdate()
        {
            base.AbilityFixedUpdate();

            SystemCore.PlayerRigidbody.AddTorque(moveDirection * (_input * (ballSpeed * FixedDeltaTime)));
        }
    }
}