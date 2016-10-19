using UnityEngine;
using System.Collections;

public class CountdownVideo : MonoBehaviour {

	private VideoPlay videoQuad;
	public CountdownVideoLogTrack countdownVideoLogTrack;


	//SINGLETON
	private static CountdownVideo _instance;

	public static CountdownVideo Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake () {

		if (_instance != null)
		{
			Debug.Log("Instance already exists!");
			return;
		}
		_instance = this;
		videoQuad = transform.GetChild (0).gameObject.GetComponent<VideoPlay>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void EnableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = true;
	}
	public void DisableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = false;
	}
	public IEnumerator PlayVideo()
	{
		yield return StartCoroutine(videoQuad.PlayVideo (10f));
		yield return null;
	}
}
