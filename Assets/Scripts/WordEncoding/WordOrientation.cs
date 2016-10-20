using UnityEngine;
using System.Collections;

public class WordOrientation : MonoBehaviour {

    public Vector3 startPos;
	public WordOrientationLogTrack wordOrientationLogTrack;
	public GameObject orientationObject;
	public GameObject retrievalOrientationObject;
	//SINGLETON
	private static WordOrientation _instance;

	public static WordOrientation Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake () {

		if (_instance != null) {
			Debug.Log ("Instance already exists!");
			return;
		}
		_instance = this;
	}
	// Use this for initialization
	void Start () {
		retrievalOrientationObject.SetActive (false);
		orientationObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public IEnumerator InitiateRetrievalOrientation()
	{
		retrievalOrientationObject.SetActive (true);
		yield return new WaitForSeconds (3f);
		retrievalOrientationObject.SetActive (false);
		yield return null;
	}

	public IEnumerator InitiateOrientation()
	{
		orientationObject.SetActive(true);
		yield return new WaitForSeconds(2.4f);
		orientationObject.SetActive(false);
        orientationObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
        yield return null;
	}
}
