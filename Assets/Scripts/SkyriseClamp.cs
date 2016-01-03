using UnityEngine;
using System.Collections;

[System.Serializable]
public class CopyTransform
{
	public GameObject gameObject;

	public Vector3 position
	{
		get { return gameObject.transform.position; }
	}
	public Quaternion rotation
	{
		get { return gameObject.transform.rotation; }
	}
	
	public CopyTransform(Transform parent)
	{
		gameObject = new GameObject ();
		gameObject.transform.position = new Vector3(
			parent.position.x,
			parent.position.y,
			parent.position.z);
		
		gameObject.transform.rotation = new Quaternion(
			parent.rotation.x,
			parent.rotation.y,
			parent.rotation.z,
			parent.rotation.w);
	}

	public CopyTransform(Vector3 position, Quaternion rotation)
	{
		gameObject = new GameObject ();
		gameObject.transform.position = new Vector3(
			position.x,
			position.y,
			position.z);
		
		gameObject.transform.rotation = new Quaternion(
			rotation.x,
			rotation.y,
			rotation.z,
			rotation.w);
	}
}

public class SkyriseClamp : MonoBehaviour 
{
	public BlueClampController blueClamp;
	public RedClampController redClamp;

	public bool hasSkyriseSection = false;
	SkyriseSection capturedSkyrise = null;
	
	CopyTransform instantTransform; //for when we need to capture a skyrise and keep it with us
	CopyTransform skyriseOffset;

	XboxInput xInput;

	enum ClampStates
	{
		OPEN,
		CLOSING,
		CLOSED,
		OPENING
	};
	
	public int clampState;

	void Start()
	{
		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}
	
	void Update()
	{
		clampState = redClamp.clampState;

		switch(clampState)
		{
		case (int)ClampStates.OPEN:
			if(hasSkyriseSection)
			{
				ReleaseSkyrise(capturedSkyrise);
			}
			break;
			
		case (int)ClampStates.CLOSED:
			if(hasSkyriseSection)
			{
			}
			else
			{
				SkyriseSection sr = FieldElementManager.GetCapturableSkyrise();
				if(sr != null)
				{
					CaptureSkyrise(sr);
				}
			}
			break;
		}
	}
	
	void CaptureSkyrise(SkyriseSection sr)
	{
		sr.Capture();
		capturedSkyrise = sr;
		hasSkyriseSection = true;
		
		instantTransform = new CopyTransform(transform);
		skyriseOffset = new CopyTransform (
			sr.transform.position - instantTransform.position,
			instantTransform.rotation);

		sr.transform.SetParent (transform);
	}
	
	void ReleaseSkyrise(SkyriseSection sr)
	{
		sr.transform.SetParent (null);
		sr.transform.localScale = new Vector3 (1f, 1f, 1f);
		sr.Release();
		capturedSkyrise = null;
		hasSkyriseSection = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Trigger Entered");
		//if a skyrise enters our trigger, it is capturable
		if(clampState == (int) ClampStates.OPEN)
		{
			Debug.Log ("Clamp is open");
			if(other.gameObject.tag == "Skyrise Section")
			{
				SkyriseSection sr = other.gameObject.GetComponent<SkyriseSection>();
				sr.isCapturable = true;
				FieldElementManager.SetCapturableSkyrise(sr);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		Debug.Log ("Trigger Exited");
		//if a skyrise exits our trigger, it is no longer capturable
		if(other.gameObject.tag == "Skyrise Section")
		{
			SkyriseSection sr = other.gameObject.GetComponent<SkyriseSection>();
			sr.isCapturable = false;
			FieldElementManager.ClearCapturableSkyrise(sr);
		}
	}	
}
