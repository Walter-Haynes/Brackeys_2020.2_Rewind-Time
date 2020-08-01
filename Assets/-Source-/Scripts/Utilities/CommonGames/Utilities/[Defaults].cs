using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities
{
    using System.Collections.Generic;
    using Random = System.Random;
    
    public static partial class Defaults
    {
        #region Alphanumerics

        /// <summary> Lowercase letters from a to z </summary>
        [PublicAPI]
        public const string LOWERCASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";
        
        /// <summary> Uppercase letters from A to Z </summary>
        [PublicAPI]
        public const string UPPERCASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        /// <summary> All <see cref="LOWERCASE_LETTERS"/> and <see cref="UPPERCASE_LETTERS"/>. </summary>
        [PublicAPI]
        public const string LETTERS = LOWERCASE_LETTERS + UPPERCASE_LETTERS;
        
        /// <summary> Digits from 0 to 9 </summary>
        [PublicAPI]
        public const string DIGITS = "0123456789";
        
        /// <summary> All <see cref="LETTERS"/> and <see cref="DIGITS"/> </summary>
        [PublicAPI]
        public const string ALPHANUMERICS = LETTERS + DIGITS;

        #endregion

        #region Static MonoBehaviours

        [PublicAPI]
        public sealed class StaticMonoBehaviourClass : EnsuredSingleton<StaticMonoBehaviourClass>
        {
            protected override bool IsVisibleInHierarchy => true;
        }

        /// <summary> A static <see cref="MonoBehaviour"/>. It's an <see cref="EnsuredSingleton"/> so it spawns one if there isn't one in the scene). </summary>
        [PublicAPI]
        public static StaticMonoBehaviourClass StaticMonoBehaviour => StaticMonoBehaviourClass.Instance;
        
        /// <summary> A static <see cref="MonoBehaviour"/> for handling Coroutines. </summary>
        [PublicAPI]
        public static StaticMonoBehaviourClass CoroutineHandler => StaticMonoBehaviour;

        #endregion

        #region Default WaitForSeconds

        /// <summary> The default wait time for WaitForSeconds' </summary>
        [PublicAPI]
        public const float DEFAULT_WAIT_TIME_0 = 0f;
        
        /// <summary> A static WaitForSeconds, used when functions don't get one send to them. </summary>
        /// <remarks> ZERO SECONDS BY DEFAULT, because when you don't overload anything, we can assume you don't want any wait time. </remarks>
        [PublicAPI]
        public static readonly WaitForSeconds DefaultWait_0 = new WaitForSeconds(DEFAULT_WAIT_TIME_0);
        
        /// <summary> The default wait time for WaitForSeconds' </summary>
        public const float DEFAULT_WAIT_TIME_1 = 1f;
        
        /// <summary> A static WaitForSeconds, used when functions don't get one send to them. </summary>
        public static readonly WaitForSeconds DefaultWait_1 = new WaitForSeconds(DEFAULT_WAIT_TIME_1);
        
        
        [PublicAPI]
        public static readonly WaitForSeconds
            WaitHalfSecond = new WaitForSeconds(.5f),
            
            Wait1s = new WaitForSeconds(1),
            Wait2s = new WaitForSeconds(2),
            Wait3s = new WaitForSeconds(3),
            Wait4s = new WaitForSeconds(4),
            Wait5s = new WaitForSeconds(5),
            Wait6s = new WaitForSeconds(6),
            Wait7s = new WaitForSeconds(7),
            Wait8s = new WaitForSeconds(8),
            Wait9s = new WaitForSeconds(9),
            Wait10s = new WaitForSeconds(10);

        #region WaitForSecondsFactory
        
        private static Dictionary<float, WaitForSeconds> _waitDictionary = new Dictionary<float, WaitForSeconds>();
        
        /// <summary> Gives you a reusable <see cref="WaitForSeconds"/>.</summary>
        /// <returns> A reusable <see cref="WaitForSeconds"/>. </returns>
        [PublicAPI]
        public static WaitForSeconds Wait(in float seconds)
        {
            //If the dictionary contains an entry with key 'seconds' it returns the found entry.
            if(!_waitDictionary.TryGetValue(key: seconds, value: out WaitForSeconds __result))
            {
                //If not, it adds it and returns the result.
                _waitDictionary.Add(
                    key: seconds, 
                    value: new WaitForSeconds(seconds: seconds));

                __result = _waitDictionary[key: seconds];
            }

            return __result;
        }
        
        #endregion

        #endregion

        #region Math Constants
        
        //TODO -Walter- Add TAU, it's better but no-one is taught about it.

        ///// <summary> Square root of 0.5 </summary>
        //[PublicAPI]
        //public const float SQRT_05 = 0.7071067811865475244f;
        
        /// <summary> Square root of 2. </summary>
        [PublicAPI]
        public const float SQRT_2 = 1.4142135623730950488f;
        
        /// <summary> Square root of 5. </summary>
        [PublicAPI]
        public const float SQRT_5 = 2.2360679774997896964f;
        
        /// <summary> Golden angle in radians. </summary>
        [PublicAPI]
        public const float GOLDEN_ANGLE = Mathf.PI * (3 - SQRT_5);

        /// <summary> PI times 2. </summary>
        [PublicAPI]
        public const float PI_2 = Mathf.PI * 2;
        
        /// <summary> PI times 4. </summary>
        [PublicAPI]
        public const float PI_4 = PI_2* 2;

        /// <summary> PI times 8. </summary>
        [PublicAPI]
        public const float PI_8 = PI_4 * 2;
        
        /// <summary> PI times 16. </summary>
        [PublicAPI]
        public const float PI_16 = PI_8 * 2;

        #endregion

        #region Default Random

        /// <summary> A static System.Random, used when functions don't get one send to them. </summary>
        [PublicAPI]
        // ReSharper disable once InconsistentNaming
        public static Random RANDOM { get; } = new Random();

        #endregion
        
    }
}
