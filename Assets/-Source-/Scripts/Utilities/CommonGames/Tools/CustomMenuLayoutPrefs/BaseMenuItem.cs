using System;

using UnityEngine;

using Sirenix.OdinInspector;

using UnityEditor;

namespace CommonGames.Tools.CustomMenuLayout
{
    [Serializable]
    public abstract class BaseMenuItem
    {
        #region Variables

        [HorizontalGroup(@group: "Horizontal", LabelWidth = 0, Width = 100), HideLabel]
        public ElementType elementType;

        #region None

        [ShowIf(nameof(elementType), ElementType.None)]
        [HorizontalGroup(@group: "Horizontal", LabelWidth = 0f), HideLabel]
        [DisplayAsString]
        [SerializeField] private string separator = "Please select an Element Type from the dropdown.";

        #endregion

        #region MenuPath

        [ShowIf(nameof(elementType), ElementType.MenuPath)]
        [HorizontalGroup(@group: "Horizontal", LabelWidth = 0f), HideLabel]
        [ValueDropdown(memberName: nameof(AssetMenus))]
        public string originalPath;

        [ShowIf(nameof(elementType), ElementType.MenuPath)]
        [HorizontalGroup(@group: "Horizontal", LabelWidth = 0f), HideLabel]
        public string overridePath;

        protected abstract string MenuPath { get; }

        #endregion

        #endregion

        #region Methods

        /// <summary> Gets a tree-view of all the submenus of "<see cref="MenuPath"/>". </summary>
        /// <returns> A tree-view of all the submenus of "<see cref="MenuPath"/>". </returns>
        private string[] AssetMenus => Unsupported.GetSubmenus(menuPath: MenuPath);

        /// <summary> Gets a tree-view of all the submenus of <para>menuPath</para>. </summary>
        /// <returns> A tree-view of all the submenus of <para>menuPath</para>. </returns>
        private static string[] GetMenus(in string menuPath) => Unsupported.GetSubmenus(menuPath: menuPath);

        #endregion
    }

    public enum ElementType
    {
        None,
        MenuPath,
        Separator,
    }
}