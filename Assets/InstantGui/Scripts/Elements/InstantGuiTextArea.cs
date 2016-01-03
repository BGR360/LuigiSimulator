
using UnityEngine;
using System.Collections;

public class InstantGuiTextArea : InstantGuiElement
{
	public string rawText = ""; //initial unformatted text
	
	private string oldText = ""; //to determine change
	private InstantGuiElementPos oldAbsolute = new InstantGuiElementPos(0,0,0,0);
	
	public int firstLine;
	private int oldFirstLine;
	
	private int numLinesTotal;
	private int numLinesDisplayed;
	
	public int widthAdjust;
	public int heightAdjust;
	
	private int oldWidth;
	private int oldHeight;
	
	public InstantGuiSlider slider;
	
	public Vector2 guiScrollPos;
	
	public override void Action () //should be done before slider action
	{
		base.Action();
		
		//getting value from slider
		if (slider!=null)
		{
			slider.min = 0;
			slider.max = numLinesTotal - numLinesDisplayed;
			slider.shownValue = numLinesDisplayed;
			
			//scrolling slider
			slider.value -= Input.GetAxisRaw("Mouse ScrollWheel")*10;
			slider.value = Mathf.Clamp(slider.value, slider.min, slider.max);
			
			firstLine = Mathf.Max(0, (int)slider.value); //could be -1 occasionly
		}
		
		//if any change - re-calc text and numLines
		int newWidth = absolute.right-absolute.left-widthAdjust;
		int newHeight = absolute.bottom-absolute.top-heightAdjust;
		if (rawText != oldText || rawText != oldText || firstLine != oldFirstLine || oldWidth != newWidth || oldHeight != newHeight)
		{
			if (rawText.Length != 0)
			{
				if (guiTexts.Length == 0 || !guiTexts[0])  //Fit functions work with guitext, so we have to check it
				{
					text = "Loading...";
					ApplyStyle();
				}
				
				if (guiTexts.Length != 0 && guiTexts[0]!=null) //checking if guitext was created after all
				{
					Profiler.BeginSample("Fitting text");
					
					text = FitWidth(rawText, newWidth);
					numLinesTotal = GetNumLines(text);
					
					text = FitHeight(text, newHeight);
					numLinesDisplayed = GetNumLines(text);
					
					Profiler.EndSample();
				}
			}
			
			oldText = rawText;
			oldFirstLine = firstLine;
			oldWidth = newWidth;
			oldHeight = newHeight;
			oldAbsolute.Set(absolute);
		}
		
	}
	
	int GetNumLines ( string inputText  )
	{
		int result=0;
		foreach(char c in inputText)
			if (c == "\n"[0]) result++;
		return result;
	}
	
	string FitWidth ( string inputText ,   int width  )
	{
		string[] split = inputText.Split(" "[0]);
		
		string newText = "";
		string prewText = "";
		
		foreach(string word in split)
		{
			newText += word + " ";
			guiTexts[0].text = newText;
			Rect textRect = guiTexts[0].GetScreenRect();
			
			if (textRect.width > width) newText = prewText + "\n" + word + " ";
			else prewText = newText;
		}
		
		return newText;
	}
	
	string FitHeight ( string inputText ,   int height  )
	{
		string[] split = inputText.Split("\n"[0]);
		
		string newText = "";
		
		for (int i=firstLine; i<split.Length; i++)
		{
			string word = split[i];
			
			newText += word + "\n";
			guiTexts[0].text = newText;
			Rect textRect = guiTexts[0].GetScreenRect();
			
			if (textRect.height > height) 
			{ 
				//newText = prewText; 
				break; 
			}
		}
		
		return newText;
	}
	
}
