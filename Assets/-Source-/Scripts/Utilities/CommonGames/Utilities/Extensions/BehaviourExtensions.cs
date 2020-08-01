using System;
using System.Collections;
using System.Reflection;

using UnityEngine;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
	public static partial class GeneralExtensions
	{
		#region ToggleInTime

		public static void EnableInTime(this Behaviour obj, bool state, float time = DEFAULT_WAIT_TIME_0)
		{
			IEnumerator __EnableInTimeCoroutine()
			{
				yield return (time.Approximately(default) ? DefaultWait_0 : new WaitForSeconds(time));
			
				obj.enabled = state;
			}
			
			CoroutineHandler.StartCoroutine(__EnableInTimeCoroutine());	
		}

		#endregion
	}
}