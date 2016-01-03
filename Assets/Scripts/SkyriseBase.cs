using UnityEngine;
using System.Collections;

public class SkyriseBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		//when another skyrise section enters this one

		if(other.gameObject.tag == "Skyrise Section")
		{
			other.gameObject.GetComponent<SkyriseSection>().isScorable = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Skyrise Section")
		{
			other.gameObject.GetComponent<SkyriseSection>().isScorable = false;
		}
	}
}
