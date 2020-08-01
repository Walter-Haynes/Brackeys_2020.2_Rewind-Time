using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR

using System.IO;
using UnityEditor;

#endif

using static CommonGames.Utilities.Helpers.PlatformHelpers;

namespace CommonGames.Utilities.CGTK.AssHat
{
    using JetBrains.Annotations;
    
    using Sirenix.OdinInspector;
    
    /// <summary>
    /// Bundles are used in several circumstances in UNITY development. With bundles, you can postpone the asset download 
    /// at startup time, you can update your game without re-publish it, you can manage asset variants to support low-level devices, 
    /// you can manage dynamic load/unload of entire parts of your game, and so on. 
    /// AssetBundleManager is born with the goal of simplifying the workflow of asset bundles management, 
    /// by driving the user in a well-defined process to support all possible use cases.
    /// </summary>
    [ExecuteAlways]
    public sealed partial class AssetBundleManager : PersistentSingleton<AssetBundleManager>
    {
        #region Variables

        #region Dictionaries

        /*private Dictionary<string, uint> _crcs;
        [PublicAPI]
        [Sirenix.Serialization.OdinSerializeAttribute] public static Dictionary<string, uint> CrCs 
            => Instance._crcs ?? (Instance._crcs = new Dictionary<string, uint>());
        
        private Dictionary<string, uint> _versions;
        [PublicAPI]
        public static Dictionary<string, uint> Versions 
            => Instance._versions ?? (Instance._versions = new Dictionary<string, uint>());
        
        private Dictionary<string,AssetBundle> _bundles;
        /// <summary> The internal map of bundles, by name. </summary>
        [PublicAPI]
        [Sirenix.Serialization.OdinSerializeAttribute] public static Dictionary<string, AssetBundle> Bundles 
            => Instance._bundles ?? (Instance._bundles = new Dictionary<string, AssetBundle>());*/

        [PublicAPI]
        public static Dictionary<string, uint> CrCs = new Dictionary<string, uint>();
        
        [PublicAPI]
        public static Dictionary<string, uint> Versions = new Dictionary<string, uint>();
        
        [PublicAPI]
        public static Dictionary<string, AssetBundle> Bundles = new Dictionary<string, AssetBundle>();    

        #endregion

        #region Loading

        /// <summary> The base URL, from which bundles and Versions.txt files are downloaded. </summary>
        [BoxGroup("Loading")]
        [SerializeField] private string bundlesBaseUrl = "http://127.0.0.1:8000";

        /// <summary>
        /// The base path of bundles, where the generated bundles will be added by default, and from
        /// which bundles are loaded by the user.
        /// </summary>
        [BoxGroup("Loading")]
        [SerializeField] private string bundlesBasePath = "Assets/StreamingAssets";
        
        /// <summary>
        /// Disable the HTTP server cache. If true, AssetBundleManager, will insert
        /// some headers in the HTTP request to try to disable server HTTP cache.
        /// </summary>
        [BoxGroup("Loading")]
        [SerializeField] private bool disableHttpServerCache = true;

        /// <summary> In test mode, AssetBundleManager will load bundles locally, even if you use network APIs functions. </summary>
        [BoxGroup("Loading")]
        [SerializeField] private bool testMode = false;

        #endregion

        #region Generating
        
        [BoxGroup("Generating")]
        public Platforms buildPlatforms = Platforms.Windows;

        [BoxGroup("Generating")]
        [SerializeField]
        public VersionsDictionary buildVersions = new VersionsDictionary();

        #if UNITY_EDITOR
        [BoxGroup("Generating")]
        [Button("BuildAssetBundles")]
        public void BuildAssetBundlesShortCut()
            => BuildAssetBundles();
        #endif

        #endregion

        #endregion
        
        #region Methods
        
        private void Update()
        {
            #if UNITY_EDITOR
            
            string[] __allBundles = AssetDatabase.GetAllAssetBundleNames();
            foreach(string __bundle in __allBundles)
            {
                uint __bundleVersion = 0;
                if(buildVersions.ContainsKey(key: __bundle))
                {
                    __bundleVersion = buildVersions[key: __bundle];
                }
                buildVersions[key: __bundle] = __bundleVersion;
            }
            
            #endif
        }

