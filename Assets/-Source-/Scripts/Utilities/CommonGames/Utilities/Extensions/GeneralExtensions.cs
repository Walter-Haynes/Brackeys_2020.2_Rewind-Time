using System;
using System.Collections;
using System.Reflection;

using UnityEngine;
using UnityEngine.SceneManagement;

using JetBrains.Annotations;

using static CommonGames.Utilities.Defaults;

namespace CommonGames.Utilities.Extensions
{
    using SRandom = System.Random;

    public static partial class GeneralExtensions
    {
        /// <summary>
        /// Swaps values <paramref name="left"/> and <paramref name="right"/>
        /// </summary>
        public static void Swap<T>(ref T left, ref T right)
        {
            T __temp = right;
            
            right = left;
            left = __temp;
        }

        /// <summary>
        /// Do with target item.
        /// </summary>
        public static void Do<T>(this T t, params Action<T>[] actions) => actions.For(action: x => x(obj: t));

        /// <summary>
        /// If bool is true, execute action.
        /// </summary>
        public static void If(this bool b, in Action action)
        {
            if(b)
            {
                action();
            }
        }

        /// <summary> Executes the first action on true, the second on false. </summary>
        public static void IfElse(this bool checkIfTrue, in Action onTrue, in Action onFalse)
        {
            if(checkIfTrue)
            {
                onTrue();
            }
            else
            {
                onFalse();
            }
        }

        /// <summary>
        /// Execute target function i times where func returns true with overload i.
        /// </summary>
        public static void For(this int i, Func<int, bool> func, Action<int> action)
        {
            for(int __j = 0; __j < i; __j++)
            {
                if(func(arg: i))
                {
                    action(obj: i);
                }
            }
        }

        /// <summary>
        /// Execute target function i times.
        /// </summary>
        public static void For(this int i, Action action)
        {
            for(int __j = 0; __j < i; __j++)
            {
                action();
            }
        }

        /// <summary>
        /// Execute target function i times, with the index as overload.
        /// </summary>
        public static void For(this int i, in Action<int> action)
        {
            for(int __j = 0; __j < i; __j++)
            {
                action(obj: __j);
            }
        }

        /// <summary>
        /// Returns random value in range
        /// </summary>
        public static float RandomRange(float min, float max, SRandom random = null)
        {
            SRandom __usedRandom = random ?? RANDOM;
            float __lerp = (float)__usedRandom.NextDouble();
            return Mathf.Lerp(a: min, b: max, t: __lerp);
        }

        public static Vector3 RandomVector3(float min, float max, SRandom random = null)
        {
            SRandom __usedRandom = random ?? RANDOM;
            return new Vector3(
                x: RandomRange(min: min, max: max, random: random), 
                y: RandomRange(min: min, max: max, random: random), 
                z: RandomRange(min: min, max: max, random: random));
        }

        public static Vector3Int RandomVector3Int(float min, float max, SRandom random = null)
        {
            Vector3 __vector = RandomVector3(min: min, max: max, random: random);
            return new Vector3Int(
                x: Mathf.RoundToInt(f: __vector.x), 
                y: Mathf.RoundToInt(f: __vector.y), 
                z: Mathf.RoundToInt(f: __vector.z));
        }

        /// <summary>
        /// Converts class to type T, instead of having to do (MyClass as T).
        /// </summary>
        public static T Convert<T>(this object obj) where T : class => obj as T;

        public static void Stop(this Coroutine coroutine, MonoBehaviour behaviour)
        {
            if(coroutine != null)
                behaviour.StopCoroutine(routine: coroutine);
        }

        /// <summary>
        /// The next time a scene is loaded, execute this function.
        /// </summary>
        public static void AddInstanceToSceneLoaded(Action action)
        {
            void __Wrapper(Scene scene, LoadSceneMode mode)
            {
                action();
                SceneManager.sceneLoaded -= __Wrapper;
            }

            SceneManager.sceneLoaded += __Wrapper;
        }

        /// <summary>
        /// Shortcut for Unity's SetActive.
        /// </summary>
        public static void SetActive(this MonoBehaviour behaviour, bool enabled = true)
            => behaviour.gameObject.SetActive(value: enabled);

        /// <summary>
        /// Converts float into a string that visualizes the time digitally.
        /// </summary>
        public static string ToTimeString(this float seconds)
        {
            TimeSpan __result = TimeSpan.FromSeconds(value: seconds);
            return string.Format(format: $"{__result.Hours}:{__result.Minutes}:{__result.Seconds}");
        }

        #region Logging

        #region Logs

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void Log(this string message)
        {
            CommonGames.Utilities.CGTK.CGDebug.Log(message: message);
        }
        
        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void Log<T>(this T obj)
        {
            CommonGames.Utilities.CGTK.CGDebug.Log(message: obj.ToString());
        }
        
        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void Log<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogFormat(format: message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError(message: "Debug Logging Went Wrong");
            }
        }

        #endregion

