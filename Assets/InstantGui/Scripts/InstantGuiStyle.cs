
using UnityEngine;
using System.Collections;

[System.Serializable]
public class SubStyle 
{
	public bool  enabled;
	
	public Texture texture;
	//Color color = new Color(1,1,1,0.5f);
	
	public int textOffsetX;
	public int textOffsetY;
	public Color textColor = new Color(1,1,1,1);
	
	//Material textMaterial;
	
	public bool  guiBorders;

	public SubStyle Clone() 
	{ 
		return (SubStyle)this.MemberwiseClone(); 
	}
}

public enum AutoSetBorders {off, zero, half, quater} 
public enum InstantGuiTextEffect {simple=0, shadow=1, stroke=2 };
public enum InstantGuiTextAligment {center=0, text=1, heading=2, above=3, label=4, rightLabel=5};

[System.Serializable]
public class InstantGuiStyle
{
	public string name;
	public bool  show;
		
	public bool  fixedSize;
		
	public bool  fixedWidth;
	public bool  fixedHeight;
	public int fixedWidthSize;
	public int fixedHeightSize;

	public bool proportional;
	public float proportionalAspect = 1;
		
	public SubStyle main = new SubStyle();
	public SubStyle pointed = new SubStyle();
	public SubStyle active = new SubStyle();
	public SubStyle disabled = new SubStyle();
		
	public RectOffset borders;// = new RectOffset(0,0,0,0);
	public float blendSpeed = 2;
		
	public Font font;
	public Font oldFont;
	public int fontSize;
		
	public InstantGuiTextAligment textAligment;
	public int textOffsetX;
	public int textOffsetY;
	public InstantGuiTextEffect textEffect;
	public int textEffectSize = 1;
	public Color textEffectColor;

	//default placement
	public InstantGuiElementPos relative  = new InstantGuiElementPos(50,50,50,50,true);
	public InstantGuiElementPos offset = new InstantGuiElementPos(-20,20,-20,20,true);
	public float layerOffset;
		
	public RectOffset pointOffset;// = new RectOffset(0,0,0,0);

	/*
	public InstantGuiStyle()
	{
		main = new SubStyle();
		pointed = result.pointed.Clone();
		active = result.active.Clone();
		result.disabled = result.disabled.Clone();
		result.relative = result.relative.Clone();
		result.offset = result.offset.Clone();
		result.pointOffset = new RectOffset(result.pointOffset.left, result.pointOffset.right, result.pointOffset.top, result.pointOffset.bottom);
		result.borders = new RectOffset(result.borders.left, result.borders.right, result.borders.top, result.borders.bottom);
		return result;
	}
	*/

