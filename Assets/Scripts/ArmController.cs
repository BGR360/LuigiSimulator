using UnityEngine;
using System.Collections;

public class ArmController : MonoBehaviour 
{
	public float armSpeed = 10f;	//degrees per second
	public float leftLimit = 310f;
	public float rightLimit = 50f;

	public float arm;
	public Vector3 angle;

	Quaternion leftLimitQ;
	Quaternion rightLimitQ;

	XboxInput xInput;

	// Use this for initialization
	void Start () 
	{
		leftLimitQ = Quaternion.Euler (0f, 0f, leftLimit);
		rightLimitQ = Quaternion.Euler (0f, 0f, rightLimit);

		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		arm = Input.GetAxis ("Arm");

		if (xInput.IsWorking ()) {
			arm = xInput.GetAxis ("Arm");
		}

		transform.Rotate (new Vector3 (0f, 0f, armSpeed * arm * Time.deltaTime));

		if (transform.localRotation.eulerAngles.z > 180) {
			transform.localRotation = Quaternion.Euler (
				transform.localRotation.eulerAngles.x,
				transform.localRotation.eulerAngles.y,
				Mathf.Clamp (transform.localRotation.eulerAngles.z, leftLimit, 360f));
		}
		if (transform.localRotation.eulerAngles.z < 180 && transform.localRotation.eulerAngles.z > 0) {
			transform.localRotation = Quaternion.Euler (
				transform.localRotation.eulerAngles.x,
				transform.localRotation.eulerAngles.y,
				Mathf.Clamp (transform.localRotation.eulerAngles.z, 0f, rightLimit));
		}

//		traansform.localRotation = Quaternion.Euler (
//			transform.localRotation.eulerAngles.x,
//			transform.localRotation.eulerAngles.y,
//			Mathf.Clamp (transform.localRotation.eulerAngles.z, rightLimit, leftLimit));

		angle = transform.localRotation.eulerAngles;
	}
}
