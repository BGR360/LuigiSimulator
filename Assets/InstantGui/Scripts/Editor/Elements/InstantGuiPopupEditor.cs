using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiPopup))]

class InstantGuiPopupEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiPopup script = (InstantGuiPopup)target;

		base.OnInspectorGUI();
		//EditorGUILayout.Space();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "Popup");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 2;
			
			script.selected = EditorGUILayout.IntField("Selected:", script.selected);
			script.list = (InstantGuiList)EditorGUILayout.ObjectField("List:", script.list, typeof(InstantGuiList), true);
			
			
			
			EditorGUI.indentLevel = 0;
		}

	}
}
