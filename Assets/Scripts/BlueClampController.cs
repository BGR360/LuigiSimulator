using UnityEngine;
using System.Collections;

public class BlueClampController : MonoBehaviour {

	Vector3 closedPosition = new Vector3(-0.0013274f, 0.22f, -0.0545183f);
	Vector3 openPosition = new Vector3(-0.0013274f, 0.17f, -0.0545183f);

	public Vector3 localPosition;

	Quaternion closedRotation = Quaternion.Euler (0, 0, 29.16f);
	Quaternion openRotation = Quaternion.Euler (0, 0, -33f);

	XboxInput xInput;

	enum ClampStates
	{
		OPEN,
		CLOSING,
		CLOSED,
		OPENING
	};

	public int clampState;

	public float openingSpeed = 1.0f;
	public float closingSpeed = 1.0f;

	float angleConversion = 875;

	// Use this for initialization
	void Start () 
	{
		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();

		clampState = (int)ClampStates.CLOSED;
	}
	
	// Update is called once per frame
	void Update () {

		switch (clampState) {
		case (int)ClampStates.OPEN:
			transform.localRotation = openRotation;
			transform.localPosition = openPosition;
			if (Input.GetButtonDown ("Skyrise Clamp") || xInput.GetButtonDown ("Skyrise Clamp")){
				clampState = (int)ClampStates.CLOSING;
			}
			break;
			
		case (int)ClampStates.CLOSING:
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, closedRotation, closingSpeed * angleConversion * Time.deltaTime);
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, closedPosition, closingSpeed * Time.deltaTime);
			if (Mathf.Abs (closedPosition.y - transform.localPosition.y) < 0.00001f)
				clampState = (int)ClampStates.CLOSED;
			break;
			
		case (int)ClampStates.CLOSED:
			transform.localRotation = closedRotation;
			transform.localPosition = closedPosition;
			if (Input.GetButtonDown ("Skyrise Clamp") || xInput.GetButtonDown ("Skyrise Clamp")){
				clampState = (int)ClampStates.OPENING;
			}
			break;
			
		case (int)ClampStates.OPENING:
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, openRotation, openingSpeed * angleConversion * Time.deltaTime);
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, openPosition, openingSpeed * Time.deltaTime);
			if (Mathf.Abs (openPosition.y - transform.localPosition.y) < 0.01f)
				clampState = (int)ClampStates.OPEN;
			break;
			
		default:
			break;
		}

		localPosition = transform.localPosition;
	}
}
