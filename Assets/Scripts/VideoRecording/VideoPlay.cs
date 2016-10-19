using UnityEngine;
using System.Collections;

public class VideoPlay : MonoBehaviour {

    public MovieTexture movie;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.mainTexture = movie;
        movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public IEnumerator PlayVideo(float waitTime)
    {
        movie.Stop();
        movie.Play();
		yield return StartCoroutine ("WaitForTime", waitTime);
        movie.Stop();
        yield return null;
    }


	IEnumerator WaitForTime(float waitTime)
	{
		float currentTime = 0f;
	//	UnityEngine.Debug.Log ("outside the loop");
		while (currentTime < waitTime) {
			currentTime += Time.fixedDeltaTime;
			//UnityEngine.Debug.Log ("in the loop");
			if (Input.GetKeyDown (KeyCode.A)) {
				Debug.Log ("detected keypress");
				//skipping to the end of the video
				currentTime = 161f;
			}
			else
				yield return 0;
		}
	}
}
