using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiToggle))]

class InstantGuiToggleEditor : InstantGuiElementEditor
{
	float guiScrollPos;
	
	public override void  OnInspectorGUI ()
	{
		InstantGuiToggle script = (InstantGuiToggle)target;

		base.OnInspectorGUI();
		//EditorGUILayout.Space();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "Toggle");
		
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 1;
			
			script.check = EditorGUILayout.Toggle("Checked", script.check);
			script.couldBeUnchecked = EditorGUILayout.Toggle("Could Be Unchecked", script.couldBeUnchecked);
			
			//uncheck toggles array
			EditorGUILayout.LabelField("Uncheck Toggles on Checked:");
			script.uncheckToggles = InstantGuiInspector.DrawElementsArray(script.uncheckToggles);
			
			
			
			
			EditorGUI.indentLevel = 0;
		}
		
		InstantGuiInspector.DrawActivator ("On Checked:", script.onChecked);
		InstantGuiInspector.DrawActivator ("On Unchecked:", script.onUnchecked);
	}
}