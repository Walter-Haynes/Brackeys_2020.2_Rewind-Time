using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_DOTS
using Unity.Mathematics;
#endif

using CommonGames.Utilities.CGTK;

namespace CommonGames.Utilities.Extensions
{
	public static partial class Math
	{
		#region Clamping

		public static float Clamp(this float value, in float min, in float max) 
			=> Mathf.Clamp(value: value, min: min, max: max);

		public static float Clamp01(this float value) 
			=> Mathf.Clamp01(value: value);
		
		public static float ClampAngle(this float angle, float min, float max)
		{
			while (angle < -360 || angle > 360)
			{
				if(angle < -360)
				{
					angle += 360;
				}
				if(angle > 360)
				{
					angle -= 360;
				}
			}

			return Clamp(angle, min, max);
		}

		#endregion

		#region Rounding

		public static float Round(this float value, in CGMath.RoundingMode mode)
			=> CGMath.Round(value: value, mode: mode);
		public static int RoundToInt(this float value, in CGMath.RoundingMode mode)
			=> CGMath.RoundToInt(value: value, mode: mode);
		
		public static float Round(this float value) 
			=> Mathf.Round(value);
		public static int RoundToInt(this float value) 
			=> Mathf.RoundToInt(value);
		
		public static float Floor(this float value) 
			=> Mathf.Floor(value);
		public static int FloorToInt(this float value) 
			=> Mathf.FloorToInt(value);

		public static float Ceil(this float value) 
			=> Mathf.Ceil(value);
		public static int CeilToInt(this float value) 
			=> Mathf.CeilToInt(value);

		public static float RoundToMultipleOf(this float value, in float multiple)
			=> CGMath.RoundToMultipleOf(value: value, multiple: multiple);
		public static int RoundToMultipleIntOf(this float value, in int multiple)
			=> CGMath.RoundToMultipleIntOf(value: value, multiple: multiple);

		#endregion
		
		public static bool Approximately(this float a, in float b) => Mathf.Approximately(a, b);
		
		#if UNITY_DOTS
		public static bool BurstApproximately(this float a, in float b)
		{
			return Mathf.Abs(b - a) < Mathf.Max(0.000001f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), 1.17549435E-38f * 8);
		}
		
		//public static bool BurstApproximately(this float a, in float b) => math.(a, b);
		
		#endif

		public static float Abs(this float value) => Mathf.Abs(value);
		
		public static float ToAbs(ref this float value) => Mathf.Abs(value);
		
		public static float Pow(this float value, in float power) => Mathf.Pow(value, power);
		
		public static float Sqrt(this float value) => Mathf.Sqrt(value);

		/*
		public static float ClosestPowerOfTen(this float value)
		{
			float absoluteValue = value.Abs;
			
			return (absoluteValue <= 0) ? 1 : Mathf.Pow(10, Mathf.Log10(absoluteValue).RoundToInt());
		}
	
		public static float ClosestPowerOfTwo(this float value)
		{
			return Mathf.ClosestPowerOfTwo(value);
		}
		*/

	}
}