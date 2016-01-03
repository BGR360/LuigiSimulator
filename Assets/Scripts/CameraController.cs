using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	private TargetChaser chaser;
	private Vector3 initialPosition;
	private Quaternion initialRotation;

	XboxInput xInput;

	public enum CameraViews
	{
		OBJECTIVE,
		BACK,
		NUM_CAMERA_VIEWS
	};

	private int currentView = 0;

	// Use this for initialization
	void Start () 
	{
		initialPosition = new Vector3 (
			transform.position.x,
			transform.position.y,
			transform.position.z);
		initialRotation = new Quaternion (
			transform.rotation.x,
			transform.rotation.y,
			transform.rotation.z,
			transform.rotation.w);

		chaser = GetComponent<TargetChaser> ();
		chaser.enabled = false;

		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (1) || xInput.GetButtonDown("Camera View")) 
		{
			currentView++;
			if(currentView >= (int)CameraViews.NUM_CAMERA_VIEWS) currentView = 0;
		}

		switch (currentView) 
		{
		case (int)CameraViews.OBJECTIVE:
			chaser.enabled = false;
			transform.position = initialPosition;
			transform.rotation = initialRotation;
			break;

		case (int)CameraViews.BACK:
			chaser.enabled = true;
			break;

		default:
			break;
		}
	}
}
