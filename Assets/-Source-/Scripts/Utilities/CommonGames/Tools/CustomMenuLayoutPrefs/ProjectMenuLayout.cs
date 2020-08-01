using System;

using UnityEngine;

namespace CommonGames.Tools.CustomMenuLayout
{
    [Serializable]
    public sealed class ProjectMenuItem : BaseMenuItem
    {
        private const string _MENUPATH = "Assets";
        
        protected override string MenuPath => _MENUPATH;
    }
    
    [CreateAssetMenu(menuName = "Common-Games/CustomMenuLayout/Project Layout Preference", fileName = "New ProjectMenuLayout")]
    public sealed class ProjectMenuLayout : BaseLayoutAsset<ProjectMenuLayout, ProjectMenuItem>
    {
    }
}