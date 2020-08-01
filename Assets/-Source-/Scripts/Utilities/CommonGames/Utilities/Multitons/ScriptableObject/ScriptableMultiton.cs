using System.Collections.Generic;

//using System.Linq;

using UnityEngine;

// ReSharper disable ArgumentsStyleOther
// ReSharper disable once ArgumentsStyleNamedExpression
// ReSharper disable once CheckNamespace
namespace CommonGames.Utilities
{
    using JetBrains.Annotations;
    
    using Sirenix.OdinInspector;
    
    #if ODIN_INSPECTOR
    using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
    #endif
    
    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    
    using Extensions;

    [ExecuteAlways]
    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("ReSharper", "ArrangeThisQualifier")]
    public abstract class ScriptableMultiton<T> : ScriptableObject 
        where T : ScriptableMultiton<T>
    {
        [PublicAPI]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public static T[] Instances
        {
            get
            {
                Resources.LoadAll(path: "", typeof(T));

                T[] __instances = Resources.FindObjectsOfTypeAll<T>();

                #region Preloading

                #if UNITY_EDITOR

                List<UnityEngine.Object> __preloadedAssets = new List<UnityEngine.Object>();
                __preloadedAssets.AddRange(collection: UnityEditor.PlayerSettings.GetPreloadedAssets());

                foreach(T __multiton in __instances)
                {
                    if(__preloadedAssets.Contains(__multiton)) continue; 
                    
                    __preloadedAssets.Add(__multiton);
                }

                PlayerSettings.SetPreloadedAssets(__preloadedAssets.ToArray());

                #endif
                
                #endregion

                return __instances;
            }
        }
        
        [PublicAPI]
        public static int IndexFromInstance(T instance) => Instances.GetIndex(func: t => t == instance);

        /// <summary> Access you the vehicle at Index i </summary>
        [PublicAPI]
        public T this[in int i]
        {
            get => Instances[i];
            set => Instances[i] = value;
        }
    }
}