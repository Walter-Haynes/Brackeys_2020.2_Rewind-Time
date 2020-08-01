namespace CommonGames.Utilities.CustomTypes
{
	using System;

	#if UNITY_EDITOR

	using UnityEditor;
	
	using UnityEditor.UIElements;
	
	using UnityEngine.UIElements;

	#endif

	[Serializable]
	public struct RangedFloat
	{
		public float MinValue { get; }
		public float MaxValue { get; }

		public RangedFloat(in float MinValue, in float MaxValue)
		{
			this.MinValue = MinValue;
			this.MaxValue = MaxValue;
		}
	}

	#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(CommonGames.Utilities.CustomTypes.RangedFloat), useForChildren: true)]
	public class RangedFloatDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement __container = new VisualElement();
			
			PropertyField __MinProperty = new PropertyField(property.FindPropertyRelative(relativePropertyPath: "MinValue"));
			PropertyField __MaxProperty = new PropertyField(property.FindPropertyRelative(relativePropertyPath: "MaxValue"));
			
			__container.Add(__MinProperty);
			__container.Add(__MaxProperty);

			return __container;
		}
		
		/*
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			SerializedProperty __minProp = property.FindPropertyRelative(relativePropertyPath: "MinValue");
			SerializedProperty __maxProp = property.FindPropertyRelative(relativePropertyPath: "MaxValue");

			float __minValue = __minProp.floatValue;
			float __maxValue = __maxProp.floatValue;

			float __rangeMin = 0;
			float __rangeMax = 1;

			MinMaxRangeAttribute[] __ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof (MinMaxRangeAttribute), true);
			if (__ranges.Length > 0)
			{
				__rangeMin = __ranges[0].Min;
				__rangeMax = __ranges[0].Max;
			}

			const float __RANGE_BOUNDS_LABEL_WIDTH = 40f;

			Rect __rangeBoundsLabel1Rect = new Rect(position) {width = __RANGE_BOUNDS_LABEL_WIDTH};
		
			GUI.Label(__rangeBoundsLabel1Rect, new GUIContent(__minValue.ToString("F2")));
			position.xMin += __RANGE_BOUNDS_LABEL_WIDTH;

			Rect __rangeBoundsLabel2Rect = new Rect(position);
			__rangeBoundsLabel2Rect.xMin = __rangeBoundsLabel2Rect.xMax - __RANGE_BOUNDS_LABEL_WIDTH;
			GUI.Label(__rangeBoundsLabel2Rect, new GUIContent(__maxValue.ToString("F2")));
			position.xMax -= __RANGE_BOUNDS_LABEL_WIDTH;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, ref __minValue, ref __maxValue, __rangeMin, __rangeMax);
			if (EditorGUI.EndChangeCheck())
			{
				__minProp.floatValue = __minValue;
				__maxProp.floatValue = __maxValue;
			}

			EditorGUI.EndProperty();
		}
		*/
	}


	#endif
}