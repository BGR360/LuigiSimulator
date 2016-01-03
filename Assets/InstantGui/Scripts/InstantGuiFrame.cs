#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class InstantGuiFrame : MonoBehaviour 
{
	/*
	static string[] textureNames = {"GuiFrameLines", "GuiFrameSquare", "GuiFrameSquare", "GuiFrameSquare", "GuiFrameSquare",
	                                "GuiFrameCircle", "GuiFrameCircle", "GuiFrameCircle", "GuiFrameCircle", 
	                                "GuiFrameLine", "GuiFrameLine", "GuiFrameLine", "GuiFrameLine",
	                                "GuiFrameArrowBottom", "GuiFrameArrowTop", "GuiFrameArrowLeft", "GuiFrameArrowRight"};
	*/
	static Texture[] textures;
	//static GUIText[] texts;
	
	static Texture frameTexture;
	static Texture lineTexture;
	static Texture squareTexture;
	static Texture circleTexture;
	static Texture triTopTexture; static Texture triBottomTexture;
	static Texture triLeftTexture; static Texture triRightTexture;
	static Texture lockTexture;
	
	static RectOffset nonGridOffset= new RectOffset(0,0,0,0);
	static RectOffset nonGridRelative= new RectOffset(0,0,0,0);
	
	static int dragging;
	
	static Vector2 dragStart;
	static bool  mouseWasPressed;
	
	static InstantGui pureGui;
	
	static Vector2 temp;
	
	static public bool  drawFrames = true;
	static public int frameSize = 10;
	static public InstantGuiElement lockedElement;

	public enum InstantGuiFrameShowValues {hide=0, onChange=1, always=2};
	static public InstantGuiFrameShowValues showValues = InstantGuiFrameShowValues.onChange;
	
	static bool  selectionChanged = true;
	
	static public void  EditFrame ( InstantGuiElement element   )
	{
		Undo.RecordObject(element, "InstantGui Move");
		
		//getting mouse pos
		Vector2 mousePos= Event.current.mousePosition;
		mousePos.y = InstantGui.Invert(mousePos.y);
		
		//shift-aligning
		if (Event.current.shift) 
		{
			if (Mathf.Abs(mousePos.x-dragStart.x) < Mathf.Abs(mousePos.y-dragStart.y)) mousePos.x = dragStart.x;
			else mousePos.y = dragStart.y;
		}
		
		//getting mouse button
		bool  mouseDown=false; bool  mouseUp=false;
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0) { mouseDown = true; dragStart = mousePos; }
		if (Event.current.type == EventType.MouseUp  && Event.current.button == 0) { mouseUp = true; }
		
		//selecting locked element
		if (lockedElement!=null)
		{
			//element = lockedElement;
			//Selection.activeGameObject = lockedElement.gameObject;
		}
		
		//duplicating element
		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Duplicate")		
		{
			GameObject newElement = (GameObject)GameObject.Instantiate(element.gameObject);
			newElement.name = element.gameObject.name;
			if (element.transform.parent!=null) newElement.transform.parent = element.transform.parent;
			newElement.transform.localPosition = element.transform.localPosition;
			newElement.transform.localRotation = element.transform.localRotation;
			newElement.transform.localScale = element.transform.localScale;
			Selection.activeGameObject = newElement;
			element = newElement.GetComponent<InstantGuiElement>();
		}
		
		//key events
		if (Event.current.type == EventType.keyDown)
		{
			//Undo.RegisterUndo(element, "Move PureGUI");
			
			//locking element
			if (Event.current.keyCode == KeyCode.L)
			{
				if (!lockedElement) lockedElement = element;
				else lockedElement = null;
			}
			
			//hiding frame
			if (Event.current.keyCode == KeyCode.H)
			{
				drawFrames = !drawFrames;
			}
			
			//moving with keys
			if (Event.current.keyCode == KeyCode.UpArrow) { element.offset.top--; element.offset.bottom--; }
			if (Event.current.keyCode == KeyCode.DownArrow) { element.offset.top++; element.offset.bottom++; }
			if (Event.current.keyCode == KeyCode.LeftArrow) { element.offset.left--; element.offset.right--; }
			if (Event.current.keyCode == KeyCode.RightArrow) { element.offset.left++; element.offset.right++; }
		}
		
		Rect rect = new Rect(0,0,frameSize,frameSize);
		Rect textRect = new Rect(0,0,40,20);
		
		//getting if any of dimension is fixed
		bool  fixedWidth = false;
		bool  fixedHeight = false;
		if (element.style!=null)
		{
			if (element.style.fixedWidth) fixedWidth = true;
			if (element.style.fixedHeight) fixedHeight = true;
		}
		if (mouseDown)
		{
			nonGridOffset.left = element.offset.left;
			nonGridOffset.right = element.offset.right;
			nonGridOffset.top = element.offset.top;
			nonGridOffset.bottom = element.offset.bottom;
			
			nonGridRelative.left = element.relative.left;
			nonGridRelative.right = element.relative.right;
			nonGridRelative.top = element.relative.top;
			nonGridRelative.bottom = element.relative.bottom;
		}
		
		if (mouseUp || mouseDown) dragging = 0;
		
		
		//storing borders to assign comfortable
		int left = element.absolute.left;
		int right = element.absolute.right;
		int top = element.absolute.top;
		int bottom = element.absolute.bottom;
		
		//relative left
		rect.width = frameSize * 1.5f;
		rect.height = frameSize;
		
		rect.x = left - element.offset.left - rect.width*0.5f; 
		rect.y = element.absolute.GetCenter().y - rect.height*0.5f;
		
		textRect.x = rect.x; textRect.y = rect.y+16;
		
		if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
		{ dragging = 1; }
		
		if (dragging==1)
		{
			nonGridRelative.left = Mathf.RoundToInt(InstantGuiElementPos.ScreenPointToRelative(element.parentpos, Event.current.mousePosition).x); 
			if (Event.current.control) { element.relative.left = (Mathf.RoundToInt(nonGridRelative.left*0.2f))*5; }
			else { element.relative.left = nonGridRelative.left; }
			element.offset.GetOffset(element.parentpos, element.relative, element.absolute);
		}
		
		//relative right
		if (!fixedWidth)
		{
			rect.x = right - element.offset.right - rect.width*0.5f; 
			rect.y = element.absolute.GetCenter().y - rect.height*0.5f;
			
			textRect.x = rect.x; textRect.y = rect.y-20;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 2; }
			if (dragging==2)
			{
				nonGridRelative.right = Mathf.RoundToInt(InstantGuiElementPos.ScreenPointToRelative(element.parentpos, Event.current.mousePosition).x); 
				if (Event.current.control) { element.relative.right = (Mathf.RoundToInt(nonGridRelative.right*0.2f))*5; }
				else { element.relative.right = nonGridRelative.right; }
				element.offset.GetOffset(element.parentpos, element.relative, element.absolute);
			}
		}
		else element.relative.right = element.relative.left;
		
		//relative top
		rect.width = frameSize;
		rect.height = frameSize * 1.5f;
		
		rect.x = element.absolute.GetCenter().x - rect.width*0.5f;
		rect.y = top - element.offset.top - rect.height*0.5f;
		
		textRect.x = rect.x+20; textRect.y = rect.y-2;
		
		if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
		{ dragging = 3; }
		if (dragging==3)
		{
			nonGridRelative.top = Mathf.RoundToInt(InstantGuiElementPos.ScreenPointToRelative(element.parentpos, Event.current.mousePosition).y); 
			if (Event.current.control) { element.relative.top = (Mathf.RoundToInt(nonGridRelative.top*0.2f))*5; }
			else { element.relative.top = nonGridRelative.top; }
			element.offset.GetOffset(element.parentpos, element.relative, element.absolute);
		}
		
		//relative bottom
		if (!fixedHeight)
		{
			rect.x = element.absolute.GetCenter().x - rect.width*0.5f; 
			rect.y = bottom - element.offset.bottom - rect.height*0.5f;
			
			textRect.x = rect.x-30; textRect.y = rect.y-2;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 4; }
			if (dragging==4)
			{
				nonGridRelative.bottom = Mathf.RoundToInt(InstantGuiElementPos.ScreenPointToRelative(element.parentpos, Event.current.mousePosition).y); 
				if (Event.current.control) { element.relative.bottom = (Mathf.RoundToInt(nonGridRelative.bottom*0.2f))*5; }
				else { element.relative.bottom = nonGridRelative.bottom; }
				element.offset.GetOffset(element.parentpos, element.relative, element.absolute);
			}
		}
		else element.relative.bottom = element.relative.top;
		
		
		rect = new Rect(0,0,frameSize,frameSize);
		
		
		if (!fixedWidth || !fixedHeight)
		{
			//left-top
			rect.x = element.absolute.left-frameSize*0.5f; 
			rect.y = element.absolute.top-frameSize*0.5f;
			textRect.x = rect.x-25; 
			textRect.y = (top+bottom)*0.5f + (top-bottom)*0.25f;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 5; }
			if (dragging==5)
			{
				element.offset.left = (int)(nonGridOffset.left + (mousePos.x-dragStart.x)); 
				element.offset.top = (int)(nonGridOffset.top + (dragStart.y-mousePos.y));
				if (Event.current.control)
				{
					element.offset.left = Mathf.RoundToInt(element.offset.left*0.1f)*10;
					element.offset.top = Mathf.RoundToInt(element.offset.top*0.1f)*10;
				}
			}
			
			//left-bottom
			rect.x = element.absolute.left-frameSize*0.5f; 
			rect.y = element.absolute.bottom-frameSize*0.5f; 
			textRect.x = (left+right)*0.5f + (right-left)*0.25f;
			textRect.y = rect.y+10;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 6; }
			if (dragging==6)
			{
				element.offset.left = (int)(nonGridOffset.left + (mousePos.x-dragStart.x)); 
				element.offset.bottom = (int)(nonGridOffset.bottom + (dragStart.y-mousePos.y));
				if (Event.current.control)
				{
					element.offset.left = Mathf.RoundToInt(element.offset.left*0.1f)*10;
					element.offset.bottom = Mathf.RoundToInt(element.offset.bottom*0.1f)*10;
				}
			}
			
			//right-top
			rect.x = element.absolute.right-frameSize*0.5f; 
			rect.y = element.absolute.top-frameSize*0.5f;
			textRect.x = (left+right)*0.5f + (left-right)*0.25f;
			textRect.y = rect.y-13;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 7; }
			if (dragging==7)
			{
				element.offset.right = (int)(nonGridOffset.right + (mousePos.x-dragStart.x)); 
				element.offset.top = (int)(nonGridOffset.top + (dragStart.y-mousePos.y));
				if (Event.current.control)
				{
					element.offset.right = Mathf.RoundToInt(element.offset.right*0.1f)*10;
					element.offset.top = Mathf.RoundToInt(element.offset.top*0.1f)*10;
				}
			}
			
			//right-bottom
			rect.x = element.absolute.right-frameSize*0.5f; 
			rect.y = element.absolute.bottom-frameSize*0.5f; 
			textRect.x = rect.x+15; 
			textRect.y = (top+bottom)*0.5f + (bottom-top)*0.25f;
			
			if (rect.Contains(Event.current.mousePosition) && mouseDown && dragging==0 && drawFrames && !Event.current.alt && !element.lockPosition) 
			{ dragging = 8; }
			if (dragging==8)
			{
				element.offset.right = (int)(nonGridOffset.right + (mousePos.x-dragStart.x)); 
				element.offset.bottom = (int)(nonGridOffset.bottom + (dragStart.y-mousePos.y));
				if (Event.current.control)
				{
					element.offset.right = Mathf.RoundToInt(element.offset.right*0.1f)*10;
					element.offset.bottom = Mathf.RoundToInt(element.offset.bottom*0.1f)*10;
				}
			}
		}
		
		//aligning
		if (!InstantGui.instance) InstantGui.instance = (InstantGui)FindObjectOfType(typeof(InstantGui));
		if (!EditorApplication.isPlaying) 
		{ 
			//InstantGui.instance.Update();
			//InstantGui.instance.element.Point(mousePos);
			
			InstantGui.width = Screen.width;
			InstantGui.height = Screen.height;
			
			InstantGui.instance.element = InstantGui.instance.GetComponent<InstantGuiElement>();
			InstantGui.pointed = null;
			
			InstantGui.instance.element.GetChildren(); 
			InstantGui.instance.element.Align();
			//element.PreventZeroSize(true);
			InstantGui.instance.element.Point(true);
			InstantGui.instance.element.Action();
			InstantGui.instance.element.ApplyStyle();
		}
		
		//moving selected element (or setting fixed)
		rect = element.absolute.ToRect();
		if (InstantGui.pointed==element && mouseDown && dragging==0 && !Event.current.alt && !element.lockPosition && !selectionChanged) 
		{ dragging = 9; }
		if (dragging==9)
		{
			//Undo.RecordObject(element, "InstantGui Move");
			
			int sizeX = element.absolute.right-element.absolute.left;
			int sizeY = element.absolute.bottom-element.absolute.top;
			
			element.offset.left = (int)(nonGridOffset.left + (mousePos.x-dragStart.x)); 
			element.offset.top = (int)(nonGridOffset.top + (dragStart.y-mousePos.y));
			if (Event.current.control)
			{
				element.offset.left = Mathf.RoundToInt(element.offset.left*0.1f)*10;
				element.offset.top = Mathf.RoundToInt(element.offset.top*0.1f)*10;
			}
			
			element.absolute.GetAbsolute (element.parentpos, element.relative, element.offset);
			element.offset.GetOffset(element.parentpos, element.relative, 
			                       new  InstantGuiElementPos(element.absolute.left, element.absolute.left + sizeX, element.absolute.top, element.absolute.top + sizeY));
		}
		
		//selecting other objs
		if (mouseDown && dragging==0 && InstantGui.pointed!=element && InstantGui.pointed!=null && !lockedElement && !Event.current.alt)
		{
			Selection.activeGameObject = InstantGui.pointed.gameObject;
			element = InstantGui.pointed;
			
			nonGridOffset.left = element.offset.left;
			nonGridOffset.right = element.offset.right;
			nonGridOffset.top = element.offset.top;
			nonGridOffset.bottom = element.offset.bottom;
			
			nonGridRelative.left = element.relative.left;
			nonGridRelative.right = element.relative.right;
			nonGridRelative.top = element.relative.top;
			nonGridRelative.bottom = element.relative.bottom;
			
			selectionChanged = true;
		}
		else if (selectionChanged) selectionChanged = false;

	}
	
	static public void  DrawFrame ( InstantGuiElement element  )
	{	
		if (!drawFrames) return;
		
		//setting textures	
		if (frameTexture==null) frameTexture = GetTexture("GuiFrameSquare.png");
		if (lineTexture==null) lineTexture = GetTexture("GuiFrameLine.png");
		if (squareTexture==null) squareTexture = GetTexture("GuiFrameSquare.png");
		if (circleTexture==null) circleTexture = GetTexture("GuiFrameCircle.png");
		if (triBottomTexture==null) triBottomTexture = GetTexture("GuiFrameArrowBottom.png");
		if (triTopTexture==null) triTopTexture = GetTexture("GuiFrameArrowTop.png");
		if (triLeftTexture==null) triLeftTexture = GetTexture("GuiFrameArrowLeft.png");
		if (triRightTexture==null) triRightTexture = GetTexture("GuiFrameArrowRight.png");
		if (lockTexture==null) lockTexture = GetTexture("GuiFrameLock.png");
		
		//getting if any of dimension is fixed
		bool  fixedWidth = false;
		bool  fixedHeight = false;
		if (element.style!=null)
		{
			if (element.style.fixedWidth) fixedWidth = true;
			if (element.style.fixedHeight) fixedHeight = true;
		}
		
		Rect rect = new Rect(0,0,frameSize,frameSize);
		Rect textRect = new Rect(0,0,40,20);
		
		//storing borders to assign comfortable
		int left = element.absolute.left;
		int right = element.absolute.right;
		int top = element.absolute.top;
		int bottom = element.absolute.bottom;
		
		//drawing lines
		GUI.DrawTexture( new Rect(left, (top+bottom)*0.5f, -element.offset.left, 1), lineTexture);
		if (!fixedWidth) GUI.DrawTexture( new Rect(right, (top+bottom)*0.5f, -element.offset.right, 1), lineTexture);
		GUI.DrawTexture( new Rect((left+right)*0.5f, top, 1, -element.offset.top), lineTexture);
		if (!fixedHeight) GUI.DrawTexture( new Rect((left+right)*0.5f, bottom, 1, -element.offset.bottom), lineTexture);
		
		GUI.DrawTexture( new Rect(left, top, right-left, 1), lineTexture);
		GUI.DrawTexture( new Rect(left, bottom, right-left, 1), lineTexture);
		GUI.DrawTexture( new Rect(left, top, 1, bottom-top), lineTexture);
		GUI.DrawTexture( new Rect(right, top, 1, bottom-top), lineTexture);
		
		//drawing point offset
		if (element.style!=null && 
		    (element.style.pointOffset.left!=0 || element.style.pointOffset.right!=0 || element.style.pointOffset.top!=0 || element.style.pointOffset.bottom!=0))
		{
			RectOffset pointOffset = element.style.pointOffset;
			
			GUI.DrawTexture( new Rect(left+pointOffset.left, top+pointOffset.top, right-left-pointOffset.left-pointOffset.right, 1), lineTexture);
			GUI.DrawTexture( new Rect(left+pointOffset.left, bottom-pointOffset.bottom, right-left-pointOffset.left-pointOffset.right, 1), lineTexture);
			GUI.DrawTexture( new Rect(left+pointOffset.left, top+pointOffset.top, 1, bottom-top-pointOffset.bottom-pointOffset.top), lineTexture);
			GUI.DrawTexture( new Rect(right-pointOffset.right, top+pointOffset.top, 1, bottom-top-pointOffset.bottom-pointOffset.top), lineTexture);
		}
		
		//lock
		if (lockedElement!=null)
		{
			GUI.DrawTexture( new Rect(left+5, top+5, 24, 24), lockTexture);
		}
		
		//relative right
		rect.width = frameSize * 1.5f;
		rect.height = frameSize;
		
		if (!fixedWidth)
		{
			rect.x = right - element.offset.right - rect.width*0.5f; 
			rect.y = element.absolute.GetCenter().y - rect.height*0.5f;
			
			GUI.DrawTexture(rect, triRightTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && dragging==2))
			{
				textRect.x = rect.x-frameSize*0.25f-15; textRect.y = rect.y-frameSize*0.25f-15;
				GUI.Label(textRect, element.relative.right.ToString() + "%");
			}
		}
		else element.relative.right = element.relative.left;
		
		//relative left
		rect.x = left - element.offset.left - rect.width*0.5f; 
		rect.y = element.absolute.GetCenter().y - rect.height*0.5f;
		
		GUI.DrawTexture(rect, triLeftTexture);
		if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && dragging==1))
		{
			//textRect.x = rect.x+frameSize*0.25f+15; textRect.y = rect.y+frameSize*0.1f+17;
			textRect.x = rect.x-frameSize*0.25f-15; textRect.y = rect.y-frameSize*0.25f-15;
			GUI.Label(textRect, element.relative.left.ToString() + "%");
		}
		
		//relative bottom
		rect.width = frameSize;
		rect.height = frameSize * 1.5f;
		
		if (!fixedHeight)
		{
			rect.x = element.absolute.GetCenter().x - rect.width*0.5f; 
			rect.y = bottom - element.offset.bottom - rect.height*0.5f;
			
			GUI.DrawTexture(rect, triBottomTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && dragging==4))
			{
				//textRect.x = rect.x-frameSize-10; textRect.y = rect.y-frameSize*0.05f-20;
				textRect.x = rect.x-frameSize*0.25f-15; textRect.y = rect.y-frameSize*0.25f-15;
				GUI.Label(textRect, element.relative.bottom.ToString() + "%");
			}
		}
		else element.relative.bottom = element.relative.top;
		
		//relative top	
		rect.x = element.absolute.GetCenter().x - rect.width*0.5f;
		rect.y = top - element.offset.top - rect.height*0.5f;
		
		GUI.DrawTexture(rect, triTopTexture);
		if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && dragging==3))
		{
			//textRect.x = rect.x+frameSize*1.1f; textRect.y = rect.y+frameSize;
			textRect.x = rect.x-frameSize*0.25f-15; textRect.y = rect.y-frameSize*0.25f-15;
			GUI.Label(textRect, element.relative.top.ToString() + "%");
		}
		
		
		rect = new Rect(0,0,frameSize,frameSize);
		
		
		if (!fixedWidth || !fixedHeight)
		{
			//left-top 5
			rect.x = element.absolute.left-frameSize*0.5f; 
			rect.y = element.absolute.top-frameSize*0.5f;
			textRect.x = rect.x-25; 
			textRect.y = (top+bottom)*0.5f + (top-bottom)*0.25f;
			
			GUI.DrawTexture(rect, circleTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && (dragging==6 || dragging==5 || dragging==9)))
				GUI.Label(textRect, element.offset.left.ToString() + "p");	
			
			//left-bottom 6
			rect.x = element.absolute.left-frameSize*0.5f; 
			rect.y = element.absolute.bottom-frameSize*0.5f; 
			textRect.x = (left+right)*0.5f + (right-left)*0.25f;
			textRect.y = rect.y+10;
			
			GUI.DrawTexture(rect, circleTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && (dragging==8 || dragging==6 || dragging==9)))
				GUI.Label(textRect, element.offset.bottom.ToString() + "p");
			
			//right-top 7
			rect.x = element.absolute.right-frameSize*0.5f; 
			rect.y = element.absolute.top-frameSize*0.5f;
			textRect.x = (left+right)*0.5f + (left-right)*0.25f;
			textRect.y = rect.y-13;
			
			GUI.DrawTexture(rect, circleTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && (dragging==5 || dragging==7 || dragging==9)))
				GUI.Label(textRect, element.offset.top.ToString() + "p");
			
			//right-bottom 8
			rect.x = element.absolute.right-frameSize*0.5f; 
			rect.y = element.absolute.bottom-frameSize*0.5f; 
			textRect.x = rect.x+15; 
			textRect.y = (top+bottom)*0.5f + (bottom-top)*0.25f;
			
			GUI.DrawTexture(rect, circleTexture);
			if (showValues==InstantGuiFrameShowValues.always || (showValues==InstantGuiFrameShowValues.onChange && (dragging==7 || dragging==8 || dragging==9)))
				GUI.Label(textRect, element.offset.right.ToString() + "p");
		
		}
	}


	//File operations
	static Texture GetTexture ( string name  ) 
	{
		if (InstantGui.instance==null) InstantGui.ForceUpdate();

		MonoScript script = MonoScript.FromMonoBehaviour(InstantGui.instance);
		string path = AssetDatabase.GetAssetPath(script);

		path = path.Replace("Scripts/InstantGui.cs", "Textures/" + name);

		Texture tex= (Texture)AssetDatabase.LoadAssetAtPath (path, typeof(Texture)); 
		if (tex==null) return null;
		return tex;
	}
	/*
	static string LocalToAbsolute ( string path  ){ return Application.dataPath.Remove(Application.dataPath.Length-6) + path; }
	
	static string AbsoluteToLocal ( string path  ) //only string operations, no files or dirs
	{ 
		return path.Replace(new System.IO.DirectoryInfo(Application.dataPath).FullName,"Assets"); 
	}
	
	static string SearchFileDown ( string startPath ,   string fileName  ) //in local path
	{
		System.IO.FileInfo result = SearchFileDown(
			new System.IO.DirectoryInfo(LocalToAbsolute(startPath)),
			fileName);
		if (result!=null) return AbsoluteToLocal(result.FullName);
		return null;
	}

	static public System.IO.FileInfo SearchFileDown ( System.IO.DirectoryInfo startDirectory ,   string fileName  )  //in absolute path
	{
		//System.IO.FileAttributes attributes = startDirectory.Attributes;
		//if (attributes == System.IO.FileAttributes.Hidden) return;
		System.IO.FileInfo[] files = startDirectory.GetFiles(fileName);
		if (files.Length > 0) return files[0];
		
		System.IO.DirectoryInfo[] subdirs = startDirectory.GetDirectories();
		System.IO.FileInfo foundFile;
		for (int i=0; i<subdirs.Length; i++)
		{
			foundFile = SearchFileDown (subdirs[i], fileName); 
			if (foundFile!=null) return foundFile;
		}
		return null;
	}
	*/
}

