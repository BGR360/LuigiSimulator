
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(InstantGuiElement))]
public class InstantGuiElementEditor : Editor
{
	bool  listeningForGuiChanges;
	bool  guiChanged;
	static string[] positionPresets = {"Maximize", "Frame", "Mirror", "Center", "Fixed Width to Center", "Fixed Height to Center"};
	
	public override void  OnInspectorGUI ()
	{
		EditorGUI.BeginChangeCheck();

		InstantGuiElement script = (InstantGuiElement)target;

		Undo.RecordObject(script, "InstantGui Change");
		
		//EditorGUIUtility.LookLikeInspector ();
		//DrawDefaultInspector ();
		
		//text
		string newText = EditorGUILayout.TextField("Text:", script.text);
		script.guiLinkText = EditorGUILayout.Toggle("Use Object Name:", script.guiLinkText);
		if (script.guiLinkText && !EditorApplication.isPlayingOrWillChangePlaymode && script.GetType() != typeof(InstantGuiTextArea))
		{
			if (script.text != newText) //if text changed
			{
				if (newText.Length != 0) script.gameObject.name = newText;
				else script.gameObject.name = "GuiElement";
				script.text = newText;
			}
			if (script.text != script.gameObject.name && newText.Length != 0) script.text = script.gameObject.name;
		}
		else script.text = newText;
		
		//setting style
		script.guiStyle = EditorGUILayout.Foldout(script.guiStyle, "Style");
		if (script.guiStyle)
		{
			EditorGUI.indentLevel = 1;
			
			script.styleSet = (InstantGuiStyleSet)EditorGUILayout.ObjectField("Style Set:", script.styleSet, typeof(InstantGuiStyleSet), false);
			bool  customStyle = EditorGUILayout.Toggle("Custom Style:", script.customStyle);
			
			//custom style
			if (customStyle)
			{
				if (!script.customStyle) 
				{
					if (script.style!=null) script.style = script.style.Clone();
					else script.style = new InstantGuiStyle();
				}
				script.style.name = EditorGUILayout.TextField("Name:", script.style.name);
				script.styleName = script.style.name;
				
				InstantGuiInspector.DrawStyle(script.style, false);
				
				EditorGUILayout.Space();
				if (script.styleSet!=null && script.style!=null && script.style.name.Length>0) InstantGuiInspector.DrawSaveToStyleButton("Add to StyleSet", script, true, false);
			}
			
			//else
			if (!customStyle && script.styleSet!=null)
			{
				string[] styleNames = new string[script.styleSet.styles.Length];
				int selectedStyle = -1; //default if style could not be found
				
				//compiling popup array
				for (int i=0; i<script.styleSet.styles.Length; i++) 
					styleNames[i] = script.styleSet.styles[i].name;
				
				//finding popup selected
				if (script.style==null) selectedStyle = 0;
				else 
					for (int i=0; i<styleNames.Length; i++) 
						if (styleNames[i] == script.styleName) 
							selectedStyle = i;
				
				styleNames[0] = "None";
				
				int newSelected = EditorGUILayout.Popup("Style:", selectedStyle, styleNames);
				if (newSelected != selectedStyle) 
				{
					script.style = script.styleSet.styles[newSelected];
					script.styleName = script.style.name;
					//script.styleNum = newSelected;
				}
			}
			
			script.customStyle = customStyle;
			
			EditorGUI.indentLevel = 0;
		}
		
		//position
		script.guiPosition = EditorGUILayout.Foldout(script.guiPosition, "Position");
		if (script.guiPosition)
		{
			EditorGUI.indentLevel = 1;
			
			script.useStylePlacement = EditorGUILayout.ToggleLeft("Use Style Placement", script.useStylePlacement);
			
			if (!script.useStylePlacement)
			{
				int preset= EditorGUILayout.Popup("Preset:", -1, positionPresets);
				switch (preset)
				{
					//case 0: if (script.style!=null) { script.relative.Set(script.style.relative); script.offset.Set(script.style.offset); script.layerOffset = script.style.layerOffset; } break;
				case 0: script.relative.Set(0,100,0,100); script.offset.Set(0,0,0,0); break;
				case 1: script.relative.Set(10,90,10,90); script.offset.Set(10,-10,10,-10); break;
				case 2: script.offset.right = -script.offset.left; script.offset.bottom = -script.offset.top; break;
				case 3: script.relative.Set(50,50,50,50); script.offset.Set(20,-20,20,-20); break;
				case 4: if (script.style!=null) {script.offset.left = (int)(-script.style.fixedWidthSize*0.5f); script.offset.right = (int)(script.style.fixedWidthSize*0.5f);} break;
				case 5: if (script.style!=null) {script.offset.top = (int)(-script.style.fixedHeightSize*0.5f); script.offset.bottom = (int)(script.style.fixedHeightSize*0.5f);} break;
				}
				
				InstantGuiInspector.DrawElementPosLabels("", "Left", "Right", "Top", "Bottom");
				script.relative = InstantGuiInspector.DrawElementPos("Relative:", script.relative);
				script.offset = InstantGuiInspector.DrawElementPos("Offset:", script.offset);
				InstantGuiInspector.DrawElementPosLabels("Absolute:", script.absolute.left.ToString(), script.absolute.right.ToString(), script.absolute.top.ToString(), script.absolute.bottom.ToString());
				script.layerOffset = InstantGuiInspector.DrawLayerOffset("Layer Offset:", script.layerOffset);
				
				script.lockPosition = EditorGUILayout.Toggle("Lock Position", script.lockPosition);
				
				if (script.styleSet!=null && script.style!=null && script.style.name!=null && script.style.name.Length>0) InstantGuiInspector.DrawSaveToStyleButton("Set as Default in Style", script, false, true);
			}
			EditorGUI.indentLevel = 0;
		}
		
		script.guiAttributes = EditorGUILayout.Foldout(script.guiAttributes, "Attributes");
		if (script.guiAttributes)
		{
			EditorGUI.indentLevel = 1;
			
			//EditorGUI.indentLevel = 20;
			script.dynamic = EditorGUILayout.Toggle("Dynamic", script.dynamic);
			script.editable = EditorGUILayout.Toggle("Editable", script.editable);
			script.pointed = EditorGUILayout.Toggle("Pointed", script.pointed);
			script.disabled = EditorGUILayout.Toggle("Disabled", script.disabled);
			script.activated = EditorGUILayout.Toggle("Activated", script.activated);
			script.pressed = EditorGUILayout.Toggle("Pressed", script.pressed);
			//script.checkbutton = EditorGUILayout.Toggle("\tCheckbutton", script.checkbutton);
			script.check = EditorGUILayout.Toggle("Checked", script.check);
			script.instant = EditorGUILayout.Toggle("Instant", script.instant);
			script.password = EditorGUILayout.Toggle("Password", script.password);
			
			EditorGUI.indentLevel = 0;
		}
		
		if (EditorGUI.EndChangeCheck ()) 
		{
			EditorUtility.SetDirty (script);
			InstantGui.instance.Update();
		}
		else Undo.ClearUndo(script);
		
	}
	
	
}
