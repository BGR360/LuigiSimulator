
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(InstantGui))]
class InstantGuiEditor : Editor
{
	public override void  OnInspectorGUI ()
	{
		InstantGuiFrame.drawFrames = EditorGUILayout.Toggle("Display Controls", InstantGuiFrame.drawFrames);
		InstantGuiFrame.frameSize = EditorGUILayout.IntField("Controls Size:", InstantGuiFrame.frameSize);
		InstantGuiFrame.showValues = (InstantGuiFrame.InstantGuiFrameShowValues)EditorGUILayout.EnumPopup("Show Values:", InstantGuiFrame.showValues);

		//InstantGui.scale = EditorGUILayout.FloatField("Scale:", InstantGui.scale);

		//lock element
		bool lockElement = EditorGUILayout.Toggle("Lock element", InstantGuiFrame.lockedElement!=null);
		if (lockElement && InstantGuiFrame.lockedElement==null && Selection.activeTransform!=null) //locking
			InstantGuiFrame.lockedElement = Selection.activeTransform.GetComponent<InstantGuiElement>();
		if (!lockElement && InstantGuiFrame.lockedElement!=null) //unlocking 
			InstantGuiFrame.lockedElement = null;


		if (Selection.activeTransform!=null && Selection.activeTransform.GetComponent<InstantGuiElement>()) 
			EditorUtility.SetDirty(Selection.activeTransform.GetComponent<InstantGuiElement>());
	}

	static InstantGuiElement GetSelectedElement ()
	{
		InstantGuiElement result;

		if (InstantGui.instance==null) InstantGui.instance = (InstantGui)FindObjectOfType(typeof(InstantGui));
		if (InstantGui.instance==null) return null;
		
		if (Selection.activeTransform!=null && Selection.activeTransform.IsChildOf(InstantGui.instance.transform))
		{
			result = (InstantGuiElement)Selection.activeTransform.GetComponent(typeof(InstantGuiElement));
			
			if (result.GetType() == typeof(InstantGuiTabs) )
			{
				InstantGuiTabs parentTabs = (InstantGuiTabs)result;
				if (parentTabs.fields!=null && parentTabs.fields.Length>parentTabs.selected && parentTabs.fields[parentTabs.selected]!=null)
					result = parentTabs.fields[parentTabs.selected];
			}
		}
		else result = (InstantGuiElement)InstantGui.instance.gameObject.GetComponent(typeof(InstantGuiElement));

		return result;
	}

	[MenuItem("GameObject/Create InstantGUI/New InstantGUI", false, 1)]
	static public void  CreateRoot ()
	{
		if (InstantGui.instance==null) InstantGui.instance = (InstantGui)FindObjectOfType(typeof(InstantGui));
		if (InstantGui.instance==null) InstantGui.CreateRoot();
	}

	[MenuItem("GameObject/Create InstantGUI/Empty Element")]
	static public InstantGuiElement  CreateEmpty ()
	{ 
		InstantGuiElement element = InstantGuiElement.Create ("Element", typeof(InstantGuiElement), GetSelectedElement()); 
		Selection.activeGameObject = element.gameObject;
		return element;
	}

