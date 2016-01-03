
using UnityEngine;
using System.Collections;

public class InstantGuiInputText : InstantGuiElement
{
	private string initialText = ""; //unchanged text during typing
	private string oldText = ""; //to determine change
	
	public float cursorBlinkTime = 0.5f;
	private float cursorBlinkTimeLeft = 0;

	public bool  alwaysTyping; //always input data, for ex - console
	public bool confirmOnEnterOnly;
	
	public int cursorPos;
	private int oldCursorPos;
	
	public GUITexture cursor;
	public int cursorWidth = 2;
	public Color cursorColor = Color.gray;
	public Texture2D cursorTexture;

	private InstantGuiElementPos oldAbsolute = new InstantGuiElementPos(0,0,0,0);
	
	public override void  Action ()
	{
		//if (initialText != text) text = initialText; //assigning text when it changed from somewhere else
		
		base.Action();
		
		if (activated) { check = true; initialText = text; cursorBlinkTimeLeft = cursorBlinkTime; }
		
		//if clicked esc - returning text
		if (Input.GetKey(KeyCode.Escape)) { check = false; text = initialText; }

		//if clicked somewhere else
		if (Input.GetMouseButtonDown(0) && !activated)
		{
			if (confirmOnEnterOnly) { check = false; text = initialText; }
			else { check = false; initialText = text; }
		}
		
		//if (alwaysTyping) check = true;
		
		if (check)
		{
			//input text
			foreach(char c in Input.inputString) 
			{
				//backspace
				if (c == "\b"[0])
				{
					if (text.Length != 0 && cursorPos != 0) //no &&: if only "\b"[0] than add nothing to text
					{
						cursorPos--;
						text = text.Remove(cursorPos, 1);
					}
				}
				
				//end of entry
				else if (c == "\n"[0] || c == "\r"[0]) { check = false; initialText = text; break; }
				
				//normal text input
				else 
				{
					text = text.Insert(cursorPos, c.ToString()); // += c;
					cursorPos++;
				}
			}

			if (Input.GetKeyDown(KeyCode.Delete) && text.Length != 0 && cursorPos != text.Length) text = text.Remove(cursorPos, 1); 
			if (Input.GetKeyDown(KeyCode.LeftArrow)) cursorPos = Mathf.Max(cursorPos-1, 0); 
			if (Input.GetKeyDown(KeyCode.RightArrow)) cursorPos = Mathf.Min(cursorPos+1, text.Length);
			#if UNITY_EDITOR
			if ((Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.G)) 
			    && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) 
				text += UnityEditor.EditorGUIUtility.systemCopyBuffer;
			#endif

			//set cursor appearance
			if (cursor!=null)
			{
				if (cursorBlinkTimeLeft > 0) { if (!cursor.enabled) cursor.enabled = true; }
				else { if (cursor.enabled) cursor.enabled = false; }
				if (cursorBlinkTimeLeft < -cursorBlinkTime) cursorBlinkTimeLeft = cursorBlinkTime;
				cursorBlinkTimeLeft -= Time.deltaTime;
			}
			
			//setting cursor by mouse
			if (check && pressed) FindCursorPos((int)Input.mousePosition.x);
			
			//setting a cursor texture on any change
			if (activated || text != oldText || cursorPos != oldCursorPos || !InstantGuiElementPos.Equals(absolute, oldAbsolute)) DrawCursor();
			oldAbsolute.Set(absolute);
			
			oldText = text;
			oldCursorPos = cursorPos;
		}
		
		if (!check && cursor!=null && cursor.enabled) cursor.enabled = false;
	}
	
	public void  DrawCursor ()
	{
		string workText = text.Replace(" "[0], "."[0]);
		
		if (!cursor)
		{
			GameObject obj = new GameObject ("Cursor"); //the object component will be assigned to
			cursor = obj.AddComponent<GUITexture>();
			cursor.transform.parent = transform;
			obj.transform.localScale = new Vector3(0,0,1);
			if (cursorTexture==null) { cursorTexture = new Texture2D(1,1); cursorTexture.SetPixel(0,0,Color.white); cursorTexture.Apply(); }
			cursor.texture = cursorTexture;
		}

		//setting cursor layer
		cursor.transform.localPosition = new Vector3(0f,0f,cursor.transform.parent.localPosition.z+0.1f);

		//creating unexistant text
		if (guiTexts.Length == 0 || guiTexts[0]==null)
		{
			string tmpText = text;
			text = "|";
			ApplyStyle();
			text = tmpText;
		}

		//defining start rect
		guiTexts[0].text = workText;
		if (guiTexts[0].text.Length == 0) guiTexts[0].text = "|";
		Rect cursorRect = guiTexts[0].GetScreenRect();
		
		//shifting cursor
		guiTexts[0].text = workText.Remove(cursorPos, workText.Length-cursorPos);
		cursorRect.x += guiTexts[0].GetScreenRect().width;
		
		//setting other cursor params
		cursorRect.width = cursorWidth;
		cursorRect.x -= cursorWidth*0.5f+1;
		cursor.pixelInset = cursorRect;
	}
	
	public void  FindCursorPos ( int clickPosX  ){
		if (guiTexts.Length == 0 || !guiTexts[0]) { cursorPos = 0; return; }
		
		string workText = text.Replace(" "[0], "."[0]);
		
		guiTexts[0].text = workText;
		int clickWidth = (int)(clickPosX-guiTexts[0].GetScreenRect().x);
		if (clickWidth <=0) { cursorPos = 0; return; }
		guiTexts[0].text = "";
		float initialTextWidth=0f;
		
		for (int i=0; i<workText.Length; i++) 
		{
			guiTexts[0].text += workText[i];
			float textWidth = guiTexts[0].GetScreenRect().width;
			if ((textWidth+initialTextWidth)*0.5f > clickWidth) { cursorPos = i; return; }
			initialTextWidth = textWidth;
		} 
		
		//selecting after-text
		cursorPos = workText.Length;
	}
	
}
