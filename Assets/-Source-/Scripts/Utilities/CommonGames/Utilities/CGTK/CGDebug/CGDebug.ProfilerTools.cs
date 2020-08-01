namespace CommonGames.Utilities.CGTK
{
    using System;
    using UnityEngine;
    using UnityEngine.Profiling;
    using Object = UnityEngine.Object;
    
    public static partial class CGDebug
    {
        public class ProfilerSample : IDisposable
        {
            public ProfilerSample(in string tag)
                => Profiler.BeginSample(name: tag);

            public void Dispose()
                => Profiler.EndSample();
        }
        
    }
}
