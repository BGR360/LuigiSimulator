using UnityEngine;
using System.Collections;

public class FieldElementManager : MonoBehaviour 
{
	Vector3 skyriseStartPos = new Vector3(1.778f, 1.677f, -1.192f);
	private static SkyriseSection capturableSkyrise;
	private static SkyriseSection preloadingSkyrise;

	//private static Block CapturableBlock;

	void Start()
	{
		SkyriseSection sr = GameObject.FindWithTag ("Skyrise Section").GetComponent<SkyriseSection> ();
		sr.transform.position = skyriseStartPos;
		preloadingSkyrise = sr;
		sr.Preload ();
	}

	public static void SetCapturableSkyrise(SkyriseSection sr)
	{
		capturableSkyrise = sr;
	}

	public static SkyriseSection GetCapturableSkyrise()
	{
		return capturableSkyrise;
	}

	public static void ClearCapturableSkyrise(SkyriseSection sr)
	{
		if (capturableSkyrise == sr)
			capturableSkyrise = null;
	}

	void Update()
	{
		if (preloadingSkyrise.isInPlay) {
			preloadingSkyrise = Instantiate(
				preloadingSkyrise,
				skyriseStartPos,
				Quaternion.Euler (0.2f, 0f, 0.2f)) as SkyriseSection;
			preloadingSkyrise.transform.localScale = new Vector3(1f, 1f, 1f);
			preloadingSkyrise.Preload ();
		}
	}
}
