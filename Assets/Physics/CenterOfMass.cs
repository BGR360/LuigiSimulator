using UnityEngine;
using System.Collections;

public class CenterOfMass : MonoBehaviour {

	public Vector3 centerOfMass;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		rb.centerOfMass = transform.localToWorldMatrix.MultiplyVector (centerOfMass);
	}
}
