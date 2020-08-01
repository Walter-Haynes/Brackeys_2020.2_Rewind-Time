using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using JetBrains.Annotations;

#if ODIN_INSPECTOR
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
#endif

namespace CommonGames.Tools.CustomMenuLayout
{
    using CommonGames.Utilities;

    public abstract class BaseLayoutAsset<T_Inheritor, T_MenuItem> : ScriptableMultiton<T_Inheritor>
        where T_Inheritor : BaseLayoutAsset<T_Inheritor, T_MenuItem>
        where T_MenuItem : BaseMenuItem
    {
        #region Variables

        #region Current Menu Layout Toggle

        [PropertyOrder(-1)]
        [BoxGroup("Toggle", showLabel: false)]
        [GUIColor(nameof(GetCurrentButtonColor))]
        [Button(ButtonSizes.Large)]
        private void ToggleCurrentMenuLayout()
        {
            IsCurrentMenuLayout = !IsCurrentMenuLayout;
            CustomMenuLayoutManager.Validate();
        }

        private Color GetCurrentButtonColor
            => IsCurrentMenuLayout ? Color.green : Color.red;
        
        #endregion
        
        private bool _isCurrentMenuLayout = false;
        [PublicAPI]
        public bool IsCurrentMenuLayout
        {
            get => _isCurrentMenuLayout;
            private set
            {
                if(value)
                {
                    foreach(T_Inheritor __instance in Instances)
                    {
                        if(__instance == this) continue;
                        __instance.IsCurrentMenuLayout = false;
                    }
                }
                
                _isCurrentMenuLayout = value;
            }
        }

        [BoxGroup("Menu", showLabel: false)]
        [ListDrawerSettings(NumberOfItemsPerPage = 30, Expanded = true, DraggableItems = true)]
        [HideReferenceObjectPicker]
        [SerializeReference]
        public List<T_MenuItem> layout = new List<T_MenuItem>();

        #endregion

        #region Custom Types

        #endregion

        #region Methods

        private void Reset()
        {
            CustomMenuLayoutManager.Validate();
        }

        private void OnValidate()
        {
            CustomMenuLayoutManager.Validate();
        }

        #endregion

    }
}