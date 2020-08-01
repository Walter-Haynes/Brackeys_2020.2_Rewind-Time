using CommonGames.Utilities.Extensions;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using System.Collections.Generic;

#endif
  
public class TagAttribute : PropertyAttribute
{
    public bool isUnityTagField = false;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagSelectorPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position: position, property: property, label: label);
            
            return;
        }
            
        EditorGUI.BeginProperty(totalPosition: position, label: label, property: property);

        if(!(this.attribute is TagAttribute __attribute)) return;

        if(__attribute.isUnityTagField)
        {
            property.stringValue = EditorGUI.TagField(position: position, label: label, tag: property.stringValue);
        }
        else
        {
            List<string> __tagList = new List<string> {"[null]"};
            __tagList.AddRange(collection: UnityEditorInternal.InternalEditorUtility.tags);
            string __propertyString = property.stringValue;
            int __index = -1;

            if(__propertyString.IsNullOrWhitespace())
            {
                //The tag is empty
                __index = 0; //first index is the special <notag> entry
            }
            else
            {
                //check if there is an entry that matches the entry and get the index
                //we skip index 0 as that is a special custom case
                for(int __i = 1; __i < __tagList.Count; __i++)
                { 
                    if(__tagList[index: __i] != __propertyString) continue;
                 
                    __index = __i;
                    break;
                }
            }

            //Draw the popup box with the current selected index
            __index = EditorGUI.Popup(position: position, label: label.text, selectedIndex: __index, displayedOptions: __tagList.ToArray());

            //Adjust the actual string value of the property based on the selection
            if(__index == 0)
            {
                property.stringValue = null;
            }
            else if(__index > 0)
            {
                property.stringValue = __tagList[index: __index];
            }
            else //In case we have something selected that was a tag once but isn't anymore.
            {
                property.stringValue = null;
            }
        }

        EditorGUI.EndProperty();
    }

    private void GenerateTagList()
    {

    }
}

#endif