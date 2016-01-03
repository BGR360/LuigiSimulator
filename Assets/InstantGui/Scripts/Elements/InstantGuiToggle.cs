
using UnityEngine;
using System.Collections;

public class InstantGuiToggle : InstantGuiElement
{
	public bool  couldBeUnchecked = true;
	
	public InstantGuiElement[] uncheckToggles = new InstantGuiElement[0];
	
	public InstantGuiActivator onChecked;
	public InstantGuiActivator onUnchecked;
	
	public override void  Action (){
		base.Action();
		
		if (activated) 
		{
			//checking
			if (!check)
			{
				check = true;
				
				for (int i = 0; i<uncheckToggles.Length; i++)
				{
					if (uncheckToggles[i].check)
					{
						uncheckToggles[i].check = false; 
						if (uncheckToggles[i].GetType() == typeof(InstantGuiToggle))
							((InstantGuiToggle)uncheckToggles[i]).onUnchecked.Activate(this);

					}
				}
				
				onChecked.Activate(this);
			}
			
			//unchecking
			else if (couldBeUnchecked)
			{
				check = false;
				onUnchecked.Activate(this);
			}
		}		
	}
}
