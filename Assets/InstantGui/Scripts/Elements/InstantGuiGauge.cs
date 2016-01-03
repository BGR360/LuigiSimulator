
using UnityEngine;
using System.Collections;

public enum InstantGuiGaugeAlign {left, right, top, bottom}; 


public class InstantGuiGauge : InstantGuiElement
{
	public InstantGuiElement gauge;
	
	public float min;
	public float max;
	public float value;
	
	public InstantGuiGaugeAlign align;
	
	public override void  Align ()
	{
		base.Align();
		
		if (gauge!=null)
		{
			if (align == InstantGuiGaugeAlign.right) gauge.relative.left = (int)(value/(max-min)*100); else gauge.relative.left = 0;
			if (align == InstantGuiGaugeAlign.left) gauge.relative.right = (int)(value/(max-min)*100); else gauge.relative.right = 100;
			if (align == InstantGuiGaugeAlign.top) gauge.relative.bottom = (int)(value/(max-min)*100); else gauge.relative.bottom = 100;
			if (align == InstantGuiGaugeAlign.bottom) gauge.relative.top = (int)(value/(max-min)*100); else gauge.relative.top = 0;
		}
	}
}
