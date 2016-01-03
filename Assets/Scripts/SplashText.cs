using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashText : MonoBehaviour 
{

	public float outZ = 1.0f;
	public float outScale = 1.5f;
	public float oscillateSpeed = 1.0f;

	double idleTime = 60.0;
	double timeAtLastAction;

	Text splashText;
	bool comingOut = true;

	public string[] splashTexts;

	Vector3 outPos;
	Vector3 inPos;

	// Use this for initialization
	void Start () 
	{
		splashText = GetComponent<Text> ();

		outPos = transform.localPosition + new Vector3 (0, 0, -outZ);
		inPos = transform.localPosition + new Vector3 (0, 0, outZ);

		Show ();
		NewSplashText ();

		timeAtLastAction = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (1)) {
			NewSplashText ();
		}

		if (comingOut) {
			float distance = outPos.z - transform.localPosition.z;
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, outPos, oscillateSpeed * Time.deltaTime);
			if(Mathf.Abs (distance) < 0.01)
				comingOut = false;
		} else {
			float distance = inPos.z - transform.localPosition.z;
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, inPos, oscillateSpeed * Time.deltaTime);
			if(Mathf.Abs (distance) < 0.01)
				comingOut = true;
		}

		if (Time.time - timeAtLastAction > idleTime) {
			NewSplashText ();
		}
	}

	public void NewSplashText()
	{
		int random = Random.Range (0, splashTexts.Length);
		splashText.text = splashTexts [random];
		timeAtLastAction = Time.time;
	}

	public void Show()
	{
		splashText.enabled = true;
	}

	public void Hide()
	{
		splashText.enabled = false;
	}
}