	public void  Apply (SubStyle sub, InstantGuiElement element)
	{
		//getting number of texts that should be in element
		int textsLength=0;
		if (element.text.Length==0) textsLength = 0;
		else switch (textEffect)
		{
		case InstantGuiTextEffect.simple: textsLength = 1; break;
		case InstantGuiTextEffect.shadow: textsLength = 2; break;
		case InstantGuiTextEffect.stroke: textsLength = 5; break;
		}
		
		//should a texture ot text be removed?
		bool  removeTexture=false; 
		bool  removeTexts=false;
		
		if (!sub.enabled) { removeTexture = true; removeTexts = true; }
		
		if (!sub.texture) removeTexture = true;
		
		if (element.guiTexts.Length != textsLength) removeTexts = true; //if text Length changed. No text included
		if (font!=element.currentFont) removeTexts = true; //if font changed
		for (int i=0; i<element.guiTexts.Length; i++) 
			if (!element.guiTexts[i]) removeTexts = true;	//if any of guitexts array do not exists
		
		
		//removing
		if (removeTexture && element.mainGuiTexture!=null) { GameObject.DestroyImmediate(element.mainGuiTexture.gameObject); }
		if (removeTexts)
		{
			for(int i=element.guiTexts.Length-1; i>=0 ; i--)
				if (element.guiTexts[i]!=null) 
					GameObject.DestroyImmediate(element.guiTexts[i].gameObject);
			
			//creating new array
			element.guiTexts = new GUIText[textsLength];
		}
		
		if (!sub.enabled || (!sub.texture && element.text.Length==0)) return;
		
		//setting texture
		if (sub.texture!=null)
		{
			if (element.mainGuiTexture==null)
			{
				GameObject obj = new GameObject (element.transform.name + "_mainTexture");
				obj.transform.parent = element.transform;
				obj.hideFlags = HideFlags.HideInHierarchy;
				element.mainGuiTexture = obj.AddComponent<GUITexture>();
			}
			
			if (element.mainGuiTexture.texture != sub.texture) element.mainGuiTexture.texture = sub.texture;
			
			element.mainGuiTexture.transform.localPosition = new Vector3(0,0,element.transform.localPosition.z);
			element.mainGuiTexture.pixelInset = InstantGui.Invert(element.absolute.ToRect(InstantGui.scale));
			if (borders!=null) element.mainGuiTexture.border = borders;
		}

		//proportional
		if (proportional && element.mainGuiTexture!=null)
		{
			Rect newInset = element.mainGuiTexture.pixelInset;
			if (1.0f * element.mainGuiTexture.pixelInset.width / element.mainGuiTexture.pixelInset.height < proportionalAspect)
			{ 
				newInset.width = element.mainGuiTexture.pixelInset.height*proportionalAspect;
				newInset.x -= (newInset.width-element.mainGuiTexture.pixelInset.width)*0.5f;
			}
			else
			{
				newInset.height = element.mainGuiTexture.pixelInset.width*(1.0f/proportionalAspect);
				newInset.y -= (newInset.height-element.mainGuiTexture.pixelInset.height)*0.5f;
			}
			element.mainGuiTexture.pixelInset = newInset;
		}
		
		
		//setting text
		if (element.text.Length!=0)
		{		
			//on font change - remove materials
			if (!font) font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			if (font != element.currentFont) 
			{ 
				//element.textMaterial=null; 
				//if (element.guiTexts.Length > 1) element.textEffectMaterial=null;
				for (int i=0;i<element.guiTexts.Length;i++) if (element.guiTexts[i]!=null) GameObject.DestroyImmediate(element.guiTexts[i].gameObject);
				element.currentFont = font;
			} 
			
			//creating texts
			for (int i=0;i<element.guiTexts.Length;i++) 
			{
				if (element.guiTexts[i]==null)
				{
					GameObject obj = new GameObject (element.transform.name + "_text" + i.ToString());
					obj.transform.parent = element.transform;
					obj.hideFlags = HideFlags.HideInHierarchy;
					element.guiTexts[i] = obj.AddComponent<GUIText>();
					
					//setting materials		
					if (i==0)
					{
						element.textMaterial = new Material (font.material);
						element.guiTexts[i].material = element.textMaterial;
					}
					else if (i>0 && i==element.guiTexts.Length-1) //on the last one effect text
					{
						element.textEffectMaterial = new Material (font.material);
						for (int j=1;j<element.guiTexts.Length;j++)
							element.guiTexts[j].material = element.textEffectMaterial;
					}
				}
			}
			
			//setting text params
			for (int i=0;i<element.guiTexts.Length;i++) 
			{
				if (i==0) element.textMaterial.color = sub.textColor;
				else element.textEffectMaterial.color = textEffectColor;
				
				element.guiTexts[i].transform.localPosition = new Vector3(0,0,element.transform.localPosition.z+0.01f - i*0.001f);
				
				//setting text		
				if (element.guiTexts[i].text != element.text && !element.password) element.guiTexts[i].text = element.text;
				else if (element.password)
				{
					element.guiTexts[i].text = "";
					for (int j=0;j<element.text.Length;j++) element.guiTexts[i].text += "*";
				}
				
				if (element.guiTexts[i].font != font) element.guiTexts[i].font = font;
				if (element.guiTexts[i].fontSize != fontSize) element.guiTexts[i].fontSize = fontSize;
				
				Vector2 pixelOffset = element.absolute.GetCenter();
				
				switch (textAligment)
				{
				case InstantGuiTextAligment.center: element.guiTexts[i].anchor = TextAnchor.MiddleCenter; break;
				case InstantGuiTextAligment.text: 
					pixelOffset.y = element.absolute.top; 
					pixelOffset.x = element.absolute.left;
					element.guiTexts[i].anchor = TextAnchor.UpperLeft; break;
				case InstantGuiTextAligment.heading: pixelOffset.y = element.absolute.top; element.guiTexts[i].anchor = TextAnchor.UpperCenter; break;
				case InstantGuiTextAligment.above: pixelOffset.y = element.absolute.top; element.guiTexts[i].anchor = TextAnchor.LowerCenter; break;
				case InstantGuiTextAligment.label: 
					pixelOffset.x = element.absolute.left;
					element.guiTexts[i].anchor = TextAnchor.MiddleRight; break;
				case InstantGuiTextAligment.rightLabel: 
					pixelOffset.x = element.absolute.right;
					element.guiTexts[i].anchor = TextAnchor.MiddleLeft; break;
				}
				
				pixelOffset += new Vector2(textOffsetX+sub.textOffsetX, textOffsetY+sub.textOffsetY);
				
				element.guiTexts[i].pixelOffset = InstantGui.Invert(pixelOffset);		
			}
			
			
			//aligning text effect
			if (textEffect==InstantGuiTextEffect.shadow)
			{
				element.guiTexts[1].pixelOffset = element.guiTexts[0].pixelOffset + new Vector2(textEffectSize*0.75f,-textEffectSize);
			}
			else if (textEffect==InstantGuiTextEffect.stroke) 
			{
				element.guiTexts[1].pixelOffset = element.guiTexts[0].pixelOffset + new Vector2(textEffectSize,textEffectSize);
				element.guiTexts[2].pixelOffset = element.guiTexts[0].pixelOffset + new Vector2(-textEffectSize,textEffectSize);
				element.guiTexts[3].pixelOffset = element.guiTexts[0].pixelOffset + new Vector2(-textEffectSize,-textEffectSize);
				element.guiTexts[4].pixelOffset = element.guiTexts[0].pixelOffset + new Vector2(textEffectSize,-textEffectSize);
			}
		}
	}
		
		
	public void Blend ( SubStyle sub ,   InstantGuiElement element ,   float blend  ) //done after base style apply
		{
			//removing a blended texture if necessary
			if ((!sub.enabled || sub.texture==null) && element.blendGuiTexture!=null) GameObject.DestroyImmediate(element.blendGuiTexture.gameObject);
			
			//setting texture
			if (sub.texture!=null) 
			{
				if (!element.blendGuiTexture)
				{
					GameObject obj = new GameObject (element.transform.name + "_blendTexture");
					obj.transform.parent = element.transform;
					//obj.hideFlags = HideFlags.HideInHierarchy;
					element.blendGuiTexture = obj.AddComponent<GUITexture>();
				}
				
				if (element.blendGuiTexture.texture != sub.texture) element.blendGuiTexture.texture = sub.texture;
				
				element.blendGuiTexture.transform.localPosition = new Vector3(0,0,element.transform.localPosition.z+0.005f);
				element.blendGuiTexture.pixelInset = InstantGui.Invert(element.absolute.ToRect(InstantGui.scale));
				if (borders!=null) element.blendGuiTexture.border = borders;
				
				element.blendGuiTexture.color = new Color(element.blendGuiTexture.color.r, element.blendGuiTexture.color.g, element.blendGuiTexture.color.b, blend*0.5f);
				if (element.mainGuiTexture!=null) element.mainGuiTexture.color = 
					new Color(element.mainGuiTexture.color.r, element.mainGuiTexture.color.g, element.mainGuiTexture.color.b, (1-blend*blend)*0.5f);
			}
			
			if (element.guiTexts!=null && element.guiTexts.Length > 0)
				element.textMaterial.color = element.textMaterial.color*(1-blend) + sub.textColor*blend;
		}
		
