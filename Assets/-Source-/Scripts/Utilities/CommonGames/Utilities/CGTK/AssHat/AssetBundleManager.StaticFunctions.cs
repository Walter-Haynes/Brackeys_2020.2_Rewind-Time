namespace CommonGames.Utilities.CGTK.AssHat
{
    using System;
    using UnityEngine;
    using UnityEngine.Networking;
    
    public sealed partial class AssetBundleManager
    {
        /// <summary>
        /// Unload a specific bundle if loaded, and removes the bundle name from the list
        /// of current loaded bundles inside AssetBundleManager package.
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="unloadAssets"></param>
        public static void UnloadBundle (string bundleName, bool unloadAssets)
        {
            if (Bundles.ContainsKey (key: bundleName)) 
            {
                if (Bundles [key: bundleName] != null)
                    Bundles [key: bundleName].Unload (unloadAllLoadedObjects: unloadAssets);
                Bundles.Remove (key: bundleName);
            }
        }

        /// <summary>
        /// This is the simplest method to load a bundle. You specify the name of the bundle (bundleName) 
        /// and AssetBundleManager will return the bundle as method result, synchronously. This method is 
        /// not the better way to load an asset bundle, except for very particular situations.
        /// </summary>
        /// <param name="bundleName">The name of the requested bundle.</param>
        /// <returns>The loaded bundle instance.</returns>
        public static AssetBundle LoadBundle (string bundleName)
        {
            AssetBundle __ab = AssetBundle.LoadFromFile (path:
                $"{Application.streamingAssetsPath}/{CurrentPlatformString()}/{bundleName}");
            Bundles.Add (key: bundleName, value: __ab);
            return __ab;
        }

        /// <summary>
        /// This is the asynchronous version of the LoadBundle(string) method, and let you supply a delegate to receive 
        /// the bundle when it is ready to use. In the meantime, you can use the AssetBundleManager.Progress instance 
        /// returned from the method, to check the progress status of the download. The Progress class exposes 
        /// the GetProgress() method to check it.
        /// </summary>
        /// <param name="bundleName">The name of the requested bundle.</param>
        /// <param name="finished">Delegate method called when the bundle is successfully loaded, it provide the bundle instance.</param>
        /// <returns>An AssetBundleManager.Progress instance suitable to follow the current download progress.</returns>
        public static Progress LoadBundle (string bundleName, LoadBundleFinishedDelegate finished)
        {
            AssetBundleCreateRequest __createRequest = AssetBundle.LoadFromFileAsync (path:
                $"{Application.streamingAssetsPath}/{CurrentPlatformString()}/{bundleName}");
            
            Instance.StartCoroutine(routine: Instance.LoadBundleCoroutine(r: __createRequest, bundleName: bundleName, finished: finished));
            return new LoadFileProgress (request: __createRequest);
        }
        
        
        /// <summary>
        /// This is the call you have to made, for update versions information inside AssetBundleManager engine. 
        /// Typically, this is the first call to the AssetBundleManager package, and if you aren’t interested in 
        /// hot update of bundles, this call is made only once per run of the game. If you don’t call this method, 
        /// AssetBundleManager doesn’t know versions on the server, and the it always proceeds to download bundles when requested.
        /// The success delegate is called once the versions file is download and processed.The content of the Versions.txt file is passed to the delegate only for debugging purposes; when the delegate is called, its content has already been processed, and internal versions state of AssetBundleManager package has already been updated.
        /// Tip: Call this method at least one time, at the game start.
        /// </summary>
        /// <param name="finished">The called delegate method when the bundle was successfully downloaded.</param>
        /// <param name="error">The called delegate method when there is an error in downloading the bundle.</param>
        public static void DownloadVersions (DownloadVersionsFinishedDelegate finished, DownloadVersionsErrorDelegate error)
        {
            if (Instance.testMode) 
            {
                finished (versions: "Fake Version.txt content");
            }
            else 
            {
                UnityWebRequest __wr = UnityWebRequest.Get (uri:
                    $"{Instance.bundlesBaseUrl}/{CurrentPlatformString()}/Versions.txt");

                // If requested, the package try to download server-side cache management by setting
                // up some header in the request
                if (Instance.disableHttpServerCache) 
                {
                    __wr.SetRequestHeader (name: "Cache-Control", value: "no-cache, no-store, must-revalidate");
                    __wr.SetRequestHeader (name: "Pragma", value: "no-cache");
                    __wr.SetRequestHeader (name: "Expires", value: "0");
                }

                Instance.StartCoroutine (routine: Instance.DownloadVersionsCoroutine (wr: __wr, finished: finished, error: error));
            }
        }
        
        /// <summary>
        /// This method is equivalent to the previous one, except for the fact that AssetBundleManager first check 
        /// versions of bundles by downloading a fresh copy of Version.txt file. For that reason, this method doesn’t 
        /// start immediately the download of the bundle (because it must download Versions.txt first), and returns nothing (void). 
        /// When the download of the bundle starts, the LoadBundleStartedDelegate is called, and the Progress is passed to it, 
        /// so you can monitor the download progress. As in the previous version of the method, delegate methods are called to 
        /// signal the download finished or download error situations.
        /// Tip: Use this method if you want to be secure to download the most updated version of a bundle.
        /// </summary>
        /// <param name="bundleName">The bundle name.</param>
        /// <param name="started">Callback delegate method called when the download starts.</param>
        /// <param name="finished">Callback delegate method called when the download finished successfully.</param>
        /// <param name="error">Callback delegate method called when the download finished with error.</param>
        public static void DownloadUpdatedBundle(string bundleName, LoadBundleStartedDelegate started, LoadBundleFinishedDelegate finished, LoadBundleErrorDelegate error)
        {
            if (Instance.testMode) 
            {
                started (p: LoadBundle (bundleName: bundleName, finished: finished));
            } 
            else 
            {
                DownloadVersions (finished: delegate (string versions) 
                {

                    Progress __p = DownloadBundle (bundleName: bundleName, finished: finished, error: error);

                    started (p: __p);

                }, error: errorString => error(error: $"Error updating bundle versions: {errorString}"));
            }
        }

        /// <summary>
        /// This is the main method to download a bundle. You specify the bundle name, and AssetBundleManager download the bundle 
        /// giving you the chance to monitor the download process via AssetBundleManager.Process instance, immediately returned by this method.
        /// When the download has finished, the LoadBundleFinishedDelegate is called, passing the downloaded asset bundle.In case of error, 
        /// the LoadBundleErrorDelegate is called, passing the error string.
        /// Tip: Call this method to download bundles from a server, without the need to check last-minute updates of the downloaded bundle.
        /// </summary>
        /// <param name="bundleName">The bundle name</param>
        /// <param name="finished">Callback delegate method called when the download has finished.</param>
        /// <param name="error">Callback delegate method called when the download terminated with error.</param>
        /// <returns></returns>
        public static Progress DownloadBundle (string bundleName, LoadBundleFinishedDelegate finished, LoadBundleErrorDelegate error)
        {
            if (Instance.testMode) 
            {
                return LoadBundle(bundleName, finished);
            }

            string __url = $"{Instance.bundlesBaseUrl}/{CurrentPlatformString()}/{bundleName}";
            UnityWebRequest __wr;

            if (Versions.ContainsKey (key: bundleName)) 
            {
                __wr = UnityWebRequestAssetBundle.GetAssetBundle (uri: __url,
                    version: Versions [key: bundleName], crc: CrCs [key: __url]);
            } 
            else 
            {
                __wr = UnityWebRequestAssetBundle.GetAssetBundle (uri: __url);
            }

            if (Instance.disableHttpServerCache) 
            {
                __wr.SetRequestHeader (name: "Cache-Control", value: "no-cache, no-store, must-revalidate");
                __wr.SetRequestHeader (name: "Pragma", value: "no-cache");
                __wr.SetRequestHeader (name: "Expires", value: "0");
            }

            Instance.StartCoroutine (routine: Instance.DownloadBundleCoroutine (wr: __wr, bundleName: bundleName, finished: finished, error: error));
            return new DownloadProgress (webRequest: __wr);
        }
        

        /// <summary>
        /// Return a string representing the current application platform. Used to compose
        /// directory name when create bundles, and when search for bundles locally.
        /// </summary>
        /// <returns></returns>
        private static string CurrentPlatformString()
        {
            switch (Application.platform) 
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "macOS";
                default:
                    return "unknown";
            }
        }

    }

}
