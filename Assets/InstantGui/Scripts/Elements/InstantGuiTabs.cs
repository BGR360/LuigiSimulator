
using UnityEngine;
using System.Collections;

public class InstantGuiTabs : InstantGuiElement
{
	public bool  couldBeUnchecked = true;
	
	public InstantGuiElement[] tabs = new InstantGuiElement[0];
	public InstantGuiElement[] fields = new InstantGuiElement[0];
	public int selected;
	//private int oldActive;

	public InstantGuiActivator onChecked;
	public InstantGuiActivator onUnchecked;

	public override void  Action ()
	{
		base.Action();
		
		if (tabs.Length != fields.Length) return; //filling arrays in editor
		
		for (int i=0; i<tabs.Length; i++)
		{
			if (tabs[i].activated) selected = i;
			
			//enabling and disabling tabs and fields
			if (i==selected && !tabs[i].check)
			{
				fields[i].gameObject.SetActive(true);
				if (onChecked!=null) onChecked.Activate(this);
				tabs[i].check = true;
			}
			
			if (i!=selected && tabs[i].check)
			{
				fields[i].gameObject.SetActive(false);
				if (onUnchecked!=null) onUnchecked.Activate(this);
				tabs[i].check = false;
			}
		}
		
	}
	
	public InstantGuiElement CreateField ()
	{
		InstantGuiElement field = InstantGuiElement.Create("", typeof(InstantGuiElement), this);
		field.relative.Set(0,100,0,100);
		field.offset.Set(0,0,0,0);
		field.lockPosition = true;
		field.dynamic = false;
		return field;
	}
}
