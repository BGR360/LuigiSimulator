using UnityEngine;
using System.Collections;

public class Forcer : MonoBehaviour {

	public float magnitude;

	private Vector3 force;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = (new Vector3 (0, 0, 0)) - transform.position;
		direction.Normalize ();
		force = magnitude * direction;

		rb.AddForce (force);

//		if (Input.GetButtonDown ("Fire1")) {
//			Reset ();
//		}
	}

	void Reset()
	{
		magnitude = Random.Range (10, 20);

		transform.position = new Vector3 (Random.Range (-2, 2), Random.Range (1, 3), Random.Range(-2, 2));
		transform.rotation = Random.rotation;
	}
}