	public void Unblend ( InstantGuiElement element  ) //removes any blend, done after base style apply
		{
			if (element.blendGuiTexture!=null) GameObject.DestroyImmediate(element.blendGuiTexture.gameObject);
			if (element.mainGuiTexture!=null && !Mathf.Approximately(element.mainGuiTexture.color.a, 0.5f)) 
				element.mainGuiTexture.color = new Color (element.mainGuiTexture.color.r, element.mainGuiTexture.color.g, element.mainGuiTexture.color.b, 0.5f);
		}
		
	static public void  Clear ( InstantGuiElement element  ){
			if (element.mainGuiTexture!=null) GameObject.DestroyImmediate(element.mainGuiTexture.gameObject);
			
			for(int i=element.guiTexts.Length-1; i>=0 ; i--)
				if (element.guiTexts[i]!=null) 
					GameObject.DestroyImmediate(element.guiTexts[i].gameObject);
			
			if (element.blendGuiTexture!=null) GameObject.DestroyImmediate(element.blendGuiTexture.gameObject);
		}
		
	public InstantGuiStyle Clone ()
	{
			InstantGuiStyle result = (InstantGuiStyle)this.MemberwiseClone(); 

			result.main = result.main.Clone();
			result.pointed = result.pointed.Clone();
			result.active = result.active.Clone();
			result.disabled = result.disabled.Clone();
			result.relative = result.relative.Clone();
			result.offset = result.offset.Clone();
			result.pointOffset = new RectOffset(result.pointOffset.left, result.pointOffset.right, result.pointOffset.top, result.pointOffset.bottom);
			result.borders = new RectOffset(result.borders.left, result.borders.right, result.borders.top, result.borders.bottom);
			return result;
	}
		
}
