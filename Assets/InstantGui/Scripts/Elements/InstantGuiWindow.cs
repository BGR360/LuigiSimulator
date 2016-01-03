
using UnityEngine;
using System.Collections;

public enum InstantGuiWindowScape {off, screen, parent};

public class InstantGuiWindow : InstantGuiElement
{
	public InstantGuiWindowScape scape;
	
	public bool  movable;
	
	public InstantGuiElement closeButton;
	public InstantGuiElement expandButton;
	public InstantGuiElement minimizeButton;
	
	private bool  dragging;
	private Vector2 dragOffset;
	
	private int sizeX;
	private int sizeY;
	
	private InstantGuiElementPos nonMaximizedRelative;
	private InstantGuiElementPos nonMaximizedOffset;
	
	
	public override void  Action ()
	{
		base.Action();
		
		//moving
		if (movable)
		{
			if (pointed && Input.GetMouseButtonDown(0))
			{
				dragging = true;
				
				sizeX = absolute.right-absolute.left;
				sizeY = absolute.bottom-absolute.top;
				
				dragOffset = new Vector2(
					offset.left-Input.mousePosition.x, 
					offset.top-InstantGui.Invert(Input.mousePosition.y) ); //negative, in common
			}
			
			if (Input.GetMouseButtonUp(0)) dragging = false;
			
			if (dragging)
			{
				offset.left = (int)(Input.mousePosition.x + dragOffset.x); 
				offset.top = (int)(InstantGui.Invert(Input.mousePosition.y) + dragOffset.y);
				
				absolute.GetAbsolute (parentpos, relative, offset);
				
				//limiting window movement
				if (scape == InstantGuiWindowScape.screen)
				{
					absolute.left = Mathf.Max(absolute.left, 0);
					absolute.top = Mathf.Max(absolute.top, 0);
					if (absolute.left > Screen.width - sizeX) absolute.left = Screen.width - sizeX;
					if (absolute.top > Screen.height - sizeY) absolute.top = Screen.height - sizeY;
				}
				
				if (scape == InstantGuiWindowScape.parent)
				{
					absolute.left = Mathf.Max(absolute.left, parentpos.left);
					absolute.top = Mathf.Max(absolute.top, parentpos.top);
					if (absolute.left > parentpos.right - sizeX) absolute.left = parentpos.right - sizeX;
					if (absolute.top > parentpos.bottom - sizeY) absolute.top = parentpos.bottom - sizeY;
				}
				
				offset.GetOffset(parentpos, relative, 
				     new InstantGuiElementPos(absolute.left, absolute.left + sizeX, absolute.top, absolute.top + sizeY));
			}
		}
		
		//closing
		if (closeButton!=null && closeButton.activated) gameObject.SetActive(false);
		
		//expanding
		if (expandButton!=null)
		{
			if (expandButton.activated && expandButton.check)
			{
				nonMaximizedRelative = new InstantGuiElementPos(relative);
				nonMaximizedOffset = new InstantGuiElementPos(offset);
				
				if (scape == InstantGuiWindowScape.screen || scape == InstantGuiWindowScape.off)
				{
					relative = new InstantGuiElementPos(0, 0, 0, 0);
					offset = new InstantGuiElementPos(0, Screen.width, 0, Screen.height);
					
				}
				
				if (scape == InstantGuiWindowScape.parent)
				{
					relative = new InstantGuiElementPos(0, 100, 0, 100);
					offset = new InstantGuiElementPos(0, 0, 0, 0);
				}
			}
			
			if (expandButton.activated && !expandButton.check)
			{
				relative = nonMaximizedRelative;
				offset = nonMaximizedOffset;
			}
			
			//re-calc offset on resolution change
			if (expandButton.check && (scape == InstantGuiWindowScape.screen || scape == InstantGuiWindowScape.off)) 
				offset = new InstantGuiElementPos(0, Screen.width, 0, Screen.height);
		}
	}
}