        #region LogWarnings

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void LogWarning(this string message)
            => CommonGames.Utilities.CGTK.CGDebug.LogWarning(message: message);

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogWarning<T>(this T obj)
            => CommonGames.Utilities.CGTK.CGDebug.LogWarning(message: obj.ToString());

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogWarning<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogWarningFormat(format: message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError(message: "Debug Logging Went Wrong");
            }
        }

        #endregion

        #region LogErrors

        /// <summary> Logs a string value to the console. </summary>
        [PublicAPI]
        public static void LogError(this string message)
            => CommonGames.Utilities.CGTK.CGDebug.LogError(message: message);

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogError<T>(this T obj)
            => CommonGames.Utilities.CGTK.CGDebug.LogError(message: obj.ToString());

        /// <summary> Logs a value (ToString()) to the console. </summary>
        [PublicAPI]
        public static void LogError<T>(this T obj, in string message)
        {
            try
            {
                CommonGames.Utilities.CGTK.CGDebug.LogErrorFormat(format: message, obj.ToString());
            }
            catch
            {
                CommonGames.Utilities.CGTK.CGDebug.LogError(message: "Debug Logging Went Wrong");
            }
        }

        #endregion
        
        #endregion

        #region GetIfNull
        
        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in Func<T> getMethod) where T : Component
            => obj ? obj : getMethod?.Invoke();
        
        #region GameObject Context
        
        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in GameObject context) where T : Component
            => obj ? obj : context.GetComponent<T>();
        
        [PublicAPI]
        public static T GetInChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
            => obj ? obj : context.GetComponentInChildren<T>();
        
        [PublicAPI]
        public static T TryGetIfNull<T>(this T obj, in GameObject context) where T : Component
        {
            if(obj != null) return obj;

            context.TryGetComponent(component: out T __returnedObject);
            return __returnedObject;
        }

        [PublicAPI]
        public static T TryGetInChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
            => context.TryGetComponent(component: out T __returnedObject) ? __returnedObject : context.GetComponentInChildren<T>();
        
        [PublicAPI]
        public static T TryGetInParentIfNull<T>(this T obj, in GameObject context) where T : Component
            => context.TryGetComponent(component: out T __returnedObject) ? __returnedObject : context.GetComponentInParent<T>();
        
        [PublicAPI]
        public static T TryGetInParentOrChildrenIfNull<T>(this T obj, in GameObject context) where T : Component
        {
            if(context.TryGetComponent(component: out T __triedComponent)) return __triedComponent;

            T __componentInParent = context.GetComponentInParent<T>();
            
            return __componentInParent ? __componentInParent : context.GetComponentInParent<T>();
        }

        #endregion

        #region Component Context

        [PublicAPI]
        public static T GetIfNull<T>(this T obj, in Component context) where T : Component
            => GetIfNull(obj: obj, context: context.gameObject);

        [PublicAPI]
        public static T GetInChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => GetInChildrenIfNull(obj: obj, context: context.gameObject);

        [PublicAPI]
        public static T TryGetIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetIfNull(obj: obj, context: context.gameObject);

        [PublicAPI]
        public static T TryGetInChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInChildrenIfNull(obj: obj, context: context.gameObject);

        [PublicAPI]
        public static T TryGetInParentIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInParentIfNull(obj: obj, context: context.gameObject);

        [PublicAPI]
        public static T TryGetInParentOrChildrenIfNull<T>(this T obj, in Component context) where T : Component
            => TryGetInParentOrChildrenIfNull(obj: obj, context: context.gameObject);

        #endregion

        #endregion
    }

    public class ListArray<T>
    {
        public T[] Array { get; private set; }

        public int Length => Array.Length;
        public int Count { get; private set; }

        public T this[int index]
        {
            get => Array[index];
            set => Array[index] = value;
        }

        public ListArray(int length) => Array = new T[length];

        public void Add(T item)
        {
            Array[Length] = item;
            Count++;
        }

        public void Remove(T item, bool removeAll = false)
        {
            for (int __i = 0; __i < Length; __i++)
                if(Array[__i].Equals(obj: item))
                {
                    RemoveAt(index: __i);
                    if(!removeAll)
                        return;
                }
        }

        public void RemoveAt(int index)
        {
            int __length = Length - index - 1;

            for (int __i = 0; __i < __length; __i++)
            {
                int __current = index + __length - __i, __next = __current - 1;

                T __item = Array[__current];
                ref T __nextItem = ref Array[__next];

                Array[__current] = __nextItem;
                __nextItem = __item;
            }
        }

        public static implicit operator T[](ListArray<T> listArray) => listArray.Array;
        public static implicit operator ListArray<T>(T[] array) => new ListArray<T>(length: array.Length); 
    }

    /// <summary>
    /// An ragged array that decrements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoriumArray<T>
    {
        public T[] this[int i]
        {
            get => _data[i];
            set => _data[i] = value;
        }

        public T this[int i, int j]
        {
            get => _data[i][j];
            set => _data[i][j] = value;
        }

        public int Length => _data.Length;

        private T[][] _data;

        public FactoriumArray(int length)
        {
            _data = new T[length][];
            for (int __i = 0; __i < length; __i++)
                _data[__i] = new T[length - __i];
        }

        public void Reset()
        {
            int __length = _data.Length;
            for (int __i = 0; __i < __length; __i++)
                for (int __j = 0; __j < __length - __i; __j++)
                    _data[__i][__j] = default;
        }
    }
    
    /// <summary>
    /// Used to invoke function with a return type by name.
    /// </summary>
    [Serializable]
    public class FuncEvent
    {
        public MonoBehaviour behaviour;
        public string name;

        private MethodInfo _method = null;

        public T Invoke<T>()
        {
            if(_method != null) return (T)_method.Invoke(obj: behaviour, null);
            
            Type __type = behaviour.GetType();
            _method = __type.GetMethod(name: name);
            return (T)_method.Invoke(obj: behaviour, null);
        }
    }
}