        /// <summary>
        /// The Coroutine used to implement LoadBundle in asynchronous mode.
        /// </summary>
        /// <param name="r">The request.</param>
        /// <param name="bundleName">The name of the bundle.</param>
        /// <param name="finished">Callback method delegate called when the load of the bundles has terminated successfully.</param>
        /// <returns></returns>
        private IEnumerator LoadBundleCoroutine(AssetBundleCreateRequest r, string bundleName, LoadBundleFinishedDelegate finished)
        {
            yield return r;

            Bundles.Add (key: bundleName, value: r.assetBundle);
            finished.Invoke (ab: r.assetBundle);
        }

        /// <summary>
        /// The Coroutine used to implement DownloadBundle method.
        /// </summary>
        /// <param name="wr">The UnityWebRequest already prepared for the download-</param>
        /// <param name="finished">The called delegate method when the bundle was successfully downloaded.</param>
        /// <param name="error">The called delegate method when there is an error in downloading the bundle.</param>
        /// <param name="bundleName">The name of the bundle.</param>
        /// <returns></returns>
        private IEnumerator DownloadBundleCoroutine(UnityWebRequest wr, string bundleName, LoadBundleFinishedDelegate finished, LoadBundleErrorDelegate error)
        {
            yield return wr.SendWebRequest();

            if (wr.isNetworkError) {
                error (error: wr.error);
            } else 
            {
                
                AssetBundle __ab = ((DownloadHandlerAssetBundle)wr.downloadHandler).assetBundle;

                if (__ab == null) {
                    error (error: "Error loading bundle, probably another bundle with same files is already loaded.");
                } else {
                    if (Bundles.ContainsKey (key: bundleName)) {
                        Bundles.Remove (key: bundleName);
                    }

                    Bundles.Add (key: bundleName, value: __ab);
                    finished.Invoke (ab: __ab);
                }
            }
        }

        /// <summary>
        /// The Coroutine used to implement DownloadVersions mthod.
        /// </summary>
        /// <param name="wr">The UnityWebRequest already prepared for the download-</param>
        /// <param name="finished">The called delegate method when the bundle was successfully downloaded.</param>
        /// <param name="error">The called delegate method when there is an error in downloading the bundle.</param>
        /// <returns></returns>
        private IEnumerator DownloadVersionsCoroutine(UnityWebRequest wr, DownloadVersionsFinishedDelegate finished, DownloadVersionsErrorDelegate error)
        {
            yield return wr.SendWebRequest();

            if (wr.isNetworkError) 
            {
                error(error: wr.error);
                yield return null;
            }
  
            string __result = wr.downloadHandler.text;

            // Deserialize the versions.txt file
            VersionDataCollection __dataCollection = JsonUtility.FromJson<VersionDataCollection> (json: __result);

            if (__dataCollection == null) 
            {
                error (error: $"Unable to parse version JSON: {__result}");
                yield break;
            }

            // Update the internal state of versions and CRCs
            VersionData[] __datas = __dataCollection.bundles;

            foreach(VersionData __versionData in __datas)
            {
                if (Versions.ContainsKey (key: __versionData.bundleName)) 
                {
                    Versions [key: __versionData.bundleName] = __versionData.version;
                }
                else 
                {
                    Versions.Add (key: __versionData.bundleName, value: __versionData.version);
                }

                if (CrCs.ContainsKey (key:
                    $"{Instance.bundlesBaseUrl}/{CurrentPlatformString()}/{__versionData.bundleName}")) {
                    CrCs [key: $"{Instance.bundlesBaseUrl}/{CurrentPlatformString()}/{__versionData.bundleName}"] = __versionData.crc;
                } 
                else 
                {
                    CrCs.Add (key:
                        $"{Instance.bundlesBaseUrl}/{CurrentPlatformString()}/{__versionData.bundleName}", value: __versionData.crc);
                }
            }

            finished.Invoke(versions: __result);
        }

        #if UNITY_EDITOR
        
