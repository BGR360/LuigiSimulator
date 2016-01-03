using UnityEngine;
using System.Collections;

public class InstantGuiButton : InstantGuiElement
{
	public InstantGuiActivator onPressed;
	
	public override void Action ()
	{
		base.Action();
		if (activated) onPressed.Activate(this);
	}
}
