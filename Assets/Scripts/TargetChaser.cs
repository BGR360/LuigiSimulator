using UnityEngine;
using System.Collections;

public class TargetChaser : MonoBehaviour 
{
	public float normalizeSpeed = 1.0f;
	public float maxDegreesDelta = 30.0f;

	private CameraLiftBehaviour liftBehaviour;

	private Transform target;

	// Use this for initialization
	void Start () 
	{
		target = GameObject.FindWithTag ("Camera Target").transform;
		liftBehaviour = GetComponent<CameraLiftBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate()
	{
		transform.position = target.position + transform.rotation * liftBehaviour.GetMoveBack();

		float angleBetween = Quaternion.Angle (transform.rotation, target.rotation);
		angleBetween = Mathf.Clamp (angleBetween, 0, 90);

		float parameter = Mathf.InverseLerp (0, 90, angleBetween);
		float degreesDelta = Mathf.Lerp (0, maxDegreesDelta, parameter);

		transform.rotation = Quaternion.RotateTowards (transform.rotation, target.rotation, degreesDelta);
	}
}
