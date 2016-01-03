
using UnityEngine;
using System.Collections;


[System.Serializable]
public class InstantGuiElementPos
{
	public int left;
	public int right;
	public int top;
	public int bottom;
	public bool  isStyle;
	
	public InstantGuiElementPos ( int l ,   int r ,   int t ,   int b  ){ left=l; right=r; top=t; bottom=b; }
	public InstantGuiElementPos ( int l ,   int r ,   int t ,   int b ,   bool st  ){  left=l; right=r; top=t; bottom=b; isStyle=st; }
	public InstantGuiElementPos ( InstantGuiElementPos s  ){ left=s.left; right=s.right; top=s.top; bottom=s.bottom; }
	public Rect ToRect (){ return new Rect (left, top, right-left, bottom-top); }
	public Rect ToRect ( float scale  ){ return new Rect (left*scale, top*scale, (right-left)*scale, (bottom-top)*scale); }
	public new string ToString (){ return ("InstantGuiElementPos left:"+left.ToString()+" right:"+right.ToString()+" top:"+top.ToString()+" bottom:"+bottom.ToString()); }
	public Vector2 GetCenter (){ return new Vector2(left+(right-left)*0.5f , top+(bottom-top)*0.5f); }
	public void  Set ( int l ,   int r ,   int t ,   int b  ){ left=l; right=r; top=t; bottom=b; }
	public void  Set ( InstantGuiElementPos s  ){ left=s.left; right=s.right; top=s.top; bottom=s.bottom; }
	
	public int  GetWidth (){ return right-left; }
	public int  GetHeight (){ return bottom-top; }
	
	public void Add ( InstantGuiElementPos e  ){
		left += e.left;
		right += e.right;
		top += e.top;
		bottom += e.bottom;
	}
	
	public void  Subtract ( InstantGuiElementPos e2  ){
		left -= e2.left;
		right -= e2.right; 
		top -= e2.top; 
		bottom -= e2.bottom;
	}
	
	//transforming relative pos to absolute
	static public InstantGuiElementPos  GetRelativeAbsolute ( InstantGuiElementPos parent ,   InstantGuiElementPos relative  ){
		return new InstantGuiElementPos (
			Mathf.RoundToInt(parent.left + ((parent.right-parent.left) * relative.left * 0.01f)), 
			Mathf.RoundToInt(parent.left + ((parent.right-parent.left) * relative.right * 0.01f)), 
		    Mathf.RoundToInt(parent.top + ((parent.bottom-parent.top) * relative.top * 0.01f)),  
			Mathf.RoundToInt(parent.top + ((parent.bottom-parent.top) * relative.bottom * 0.01f)) );
	}
	
	static public Vector2 ScreenPointToRelative ( InstantGuiElementPos parent ,   Vector2 point  ){
		return new Vector2 (
			(point.x-parent.left)/(parent.right-parent.left)*100.0f,
			(point.y-parent.top)/(parent.bottom-parent.top)*100.0f );
	}

	
	static public bool Equals ( InstantGuiElementPos e1 ,   InstantGuiElementPos e2  ){
		return (e1.left==e2.left && e1.right==e2.right && e1.top==e2.top && e1.bottom==e2.bottom);
	}
	
	public void  GetAbsolute ( InstantGuiElementPos parent ,   InstantGuiElementPos relative ,   InstantGuiElementPos offset  ){
		InstantGuiElementPos relativeAbsolute = GetRelativeAbsolute(parent, relative);
		
		left = relativeAbsolute.left + offset.left;
		right = relativeAbsolute.right + offset.right;
		top = relativeAbsolute.top + offset.top;
		bottom = relativeAbsolute.bottom + offset.bottom;
	}
	