	[MenuItem("GameObject/Create InstantGUI/Label")]
	static public InstantGuiElement  CreateLabel ()
	{ 
		InstantGuiElement element = InstantGuiElement.Create ("Label", typeof(InstantGuiElement), GetSelectedElement()); 
		Selection.activeGameObject = element.gameObject;
		return element;
	}
	
	
	[MenuItem("GameObject/Create InstantGUI/Button")]
	static public InstantGuiButton CreateButton ()
	{
		InstantGuiButton element = (InstantGuiButton)InstantGuiElement.Create("Button", typeof(InstantGuiButton), GetSelectedElement());
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject;
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/Window")]
	static public InstantGuiWindow CreateWindow ()
	{
		InstantGuiWindow element = (InstantGuiWindow)InstantGuiElement.Create("Window", typeof(InstantGuiWindow), GetSelectedElement());
		element.closeButton = InstantGuiElement.Create("Window_CloseButton", typeof(InstantGuiElement), element);
		element.closeButton.useStylePlacement = true;
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/Toggle")]
	static public InstantGuiToggle CreateToggle ()
	{
		InstantGuiToggle element = (InstantGuiToggle)InstantGuiElement.Create("Toggle", typeof(InstantGuiToggle), GetSelectedElement());
		
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element; 
	}
	
	[MenuItem("GameObject/Create InstantGUI/Input Text")]
	static public InstantGuiInputText CreateInputText ()
	{
		InstantGuiInputText element = (InstantGuiInputText)InstantGuiElement.Create("InputText", typeof(InstantGuiInputText), GetSelectedElement());
		
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element; 
	}
	
	[MenuItem("GameObject/Create InstantGUI/Text Area")]
	static public InstantGuiTextArea CreateTextArea ()
	{
		InstantGuiTextArea element = (InstantGuiTextArea)InstantGuiElement.Create("TextArea", typeof(InstantGuiTextArea), GetSelectedElement());
		element.slider = CreateVerticalSlider();
		element.slider.useStylePlacement = true;
		element.slider.transform.parent = element.transform;
		
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element; 
	}
	
	[MenuItem("GameObject/Create InstantGUI/Horizontal Slider")]
	static public InstantGuiSlider CreateHorizontalSlider ()
	{
		InstantGuiSlider element = (InstantGuiSlider)InstantGuiElement.Create("HorizontalSlider", typeof(InstantGuiSlider), GetSelectedElement());
		element.diamond = InstantGuiElement.Create("HorizontalSlider_Diamond", typeof(InstantGuiElement), element);
		element.incrementButton = InstantGuiElement.Create("HorizontalSlider_IncrementButton", typeof(InstantGuiElement), element);
		element.decrementButton = InstantGuiElement.Create("HorizontalSlider_DecrementButton", typeof(InstantGuiElement), element);
		element.horizontal = true;
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/Vertical Slider")]
	static public InstantGuiSlider CreateVerticalSlider ()
	{
		InstantGuiSlider element = (InstantGuiSlider)InstantGuiElement.Create("VerticalSlider", typeof(InstantGuiSlider), GetSelectedElement());
		element.diamond = InstantGuiElement.Create("VerticalSlider_Diamond", typeof(InstantGuiElement), element);
		element.incrementButton = InstantGuiElement.Create("VerticalSlider_IncrementButton", typeof(InstantGuiElement), element);
		element.decrementButton = InstantGuiElement.Create("VerticalSlider_DecrementButton", typeof(InstantGuiElement), element);
		element.diamond.editable = false;
		//element.diamond.useStylePlacement = true;
		element.incrementButton.useStylePlacement = true;
		element.decrementButton.useStylePlacement = true;
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/List")]
	static public InstantGuiList CreateList ()
	{
		InstantGuiList element = (InstantGuiList)InstantGuiElement.Create("List", typeof(InstantGuiList), GetSelectedElement());
		element.slider = CreateVerticalSlider();
		element.slider.useStylePlacement = true;
		element.slider.transform.parent = element.transform;
		
		element.elementStyleName = "List_Element";
		
		//element.diamond = InstantGuiElement.Create("Slider_Diamond", InstantGuiElement, element);
		//element.incrementButton = InstantGuiElement.Create("Slider_IncrementButton", InstantGuiElement, element);
		//element.decrementButton = InstantGuiElement.Create("Slider_DecrementButton", InstantGuiElement, element);
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/Popup")]
	static public InstantGuiPopup CreatePopup ()
	{
		InstantGuiPopup element = (InstantGuiPopup)InstantGuiElement.Create("Popup", typeof(InstantGuiPopup), GetSelectedElement());
		element.list = CreateList();
		//element.list.useStylePlacement = true;
		element.list.transform.parent = element.transform;
		element.list.relative.Set(0,100,100,100);
		element.list.offset.Set(0,0,0,200);
		
		//element.diamond = InstantGuiElement.Create("Slider_Diamond", InstantGuiElement, element);
		//element.incrementButton = InstantGuiElement.Create("Slider_IncrementButton", InstantGuiElement, element);
		//element.decrementButton = InstantGuiElement.Create("Slider_DecrementButton", InstantGuiElement, element);
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}
	
	[MenuItem("GameObject/Create InstantGUI/Tab Group")]
	static public InstantGuiTabs CreateTabGroup ()
	{
		InstantGuiTabs element = (InstantGuiTabs)InstantGuiElement.Create("TabGroup", typeof(InstantGuiTabs), GetSelectedElement());
		element.tabs = new InstantGuiElement[1]; element.tabs[0] = InstantGuiElement.Create("Tab", typeof(InstantGuiElement), element);	
		element.fields = new InstantGuiElement[1];  element.fields[0] =InstantGuiElement.Create("Tab_Field", "", typeof(InstantGuiElement), element);
		
		element.fields[0].relative.Set(0,100,0,100);
		element.fields[0].offset.Set(0,0,0,0);
		element.fields[0].lockPosition = true;
		element.fields[0].dynamic = false;
		
		InstantGui.ForceUpdate();
		Selection.activeGameObject = element.gameObject; 
		return element;
	}

	/*
	[MenuItem("Instant GUI/Lock(Unlock) Element")]
	static public void  LockUnlockElement (){
		if (!InstantGuiFrame.lockedElement) 
		{
			if (Selection.activeTransform!=null) InstantGuiFrame.lockedElement = Selection.activeTransform.GetComponent<InstantGuiElement>();
		}
		else InstantGuiFrame.lockedElement = null;
		
		if (Selection.activeTransform!=null && Selection.activeTransform.GetComponent<InstantGuiElement>()) 
			EditorUtility.SetDirty(Selection.activeTransform.GetComponent<InstantGuiElement>());
	}
	
	[MenuItem("Instant GUI/Hide(Unhide) Frame")]
	static public void  HideUnhideFrame (){
		InstantGuiFrame.drawFrames = !InstantGuiFrame.drawFrames;
		if (Selection.activeTransform!=null && Selection.activeTransform.GetComponent<InstantGuiElement>()) 
			EditorUtility.SetDirty(Selection.activeTransform.GetComponent<InstantGuiElement>());
	}
	*/
	
	[MenuItem ("Assets/Create/InstantGui Style Set")]
	static public void  CreateStyleSet ()
	{
		Object asset= Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0];
		string pathString = AssetDatabase.GetAssetOrScenePath(asset);
		if (asset.GetType()!=typeof(UnityEngine.Object)) pathString = System.IO.Path.GetDirectoryName(pathString);
		
		InstantGuiStyleSet newasset=ScriptableObject.CreateInstance<InstantGuiStyleSet>();
		AssetDatabase.CreateAsset (newasset, pathString + "/New Style Set.asset");
	}
}
