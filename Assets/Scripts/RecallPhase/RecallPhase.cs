using UnityEngine;
using System.Collections;

public class RecallPhase : MonoBehaviour {

    public GameObject recallCanvas;
    AudioSource aud;
    public AudioClip beepHigh;
    public AudioClip beepLow;
    public GameObject audioRecorder;
	public RecallPhaseLogTrack recallPhaseLogTrack;
    //SINGLETON
    private static RecallPhase _instance;

    public static RecallPhase Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {

        if (_instance != null)
        {
            Debug.Log("Instance already exists!");
            return;
        }
        _instance = this;
        aud = GetComponent<AudioSource>();

    }

    public void EnableCamera()
    {
        aud.PlayOneShot(beepHigh);
        gameObject.GetComponent<Camera>().enabled = true;
        recallCanvas.SetActive(true);
        
    }

	public void DisableCanvas()
	{
		recallCanvas.SetActive (false);
	}

    public void DisableCamera()
    {
        aud.PlayOneShot(beepLow);
        gameObject.GetComponent<Camera>().enabled = false;
        recallCanvas.SetActive(false);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
