
using UnityEngine;
using UnityEditor;
using System.Collections;

public class InstantGuiInspector : MonoBehaviour 
{
	
	
	static int indentPixels = 15;
	static float[] scrolls = new float[10];
	

	static public InstantGuiElementPos  DrawElementPos ( string label ,   InstantGuiElementPos pos  )
	{
		int space = 5; 
		int labelWidth = 60;
		
		Rect rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=EditorGUI.indentLevel*indentPixels; rect.width-=EditorGUI.indentLevel*indentPixels;
		int originalWidth= (int)rect.width;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		rect.width = labelWidth; EditorGUI.LabelField(rect,label);
		rect.width = (originalWidth-labelWidth)*0.25f - space;
		
		rect.x += labelWidth+space; pos.left = EditorGUI.IntField(rect,pos.left);
		rect.x += rect.width+space; pos.right = EditorGUI.IntField(rect,pos.right);
		rect.x += rect.width+space; pos.top = EditorGUI.IntField(rect,pos.top);
		rect.x += rect.width+space; pos.bottom = EditorGUI.IntField(rect,pos.bottom);
		
		EditorGUI.indentLevel = indent;
		
		return pos;
	}
	
	static public float DrawLayerOffset ( string label ,   float val  ){
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		Rect rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=(indent)*indentPixels; rect.width-=(indent)*indentPixels;
		EditorGUI.LabelField(rect, label);
		rect.x += rect.width - (rect.width-60)*0.25f + 5; rect.width = (rect.width-60)*0.25f - 5;
		val = EditorGUI.FloatField(rect, val);
		
		EditorGUI.indentLevel = indent;
		
		return val;
	}
	
	static public void  DrawElementPosLabels ( string label0 ,   string label1 ,   string label2 ,   string label3 ,   string label4  ){
		int space = 5; 
		int labelWidth = 60;
		
		Rect rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=EditorGUI.indentLevel*indentPixels; rect.width-=EditorGUI.indentLevel*indentPixels;
		int originalWidth= (int)rect.width;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		rect.width = labelWidth; EditorGUI.LabelField(rect,label0);
		rect.width = (originalWidth-labelWidth)*0.25f - space;
		
		rect.x += labelWidth+space; EditorGUI.LabelField(rect,label1);
		rect.x += rect.width+space; EditorGUI.LabelField(rect,label2);
		rect.x += rect.width+space; EditorGUI.LabelField(rect,label3);
		rect.x += rect.width+space; EditorGUI.LabelField(rect,label4);
		
		EditorGUI.indentLevel = indent;
	}
	
	
	static public void  DrawActivator ( string label ,   InstantGuiActivator activator  )
	{
		if (activator==null) return;
		
		activator.guiDraw = EditorGUILayout.Foldout(activator.guiDraw, label);
		if (activator.guiDraw)
		{
			EditorGUI.indentLevel = 1;
			
			EditorGUILayout.LabelField("Enable Objects:");
			activator.enableObjects = DisplayGameObjectArray(activator.enableObjects,0);
			EditorGUILayout.LabelField("Disable Objects:");
			activator.disableObjects = DisplayGameObjectArray(activator.disableObjects,1);
			activator.message = EditorGUILayout.TextField("Send Message:", activator.message);
			activator.messageRecievers = (MessageRecievers)EditorGUILayout.EnumPopup("Recievers:", activator.messageRecievers);
			activator.messageInEditor = EditorGUILayout.ToggleLeft("Send Message in Edit Mode", activator.messageInEditor);
			EditorGUI.indentLevel = 0;
		}
	}
	
	
	
