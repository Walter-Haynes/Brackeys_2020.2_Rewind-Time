using System;

using UnityEngine;

namespace CommonGames.Utilities.CustomTypes
{
    using JetBrains.Annotations;
    using Sirenix.OdinInspector;
    
    using static CommonGames.Utilities.Helpers.PlatformHelpers;

    /// <summary> A wrapper that switches between different prefabs based on your specifications. </summary>
    [Serializable]
    [CreateAssetMenuAttribute(menuName = "Common-Games/Create Platfab", fileName = "New Platfab")]
    public sealed partial class Platfab : ScriptableObject
    {
        #region Variables

        [Serializable]
        private struct Prefab
        {
            [AssetsOnly] 
            [InlineEditor]
            [UsedImplicitly]
            [HorizontalGroup(GroupName = "Prefab", LabelWidth = 40)] public GameObject asset;
            
            [UsedImplicitly]
            [HorizontalGroup(GroupName = "Prefab", LabelWidth = 60)] public Platforms platforms;
        }

        [UsedImplicitly]
        [SerializeField] private Prefab[] prefabs; 

        #endregion

        #region Operators

        public static implicit operator GameObject(in Platfab sceneReference)
            => sceneReference.GetPrefabByPlatform;

        #endregion

        #region Methods

        /// <summary> Returns a prefab based on the Platform you're on currently. </summary>
        public GameObject GetPrefabByPlatform
        {
            get
            {
                Debug.Log("Getting Prefab by Platform, let's go!");
                
                foreach(Prefab __prefab in prefabs)
                {
                    switch(Application.platform)
                    {
                        case RuntimePlatform.OSXEditor:
                            if(__prefab.platforms.HasFlag(Platforms.OSX))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.WindowsEditor:
                            if(__prefab.platforms.HasFlag(Platforms.Windows))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.LinuxEditor:
                            if(__prefab.platforms.HasFlag(Platforms.Linux))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.OSXPlayer:
                            if(__prefab.platforms.HasFlag(Platforms.OSX))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.WindowsPlayer:
                            if(__prefab.platforms.HasFlag(Platforms.Windows))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.LinuxPlayer:
                            if(__prefab.platforms.HasFlag(Platforms.Linux))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.Android:
                            if(__prefab.platforms.HasFlag(Platforms.Android))
                            {
                                return __prefab.asset;
                            }
                            break;

                        case RuntimePlatform.IPhonePlayer:
                            if(__prefab.platforms.HasFlag(Platforms.IPhone))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.WebGLPlayer:
                            if(__prefab.platforms.HasFlag(Platforms.WebGL))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.PS4:
                            if(__prefab.platforms.HasFlag(Platforms.PS4))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.XboxOne:
                            if(__prefab.platforms.HasFlag(Platforms.XboxOne))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.Switch:
                            if(__prefab.platforms.HasFlag(Platforms.Switch))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.Lumin:
                            if(__prefab.platforms.HasFlag(Platforms.Lumin))
                            {
                                return __prefab.asset;
                            }
                            break;
                        case RuntimePlatform.Stadia:
                            if(__prefab.platforms.HasFlag(Platforms.Stadia))
                            {
                                return __prefab.asset;
                            }
                            break;
                        
                        case RuntimePlatform.tvOS:
                            if(__prefab.platforms.HasFlag(Platforms.TvOS))
                            {
                                return __prefab.asset;
                            }
                            break;
                        //throw new ArgumentOutOfRangeException();
                    }
                }

                return null;
            }
        }
        
        #region Instantiation
        
        public void Instantiate()
            => Instantiate(original: (GameObject) this);

        public void Instantiate(in Transform parent)
            => Instantiate(original: (GameObject) this, parent: parent);

        public void Instantiate(in Transform parent, in bool instantiateInWorldSpace)
            => Instantiate(original: (GameObject) this, parent: parent, instantiateInWorldSpace: instantiateInWorldSpace);

        public void Instantiate(in Vector3 position, in Quaternion rotation)
            => Instantiate(original: (GameObject) this, position: position, rotation: rotation);

        public void Instantiate(in Vector3 position, in Quaternion rotation, in Transform parent)
            => Instantiate(original: (GameObject) this, position: position, rotation: rotation, parent: parent);

        #endregion

        #endregion
    }
}