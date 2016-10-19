using UnityEngine;
using System.Collections;

public class CountdownVideoLogTrack : LogTrack {


	public void LogCountdownStarted(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "COUNTDOWN_START");
		}
	}

	public void LogCountdownEnded(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds,"1" + separator +  "COUNTDOWN_END");

		}
	}

}
