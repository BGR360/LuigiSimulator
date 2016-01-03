using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	bool isHollow;

	GameObject solid;
	GameObject hollow;

	void Awake()
	{
		solid = transform.Find ("Solid Collider").gameObject;
		hollow = transform.Find ("Hollow Colliders").gameObject;
	}

	void Start()
	{
		isHollow = false;
		hollow.SetActive (false);
		solid.SetActive (true);
	}

	public void BecomeHollow()
	{
		isHollow = true;
		solid.SetActive (false);
		hollow.SetActive (true);
	}

	public void BecomeSolid()
	{
		isHollow = false;
		hollow.SetActive (false);
		solid.SetActive (true);
	}
}
