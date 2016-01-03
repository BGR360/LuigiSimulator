
using UnityEngine;
using System.Collections;

public class InstantGuiElement : MonoBehaviour 
{

	class TextParams
	{
		bool  enabled;
		//Color color = new Color(1,1,1,1);
		float size;
		Vector2 offset;
	}
	
	//element position
	public InstantGuiElementPos parentpos = new InstantGuiElementPos(0,Screen.width,0,Screen.height);
	public InstantGuiElementPos relative  = new InstantGuiElementPos(50,50,50,50);
	public InstantGuiElementPos offset = new InstantGuiElementPos(-20,20,-20,20);
	public InstantGuiElementPos absolute = new InstantGuiElementPos(-20,20,-20,20);
	public float layerOffset = 1;
	public bool  lockPosition = false;
	
	private InstantGuiElementPos oldParentpos = new InstantGuiElementPos(0,0,0,0);
	private InstantGuiElementPos oldRelative = new InstantGuiElementPos(0,0,0,0);
	private InstantGuiElementPos oldOffset = new InstantGuiElementPos(0,0,0,0);
	
	//active element params
	public bool  dynamic = true; //only dynamic elements could be on hover (ie selected in game AND in editor)
	public bool  editable = true; //could be selected in editor
	public bool  pointed;
	public bool  pointedOnBackground;
	public float pointedTime; //how much time this element is under cursor. Increment on pointed, resets when element becomes not pointed
	public float unpointedTime;  //how much time this element is not under cursor
	public bool  disabled;
	public bool  activated = false; //user pressed button and released the mouse on it. Activates for 1 frame only. Could be activated in editor
	public bool  pressed = false; //user is holding mouse pressed above button
	public bool  check = false;
	public bool  instant = true; //activated right after pressing. Otherwise activated on mouse up
	public bool  password = false; //text is displayed in dots
	private bool  holdingPressed = false; //for pressing only
	protected float pointedBlend = 0;

	//text params
	public string text = "";
	
	public GUITexture mainGuiTexture; //accessable with style
	public GUITexture blendGuiTexture;
	public GUIText[] guiTexts;
	
	public Material textMaterial;
	public Material textEffectMaterial;
	
	public InstantGuiStyleSet styleSet;
	public bool  customStyle;
	public string styleName = ""; //to find style in styleset after scene loading
	public InstantGuiStyle style;
	private SubStyle currentSubStyle;
	
	public Font currentFont;
	
	public bool  forceAlign = false;
	
	public System.Collections.Generic.List<InstantGuiElement> childElements = new System.Collections.Generic.List<InstantGuiElement>();
	public System.Collections.Generic.List<Transform> childTfms = new System.Collections.Generic.List<Transform>();
	public System.Collections.Generic.List<bool> childTfmsActive = new System.Collections.Generic.List<bool>();

	public bool  deleteChildren;
	
	public bool  guiLinkText = true;
	public bool  guiEditor;
	public bool  guiStyle = true;
	public bool  guiPosition;
	public bool  guiAttributes;
	public bool  guiElementProps;
	public enum InstantGuiElementType { none, button, text, toggle, slider, list, enumPopup, gauge, window };
	public InstantGuiElementType guiType;
	
