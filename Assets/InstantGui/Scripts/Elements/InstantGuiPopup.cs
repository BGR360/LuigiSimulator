
using UnityEngine;
using System.Collections;

public class InstantGuiPopup : InstantGuiElement
{
	public int selected;
	public InstantGuiList list;
	
	public bool  invertedAlign; //internal value to know that element is aligned upside-down
	
	public override void  Action ()
	{
		base.Action();
		
		if (list!=null)
		{
			if (activated) 
			{
				//showing list
				if (!list.gameObject.activeSelf) 
				{
					list.gameObject.SetActive(true);
					
					//try to align in forward direction
					if (!invertedAlign) list.Align();
					else InvertAlign(list);
					
					//changing direction if it is not right
					if (list.absolute.bottom > Screen.height) InvertAlign(list);
					if (list.absolute.top < 0) InvertAlign(list);
					
					//aligning this element to prevent flickering
					Align();
				}
				else list.gameObject.SetActive(false);
			}
			
			//hiding list if clicked in any other element
			else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && list.gameObject.activeSelf) 
				list.gameObject.SetActive(false);
			
			selected = list.selected;
			
			//setting text to the selected one
			if (selected >= 0 && selected<list.labels.Length) text = list.labels[selected];
		}
	}
	
	public void  InvertAlign ( InstantGuiElement element  )
	{
		int tmp = element.relative.top;
		element.relative.top = 100-element.relative.bottom;
		element.relative.bottom = 100-tmp;
		
		tmp = element.offset.top;
		element.offset.top = -element.offset.bottom;
		element.offset.bottom = -tmp;
		
		invertedAlign = !invertedAlign;
		element.Align();
	}
}
