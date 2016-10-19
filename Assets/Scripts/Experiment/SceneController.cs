using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour { //there can be a separate scene controller in each scene


	//SINGLETON
	private static SceneController _instance;
	public static SceneController Instance{
		get{
			return _instance;
		}
	}

	public int trialCount=0;

	void Awake(){
		if (_instance != null) {
			Debug.Log("Instance already exists!");
			Destroy(transform.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad (gameObject);
	}


	// Use this for initialization
	void Start () {

	}


	// Update is called once per frame
	void Update () {

	}

	public int GetTrialCount()
	{
		return trialCount;
	}

	void IncrementTrial()
	{
		trialCount++;
	}


	public void ResetTrials()
	{

		BillboardText.billboardCount = 0;
	}

	public IEnumerator RunExperiment()
	{
		Debug.Log ("running experiment");
		//run instructions video
		//DISABLED INSTRUCTIONS UNTIL WE HAVE A NEW INSTRUCTION VIDEO
		//yield return StartCoroutine("PlayInstructions");

		//do mic test

		//generate word list
		yield return StartCoroutine(WordListGenerator.Instance.GenerateWordList());

		//reset the trial just in case
		ResetTrials();
		//do practice test
		ExperimentSettings.practice=true;
		yield return StartCoroutine(TrialController.Instance.RunTrial());
		ExperimentSettings.practice = false;
		//regenerate the word list again
		yield return StartCoroutine(WordListGenerator.Instance.GenerateWordList());
		while (trialCount < 25)
		{
			IncrementTrial ();
			ResetTrials();
			if (TrialController.Instance != null)
				yield return StartCoroutine(TrialController.Instance.RunTrial());

			else
				Debug.Log("there is no trial controller");
			yield return 0;
		}
		yield return null;
	}


	IEnumerator PlayInstructions()
	{
		Experiment.Instance.CreateSessionStartedFile ();
		UnityEngine.Debug.Log("playing instructions");
		InstructionsController.Instance.instructionVideoLogTrack.LogInstructionVideoStarted();
		InstructionsController.Instance.EnableCamera ();
		yield return StartCoroutine(InstructionsController.Instance.PlayInstructions());
		UnityEngine.Debug.Log("finished playing instructions");
		InstructionsController.Instance.DisableCamera ();
		InstructionsController.Instance.instructionVideoLogTrack.LogInstructionVideoStopped ();
		yield return null;
	}


	public void LoadMainMenu(){
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		Debug.Log("loading main menu!");
		SubjectReaderWriter.Instance.RecordSubjects();
		Application.LoadLevel(0);
	}

	public void LoadExperiment(){
		//should be no new data to record for the subject
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		if (ExperimentSettings.currentSubject != null) {
			LoadExperimentLevel();
		} 
		else if (ExperimentSettings.isReplay) {
			Debug.Log ("loading experiment!");
			Application.LoadLevel (1);
		}
		else if (ExperimentSettings.Instance.isRelease){ //no subject, not replay, is pilot
			if(ExperimentSettings.currentSubject == null){
				ExperimentSettings.Instance.subjectSelectionController.SendMessage("AddNewSubject");
				if(ExperimentSettings.currentSubject != null){
					LoadExperimentLevel();
				}
			}
		}
	}

	public void ReturnToMainMenu()
	{
		Application.LoadLevel (0);
	}

	void LoadExperimentLevel(){
		if (ExperimentSettings.currentSubject.trials < Config.GetTotalNumTrials ()) {
			Debug.Log ("loading experiment!");
			Application.LoadLevel (1);
		} else {
			Debug.Log ("Subject has already finished all blocks! Loading end menu.");
			Application.LoadLevel (2);
		}
	}

	public void LoadEndMenu(){
        /*
        if (Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}
    */
		//SubjectReaderWriter.Instance.RecordSubjects();
		Debug.Log("loading end menu!");
		Application.LoadLevel(2);
	}

	public void Quit(){
		SubjectReaderWriter.Instance.RecordSubjects();
		Application.Quit();
	}

	void OnApplicationQuit(){
		Debug.Log("On Application Quit!");
		SubjectReaderWriter.Instance.RecordSubjects();
	}
}
