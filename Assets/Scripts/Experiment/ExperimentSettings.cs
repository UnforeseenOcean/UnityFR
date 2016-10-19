using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ExperimentSettings : MonoBehaviour {

    public int trialCount = 0;
	public static bool practice=false;

	//Experiment exp { get { return Experiment.Instance; } }

	public bool isRelease { get { return GetIsRelease (); } }
	public static bool shouldStim=false;

	private static Subject _currentSubject;

	public static Subject currentSubject{ 
		get{ return _currentSubject; } 
		set{ 
			_currentSubject = value;
			//fileName = "TextFiles/" + _currentSubject.name + "Log.txt";
		}
	}

	//subject selection controller
	public SubjectSelectionController subjectSelectionController;


	public Image micTestIndicator;

	public GameObject nonPilotOptions;


	//LOGGING
	public static string defaultLoggingPath = ""; //SET IN RESETDEFAULTLOGGINGPATH();
	//string DB3Folder = "/" + Config.BuildVersion.ToString() + "/";
	//public Text defaultLoggingPathDisplay;
	//public InputField loggingPathInputField;



	//TOGGLES
	public static bool isOculus = false;
	public static bool isReplay = false;
	public static bool isLogging = true; //if not in replay mode, should log things! or can be toggled off in main menu.

	public Toggle oculusToggle; //only exists in main menu -- make sure to null check
	public Toggle loggingToggle; //only exists in main menu -- make sure to null check

	public Text gamifiedText;


	bool isWeb = false;

	//SINGLETON
	private static ExperimentSettings _instance;

	public static ExperimentSettings Instance{
		get{
			return _instance;
		}
	}

	void Awake(){

		if (_instance != null) {
			Debug.Log("Instance already exists!");
			Destroy(transform.gameObject);
			return;
		}
		_instance = this;
		DoMicTest ();
		CheckGamifiedStatus ();
	}
	// Update is called once per frame
	void Update () {
	
	}

	bool GetIsRelease(){
		if (nonPilotOptions.activeSelf) {
			return false;
		}
		return true;
	}
	public void SetReplayTrue(){
		isReplay = true;
		isLogging = false;
		loggingToggle.isOn = false;
	}


	public void SetReplayFalse(){
		isReplay = false;
		//shouldLog = true;
	}

	void CheckGamifiedStatus()
	{
		#if GAMIFIED
			gamifiedText.text = "(GAMIFIED)";
	#else
			gamifiedText.text = " (VANILLA)";
		#endif
			
	}

	public void SetLogging(){
		if(isReplay){
			isLogging = false;
		}
		else{
			if(loggingToggle){
				isLogging = loggingToggle.isOn;
				Debug.Log("should log?: " + isLogging);
			}
		}

	}

	public void SetOculus(){
		if(oculusToggle){
			isOculus = oculusToggle.isOn;
		}
	}

	void DoMicTest(){
		if (micTestIndicator != null) {
			if (AudioRecorder.CheckForRecordingDevice ()) {
				micTestIndicator.color = Color.green;
			} else {
				micTestIndicator.color = Color.red;
			}
		}
	}


	// Use this for initialization
	void Start () {

	//	StartCoroutine(WordListGenerator.Instance.GenerateWordList());
		//StartCoroutine("RunExperiment");

	}

}
