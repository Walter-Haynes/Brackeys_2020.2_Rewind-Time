using UnityEngine;
using UnityEngine.InputSystem;

using JetBrains.Annotations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Core.Player
{
    using System;
    using CommonGames.Utilities.Extensions;
    using CommonGames.Utilities.Helpers;
    
    using GenericAbilitySystem;

    public partial class PlayerGravity : PlayerBehaviour
    {
        #region Variables

        [PublicAPI]
        public float Gravity { get; set; }

        #endregion

        #region Methods

        private void FixedUpdate()
        {
            Vector2 __currVelocity = SystemCore.PlayerRigidbody2D.velocity;
            
            //SystemCore.PlayerRigidbody2D.velocity = new Vector2(x: __currVelocity.x, y: -Gravity.Abs());
        }

        #endregion
    }
}