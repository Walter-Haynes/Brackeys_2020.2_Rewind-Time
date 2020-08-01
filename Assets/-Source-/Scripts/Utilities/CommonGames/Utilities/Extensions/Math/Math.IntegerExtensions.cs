using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
	public static partial class Math
	{
		public static int Abs(this int value) => Mathf.Abs(value);

		public static void ToAbs(ref this int value) => value = Mathf.Abs(value);

		public static int ClosestPowerOfTwo(this int value)
			=> Mathf.ClosestPowerOfTwo(value);
	}
}
