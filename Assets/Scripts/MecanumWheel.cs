using UnityEngine;
using System.Collections;

public class MecanumWheel : MonoBehaviour 
{
	public static float maxSpeed = 0.1f;
	public float visibleSpinConversionFactor = 150.0f;

	public Vector3 direction;
	public float motorValue;
	public float speed;

	private GameObject visibleWheel;

	// Use this for initialization
	void Start () 
	{
		speed = 0;
		visibleWheel = transform.Find ("Visible Wheel").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		motorValue = Mathf.Clamp (motorValue, -127, 127);
		speed = LookupSpeedCurve (motorValue);

		visibleWheel.transform.Rotate (0, speed * visibleSpinConversionFactor * Time.deltaTime, 0);
	}

	public Vector3 GetVelocity()
	{
		Vector3 temp = new Vector3 (0, 0, speed * maxSpeed);
		temp = Quaternion.Euler (direction.x, direction.y, direction.z) * temp;
		return transform.localRotation * temp;
	}

	private float LookupSpeedCurve(float motorValue)
	{
		return (float) motorValue / 127f;
	}
}
