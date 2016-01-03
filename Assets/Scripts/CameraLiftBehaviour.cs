using UnityEngine;
using System.Collections;

public class CameraLiftBehaviour : MonoBehaviour {

	public float backRatio = 1.0f;
	public float upRatio = 1.0f;
	private Vector3 cameraInitial;

	public LiftController lift;

	// Use this for initialization
	void Start () 
	{
		cameraInitial = new Vector3(
			transform.localPosition.x,
			transform.localPosition.y,
			transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public Vector3 GetMoveBack()
	{
		float totalLiftHeight = 0f;
		for (int i = 0; i < LiftController.numStages; i++) 
		{
			totalLiftHeight += lift.stageHeights[i];
		}
		
		return new Vector3 (0, upRatio * totalLiftHeight, backRatio * -totalLiftHeight);
	}
}