	public bool  useStylePlacement;

	
	static public InstantGuiElement Create ( string name ,   System.Type type ,   InstantGuiElement parent  ){ return Create(name, name, type, parent); }
	static public InstantGuiElement Create ( string name ,   string style ,   System.Type type ,   InstantGuiElement parent  ){
		//finding gui parent
		if (InstantGui.instance==null) InstantGui.instance = (InstantGui)FindObjectOfType(typeof(InstantGui));
		if (InstantGui.instance==null) 
		{
			InstantGui.CreateRoot();
			parent = InstantGui.instance.element;
		}
		
		GameObject obj = new GameObject (name); //the object component will be assigned to
		
		//adding component
		InstantGuiElement element = (InstantGuiElement)obj.AddComponent(type);
		
		//parenting and setting styleset
		if (parent!=null)
		{
			element.transform.parent = parent.transform;
			element.styleSet = parent.styleSet;
			//do not assign parent to element! It will get it automaticly
		}
		
		//resetting transform
		if (obj.transform.parent!=null) obj.transform.localPosition = new Vector3(0,0,obj.transform.parent.localPosition.z + 1);
		else obj.transform.localPosition = new Vector3(0,0,0);
		obj.transform.localScale = new Vector3(0,0,1);
		
		//setting style
		element.styleName = style;
		if (element.styleSet!=null)
			for (int i=0; i<element.styleSet.styles.Length; i++) 
				if (element.styleSet.styles[i].name == element.styleName) 
					element.style = element.styleSet.styles[i];
		
		//setting default size
		if (element.style!=null)
		{
			element.relative.Set(element.style.relative); 
			element.offset.Set(element.style.offset);
			element.layerOffset = element.style.layerOffset;
		}
		
		//pureGui.Update();
		return element;
	}

