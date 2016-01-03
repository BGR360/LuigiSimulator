
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiStyleSet))]
class InstantGuiStyleSetEditor : Editor
{
	public override void  OnInspectorGUI ()
	{

		InstantGuiStyleSet script = (InstantGuiStyleSet)target;
		if (script.styles==null) { script.styles = new InstantGuiStyle[1]; script.styles[0] = new InstantGuiStyle();}

		EditorGUI.BeginChangeCheck();

		Undo.RecordObject(script, "InstantGui Style Change");

		Rect rect=new Rect(0,0,0,0);

		if (script.styles!=null)
			for (int i=1; i<script.styles.Length; i++) 
		{
			//if (script.styles[i]==null) script.styles[i] = new InstantGuiStyle();

			EditorGUI.indentLevel = 0;
			rect= GUILayoutUtility.GetRect (50, 18, "TextField");
			
			//name
			rect.x+=10; rect.width-=90;
			script.styles[i].name = EditorGUI.TextField(rect, script.styles[i].name);
			
			//move up
			rect.x = rect.width+30; rect.width=23;
			if (GUI.Button(rect, "∧ ") && i != 1)
			{
				InstantGuiStyle tmp= script.styles[i-1];
				script.styles[i-1] = script.styles[i];
				script.styles[i] = tmp;
				EditorUtility.SetDirty(script);
			}
			
			//move down
			rect.x += 25;
			if (GUI.Button(rect, "∨ ") && i != script.styles.Length-1)
			{
				InstantGuiStyle tmp = script.styles[i+1];
				script.styles[i+1] = script.styles[i];
				script.styles[i] = tmp;
				EditorUtility.SetDirty(script);
			}
			
			//delete
			rect.x += 25;
			if (GUI.Button(rect, "✕ ") && EditorUtility.DisplayDialog("Remove Style?",
			                                                          "Are you sure you want to remove style?",
			                                                          "OK", "Cancel"))
			{
				InstantGuiStyle[] newStyles = new InstantGuiStyle[script.styles.Length-1];
				int counter = 0;
				for (int j=0; j<script.styles.Length; j++)
				{
					if (j != i)
					{
						newStyles[counter] = script.styles[j];
						counter++;
					}
				}
				script.styles = newStyles;
				EditorUtility.SetDirty(script);
			}
			if (i==script.styles.Length) break;
			
			//foldout
			rect.x=20; rect.width=20;
			script.styles[i].show = EditorGUI.Foldout(rect, script.styles[i].show, "");
			
			
			//display style
			if (script.styles[i].show) InstantGuiInspector.DrawStyle(script.styles[i], true);
		}
		
		//adding
		rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x=rect.width-70; rect.width=73;
		if (GUI.Button(rect, "Add"))
		{
			InstantGuiStyle[] newStyles = new InstantGuiStyle[script.styles.Length+1];
			for (int j=0; j<script.styles.Length; j++) newStyles[j] = script.styles[j];
			newStyles[newStyles.Length-1] = new InstantGuiStyle();
			script.styles = newStyles;
			EditorUtility.SetDirty(script);
		}
		
		if (EditorGUI.EndChangeCheck ()) 
		{
			EditorUtility.SetDirty (target);
			if (InstantGui.instance!=null) InstantGui.instance.Update();
		}
		//else Undo.ClearUndo(script);
	}
	
	
}
