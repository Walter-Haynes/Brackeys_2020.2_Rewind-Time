namespace CommonGames.Utilities.CGTK.AssHat
{
    using UnityEngine;
    
    public sealed partial class AssetBundleManager
    {
        
        /// <summary>
        /// Signals that the bundle download was initiated.
        /// </summary>
        /// <param name="p">The name of the bundle being downloaded.</param>
        public delegate void LoadBundleStartedDelegate (Progress p);

        /// <summary>
        /// Signals that the bundle download is terminated.
        /// </summary>
        /// <param name="ab">The resulting bundle instance.</param>
        public delegate void LoadBundleFinishedDelegate (AssetBundle ab);

        /// <summary>
        /// Signals that the bundle download was terminated with error.
        /// </summary>
        /// <param name="error">The error string.</param>
        public delegate void LoadBundleErrorDelegate (string error);

        /// <summary>
        /// Signals that Versions.txt download was successfully terminated.
        /// </summary>
        /// <param name="versions">The content of Versions.txt.</param>
        public delegate void DownloadVersionsFinishedDelegate (string versions);

        /// <summary>
        /// Signals that download of Version.txt was terminated with error.
        /// </summary>
        /// <param name="error">The error string.</param>
        public delegate void DownloadVersionsErrorDelegate (string error);
    }
}