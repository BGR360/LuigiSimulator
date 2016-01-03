using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiTextArea))]

class InstantGuiTextAreaEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiTextArea script = (InstantGuiTextArea)target;

		base.OnInspectorGUI();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "TextArea");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 1;
			
			script.slider = (InstantGuiSlider)EditorGUILayout.ObjectField("Slider", script.slider, typeof(InstantGuiSlider), true);
			int newWidth= EditorGUILayout.IntField("Width Adjust:", script.widthAdjust);
			int newHeight= EditorGUILayout.IntField("Height Adjust:", script.heightAdjust);
			if (newWidth!=script.widthAdjust || newHeight!=script.heightAdjust)
			{
				script.widthAdjust = newWidth;
				script.heightAdjust = newHeight;
				script.Action();
				script.ApplyStyle();
			}
			
			EditorGUI.indentLevel = 0;
			
			EditorGUILayout.LabelField("Text:");
			
			script.guiScrollPos = EditorGUILayout.BeginScrollView(script.guiScrollPos, GUILayout.MaxHeight (205));		
			script.rawText = EditorGUILayout.TextArea(script.rawText);	
			EditorGUILayout.EndScrollView();
			
			
			
			
			
		}
	}
}
