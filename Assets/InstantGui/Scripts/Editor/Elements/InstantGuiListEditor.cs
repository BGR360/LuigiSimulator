using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiList))]

class InstantGuiListEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiList script = (InstantGuiList)target;

		base.OnInspectorGUI();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "List");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 1;
			
			//showing labels array;
			script.guiShowLabels = EditorGUILayout.Foldout(script.guiShowLabels, "Labels");
			if (script.guiShowLabels) script.labels = InstantGuiInspector.DrawStringArray(script.labels, 4);
			
			script.lineHeight = EditorGUILayout.IntField("Line Height:", script.lineHeight);
			script.selected = EditorGUILayout.IntField("Selected:", script.selected);
			script.firstShown = EditorGUILayout.IntField("First Shown:", script.firstShown);
			script.slider = (InstantGuiSlider)EditorGUILayout.ObjectField("Slider:", script.slider, typeof(InstantGuiSlider), true);
			script.sliderMargin = EditorGUILayout.IntField("Slider Margin:", script.sliderMargin);
			
			//setting style
			//this is a clone of element editor section.
			string[] styleNames = new string[script.styleSet.styles.Length];
			int selectedStyle = -1; //default if style could not be found
			
			//compiling popup array
			for (int i=0; i<script.styleSet.styles.Length; i++) 
				styleNames[i] = script.styleSet.styles[i].name;
			
			//finding popup selected
			if (script.elementStyle==null) selectedStyle = 0;
			else 
				for (int i=0; i<styleNames.Length; i++) 
					if (styleNames[i] == script.elementStyleName) 
						selectedStyle = i;
			
			styleNames[0] = "None";
			
			int newSelected = EditorGUILayout.Popup("Element Style:", selectedStyle, styleNames);
			if (newSelected != selectedStyle) 
			{
				script.elementStyle = script.styleSet.styles[newSelected];
				script.elementStyleName = script.elementStyle.name;
				//script.elementStyleNum = newSelected;
			}
			
			EditorGUI.indentLevel = 0;
		}
	}
}
