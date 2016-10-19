using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public enum Version
	{
		NO_STIM_SESSION, //TODO: change this for your experiment!
		ExpName2,
		ExpName3
	}
	public static Version BuildVersion = Version.NO_STIM_SESSION; //TODO: change this for your experiment!
	public static string VersionNumber = "0.1"; //TODO: change this for your experiment!


	public static bool isGamified=false;

	public static bool isSyncbox = false;
	public static bool isSystem2 = false;

    //recall
    public static int recallTime = 30;
	
	//REPLAY
	public static int replayPadding = 6;

	//SOUNDTRACK
	public static bool isSoundtrack = false;

	//trial variables
	public static int numTestTrials = 25; //IF 50% 2 OBJ, [1obj, counter1, 2a, counter2a, 2b, counter2b, 3, counter3] --> MULTIPLE OF EIGHT
	public static Vector2 trialBlockDistribution = new Vector2 (4, 4); //4 2-item trials, 4 3-item trials
	
	//practice settings
	public static int numTrialsPract = 1;
	public static bool doPracticeTrial = false;


	//JITTER
	public static float randomJitterMin = 0.0f;
	public static float randomJitterMax = 0.2f;



	//DEFAULT OBJECTS & BUFFERS
	public static int numDefaultObjects = 5;

	public static float objectToWallBuffer = 10.0f; //half of the selection diameter.
	public static float objectToObjectBuffer { get { return CalculateObjectToObjectBuffer(); } } //calculated base on min time to drive between objects!

	public static float minDriveTimeBetweenObjects = 0.5f; //half a second driving between objects
	
	public static float specialObjectLifeTime = 1.5f;

	//instructions
	public static float minInitialInstructionsTime = 0.0f; //TODO: change back to 5.0f

	//tilt variables
	public static bool isAvatarTilting = true;
	public static float turnAngleMult = 0.07f;

	//player variables
	public static float driveSpeed = 22;
	public static float avatarSmoothMoveTime = 2.0f;
	

	void Awake(){
		#if !GAMIFIED
		isGamified=false;
		#else
		isGamified=true;
		#endif
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start(){

	}

	public static int GetTotalNumTrials(){
		if (!doPracticeTrial) {
			return numTestTrials;
		} 
		else {
			return numTestTrials + numTrialsPract;
		}
	}

	public static float CalculateObjectToObjectBuffer(){
		float buffer = 0;

		if (Experiment.Instance != null) {

			buffer = driveSpeed * minDriveTimeBetweenObjects; //d = vt

			buffer += Experiment.Instance.objectController.GetMaxDefaultObjectColliderBoundXZ ();

			//Debug.Log ("BUFFER: " + buffer);

		}

		return buffer;
	}

}
