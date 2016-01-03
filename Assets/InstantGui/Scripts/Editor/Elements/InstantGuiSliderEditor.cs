using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiSlider))]

class InstantGuiSliderEditor : InstantGuiElementEditor
{

	public override void  OnInspectorGUI ()
	{
		InstantGuiSlider script = (InstantGuiSlider)target;

		base.OnInspectorGUI();
		//EditorGUILayout.Space();
		
		script.guiElementProps = EditorGUILayout.Foldout(script.guiElementProps, "Slider");
		if (script.guiElementProps)
		{
			EditorGUI.indentLevel = 2;
			script.value = EditorGUILayout.FloatField("Value:", script.value);
			script.min = EditorGUILayout.FloatField("Min:", script.min);
			script.max = EditorGUILayout.FloatField("Max:", script.max);
			script.step = EditorGUILayout.FloatField("Step:", script.step);
			script.shownValue = EditorGUILayout.FloatField("Shown Value:", script.shownValue);
			script.buttonStep = EditorGUILayout.FloatField("Button Step:", script.buttonStep);
			script.horizontal = EditorGUILayout.Toggle("Is Horizontal:", script.horizontal);
			
			script.diamond = (InstantGuiElement)EditorGUILayout.ObjectField("Diamond:", script.diamond, typeof(InstantGuiElement), true);
			script.incrementButton = (InstantGuiElement)EditorGUILayout.ObjectField("Increment Button:", script.incrementButton, typeof(InstantGuiElement), true);
			script.decrementButton = (InstantGuiElement)EditorGUILayout.ObjectField("Decrement Button:", script.decrementButton, typeof(InstantGuiElement), true);
			EditorGUI.indentLevel = 0;
		}
		/*
		if (GUI.changed) 
		{
			guiChanged = true;
			if (!InstantGui.instance) InstantGui.instance = FindObjectOfType(typeof(InstantGui));
			InstantGui.instance.Update();
		}
		*/
	}
}
