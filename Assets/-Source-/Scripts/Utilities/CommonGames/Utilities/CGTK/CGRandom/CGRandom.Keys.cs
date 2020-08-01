using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CommonGames.Utilities.CGTK
{
	using System;
	using System.Text;
	using JetBrains.Annotations;
	using Random = System.Random;

	public static partial class CGRandom
	 {
		 // Generate a random string with a given size  
		 [PublicAPI]
		 public static string RandomString(in byte size, bool isLowerCase = false)  
		 {  
			 StringBuilder __builder = new StringBuilder();  
			 Random __random = new Random();
			 for (int i = 0; i < size; i++)
			 {
				 char __ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * __random.NextDouble() + 65)));
				 __builder.Append(__ch);
			 }  
			 
			 return isLowerCase ? __builder.ToString().ToLower() : __builder.ToString();
		 }  

	 }
}