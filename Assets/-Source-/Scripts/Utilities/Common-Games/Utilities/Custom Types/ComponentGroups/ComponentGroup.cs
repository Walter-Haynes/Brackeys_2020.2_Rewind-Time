using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

#if ODIN_INSPECTOR && UNITY_EDITOR
using UnityEditor;

using Sirenix.OdinInspector.Editor;
#endif

namespace CommonGames.Utilities.CustomTypes
{
    using CommonGames.Utilities.Extensions;   
    
    public class ComponentGroup : MonoBehaviour
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        //[ListDrawerSettings(IsReadOnly = true)]
        [ListDrawerSettings(ShowIndexLabels = true, Expanded = false)]
        [SerializeField] private Component[] components = { };

        private Component[] _lastComponents = { };

        private void OnValidate()
        {
            IEnumerable<Component> __newComponents = default;
            IEnumerable<Component> __oldComponents = default;

            if(components == null && _lastComponents == null) return;
            
            if(components == null)
            {
                components = default;

                if(_lastComponents != null)
                {
                    __oldComponents = _lastComponents;   
                }
            }

            if(_lastComponents == null)
            {
                _lastComponents = new Component[]{};
                
                if(components != null)
                {
                    __newComponents = components;   
                }
            }

            if(components != null && _lastComponents != null)
            {
                __newComponents = components.Except(_lastComponents);
                __oldComponents = _lastComponents.Except(components);   
            }

            /*
            if(components.Length == 0 || components == default)
            {
                "Dimmadome".Log();
                
                _lastComponents = components;
                return;
            }
            */

            //if(components != null && _lastComponents != null)

            if(__newComponents != null)
            {
                foreach(Component __newComponent in __newComponents)
                {
                    if(__newComponent == null) continue;
                    
                    __newComponent.hideFlags = HideFlags.HideInInspector;
                        
                    Debug.Log("HideFlags done");
                }
            }

            if(__oldComponents != null)
            {
                foreach(Component __oldComponent in __oldComponents)
                {
                    if(__oldComponent != null)
                    {
                        __oldComponent.hideFlags = HideFlags.None;
                    }
                }
            }

            _lastComponents = components;
            
            Debug.Log("Last Componenets has been set");
        }

        //private void OnDestroy() => ResetComponents();

        private void Reset() => ResetComponents();

        public void ResetComponents()
        {
            Debug.Log("<color=cyan> Drop The Reset</color>");

            if(components == null)
            {
                components = default;
                _lastComponents = default;
                return;
            }

            components.ForEach(component => component.hideFlags = HideFlags.None);

            components = default;
            _lastComponents = default;
        }
    }

    #if UNITY_EDITOR

    [CustomEditor(inspectedType: typeof(ComponentGroup))]
    public class ComponentGroupEditor : OdinEditor
    {
        ComponentGroup _componentGroup;

        protected override void OnEnable()
        {
            _componentGroup = (ComponentGroup) target;
        }

        /*
        protected void OnDestroy()
        {
            _componentGroup.ResetComponents();
        }
        */
    }

    #endif
}

/*
// Custom data struct, for demonstration.
[Serializable]
public struct ComponentList
{
    
}


#if UNITY_EDITOR

[DrawerPriority(0, 0, 2)]
public class MyClassListDrawer<TList, TElement> : OdinValueDrawer<TList>
    where TList : IList<TElement>
    where TElement : Component
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        SirenixEditorGUI.DrawSolidRect(EditorGUILayout.GetControlRect(), new Color(1, 0.5f, 0));
        this.CallNextDrawer(label);
    }
}

public class CustomStructDrawer : OdinValueDrawer<ComponentList>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        ComponentList __value = this.ValueEntry.SmartValue;

        var rect = EditorGUILayout.GetControlRect();

        // In Odin, labels are optional and can be null, so we have to account for that.
        if (label != null)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
        }

        var prev = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 20;

        //__value.X = EditorGUI.Slider(rect.AlignLeft(rect.width * 0.5f), "X", __value.X, 0, 1);
        //__value.Y = EditorGUI.Slider(rect.AlignRight(rect.width * 0.5f), "Y", __value.Y, 0, 1);

        EditorGUIUtility.labelWidth = prev;

        this.ValueEntry.SmartValue = __value;
    }
}
*/