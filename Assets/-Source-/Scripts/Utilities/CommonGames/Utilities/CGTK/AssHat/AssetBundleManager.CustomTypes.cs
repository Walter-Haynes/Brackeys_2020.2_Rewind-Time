namespace CommonGames.Utilities.CGTK.AssHat
{
    using System;
    using UnityEngine;
    using UnityEngine.Networking;
    
    public sealed partial class AssetBundleManager
    {

        #region VersionData

        [Serializable]
        public struct VersionData 
        {
     
            public string bundleName;
            public uint version;
            public uint crc;
        }
         
        [Serializable]
        public class VersionDataCollection 
        {
            public VersionData[] bundles;
        }

        #endregion

        #region LoadProgress
         
         /// <summary>
         /// This class act as a tool to monitor in progress downloads. Each method that
         /// downloads anything, will return an instance of this class.
         /// </summary>
         public abstract class Progress
         {
             /// <returns>The current progress of download operation as float [0..1].</returns>
             public abstract float GetProgress { get; }
         }

         /// <summary> Type Progress that monitors a bundle file loading operation. </summary>
         public class LoadFileProgress : Progress
         {
             private readonly AssetBundleCreateRequest _request;

             public LoadFileProgress(AssetBundleCreateRequest request)
             {
                 this._request = request;
             }

             public override float GetProgress
                => _request.progress;
         }

         /// <summary> Type Progress that monitors a bundle web download operation. </summary>
         public class DownloadProgress : Progress
         {

             private readonly UnityWebRequest _webRequest;

             public DownloadProgress(UnityWebRequest webRequest)
             {
                 _webRequest = webRequest;
             }

             public override float GetProgress
                => _webRequest.downloadProgress;
         }

         #endregion

    }

}
