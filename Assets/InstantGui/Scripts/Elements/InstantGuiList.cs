
using UnityEngine;
using System.Collections;
	
public class InstantGuiList : InstantGuiElement
{
	public string[] labels = new string[0]; //if labels is not empty than auto-creating elements array
	
	
	public InstantGuiElement[] elements = new InstantGuiElement[0];
	
	//InstantGuiStyle elementStyle;
	public int lineHeight = 30; //set this to 0 if you want to show all elements
	//int elementWidth = 10;
	
	public int firstShown = 0;
	
	public bool  guiShowLabels = false;
	public bool  guiElementStyle = false;
	
	public int selected = 0;
	
	public InstantGuiSlider slider;
	
	public int sliderMargin;
	
	
	public bool  elementCustomStyle;
	public string elementStyleName = "List_Element"; int elementStyleNum; //to find style in styleset after scene loading
	public InstantGuiStyle elementStyle;
	
	public override void  Align ()
	{
		base.Align();
		
		//setting first shown value
		if (slider!=null) 
		{
			slider.min = 0;
			slider.max = Mathf.Max(0, labels.Length - elements.Length);
			slider.shownValue = elements.Length;
			
			//scrolling slider
			slider.value -= Input.GetAxisRaw("Mouse ScrollWheel")*10;
			slider.value = Mathf.Clamp(slider.value, slider.min, slider.max);
			
			firstShown = (int)slider.value;
		}
		
		//calculating number of lines
		int linesNum;
		if (lineHeight <= 0) linesNum = elements.Length;
		else
		{
			linesNum = Mathf.FloorToInt( (absolute.bottom - absolute.top)*1.0f/lineHeight );
			linesNum = Mathf.Max(linesNum, 0);
		}
		
		//re-creating elements if line num changed
		if (elements.Length != linesNum)
		{
			//destroying overcount
			for (int i=elements.Length-1;i>=linesNum;i--) 
				if (elements[i]!=null) StartCoroutine(elements[i].YieldAndDestroy()); //DestroyImmediate(elements[i].gameObject);
			
			//resizing array	
			InstantGuiElement[] newElements = new InstantGuiElement[linesNum];
			int count = Mathf.Min(linesNum, elements.Length);
			for (int i=0;i<count;i++) newElements[i] = elements[i];
			elements = newElements;
			
			//creating undefened elements
			for (int i=0;i<elements.Length;i++) 
				if (!elements[i]) 
					elements[i] = InstantGuiElement.Create("ListElement", typeof(InstantGuiElement), this);
		}
		
		//setting text and position
		for (int i=0;i<elements.Length;i++) 
		{
			if (i+firstShown < labels.Length && firstShown >= 0) elements[i].text = labels[i+firstShown];
			else elements[i].text = "";
			
			elements[i].offset = new InstantGuiElementPos(0,-sliderMargin,0,0);
			elements[i].relative = new InstantGuiElementPos(0,100,(int)((100.0f/linesNum)*i),(int)((100.0f/linesNum)*(i+1)));
			elements[i].editable = false;
			//elements[i].style = elementStyle;
		}
	}
	
	public override void  Action (){
		base.Action();
		
		for (int i=0;i<elements.Length;i++) 
		{
			if (elements[i].activated) selected = i+firstShown;
			
			if (i+firstShown == selected) elements[i].check = true;
			else elements[i].check = false;
		}
	}
	
	
	public override void  ApplyStyle ()
	{
		for (int i=0;i<elements.Length;i++) 
		{
			//if (styleSet!=null && elementCustomStyle==null) elementStyle = styleSet.FindStyle(elementStyleName, elementStyleNum);
			//elements[i].customStyle = true;
			//elements[i].style = elementStyle;

			elements[i].styleName = elementStyleName;
		}
		
		base.ApplyStyle();
	}
	
	
}