        public void BuildAssetBundles()
        {
            string __basePathDir = new DirectoryInfo(path: bundlesBasePath).Name;

            if(!AssetDatabase.IsValidFolder (path: bundlesBasePath)) 
            {
                AssetDatabase.CreateFolder (parentFolder: "Assets", newFolderName: __basePathDir);
            }

            if(buildPlatforms.HasFlag(Platforms.OSX)) 
            {
                if (!AssetDatabase.IsValidFolder (path: $"{bundlesBasePath}/macOS")) 
                {
                    AssetDatabase.CreateFolder (parentFolder: $"Assets/{__basePathDir}", newFolderName: "macOS");
                }
                
                AssetBundleManifest __manifest = BuildPipeline.BuildAssetBundles (outputPath: $"{bundlesBasePath}/macOS", 
                                            assetBundleOptions: BuildAssetBundleOptions.None, 
                                            targetPlatform: BuildTarget.StandaloneOSX);

                BuildVersionsFileForPlatform (assetBundleManifest: __manifest, basePath: $"{bundlesBasePath}/macOS");
            }
            if(buildPlatforms.HasFlag(Platforms.IPhone)) 
            {
                if (!AssetDatabase.IsValidFolder (path: $"{bundlesBasePath}/iOS")) 
                {
                    AssetDatabase.CreateFolder (parentFolder: $"Assets/{__basePathDir}", newFolderName: "iOS");
                }
                AssetBundleManifest __manifest = BuildPipeline.BuildAssetBundles (outputPath: $"{bundlesBasePath}/iOS", 
                                            assetBundleOptions: BuildAssetBundleOptions.None, 
                                            targetPlatform: BuildTarget.iOS);

                BuildVersionsFileForPlatform (assetBundleManifest: __manifest, basePath: $"{bundlesBasePath}/iOS");
            }
            if(buildPlatforms.HasFlag(Platforms.Android)) 
            {
                if (!AssetDatabase.IsValidFolder(path: $"{bundlesBasePath}/Android")) 
                {
                    AssetDatabase.CreateFolder(parentFolder: $"Assets/{__basePathDir}", newFolderName: "Android");
                }
                AssetBundleManifest __manifest = BuildPipeline.BuildAssetBundles (outputPath: $"{bundlesBasePath}/Android", 
                                            assetBundleOptions: BuildAssetBundleOptions.None, 
                                            targetPlatform: BuildTarget.Android);

                BuildVersionsFileForPlatform (assetBundleManifest: __manifest, basePath: $"{bundlesBasePath}/Android");
            }
            if(buildPlatforms.HasFlag(Platforms.Windows))
            {
                if (!AssetDatabase.IsValidFolder (path: $"{bundlesBasePath}/Windows")) 
                {
                    AssetDatabase.CreateFolder (parentFolder: $"Assets/{__basePathDir}", newFolderName: "Windows");
                }
                AssetBundleManifest __m = BuildPipeline.BuildAssetBundles (outputPath: $"{bundlesBasePath}/Windows", 
                    assetBundleOptions: BuildAssetBundleOptions.None, 
                    targetPlatform: BuildTarget.StandaloneWindows);

                BuildVersionsFileForPlatform (assetBundleManifest: __m, basePath: $"{bundlesBasePath}/Windows");
            }
        }

        private void BuildVersionsFileForPlatform(in AssetBundleManifest assetBundleManifest, in string basePath)
        {
            string[] __allBundles = AssetDatabase.GetAllAssetBundleNames ();

            List<VersionData> __versionDataList = new List<VersionData> ();

            foreach(string __bundleName in __allBundles)
            {
                VersionData __versionData = new VersionData {bundleName = __bundleName};

                BuildPipeline.GetCRCForAssetBundle(targetPath: $"{basePath}/{__bundleName}", crc: out uint __crc);

                __versionData.version = buildVersions[key: __bundleName];
                __versionData.crc = __crc;

                __versionDataList.Add (item: __versionData);
            }

            VersionDataCollection __versionDataCollection = new VersionDataCollection ();
            __versionDataCollection.bundles = __versionDataList.ToArray ();

            string __versionsContent = JsonUtility.ToJson (obj: __versionDataCollection);

            if(__versionsContent == null) return;
            
            File.WriteAllText (path: $"{basePath}/Versions.txt", contents: __versionsContent);
            AssetDatabase.Refresh ();
        }
        
        #endif

        #endregion
    }
}
