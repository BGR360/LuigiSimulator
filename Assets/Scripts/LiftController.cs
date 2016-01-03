using UnityEngine;
using System.Collections;

public class LiftController : MonoBehaviour 
{
	public float maxLiftSpeed = 0.117647059f;
	public float maxLowerSpeed = 0.19764f;
	public static int numStages = 3;
	public float[] maxHeightsPerStage = new float[numStages];

	public float[] stageHeights = new float[numStages];
	private Transform[] stageInitials = new Transform[numStages];
	public Transform[] stages = new Transform[numStages];

	XboxInput xInput;

	void Start()
	{
		stageHeights = new float[numStages];

		for (int i = 0; i < numStages; i++) 
		{
			stageInitials[i] = stages[i];
		}

		xInput = GameObject.FindWithTag ("Xbox Input").GetComponent<XboxInput> ();
	}

	void Update()
	{
		float lift = Input.GetAxis ("Lift");

		if (xInput.IsWorking ()) {
			lift = xInput.GetAxis("Lift");
		}

		float deltaHeight = (lift > 0)? maxLiftSpeed * lift * Time.deltaTime : maxLowerSpeed * lift * Time.deltaTime;
		float stageDeltaHeight = 0f;

		for(int i = 0; i < numStages; i++)
		{
			stageDeltaHeight = Mathf.Clamp (stageHeights[i] + deltaHeight, 0, maxHeightsPerStage[i]) - stageHeights[i];
			deltaHeight -= stageDeltaHeight;

			stageHeights[i] += stageDeltaHeight;
			stages[i].position += new Vector3(0, stageDeltaHeight, 0);
		}
	}
}
