using UnityEngine;
using System.Collections;

public class RecallPhaseLogTrack : LogTrack {


	public void LogRecallStarted(){
		if (ExperimentSettings.isLogging) {
			if(ExperimentSettings.practice)
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "PRACTICE_REC_START");
			else
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "REC_START");
		}
	}

	public void LogRecallEnded(){
		if (ExperimentSettings.isLogging) {
			if(ExperimentSettings.practice)
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "PRACTICE_REC_END");
			else
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "REC_END");

		}
	}

}
