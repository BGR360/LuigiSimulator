using UnityEngine;
using System.Collections;

public class BlockClampController : MonoBehaviour 
{

	public int clampState = 0;
	public bool hasBlock = false;
	public Collider capturedBlock = null;
	public Transform blockTarget;
	public float openingSpeed = 1.0f;
	public float closingSpeed = 0.5f;

	private Quaternion openRotation;
	private Quaternion closedRotation;

	XboxInput xInput;

	enum ClampStates
	{
		OPEN,
		CLOSING,
		CLOSED,
		OPENING
	};

	// Use this for initialization
	void Start () 
	{
		openRotation = Quaternion.Euler (303.37f, 180f, 180f);
		closedRotation = Quaternion.Euler (360f, 180f, 180f);
		blockTarget.localPosition = Vector3.zero;

		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (clampState) {
		case (int)ClampStates.OPEN:
			transform.localRotation = openRotation;
			if (Input.GetButtonDown ("Block Clamp") || xInput.GetButtonDown ("Block Clamp"))
				clampState = (int)ClampStates.CLOSING;
			break;

		case (int)ClampStates.CLOSING:
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, closedRotation, closingSpeed * Time.deltaTime);
			if (Mathf.Abs (closedRotation.eulerAngles.x - transform.localRotation.eulerAngles.x) < 0.1f)
				clampState = (int)ClampStates.CLOSED;
			break;

		case (int)ClampStates.CLOSED:
			transform.localRotation = closedRotation;
			if (Input.GetButtonDown ("Block Clamp") || xInput.GetButtonDown ("Block Clamp"))
				clampState = (int)ClampStates.OPENING;
			break;

		case (int)ClampStates.OPENING:
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, openRotation, openingSpeed * Time.deltaTime);
			if (Mathf.Abs (openRotation.eulerAngles.x - transform.localRotation.eulerAngles.x) < 0.1f)
				clampState = (int)ClampStates.OPEN;
			break;

		default:
			break;
		}

		if (hasBlock) {
			//lock block to the clamp's transform
			capturedBlock.transform.position = blockTarget.position;
			capturedBlock.transform.rotation = blockTarget.rotation;

			if(clampState == (int)ClampStates.OPENING || clampState == (int)ClampStates.OPEN)
			{
				ReleaseBlock ();
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (clampState == (int)ClampStates.CLOSED) {
			if(!hasBlock)
				CaptureBlock (other);
		}
	}

	void CaptureBlock(Collider blockCollider)
	{
		if (blockCollider.tag == "Block") {
			blockCollider.attachedRigidbody.isKinematic = true;
			hasBlock = true;
			capturedBlock = blockCollider;
			blockTarget.localPosition = (capturedBlock.transform.position - blockTarget.position);
			blockTarget.position = new Vector3(
				capturedBlock.transform.position.x,
				capturedBlock.transform.position.y,
				capturedBlock.transform.position.z);
			blockTarget.rotation = capturedBlock.transform.rotation;
		}
	}

	void ReleaseBlock()
	{
		if (capturedBlock != null) {
			hasBlock = false;
			capturedBlock.attachedRigidbody.isKinematic = false;
			capturedBlock = null;
			blockTarget.localPosition = Vector3.zero;
		}
	}
}
