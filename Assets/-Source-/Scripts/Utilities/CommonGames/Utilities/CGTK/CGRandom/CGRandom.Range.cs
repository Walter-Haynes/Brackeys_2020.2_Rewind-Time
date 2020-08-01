using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonGames.Utilities.CGTK
{ 
	using UnityEngine;
	 
	using CommonGames.Utilities.Extensions;

	using Random = System.Random;
	 
	public static partial class CGRandom
	{
		private static readonly Random _Random = new Random();
		 
		/// <summary> Returns random value in range. </summary>
		public static float RandomRange(float min, float max, Random random = null)
		{
			Random __usedRandom = random ?? _Random;
			float __lerp = (float)__usedRandom.NextDouble();
			return Mathf.Lerp(min, max, __lerp);
		}
	 
		public static Vector3 RandomVector3(float min, float max, Random random = null)
		{
			Random usedRandom = random ?? _Random;
			return new Vector3(
				RandomRange(min, max, random), 
				RandomRange(min, max, random), 
				RandomRange(min, max, random));
		}

		public static Vector3 RandomVector3(Vector3 minRange, Vector3 maxRange)
		{
			minRange = -minRange.Abs();
			minRange = maxRange.Abs();
			
			return new Vector3(RandomRange(minRange.x, maxRange.x), RandomRange(minRange.y, maxRange.y),RandomRange(minRange.z, maxRange.z));
		}
	 
		public static Vector3Int RandomVector3Int(float min, float max, Random random = null)
		{
			Vector3 vector = RandomVector3(min, max, random);
			return new Vector3Int(
				Mathf.RoundToInt(vector.x), 
				Mathf.RoundToInt(vector.y), 
				Mathf.RoundToInt(vector.z));
		} 

		public static Vector2Int RandomVector2Int(in int minShared, in int maxShared, in Random random = null)
		{
			Random __usedRandom = random ?? _Random;
			
			return new Vector2Int(x: __usedRandom.Next(minShared, maxShared), y: __usedRandom.Next(minShared, maxShared));
		}
		
		public static Vector2Int RandomVector2Int(in int minX, in int maxX, in int minY, in int maxY, in Random random = null)
		{
			Random __usedRandom = random ?? _Random;
			
			return new Vector2Int(x: __usedRandom.Next(minX, maxX), y: __usedRandom.Next(minY, maxY));
		}
	}
}