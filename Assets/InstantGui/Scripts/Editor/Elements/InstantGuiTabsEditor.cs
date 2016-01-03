using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiTabs))]

class InstantGuiTabsEditor : InstantGuiElementEditor
{
	float guiScrollPos;
	
	public override void  OnInspectorGUI ()
	{
		InstantGuiTabs script = (InstantGuiTabs)target;

		base.OnInspectorGUI();
		//EditorGUILayout.Space();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "Tabs");
		
		//some non-gui actions
		for (int i=0; i<script.tabs.Length; i++)
		{
			//creating field if it does not exists
			if (!script.fields[i]) script.fields[i] = script.CreateField(); 
			
			//fields names
			if (script.fields[i].transform.name.Length==0) script.fields[i].transform.name = script.tabs[i].transform.name + "_Field";
		}
		
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 1;
			
			script.selected = EditorGUILayout.IntField("Selected:", script.selected);
			
			//array
			Rect rect = GUILayoutUtility.GetRect (150, 16, "TextField");
			rect.width = rect.width*0.5f-40;
			EditorGUI.LabelField(rect, "Tabs:");
			rect.x += rect.width;
			EditorGUI.LabelField(rect, "Fields:");
			
			script.tabs = InstantGuiInspector.DrawElementsTable(script.tabs, script.fields);
			
			if (script.tabs.Length != script.fields.Length) //changing fields count (creating if necessary)
			{
				InstantGuiElement[] newFields = new InstantGuiElement[script.tabs.Length];
				for (int j=0; j<script.tabs.Length; j++)
				{
					if (j>=script.fields.Length || !script.fields[j]) { newFields[j] = script.CreateField();continue; }
					newFields[j] = script.fields[j];
				}
				
				script.fields = newFields;
			}
			
			EditorGUI.indentLevel = 0;
		}
		
		InstantGuiInspector.DrawActivator ("On Checked:", script.onChecked);
		InstantGuiInspector.DrawActivator ("On Unchecked:", script.onUnchecked);
	}
}