	public void  GetOffset ( InstantGuiElementPos parent ,   InstantGuiElementPos relative ,   InstantGuiElementPos absolute  ){
		InstantGuiElementPos relativeAbsolute = GetRelativeAbsolute(parent, relative);
		
		left = absolute.left - relativeAbsolute.left;
		right = absolute.right - relativeAbsolute.right;
		top = absolute.top - relativeAbsolute.top;
		bottom = absolute.bottom - relativeAbsolute.bottom;
		
		//Subtract(relativeAbsolute);
	}
	
	public void  GetRelative ( InstantGuiElementPos parent ,   InstantGuiElementPos absolute ,   InstantGuiElementPos offset  ){
		InstantGuiElementPos relativeAbsolute = new InstantGuiElementPos(absolute);
		relativeAbsolute.Subtract(offset);
		
		left = ((relativeAbsolute.left-parent.left)/(parent.right-parent.left))*100;
		right = ((relativeAbsolute.right-parent.left)/(parent.right-parent.left))*100;
		top = ((relativeAbsolute.top-parent.top)/(parent.bottom-parent.top))*100;
		bottom = ((relativeAbsolute.bottom-parent.top)/(parent.bottom-parent.top))*100;
	}

	public InstantGuiElementPos Clone() 
	{ 
		return (InstantGuiElementPos)this.MemberwiseClone(); 
	}

}

public enum MessageRecievers {none=0, current=1, upwards=2, broadcast=3, gameObject=4, all=5}

[System.Serializable]
public class InstantGuiActivator
{

	public GameObject[] enableObjects = new GameObject[0];
	public GameObject[] disableObjects = new GameObject[0];
	public string message;
	public MessageRecievers messageRecievers = MessageRecievers.upwards;
	public GameObject messageGameObject;
	public bool messageInEditor = false;
	
	public bool  guiDraw; //used in IGInspector
	float guiScrollPos;
	
	public void  Activate ( MonoBehaviour control)
	{
		for (int i=0;i<enableObjects.Length;i++) 
			if (enableObjects[i]!=null)
				enableObjects[i].SetActive(true);
		for (int i=0;i<disableObjects.Length;i++)
			if (disableObjects[i]!=null)
				disableObjects[i].SetActive(false);
		
		if (message!=null && message.Length!=0 && (Application.isPlaying || messageInEditor))
			switch (messageRecievers)
			{
				case MessageRecievers.current: control.gameObject.SendMessage(message, control, SendMessageOptions.DontRequireReceiver); break;
				case MessageRecievers.upwards: control.gameObject.SendMessageUpwards(message, control, SendMessageOptions.DontRequireReceiver); break;
				case MessageRecievers.broadcast: control.gameObject.BroadcastMessage(message, control, SendMessageOptions.DontRequireReceiver); break;
				case MessageRecievers.gameObject: if (messageGameObject!=null) messageGameObject.SendMessage(message, control, SendMessageOptions.DontRequireReceiver); break;
				case MessageRecievers.all: 
				GameObject[] allObjs = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
				for (int i=0;i<allObjs.Length;i++) allObjs[i].SendMessage(message, control, SendMessageOptions.DontRequireReceiver);
				break;
			}
	}
}

[ExecuteInEditMode]
public class InstantGui : MonoBehaviour 
{
	static public float Invert ( float input  ){ return height-input; }
	static public Vector2 Invert ( Vector2 input  ){ return new Vector2(input.x, Invert(input.y)); }
	static public Vector3 Invert ( Vector3 input  ){ return new Vector3(input.x, Invert(input.y), input.z); }
	static public Rect Invert ( Rect input  ){ return new Rect(input.x, height-input.y-input.height, input.width, input.height); }
	
	
	
	static public InstantGuiElement pointed;
	static public InstantGui instance;
	
	public InstantGuiElement element;
	
	static public float scale = 1;

	#if UNITY_EDITOR
	private UnityEditor.EditorWindow gameView; //to get screen size in editor.
	#endif

	static public int width;
	static public int height;
	
	static public int oldScreenWidth;
	static public int oldScreenHeight;

