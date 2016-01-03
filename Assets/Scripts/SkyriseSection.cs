using UnityEngine;
using System.Collections;

public class SkyriseSection : MonoBehaviour
{
	public Transform preload;
	public Vector3 preloadOffset;
	public float moveToPreloadSpeed = 1.0f;
	
	public bool isCaptured;
	public bool isCapturable = false;
	public bool isScored;
	public bool isScorable = false;
	
	public bool isPreloaded;
	public bool isPreloading;
	public bool isInPlay;
	
	Rigidbody rb;
	ScoreKeeper scoreKeeper;

	float heightPerSkyrise = 0.19f;
	Vector2 redPos = new Vector2(-1.195f, 1.195f);
	Vector2 bluePos = new Vector2(1.195f, -1.195f);
	
	void Start()
	{

	}

	void Awake()
	{
		isCaptured = false;
		isCapturable = false;
		isScored = false;
		isScorable = false;
		isPreloaded = false;
		isPreloading = false;
		isInPlay = false;
		
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		
		preloadOffset = new Vector3 (0f, .1f, 0f);
		scoreKeeper = GameObject.FindWithTag ("Score").GetComponent<ScoreKeeper> ();
	}

	void Update()
	{
		if(!isInPlay)
		{
			if(isPreloading)
			{
				if(!isPreloaded)
				{
					PreloadUpdate();
				}
			}
		}

		if(isCaptured)
		{
			isInPlay = true;
		}
	}
	
	public void Capture()
	{
		rb.isKinematic = true;
		isCaptured = true;
		isCapturable = false;
		FieldElementManager.SetCapturableSkyrise (this);
	}
	
	public void Release()
	{
		rb.isKinematic = false;
		isCaptured = false;

		FieldElementManager.ClearCapturableSkyrise (this);
		
		if(isScorable)
			Score ();
	}

	public void Preload()
	{
		isPreloading = true;
	}
	
	void PreloadUpdate()
	{
		isInPlay = false;
		isPreloaded = false;

		transform.position = Vector3.Lerp(
			transform.position, 
			preload.position + preloadOffset,
			moveToPreloadSpeed * Time.deltaTime);
		
		if((preload.position + preloadOffset - transform.position).magnitude < 0.01)
			isPreloaded = true;
	}
	
	void Score()
	{
		isScored = true;
		isScorable = false;
		isCapturable = false;
		rb.isKinematic = true;

		int numScored = scoreKeeper.GetNumSkyrisesScored ();
		float height = .22f + numScored * heightPerSkyrise;

		transform.position = new Vector3 (
			bluePos.x,
			height,
			bluePos.y);

		scoreKeeper.ScoreSkyriseSection();
	}
	
	void OnTriggerEnter(Collider other)
	{
		//when another skyrise section enters this one
		
		if(isScored)
		{
			if(other.gameObject.tag == "Skyrise Section")
			{
				other.gameObject.GetComponent<SkyriseSection>().isScorable = true;
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(isScored)
		{
			if(other.gameObject.tag == "Skyrise Section")
			{
				other.gameObject.GetComponent<SkyriseSection>().isScorable = false;
			}
		}
	}
}
