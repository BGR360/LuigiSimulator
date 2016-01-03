using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	double animationLength = 17.0;
	double timeAtLastStart;

	// Use this for initialization
	void Start () {
		timeAtLastStart = Time.time;
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).loop = true;
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).Play ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
