using UnityEngine;
using System.Collections;

public class TrialController : MonoBehaviour {


    Experiment exp { get { return Experiment.Instance; } }

    public enum TrialState{
		countdownVideo,
		wordEncoding,
		mathDistractor,
		recall
	}
	public TrialState currentState = TrialState.countdownVideo;
	public TrialLogTrack trialLogTrack;

	//hardware connection
	bool isConnectingToHardware = false;

	//paused?!
	public static bool isPaused = false;

    //SINGLETON
    private static TrialController _instance;

    public static TrialController Instance
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

    }
    void Start() {
		CountdownVideo.Instance.DisableCamera ();
        PlayerMovement.Instance.DisableCamera();
        MathDistractor.Instance.gameObject.GetComponent<Camera>().enabled = false;
        MathDistractor.Instance.mathCanvas.SetActive(false);
        RecallPhase.Instance.gameObject.GetComponent<Camera>().enabled = false;
        RecallPhase.Instance.recallCanvas.SetActive(false);
      //  RecallPhase.Instance.DisableCamera();

    }

    // Update is called once per frame
    void Update() {

    }
    IEnumerator PlayCountdown()
    {
        UnityEngine.Debug.Log("playing countdown");
		CountdownVideo.Instance.countdownVideoLogTrack.LogCountdownStarted ();
		CountdownVideo.Instance.EnableCamera ();
		yield return StartCoroutine(CountdownVideo.Instance.PlayVideo());
        UnityEngine.Debug.Log("finished playing video");
		CountdownVideo.Instance.DisableCamera ();
		CountdownVideo.Instance.countdownVideoLogTrack.LogCountdownEnded ();
        yield return null;
    }

	IEnumerator StartWordOrientation()
	{
		WordOrientation.Instance.wordOrientationLogTrack.LogOrientationStarted ();
		yield return StartCoroutine(WordOrientation.Instance.InitiateOrientation ());
		WordOrientation.Instance.wordOrientationLogTrack.LogOrientationStopped();
		yield return null;
	}

    IEnumerator RunWordEncoding()
    {
        UnityEngine.Debug.Log("running word encoding");
        PlayerMovement.Instance.EnableCamera();
        yield return StartCoroutine("WaitForWordEncoding");
        PlayerMovement.Instance.DisableCamera();
        yield return null;
    }

    IEnumerator WaitForWordEncoding()
    {
        while (BillboardText.billboardCount <=12)
        {
            yield return 0;
        }
    }

	public void SwitchStimStatus()
	{
		Debug.Log ("attempting to change stim from: " + ExperimentSettings.shouldStim);
		ExperimentSettings.shouldStim = !ExperimentSettings.shouldStim;
		Debug.Log ("to : " + ExperimentSettings.shouldStim);
	}
    IEnumerator RunMathDistractor()
    {
		MathDistractor.Instance.mathDistractorLogTrack.LogMathDistractorStarted ();
		MathDistractor.Instance.EnableCamera ();
        yield return StartCoroutine(MathDistractor.Instance.RunMathDistractor());
		MathDistractor.Instance.DisableCamera ();
		MathDistractor.Instance.mathDistractorLogTrack.LogMathDistractorEnded ();
        yield return null;
    }

	IEnumerator StartRetrievalOrientation()
	{

		WordOrientation.Instance.wordOrientationLogTrack.LogRetrievalOrientationStarted ();
		yield return StartCoroutine (WordOrientation.Instance.InitiateRetrievalOrientation ());
		yield return null;
	}

    IEnumerator RunRecall()
    {
		RecallPhase.Instance.recallPhaseLogTrack.LogRecallStarted ();
        string fileName = SceneController.Instance.trialCount.ToString();
        RecallPhase.Instance.EnableCamera();
        yield return StartCoroutine(exp.audioRec.Record(exp.sessionDirectory + "audio", fileName, Config.recallTime));
      //  yield return new WaitForSeconds(30f);
        RecallPhase.Instance.DisableCamera();
		RecallPhase.Instance.recallPhaseLogTrack.LogRecallEnded ();
        yield return null;
    }
    public IEnumerator RunTrial()
    {
        //change environment
        exp.environmentController.ChangeEnvironment();
        trialLogTrack.Log(SceneController.Instance.GetTrialCount ());
        UnityEngine.Debug.Log("running trial");
        //run countdown
      //  yield return StartCoroutine("PlayCountdown");
		//run word orientation
	//	yield return StartCoroutine("StartWordOrientation");
        //run the word encoding phase
       // yield return StartCoroutine("RunWordEncoding");
        //run math distractor phase
        yield return StartCoroutine("RunMathDistractor");
        //run retrieval orientation
       //	yield return StartCoroutine("StartRetrievalOrientation");
        //run recall phase
       // yield return StartCoroutine("RunRecall");
        yield return null;
    }
}
