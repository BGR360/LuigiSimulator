using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	Text text;

	int score;
	int numSkyrisesScored = 0;

	void Start()
	{
		text = GetComponent<Text> ();
	}

	public int GetNumSkyrisesScored()
	{
		return numSkyrisesScored;
	}

	public void ScoreSkyriseSection()
	{
		score += 4;
		numSkyrisesScored++;
	}

	public int GetScore()
	{
		return score;
	}

	void Update()
	{
		text.text = "Score: " + score;
	}
}