	static public void CreateRoot ()
	{
		GameObject rootobj = new GameObject ("InstantGUI");
		InstantGui.instance = rootobj.AddComponent<InstantGui>();
		InstantGui.instance.element = rootobj.AddComponent<InstantGuiElement>();
		InstantGui.instance.element.relative = new InstantGuiElementPos(0,100,0,100);
		InstantGui.instance.element.offset = new InstantGuiElementPos(0,0,0,0);
		InstantGui.instance.element.lockPosition = true;
	}


	static public void  ForceUpdate (){
		if (!instance) instance = (InstantGui)GameObject.FindObjectOfType(typeof(InstantGui));
		instance.Update();
	}
	
	//Aligning all elements and calculating hover
	public void  Update ()
	{
		//if (updateTime > 0.1f && updateTimeLeft > 0) { updateTimeLeft-=Time.deltaTime; return; }
		//updateTimeLeft = updateTime;
		
		//if (!element) 
		element = GetComponent<InstantGuiElement>();
		pointed = null;
		
		//getting game view size
		#if UNITY_EDITOR
		/*
		if (!gameView)
		{
			System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");
			System.Reflection.MethodInfo GetMainGameView = type.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			gameView = (UnityEditor.EditorWindow)GetMainGameView.Invoke(null,null);
		}
		width = (int)gameView.position.width;
		height = (int)gameView.position.height-16;
		*/
		
		//It is not possible to use Screen.height from an OnInspectorGUI (returns inspector height)
		string[] res = UnityEditor.UnityStats.screenRes.Split('x');
		width = int.Parse(res[0]);
		height = int.Parse(res[1]);
		
		#else
		width = Screen.width;
		height = Screen.height;
		#endif

		Profiler.BeginSample ("CheckChildren");	
		element.CheckChildren(); 
		Profiler.EndSample ();
		
		Profiler.BeginSample ("Align");	
		element.Align();
		Profiler.EndSample ();
		
		Profiler.BeginSample ("Prevent Zero Size");	
		//element.PreventZeroSize();
		Profiler.EndSample ();
		
		Profiler.BeginSample ("Point");	
		element.Point();
		Profiler.EndSample ();
		
		Profiler.BeginSample ("Action");	
		element.Action();
		Profiler.EndSample ();
		
		Profiler.BeginSample ("Style");	
		element.ApplyStyle();
		Profiler.EndSample ();
		
		oldScreenWidth = Screen.width;
		oldScreenHeight = Screen.height;
	}
	
	//selecting element
	#if UNITY_EDITOR
	public void OnGUI ()
	{ 	
		if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
		{
			InstantGuiElement selectedElement = null;
			if (UnityEditor.Selection.activeGameObject!=null) selectedElement = UnityEditor.Selection.activeGameObject.GetComponent<InstantGuiElement>();
			
			if (Event.current.isMouse && Event.current.button == 0)
			{	
				if (!selectedElement) //selecting element only when no element selected. Otherwise it is done from Frame
				{
					GetComponent<InstantGuiElement>().Point(true);
					if (pointed!=null && pointed.gameObject != null) UnityEditor.Selection.activeGameObject = pointed.gameObject;
					selectedElement = pointed;
				}
			}
			
			//editing and drawing frame
			if (selectedElement!=null)
			{
				if (Event.current.isMouse || Event.current.isKey || Event.current.type == EventType.ValidateCommand) InstantGuiFrame.EditFrame(selectedElement);
				if (InstantGuiFrame.drawFrames && Event.current.type == EventType.Repaint) InstantGuiFrame.DrawFrame(selectedElement);
				
				//removing element on delete
				if (Event.current.keyCode == KeyCode.Delete) StartCoroutine(selectedElement.YieldAndDestroy());
			}
			
			//making gui repaint
			if (Event.current.type == EventType.Repaint)
			{
				UnityEditor.EditorUtility.SetDirty(this);
			}
		}
	}
	#endif
	
}