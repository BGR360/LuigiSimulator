using UnityEngine;
using System.Collections;

public class InstantGuiElementsList : InstantGuiElement
{
	public bool  autoFill = true;
	public string[] labels = new string[0]; //if labels is not empty than auto-creating elements array
	
	
	public InstantGuiElement[] elements = new InstantGuiElement[0];
	
	//InstantGuiStyle elementStyle;
	public int elementSize = 10; //set this to 0 if you want to show all elements
	//int elementWidth = 10;
	
	public int firstShown = 0;

	public override void  Align ()
	{
		base.Align();
		
		//re-creating elements if labels exist
		if (autoFill && labels.Length != elements.Length)
		{
			for (int i=elements.Length-1;i>=0;i--) DestroyImmediate(elements[i].gameObject);
			elements = new InstantGuiElement[labels.Length];
			for (int i=0;i<elements.Length;i++) elements[i] = InstantGuiElement.Create("List_Element",
			                                                                               typeof(InstantGuiElement), (InstantGuiElement)this); //should be toggle
		}
		
		//calculating number of lines
		int numElements;
		if (elementSize <= 0) numElements = elements.Length;
		else
		{
			numElements = Mathf.CeilToInt( (absolute.bottom - absolute.top)*1.0f/elementSize );
			numElements = Mathf.Max(numElements, 0);
		}
		
		//enabling or disabling invisible elements
		int lastShown = Mathf.Min(elements.Length-1, firstShown+numElements-1);
		
		for (int i=0;i<firstShown;i++) //disabling
		{
			elements[i].gameObject.SetActive(false);
		}
		
		for (int i=firstShown;i<=lastShown;i++) //enabling
		{
			elements[i].gameObject.SetActive(true);
			if (autoFill) elements[i].text = labels[i];
			
			//setting position
			
			
			elements[i].Align();
		}
		
		for (int i=lastShown;i<elements.Length;i++) //disabling
		{
			elements[i].gameObject.SetActive(false);
		}
	}
	
}
