
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiWindow))]

class InstantGuiWindowEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiWindow script = (InstantGuiWindow)target;

		base.OnInspectorGUI();
		//EditorGUILayout.Space();
		
		//DrawActivator ("On Pressed:", script.onPressed);
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "Window");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 2;
			script.movable = EditorGUILayout.Toggle("Movable", script.movable);
			script.scape = (InstantGuiWindowScape)EditorGUILayout.EnumPopup("Move Scape:", script.scape);
			script.closeButton = (InstantGuiElement)EditorGUILayout.ObjectField("Close Button:", script.closeButton, typeof(InstantGuiElement), true);
			//script.expandButton = (InstantGuiElement)EditorGUILayout.ObjectField("Expand Button:", script.expandButton, typeof(InstantGuiElement), true);
			EditorGUI.indentLevel = 0;
		}
	}
}
