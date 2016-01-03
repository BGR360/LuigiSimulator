
using UnityEngine;
using System.Collections;


public class InstantGuiSlider : InstantGuiElement
{
	public float value = 100;
	public float min = 0;
	public float max = 100;
	//private float oldValue = 100;
	public float step = 0;
	public float buttonStep = 1;
	public bool  horizontal = false;
	
	public float shownValue = 50;
	
	public InstantGuiElement diamond;
	public InstantGuiElement incrementButton;
	public InstantGuiElement decrementButton;
	
	private int dragStart;
	private float valueStart;
	
	public override void  Action ()
	{
		
		
		base.Action();
		
		//increment/decrement
		if (incrementButton!=null && incrementButton.activated) value += buttonStep;
		if (decrementButton!=null && decrementButton.activated) value -= buttonStep;
		value = Mathf.Clamp(value, min, max);
		
		//setting diamond
		if (diamond!=null)
		{
			//horizontal
			if (horizontal)
			{
				if (diamond.activated)
				{
					#if UNITY_EDITOR
					if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && Event.current!=null && Event.current.alt && Event.current.isMouse) 
						dragStart = (int)Event.current.mousePosition.x;
					else dragStart = (int)InstantGui.Invert(Input.mousePosition.x);
					#else
					dragStart = (int)InstantGui.Invert(Input.mousePosition.x);
					#endif

					valueStart = value;
				}
				
				if (diamond.pressed)
				{
					int dragDist=0;
					#if UNITY_EDITOR
					if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && Event.current!=null && Event.current.alt && Event.current.isMouse) 
						dragDist = (int)(Event.current.mousePosition.x - dragStart);
					else dragDist = (int)(InstantGui.Invert(Input.mousePosition.x) - dragStart);
					#else
					dragDist = (int)(InstantGui.Invert(Input.mousePosition.x) - dragStart);
					#endif

					int totalDist = absolute.right-absolute.left;
					
					value = valueStart - (1.0f*dragDist/totalDist)*(max-min+shownValue);
				}
				
				//rounding value to step
				if (step > 0.001f) value = Mathf.Round(value/step) * step;
				
				//clamping
				value = Mathf.Clamp(value, min, max);
					
				diamond.relative.left = (int)((value)/(max-min+shownValue)*100);		
				diamond.relative.right = (int)((value+shownValue)/(max-min+shownValue)*100);				
			}
			
			//vertical
			else
			{
				//diamondSize = diamond.absolute.bottom - diamond.absolute.top;
				//fieldStart = absolute.top + diamondSize*0.5f;
				//fieldEnd = absolute.bottom - diamondSize*0.5f;
				/*
			if (pressed || activated)
			{
				if (!EditorApplication.isPlayingOrWillChangePlaymode && Event.current!=null && Event.current.alt && Event.current.isMouse) 
					clickPos = Event.current.mousePosition.y - fieldStart;
				else clickPos = InstantGui.Invert(Input.mousePosition.y) - fieldStart;
				percent = clickPos/(fieldEnd-fieldStart);
				value = percent * (max-min) + min;
			}
			*/
				
				if (diamond.activated)
				{
					#if UNITY_EDITOR
					if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && Event.current!=null && Event.current.alt && Event.current.isMouse) 
						dragStart = (int)Event.current.mousePosition.y;
					else dragStart = (int)InstantGui.Invert(Input.mousePosition.y);
					#else
					dragStart = (int)InstantGui.Invert(Input.mousePosition.y);
					#endif

					valueStart = value;
				}
				
				if (diamond.pressed)
				{
					int dragDist=0;
					#if UNITY_EDITOR
					if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && Event.current!=null && Event.current.alt && Event.current.isMouse) 
						dragDist = (int)(Event.current.mousePosition.y - dragStart);
					else dragDist = (int)(InstantGui.Invert(Input.mousePosition.y) - dragStart);
					#else
					dragDist = (int)(InstantGui.Invert(Input.mousePosition.y) - dragStart);
					#endif

					int totalDist = absolute.bottom-absolute.top;
					
					value = valueStart + (1.0f*dragDist/totalDist)*(max-min+shownValue);
				}
				
				//rounding value to step
				if (step > 0.001f) value = Mathf.Round(value/step) * step;
				
				//clamping
				value = Mathf.Clamp(value, min, max);

				diamond.relative.top = (int)((value)/(max-min+shownValue)*100);		
				diamond.relative.bottom = (int)((value+shownValue)/(max-min+shownValue)*100);				
				
				//unchangable slider size
				//percent = value/(max-min);		
				//diamond.relative.top = (value-shownValue*percent)/(max-min)*100;			
				//diamond.relative.bottom = (value+shownValue*(1-percent))/(max-min)*100;
			}
		}
		
		//oldValue = value;
	}
}