#else
using UnityEngine;
using System.Collections;

public class InstantGuiFrame : MonoBehaviour 
{
	
	//static string[] textureNames;
	static Texture[] textures;
	//static GUIText[] texts;
	
	static Texture frameTexture;
	static Texture lineTexture;
	static Texture squareTexture;
	static Texture circleTexture;
	static Texture triTopTexture; static Texture triBottomTexture;
	static Texture triLeftTexture; static Texture triRightTexture;
	static Texture lockTexture;
	
	static RectOffset nonGridOffset= new RectOffset(0,0,0,0);
	static RectOffset nonGridRelative= new RectOffset(0,0,0,0);
	
	static int dragging;
	
	static Vector2 dragStart;
	static bool  mouseWasPressed;
	
	static InstantGui pureGui;
	
	static Vector2 temp;
	
	static public bool  drawFrames = true;
	static public int frameSize = 10;
	static public InstantGuiElement lockedElement;
	
	public enum InstantGuiFrameShowValues {hide=0, onChange=1, always=2};
	static public InstantGuiFrameShowValues showValues = InstantGuiFrameShowValues.onChange;
	
	static bool  selectionChanged = true;
	
	static public void  EditFrame ( InstantGuiElement element   )
	{

	}
	
	static public void  DrawFrame ( InstantGuiElement element  )
	{	

	}

	//File operations
	static Texture GetTexture ( string name  ) {return null;}
	//static string LocalToAbsolute ( string path  ){return "";}
	//static string AbsoluteToLocal ( string path  ) {return "";}
	//static string SearchFileDown ( string startPath ,   string fileName  ) {return "";}
	//static public System.IO.FileInfo SearchFileDown ( System.IO.DirectoryInfo startDirectory ,   string fileName  )  {return null;}
}
#endif

