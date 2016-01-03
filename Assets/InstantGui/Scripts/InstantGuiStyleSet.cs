
using UnityEngine;
using System.Collections;

public class InstantGuiStyleSet : ScriptableObject 
{
	public InstantGuiStyle[] styles;// = {new InstantGuiStyle()};

	public InstantGuiStyle FindStyle ( string styleName)
	{
			if (styleName==null || styleName.Length == 0) return styles[0]; //shortcut for empty style
			
			InstantGuiStyle style = null;
			
			//finding by name
			for (int i=0; i<styles.Length; i++) 
				if (styles[i].name == styleName) 
					style = styles[i];
			
			//finding by num
			//if (style==null && styles.Length>styleNum) style = styles[styleNum];
			
			return style;
		}
	}
	
	
	