	public void  CheckChildren ()
	{	
		//init root gui
		if (!InstantGui.instance) InstantGui.instance = (InstantGui)GameObject.FindObjectOfType(typeof(InstantGui));
		if (!InstantGui.instance.element) InstantGui.instance.element = InstantGui.instance.GetComponent<InstantGuiElement>();
		
		//checking children
		if (childTfms.Count != transform.childCount || childTfmsActive.Count != transform.childCount) { InstantGui.instance.element.GetChildren(); return; }

		for (int i=0; i<transform.childCount; i++)
		{
			Transform childTfm = transform.GetChild(i);
			if (childTfm.gameObject.activeInHierarchy != childTfmsActive[i]) { InstantGui.instance.element.GetChildren(); return; }
			if (childTfm != childTfms[i]) { InstantGui.instance.element.GetChildren(); return; }
		}

		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].CheckChildren();
	}
	
	public void  GetChildren ()
	{
		childElements.Clear();
		childTfms.Clear();
		childTfmsActive.Clear();
		
		InstantGuiElement childElement;
		for (int i=0; i<transform.childCount; i++)
		{
			Transform childTfm = transform.GetChild(i);
			childTfmsActive.Add(childTfm.gameObject.activeInHierarchy);
			childTfms.Add(childTfm);

			if (!childTfm.gameObject.activeInHierarchy) continue;
			childElement = childTfm.GetComponent<InstantGuiElement>();
			if (childElement!=null) childElements.Add(childElement);
		}

		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].GetChildren();
	}
	
	
	public virtual void  Align ()
	{
		if (!this) return; //element could be deleted to this time!
		
		//assigning vars if they do not exist
		if (guiTexts == null) guiTexts = new GUIText[0];
		
		//using style position
		if (styleSet!=null && useStylePlacement)
		{
			//if (style==null) style = styleSet.FindStyle(styleName, styleNum);
			if (style!=null)
			{
				relative.Set(style.relative);
				offset.Set(style.offset);
			}
		}
		else
		{
			if (relative.isStyle) relative = new InstantGuiElementPos(relative);
			if (offset.isStyle) offset = new InstantGuiElementPos(offset);
		}
		
		//setting layer
		Transform parentTfm= transform.parent;
		if (parentTfm != null) { Vector3 localPos = parentTfm.localPosition; localPos.z += layerOffset; transform.localPosition = localPos;}
		
		//refreshing parent absolute pos
		InstantGuiElement parentElement = null;
		if (parentTfm!=null) parentElement = parentTfm.GetComponent<InstantGuiElement>();
		if (parentElement!=null) parentpos = parentElement.absolute;
		else //parentpos = InstantGuiElementPos(0,Screen.width,0,Screen.height);
		{
			if (InstantGui.instance == null) InstantGui.instance = (InstantGui)FindObjectOfType(typeof(InstantGui));
			parentpos = new InstantGuiElementPos(0,InstantGui.width,0,InstantGui.height);
		}
		
		//checking if there is a need to re-allign
		if (!InstantGuiElementPos.Equals(parentpos, oldParentpos) ||
		    !InstantGuiElementPos.Equals(relative, oldRelative) ||
		    !InstantGuiElementPos.Equals(offset, oldOffset) ||
		    InstantGui.oldScreenWidth != Screen.width ||
		    InstantGui.oldScreenHeight != Screen.height ||
		    true)
		{
			
			//transforming relative pos to absolute
			//if (!absolute || !parentpos || !relative || !offset) return;
			absolute.GetAbsolute(parentpos, relative, offset);
			
			//setting fixed size (if on)
			if (style!=null)
			{
				if (style.fixedWidth) absolute.right = absolute.left+style.fixedWidthSize;
				if (style.fixedHeight) absolute.bottom = absolute.top+style.fixedHeightSize;
			}
			
			//preventing negative size
			int minWidth = 10; int minHeight = 10;
			if (style!=null) 
			{
				minWidth = style.borders.left + style.borders.right; 
				minHeight = style.borders.bottom + style.borders.top; 
			}
			if (absolute.right < absolute.left+minWidth) absolute.right = absolute.left+minWidth;
			if (absolute.bottom < absolute.top+minHeight) absolute.bottom = absolute.top+minHeight;
			
			
			//writing compare-data
			oldParentpos = new InstantGuiElementPos(parentpos);
			oldRelative = new InstantGuiElementPos(relative);
			oldOffset = new InstantGuiElementPos(offset);
		}
		
		
		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].Align();
	}
	
	
	public void PreventZeroSize ( bool noReturn  )  //called from frame only
	{
		int minWidth = 10; int minHeight = 10;
		if (style!=null) 
		{
			minWidth = style.borders.left + style.borders.right; 
			minHeight = style.borders.bottom + style.borders.top; 
		}
		
		if (noReturn)
		{
			if (absolute.right - absolute.left < minWidth && absolute.bottom - absolute.top < minHeight)
				offset.GetOffset(parentpos, relative, new InstantGuiElementPos(absolute.left, absolute.left + minWidth, absolute.top, absolute.top + minHeight));
			else if (absolute.right - absolute.left < minWidth)
				offset.GetOffset(parentpos, relative, new InstantGuiElementPos(absolute.left, absolute.left + minWidth, absolute.top, absolute.bottom));
			else if (absolute.bottom - absolute.top < minHeight)
				offset.GetOffset(parentpos, relative, new InstantGuiElementPos(absolute.left, absolute.right, absolute.top, absolute.top + minHeight));
		}
		else
		{	
			if (absolute.right - absolute.left < minWidth) absolute.right = absolute.left + minWidth;
			if (absolute.bottom - absolute.top < minHeight) absolute.bottom = absolute.top + minHeight;
		}
		
		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].PreventZeroSize(false);
	}
	
	
	
	//void  Point (){ Point (Input.mousePosition); } 
	public void  Point (){ Point (false); } 
	public void Point ( bool isEditor  )  //special point variant with custom mouseposition for editor.
	{
		//getting mouse pos
		Vector2 mousePos;
		if (isEditor)
		{
			mousePos = Event.current.mousePosition;
			mousePos.y = InstantGui.Invert(mousePos.y);
		}
		else mousePos = Input.mousePosition;
		
		pointed = false; //resetting isPointed to all elements
		pointedOnBackground = false;
		
		//getting point offset
		int leftPointOffset=0; int rightPointOffset=0; int topPointOffset=0; int bottomPointOffset=0;
		if (style!=null)
		{
			leftPointOffset = style.pointOffset.left; rightPointOffset = style.pointOffset.right;
			topPointOffset = style.pointOffset.top; bottomPointOffset = style.pointOffset.bottom;
		}
		
		if (dynamic
		    && (editable || !isEditor)
		    && mousePos.x >= absolute.left - leftPointOffset
		    && mousePos.x <= absolute.right - rightPointOffset
		    && InstantGui.Invert(mousePos.y) >= absolute.top - topPointOffset
		    && InstantGui.Invert(mousePos.y) <= absolute.bottom - bottomPointOffset)
		{
			pointedOnBackground = true;
			
			if (!InstantGui.pointed || transform.localPosition.z > InstantGui.pointed.transform.localPosition.z)
			{
				if (InstantGui.pointed!=null) InstantGui.pointed.pointed = false; //resetting isHover to old hover element previously assigned this frame
				
				pointed = true; //assignin new hover element
				if (InstantGui.pointed != this) InstantGui.pointed = this;
			}
			
		}
		
		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].Point(isEditor);
	}
	
	
	public virtual void  Action ()
	{
		//resetting activation state - element could be activated for one frame only
		activated = false;
		
		//mouse pressed action
		if (!disabled)
		{
			//pressing mouse
			if (pointed && Input.GetMouseButtonDown(0)) 
			{ 
				pressed = true; 
				holdingPressed = true; 
				if (instant) activated = true;
			}
			
			//holding mouse
			//if (Input.GetMouseButton(0) && holdingPressed) pressed = pointed;
			
			//releasing mouse
			if (Input.GetMouseButtonUp(0) && holdingPressed) 
			{
				pressed = false;
				holdingPressed = false;
				if (pointed && !instant) activated = true;
			}
			
			//alt-pressing in editor
			#if UNITY_EDITOR
			if (pointed
			    && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode 
			    && Event.current != null
			    && Event.current.isMouse 
			    && Event.current.button == 0 
			    && Event.current.alt
			    && Event.current.type == EventType.MouseDown)
				activated = true;
			#endif
		}
		
		//calculating pointed and unpointed times
		if (pointed) { pointedTime += Time.deltaTime; unpointedTime = 0; }
		else { unpointedTime += Time.deltaTime; pointedTime = 0; }
		
		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].Action();
	}
	
	public virtual void ApplyStyle () //re-written
	{	
		//trying to load style if styleSet assigned (usually after scene loading)
		if (!customStyle && styleSet!=null)
		{
			style = styleSet.FindStyle(styleName);
		}
		
		//clearing if no style assigned
		if (style==null) InstantGuiStyle.Clear(this);
		
		else
		{
			//calculating style
			SubStyle subStyle= style.main;
			if (style.disabled.enabled && disabled) subStyle = style.disabled;
			if (style.active.enabled && (check || pressed)) subStyle = style.active;
			
			//calculating pointed blend
			if (style.pointed.enabled && !disabled && !pressed && !check)
			{
				if (pointed) pointedBlend = Mathf.Min(pointedBlend + Time.deltaTime*style.blendSpeed, 1);
				else pointedBlend = Mathf.Max(pointedBlend - Time.deltaTime*style.blendSpeed, 0);
				
				//setting pointed time and blend to zero if in editor
				#if UNITY_EDITOR
				if (!UnityEditor.EditorApplication.isPlaying)
				{
					pointedTime = 0;
					unpointedTime = 0;
					pointedBlend = 0;
				}
				#endif
			}
			
			//apply style
			if (pointedBlend < 0.001f || disabled || pressed || check) { style.Unblend(this); style.Apply(subStyle, this); }
			else if (pointedBlend > 0.999f) { style.Unblend(this); style.Apply(style.pointed, this); }
			else //blended
			{
				style.Apply(subStyle, this);
				style.Blend(style.pointed, this, pointedBlend);
			}
		}
		
		
		
		//recursive
		for (int i=0; i<childElements.Count; i++) childElements[i].ApplyStyle();
	}
	
	public IEnumerator  YieldAndDestroy ()
	{
		#if UNITY_EDITOR
		UnityEditor.Selection.activeGameObject = null;
		#endif
		yield return null; 
		DestroyImmediate(gameObject);
	}
}