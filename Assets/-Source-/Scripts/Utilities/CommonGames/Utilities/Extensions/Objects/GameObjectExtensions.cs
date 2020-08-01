using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
	public static partial class GeneralExtensions
	{
		#region ToggleInTime
		
		public static void ToggleInTime(this GameObject obj, bool state, float time = DEFAULT_WAIT_TIME_0)
			=> CoroutineHandler.StartCoroutine(ToggleInTimeCoroutine(obj, state, time.ToAbs()));
		
		private static IEnumerator ToggleInTimeCoroutine(GameObject obj, bool state, float time = DEFAULT_WAIT_TIME_0)
		{
			yield return (time.Approximately(default) ? DefaultWait_0 : new WaitForSeconds(time));
			
			obj.SetActive(state);
		}
		
		#endregion
		
		[PublicAPI]
		public static T EnsuredGetComponent<T>(this GameObject gameObject) where T : Component
			=> gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
		
		[PublicAPI]
		public static IEnumerable<Transform> GetAllChildren(this Transform transform)
		{
			Stack<Transform> __stack = new Stack<Transform>();
			__stack.Push(item: transform);
			
			while(__stack.Count != 0)
			{
				Transform __transform = __stack.Pop();
				yield return __transform;

				for(int __index = 0; __index < __transform.childCount; __index++)
				{
					__stack.Push(__transform.GetChild(__index));
				}
			}
		}

		[PublicAPI]
		public static IEnumerable<GameObject> GetAllChildren(this GameObject gameObject)
		{
			foreach(Transform __transform in gameObject.transform.GetAllChildren())
			{
				yield return __transform.gameObject;
			}
		}
	}
}