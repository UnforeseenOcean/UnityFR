using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TCP_Config : MonoBehaviour {

	public static float numSecondsBeforeAlignment = 10.0f;

	public static string HostIPAddress = "192.168.137.200"; //"169.254.50.2" for Mac Pro Desktop.
	public static int ConnectionPort = 8888; //8001 for Mac Pro Desktop communication


	public static char MSG_START = '{';
	public static char MSG_END = '}';

	public static string ExpName { get { return GetExpName (); } }
	public static string SubjectName = ExperimentSettings.currentSubject.name;

	public static float ClockAlignInterval = 60.0f; //this should happen about once a minute

	public enum EventType {
		SUBJECTID,
		EXPNAME,
		VERSION,
		INFO,
		CONTROL,
		DEFINE,
		SESSION,
		PRACTICE,
		TRIAL,
		PHASE,
		DISPLAYON,
		DISPLAYOFF,
		HEARTBEAT,
		ALIGNCLOCK,
		ABORT,
		SYNC,
		SYNCNP,
		SYNCED,
		STATE,
		EXIT
	}

	public enum SessionType{
		CLOSED_STIM,
		OPEN_STIM,
		NO_STIM
	}

	public static SessionType sessionType { get { return GetSessionType (); } }


	void Start(){

	}

	static string GetExpName(){
		return Config.BuildVersion.ToString ();
	}
	
	public static SessionType GetSessionType(){
		switch (Config.BuildVersion) {
		case Config.Version.NO_STIM_SESSION:
			return SessionType.NO_STIM;
		case Config.Version.ExpName2:
			return SessionType.CLOSED_STIM; //could change back to openstim... just use closedstim for now
		case Config.Version.ExpName3:
			return SessionType.CLOSED_STIM;
		}

		return SessionType.NO_STIM;
	}

	//fill in how you see fit!
	public enum DefineStates
	{
		NAVIGATION,
		PAUSED
	}

	public static List<string> GetDefineList(){
		List<string> defineList = new List<string> ();

		DefineStates[] values = (DefineStates[])DefineStates.GetValues(typeof(DefineStates));

		foreach (DefineStates defineState in values)
		{
			defineList.Add(defineState.ToString());
		}

		return defineList;
	}

}
