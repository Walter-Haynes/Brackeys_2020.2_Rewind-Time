using System;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Extensions
{
    public static partial class StringExtensions
    {
        /// <returns> <c>true</c> If some string is contained with this string; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public static bool Contains(
            this string source,
            in string check,
            in StringComparison comparisonType)
        {
            return (source.IndexOf(check, comparisonType) >= 0);
        }
        
        /// <returns><c>true</c> If this string is null or empty; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public static bool IsNullOrEmpty(this string source)
            => string.IsNullOrEmpty(source);

        /// <returns><c>true</c> If this string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public static bool IsNullOrWhitespace(this string source)
        {
            if(string.IsNullOrEmpty(source)) return true;
            
            foreach(char __char in source)
            {
                if(!char.IsWhiteSpace(__char))
                {
                    return false;
                }
            }
            return true;
        }
        
        /*[PublicAPI]
        public static string GetNthIndexOf(this string source, in char c, in int n)
        {
            string[] __split = source.Split(c);

            if(n >= __split.Length) return null;

            System.Text.StringBuilder __stringBuilder = new System.Text.StringBuilder();
            for(int __index = 0; __index < n; __index++)
            {
                __stringBuilder.Append(__split[__index]).Append('/');
            }

            return __stringBuilder.ToString();
        }*/
        
        [PublicAPI]
        public static string SplitAtNthOccurenceOfChar(this string source, in char c, in int n)
        {
            string[] __split = source.Split(c);

            if(n >= __split.Length) return null;

            System.Text.StringBuilder __stringBuilder = new System.Text.StringBuilder();
            for(int __index = 0; __index < n; __index++)
            {
                __stringBuilder.Append(__split[__index]).Append('/');
            }

            return __stringBuilder.ToString();
        }
        
        [PublicAPI]
        public static (string matchingPart, string nonMatchingA, string nonMatchingB) SplitAtDeviation(this string a, in string b)
        {
            if(a == null || b == null) return (null, a, b);
            
            string __shortestString = a.Length >= b.Length
                ? b 
                : a;

            string 
                __matchingPart = null,
                __nonMatchingA = a,
                __nonMatchingB = b;
            
            for(int __index = 0; __index < __shortestString.Length; __index++)
            {
                /*
                UnityEngine.Debug.Log(message:  "\n" +
                                        $"A = {a}\n" +
                                        $"B = {b}\n" +
                                        $"Index = {__index}\n" +
                                        $"Char A = {a[__index]}\n" +
                                        $"Char B = {b[__index]}");
                                        */
                
                if(a[__index] == b[__index]) continue;

                __matchingPart = __shortestString.Substring(startIndex: 0, length: __index);

                __nonMatchingA = a.Substring(startIndex: __index, length: a.Length-__index);
                    
                __nonMatchingB = b.Substring(startIndex: __index, length: b.Length-__index);
                    
                break;
            }

            return (__matchingPart, __nonMatchingA, __nonMatchingB);
        }
    }
}