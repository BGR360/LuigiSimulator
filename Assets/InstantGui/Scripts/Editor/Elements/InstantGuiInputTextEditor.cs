using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiInputText))]

class InstantGuiInputTextEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiInputText script = (InstantGuiInputText)target;

		base.OnInspectorGUI();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "InputText");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 1;
			
			//script.text = EditorGUILayout.TextField("Text:", script.text);
			script.password = EditorGUILayout.ToggleLeft("Password", script.password);
			script.alwaysTyping = EditorGUILayout.ToggleLeft("Always Typing", script.alwaysTyping);
			script.confirmOnEnterOnly = EditorGUILayout.ToggleLeft("Confirm on Enter Only", script.confirmOnEnterOnly);
			
			EditorGUILayout.Space();
			
			script.cursorWidth = EditorGUILayout.IntField("Cursor Width:", script.cursorWidth);
			script.cursorColor = EditorGUILayout.ColorField("Cursor Color:", script.cursorColor);
			EditorGUIUtility.LookLikeControls();
			//script.cursorTexture = (Texture2D)EditorGUILayout.ObjectField("Cursor Texture:", script.cursorTexture, typeof(Texture2D), false);
			
			EditorGUI.indentLevel = 0;
		}
	}
}
