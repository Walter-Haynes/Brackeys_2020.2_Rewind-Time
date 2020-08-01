using System;

using UnityEngine;

namespace CommonGames.Tools.CustomMenuLayout
{
    [Serializable]
    public sealed class HierarchyMenuItem : BaseMenuItem
    {
        private const string _MENUPATH = "GameObject";
        
        protected override string MenuPath => _MENUPATH;
    }
    
    // ReSharper disable once RequiredBaseTypesConflict
    [CreateAssetMenu(
        menuName = "Common-Games/CustomMenuLayout/Hierarchy Layout Preference",
        fileName = "New HierarchyMenuLayout")]
    public sealed class HierarchyMenuLayout : BaseLayoutAsset<HierarchyMenuLayout, HierarchyMenuItem>
    {
    }
}