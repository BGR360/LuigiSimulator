using UnityEngine;
using System.Collections;

[System.Serializable]
public class Drive
{
	public MecanumWheel frontRight;
	public MecanumWheel frontLeft;
	public MecanumWheel backRight;
	public MecanumWheel backLeft;

	public GameObject drive;

	public Vector3 GetNetVelocity ()
	{
		return frontRight.GetVelocity () +
			frontLeft.GetVelocity () +
			backRight.GetVelocity () +
			backLeft.GetVelocity (); 
	}

	public float GetNetAngularVelocity ()
	{
		Vector3 rightVelocity = frontRight.GetVelocity () + backRight.GetVelocity ();
		Vector3 leftVelocity = frontLeft.GetVelocity () + backLeft.GetVelocity ();

		Vector3 radial = drive.transform.rotation * new Vector3 (0, 0, 1);

		float rightTorque = Vector3.Dot (radial, -rightVelocity);
		float leftTorque = Vector3.Dot (radial, leftVelocity);

		return rightTorque + leftTorque;
	}
}

public class LuigiController : MonoBehaviour 
{

	public float forwardSpeed;
	public float translationalSpeed;
	public float angularSpeed;

	public Drive drive;

	public Vector3 velocity;
	public float angularVelocity;

	XboxInput xInput;


	// Use this for initialization
	void Start () 
	{
		velocity = new Vector3 (0, 0, 0);
		angularVelocity = 0.0f;

		drive.drive = GameObject.FindWithTag ("Drive");

		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}

	// Update is called once per frame
	void Update () 
	{
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		float rotate = Input.GetAxis ("Rotate");

		if (xInput.IsWorking ()) {
			vertical = xInput.GetAxis ("Vertical");
			horizontal = xInput.GetAxis ("Horizontal");
			rotate = xInput.GetAxis ("Rotate");
		}

		drive.frontRight.motorValue = 127f * (vertical - rotate - horizontal);
		drive.frontLeft.motorValue = 127f * (vertical + rotate + horizontal);
		drive.backRight.motorValue = -127f * (vertical - rotate + horizontal);
		drive.backLeft.motorValue = -127f * (vertical + rotate - horizontal);

		angularVelocity = angularSpeed * rotate;
	}

	void FixedUpdate()
	{
		velocity = drive.GetNetVelocity ();
		//angularVelocity = drive.GetNetAngularVelocity ();
	
		transform.Translate (velocity * Time.deltaTime);
		transform.Rotate (0, angularVelocity * Time.deltaTime, 0);
	}

}
