using UnityEngine;
using System.Collections;

public class MathDistractorLogTrack : LogTrack {


	public void LogMathDistractorStarted(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds,"0" + separator +  "DISTRACT_START");
		}
	}

	public void LogMathDistractorEnded(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "DISTRACT_END");

		}
	}

}
