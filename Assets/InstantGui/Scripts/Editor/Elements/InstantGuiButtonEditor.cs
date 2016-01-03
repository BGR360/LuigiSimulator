using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiButton))]

class InstantGuiButtonEditor : InstantGuiElementEditor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiButton script = (InstantGuiButton)target;

		base.OnInspectorGUI();
		if (!script) return;
		
		//EditorGUILayout.Space();
		
		InstantGuiInspector.DrawActivator ("On Pressed:", script.onPressed);
	}
}