	static public GameObject[] DisplayGameObjectArray ( GameObject[] array ,   int scrollNum  ){
		int space = 2;
		int buttonSize = 20;
		
		GameObject[] newArray = array;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		int scrollSize = Mathf.Min(array.Length*20, 110);
		
		if (array.Length>6)
			scrolls[scrollNum] = EditorGUILayout.BeginScrollView(new Vector2(0,scrolls[scrollNum]), GUILayout.Height(scrollSize)).y;

		Rect rect = new Rect();
		for (int i=0; i<array.Length; i++)
		{
			rect = GUILayoutUtility.GetRect (150, 16, "TextField");
			rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
			rect.width -= (buttonSize+space)*3;
			array[i] = (GameObject)EditorGUI.ObjectField (rect, array[i], typeof(GameObject), true);
			
			//move up
			rect.x += rect.width+space; rect.width = buttonSize; 
			if (GUI.Button(rect, "∧ ") && i != 0)
			{
				GameObject tmp= array[i-1];
				array[i-1] = array[i];
				array[i] = tmp;
			}
			
			//move down
			rect.x += buttonSize+space;
			if (GUI.Button(rect, "∨ ") && i != array.Length-1)
			{
				GameObject tmp = array[i+1];
				array[i+1] = array[i];
				array[i] = tmp;
			}
			
			//delete
			rect.x += buttonSize+space;
			if (GUI.Button (rect, "X"))
			{
				newArray = new GameObject[array.Length-1];
				for (int j=0; j<newArray.Length; j++) 
				{
					if (j<i) newArray[j] = array[j];
					else newArray[j] = array[j+1];
				}
			}
		}
		
		if (array.Length>6) EditorGUILayout.EndScrollView();
		
		//adding a little space after	
		if (array.Length>0) GUILayoutUtility.GetRect (150, 2, "TextField");
		
		//adding new obj
		rect = GUILayoutUtility.GetRect (150, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		rect.width -= (buttonSize+space)*3;
		GameObject addedVal = (GameObject)EditorGUI.ObjectField (rect, null, typeof(GameObject), true);
		if (addedVal!=null) 
		{
			newArray = new GameObject[array.Length+1];
			for (int j=0; j<array.Length; j++) newArray[j] = array[j];
			newArray[newArray.Length-1] = addedVal;
		}
		
		//adding a little space after	
		rect = GUILayoutUtility.GetRect (150, 3, "TextField");
		
		EditorGUI.indentLevel = indent;
		
		return newArray;
	}
	
	
	static public string[] DrawStringArray ( string[] array ,   int scrollNum  ){
		int space = 2;
		int buttonSize = 20;
		
		string[] newArray = array;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		int scrollSize = Mathf.Min(array.Length*20, 110);
		
		if (array.Length>6)
			scrolls[scrollNum] = EditorGUILayout.BeginScrollView(new Vector2(0,scrolls[scrollNum]), GUILayout.Height(scrollSize)).y;

		Rect rect = new Rect();
		for (int i=0; i<array.Length; i++)
		{
			rect = GUILayoutUtility.GetRect (150, 16, "TextField");
			rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
			rect.width -= (buttonSize+space)*3;
			array[i] = EditorGUI.TextField (rect, array[i]);
			
			//move up
			rect.x += rect.width+space; rect.width = buttonSize; 
			if (GUI.Button(rect, "∧ ") && i != 0)
			{
				string tmp= array[i-1];
				array[i-1] = array[i];
				array[i] = tmp;
			}
			
			//move down
			rect.x += buttonSize+space;
			if (GUI.Button(rect, "∨ ") && i != array.Length-1)
			{
				string tmp = array[i+1];
				array[i+1] = array[i];
				array[i] = tmp;
			}
			
			//delete
			rect.x += buttonSize+space;
			if (GUI.Button (rect, "X"))
			{
				newArray = new string[array.Length-1];
				for (int j=0; j<newArray.Length; j++) 
				{
					if (j<i) newArray[j] = array[j];
					else newArray[j] = array[j+1];
				}
			}
		}
		
		if (array.Length>6) EditorGUILayout.EndScrollView();
		
		//adding new obj
		rect = GUILayoutUtility.GetRect (150, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		rect.width -= (buttonSize+space)*3;
		if (GUI.Button(rect, "Add")) 
		{
			newArray = new string[array.Length+1];
			for (int j=0; j<array.Length; j++) newArray[j] = array[j];
			newArray[newArray.Length-1] = "";
		}
		
		//adding a little space after	
		rect = GUILayoutUtility.GetRect (150, 3, "TextField");
		
		EditorGUI.indentLevel = indent;
		
		return newArray;
	}
	
	
	static public InstantGuiElement[] DrawElementsArray ( InstantGuiElement[] array  )
	{
		int space = 2;
		int buttonSize = 20;
		
		InstantGuiElement[] newArray = array; //
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		int scrollSize = Mathf.Min(array.Length*20, 110);
		
		if (array.Length>6)
			scrolls[3] = EditorGUILayout.BeginScrollView(new Vector2(0,scrolls[3]), GUILayout.Height(scrollSize)).y;

		Rect rect = new Rect();
		for (int i=0; i<array.Length; i++)
		{
			rect = GUILayoutUtility.GetRect (150, 16, "TextField");
			rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
			rect.width -= (buttonSize+space)*3;
			array[i] = (InstantGuiElement)EditorGUI.ObjectField (rect, array[i], typeof(InstantGuiElement), true); //
			
			//move up
			rect.x += rect.width+space; rect.width = buttonSize; 
			if (GUI.Button(rect, "∧ ") && i != 0)
			{
				InstantGuiElement tmp= array[i-1];
				array[i-1] = array[i];
				array[i] = tmp;
			}
			
			//move down
			rect.x += buttonSize+space;
			if (GUI.Button(rect, "∨ ") && i != array.Length-1)
			{
				InstantGuiElement tmp = array[i+1];
				array[i+1] = array[i];
				array[i] = tmp;
			}
			
			//delete
			rect.x += buttonSize+space;
			if (GUI.Button (rect, "X"))
			{
				newArray = new InstantGuiElement[array.Length-1]; //
				for (int j=0; j<newArray.Length; j++) 
				{
					if (j<i) newArray[j] = array[j];
					else newArray[j] = array[j+1];
				}
			}
		}
		
		if (array.Length>6) EditorGUILayout.EndScrollView();
		
		//adding a little space after	
		if (array.Length>0) GUILayoutUtility.GetRect (150, 2, "TextField");
		
		//adding new obj
		rect = GUILayoutUtility.GetRect (150, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		rect.width -= (buttonSize+space)*3;
		InstantGuiElement addedVal = (InstantGuiElement)EditorGUI.ObjectField (rect, null, typeof(InstantGuiElement), true); //
		if (addedVal!=null) 
		{
			newArray = new InstantGuiElement[array.Length+1]; //
			for (int j=0; j<array.Length; j++) newArray[j] = array[j];
			newArray[newArray.Length-1] = addedVal;
		}
		
		//adding a little space after	
		rect = GUILayoutUtility.GetRect (150, 3, "TextField");
		
		EditorGUI.indentLevel = indent;
		
		return newArray;
	}
	
	
	static public InstantGuiElement[] DrawElementsTable ( InstantGuiElement[] array ,   InstantGuiElement[] secondary  ) //returns only the first array, but secondary members could be assigned
	{
		int space = 2;
		int buttonSize = 20;
		
		InstantGuiElement[] newArray = array;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		int scrollSize = Mathf.Min(array.Length*20, 110);
		
		if (array.Length>6)
			scrolls[3] = EditorGUILayout.BeginScrollView(new Vector2(0,scrolls[3]), GUILayout.Height(scrollSize)).y;

		Rect rect = new Rect();
		for (int i=0; i<array.Length; i++)
		{
			rect = GUILayoutUtility.GetRect (150, 16, "TextField");
			rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
			rect.width -= (buttonSize+space)*3;
			rect.width = rect.width*0.5f - 2;
			array[i] = (InstantGuiElement)EditorGUI.ObjectField (rect, array[i], typeof(InstantGuiElement), true); 
			rect.x += rect.width + 4;
			secondary[i] = (InstantGuiElement)EditorGUI.ObjectField (rect, secondary[i], typeof(InstantGuiElement), true); 
			
			//move up
			rect.x += rect.width+space; rect.width = buttonSize; 
			if (GUI.Button(rect, "∧ ") && i != 0)
			{
				InstantGuiElement tmp= array[i-1];
				array[i-1] = array[i];
				array[i] = tmp;
				
				tmp = secondary[i-1];
				secondary[i-1] = secondary[i];
				secondary[i] = tmp;
			}
			
			//move down
			rect.x += buttonSize+space;
			if (GUI.Button(rect, "∨ ") && i != array.Length-1)
			{
				InstantGuiElement tmp = array[i+1];
				array[i+1] = array[i];
				array[i] = tmp;
				
				tmp = secondary[i+1];
				secondary[i+1] = secondary[i];
				secondary[i] = tmp;
			}
			
			//delete
			rect.x += buttonSize+space;
			if (GUI.Button (rect, "X"))
			{
				newArray = new InstantGuiElement[array.Length-1]; //
				for (int j=0; j<newArray.Length; j++) 
				{
					if (j<i) newArray[j] = array[j];
					else newArray[j] = array[j+1];
				}
			}
		}
		
		if (array.Length>6) EditorGUILayout.EndScrollView();
		
		//adding a little space after	
		if (array.Length>0) GUILayoutUtility.GetRect (150, 2, "TextField");
		
		//adding new obj
		rect = GUILayoutUtility.GetRect (150, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		rect.width -= (buttonSize+space)*3;
		rect.width = rect.width*0.5f - 2;
		InstantGuiElement addedVal = (InstantGuiElement)EditorGUI.ObjectField (rect, null, typeof(InstantGuiElement), true); //
		if (addedVal!=null) 
		{
			newArray = new InstantGuiElement[array.Length+1]; //
			for (int j=0; j<array.Length; j++) newArray[j] = array[j];
			newArray[newArray.Length-1] = addedVal;
		}
		
		//adding a little space after	
		rect = GUILayoutUtility.GetRect (150, 3, "TextField");
		
		EditorGUI.indentLevel = indent;
		
		return newArray;
	}
	
	
	static public InstantGuiStyle DrawStyle (InstantGuiStyle style, bool drawDefaultPos)
	{
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel=1;

		//EditorGUILayout.HelpBox("Style settings are not available in demo version.", MessageType.Info);

		DrawSubStyle (style.main, "Main");
		DrawSubStyle (style.pointed, "Pointed");
		DrawSubStyle (style.active, "Active");
		DrawSubStyle (style.disabled, "Disabled");
		
		EditorGUILayout.Space();
		
		//texture borders
		EditorGUIUtility.LookLikeControls(15, 15);
		EditorGUI.indentLevel=0;
		Rect rect = GUILayoutUtility.GetRect (50, 44, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		
		rect.x = 150; rect.width = 75; rect.height = 16;
		style.borders.top = EditorGUI.IntField(rect, "T:", style.borders.top);	
		rect.y+=18; rect.x -=40;
		style.borders.left = EditorGUI.IntField(rect, "L:", style.borders.left);
		rect.x += 80;
		style.borders.right = EditorGUI.IntField(rect, "R:", style.borders.right);
		rect.y+=18; rect.x -=40;
		style.borders.bottom = EditorGUI.IntField(rect, "B:", style.borders.bottom);
		rect.x -=120; rect.y-=36; EditorGUI.LabelField(rect,"Texture"); 
		rect.y+=18; EditorGUI.LabelField(rect,"Borders:");
		rect.y+=18; rect.x=32; rect.width = 60; 
		if (GUI.Button(rect,"Half Size"))
		{
			Texture tex = GetUsedtexture(style);
			if (tex!=null)
			{
				style.borders.top = (int)(tex.height*0.5f);
				style.borders.bottom = (int)(tex.height*0.5f);
				style.borders.left = (int)(tex.width*0.5f);
				style.borders.right = (int)(tex.width*0.5f);
			}
		}
		GUILayoutUtility.GetRect (50, 18, "TextField");
		EditorGUIUtility.LookLikeControls();
		
		//fixed width 
		rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=(indent+1)*indentPixels; rect.width-=(indent+1)*indentPixels;
		EditorGUI.indentLevel = 0;
		int rectwidth= (int)rect.width;
		rect.width = 30;
		style.fixedWidth = EditorGUI.Toggle(rect,style.fixedWidth);
		rect.x+=20; rect.width = rectwidth -45-30;
		style.fixedWidthSize = EditorGUI.IntField(rect, "Fixed Width:", style.fixedWidthSize);
		rect.x += rect.width+5; rect.width = 50;
		if (GUI.Button(rect, "Get"))
		{
			Texture tex = GetUsedtexture(style);
			if (tex!=null) style.fixedWidthSize = tex.width;
		}
		EditorGUI.indentLevel = 1; 
		
		//fixed height 
		rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=(indent+1)*indentPixels; rect.width-=(indent+1)*indentPixels;
		EditorGUI.indentLevel = 0;
		rectwidth = (int)rect.width;
		rect.width = 30;
		style.fixedHeight = EditorGUI.Toggle(rect,style.fixedHeight);
		rect.x+=20; rect.width = rectwidth -45-30;
		style.fixedHeightSize = EditorGUI.IntField(rect, "Fixed Height:", style.fixedHeightSize);
		rect.x += rect.width+5; rect.width = 50;
		if (GUI.Button(rect, "Get"))
		{
			Texture tex = GetUsedtexture(style);
			if (tex!=null) style.fixedHeightSize = tex.height;
		}
		EditorGUI.indentLevel = 1; 

		//proportional
		rect = GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x+=(indent)*indentPixels; rect.width-=(indent)*indentPixels;
		rect.width -= 100;
		style.proportional = EditorGUI.ToggleLeft(rect, "Proportional", style.proportional);
		rect.x += 100;
		style.proportionalAspect = EditorGUI.FloatField(rect, style.proportionalAspect);

		//pointOffset
		InstantGuiElementPos tmp = new InstantGuiElementPos(style.pointOffset.left, style.pointOffset.right, style.pointOffset.top, style.pointOffset.bottom);
		tmp = InstantGuiInspector.DrawElementPos("ClickOffset:", tmp);
		style.pointOffset = new RectOffset(tmp.left, tmp.right, tmp.top, tmp.bottom);
		
		//other controls
		style.blendSpeed = EditorGUILayout.FloatField("Blend Speed:", style.blendSpeed);
		
		EditorGUILayout.Space();
		style.font = (Font)EditorGUILayout.ObjectField("Font:", style.font, typeof(Font), false);
		style.fontSize = EditorGUILayout.IntField("Font Size:", style.fontSize);
		style.textAligment = (InstantGuiTextAligment)EditorGUILayout.EnumPopup("Text Aligment:", style.textAligment);
		
		Vector2 textOffset = DrawVector2("Text Offset", new Vector2(style.textOffsetX, style.textOffsetY));
		style.textOffsetX = (int)(textOffset.x);
		style.textOffsetY = (int)(textOffset.y);
		
		EditorGUILayout.Space();
		
		//text offset
		if (drawDefaultPos)
		{
			EditorGUILayout.LabelField("Default Placement:");
			InstantGuiInspector.DrawElementPosLabels("", "Left", "Right", "Top", "Bottom");
			style.relative = InstantGuiInspector.DrawElementPos("Relative:", style.relative);
			style.offset = InstantGuiInspector.DrawElementPos("Offset:", style.offset);
			style.layerOffset = DrawLayerOffset("Layer Offset:", style.layerOffset);
		}
		EditorGUILayout.Space();
		
		EditorGUI.indentLevel = indent;

		return style;
	}
	
	static public void  DrawSubStyle ( SubStyle sub ,   string name  )
	{
		int toggleSize = 16;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		Rect rect = GUILayoutUtility.GetRect (50, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		sub.enabled = EditorGUI.Toggle(rect, sub.enabled);
		rect.x+=toggleSize; rect.width-=toggleSize; //rect.y+=2;
		EditorGUI.LabelField(rect,name);
		
		if (sub.enabled)
		{
			//texture
			rect = GUILayoutUtility.GetRect (50, 16, "TextField");
			rect.x+=(indent+1)*indentPixels; rect.width-=(indent+1)*indentPixels;
			sub.texture = (Texture)EditorGUI.ObjectField(rect, "Texture:", sub.texture, typeof(Texture), false);
			
			EditorGUI.indentLevel = 2;
			Vector2 textOffset = DrawVector2("Text Offset", new Vector2(sub.textOffsetX, sub.textOffsetY));
			sub.textOffsetX = (int)(textOffset.x);
			sub.textOffsetY = (int)(textOffset.y);
			EditorGUI.indentLevel = 0;

			EditorGUIUtility.LookLikeControls(120, 15);
			rect = GUILayoutUtility.GetRect (50, 16, "TextField");
			rect.x+=(indent+1)*indentPixels; rect.width-=(indent+1)*indentPixels;
			sub.textColor = EditorGUI.ColorField(rect, "Text Color:", sub.textColor);
		}
		
		EditorGUI.indentLevel = indent;
	}
	
	static public Vector2 DrawVector2 ( string label ,   Vector2 v2  ){
		int labelWidth = 105;
		
		Vector2 result = v2;
		
		int indent= EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		EditorGUIUtility.LookLikeControls(15, 15);
		Rect rect = GUILayoutUtility.GetRect (50, 16, "TextField");
		rect.x+=indent*indentPixels; rect.width-=indent*indentPixels;
		EditorGUI.LabelField(rect, label);
		rect.width -= labelWidth; rect.x += labelWidth+15;
		rect.width = rect.width*0.5f-15;
		result.x = EditorGUI.IntField(rect, "X:", (int)result.x);
		rect.x+=rect.width+10;
		result.y = EditorGUI.IntField(rect, "Y:", (int)result.y);
		
		EditorGUI.indentLevel = indent;
		
		return result;
	}
	
	static public void  DrawSaveToStyleButton ( string label ,   InstantGuiElement element ,   bool saveStyle ,    bool savePos  ){
		//if (!saveStyle && savePos && element.customStyle) return;
		
		Rect rect= GUILayoutUtility.GetRect (50, 18, "TextField");
		rect.x=30; rect.width-=15;
		if (GUI.Button(rect, label))
		{
			//finding in set
			bool  foundInSet = false;
			for (int j=0; j<element.styleSet.styles.Length; j++)
				if (element.styleSet.styles[j].name == element.style.name)
			{
				if (saveStyle) element.styleSet.styles[j] = element.style.Clone();
				if (savePos)
				{
					element.styleSet.styles[j].relative = element.relative.Clone();
					element.styleSet.styles[j].offset = element.offset.Clone();
					element.styleSet.styles[j].layerOffset = element.layerOffset;
				}
				foundInSet = true;
			}
			
			//adding new
			if (!foundInSet)
			{
				#if UNITY_EDITOR
				if (!saveStyle) 
				{
					if (EditorUtility.DisplayDialog("No Such Style",
					                                "Cannot find style in Style Set. Do you want to create it?",
					                                "Create", "Cancel")) saveStyle = true;
					else { EditorUtility.SetDirty(element.styleSet); return; }
				}
				#endif
				
				InstantGuiStyle[] newStyles = new InstantGuiStyle[element.styleSet.styles.Length+1];
				for (int j=0; j<element.styleSet.styles.Length; j++) newStyles[j] = element.styleSet.styles[j];
				
				if (saveStyle) newStyles[newStyles.Length-1] = element.style.Clone();
				if (savePos)
				{
					newStyles[newStyles.Length-1].relative = element.relative.Clone();
					newStyles[newStyles.Length-1].offset = element.offset.Clone();
					newStyles[newStyles.Length-1].layerOffset = element.layerOffset;
				}
				element.styleSet.styles = newStyles;
			}
			
			#if UNITY_EDITOR
			EditorUtility.SetDirty(element.styleSet);
			#endif
		}
	}
	
	
	
	static public Texture GetUsedtexture ( InstantGuiStyle style  ){
		Texture tex=null;
		
		if (style.main.enabled && style.main.texture!=null) tex = style.main.texture;
		if (tex==null && style.pointed.enabled && style.pointed.texture!=null) tex = style.pointed.texture;
		if (tex==null && style.active.enabled && style.active.texture!=null) tex = style.active.texture;
		if (tex==null && style.disabled.enabled && style.disabled.texture!=null) tex = style.disabled.texture;
		
		return tex;
	}
	
	
}