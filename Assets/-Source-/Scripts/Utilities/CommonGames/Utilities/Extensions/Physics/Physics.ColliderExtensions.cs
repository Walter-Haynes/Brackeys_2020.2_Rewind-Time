using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Physics
    {
        #region ToggleInTime

        public static void ToggleInTime(this Collider obj, bool state, float time = DEFAULT_WAIT_TIME_0)
            => CoroutineHandler.StartCoroutine(ToggleInTimeCoroutine(obj, state, time.ToAbs()));
		
        private static IEnumerator ToggleInTimeCoroutine(Collider obj, bool state, float time = DEFAULT_WAIT_TIME_0)
        {
            yield return (time.Approximately(default) ? DefaultWait_0 : new WaitForSeconds(time));
			
            obj.enabled = state;
        }
		
        #endregion
    }
}