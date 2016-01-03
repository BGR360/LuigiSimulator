using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	XboxInput xInput;

	// Use this for initialization
	void Start () {
		//load main menu
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevel == 1 || Application.loadedLevel == 2) {
			xInput = GameObject.FindWithTag("Xbox Input").GetComponent<XboxInput>();
			if(Input.GetButtonDown ("Pause") || xInput.GetButtonDown ("Start"))
			{
				Application.LoadLevel (0);
			}
		}
	}

	public void PlaySkyrise()
	{
		Application.LoadLevel (1);
	}


	public void RobotDemo()
	{
		Application.LoadLevel (2);
	}

	public void MainMenu()
	{
		Application.LoadLevel (0);
	}
}
