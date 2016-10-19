using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BillboardText : MonoBehaviour {

    public TextMesh billboardText;
    bool startCounting = false;
    float timer = 0f;
    private string wordToAssign = "";
    public static int billboardCount = 0;
	// Use this for initialization
	void Start () {
        StartCoroutine("InitiateWordAssignment");
        billboardText.gameObject.SetActive(false);
	
	}

    IEnumerator InitiateWordAssignment()
    {
        while(!WordListGenerator.canSelectWords)
            yield return 0;
        wordToAssign = WordListGenerator.Instance.SelectWords();
        AssignWord();
        yield return null;

    }

    // Update is called once per frame
    void Update () {
	}

	public void OrientationDisplay()
	{
		StartCoroutine("WordDisplay");	
	}
    void OnTriggerEnter(Collider col)
    {
        billboardCount++;
		if(billboardCount>2 && (billboardCount-1)%2==0)
			TrialController.Instance.SwitchStimStatus ();
        if(col.gameObject.tag=="Player")
        {
			col.transform.parent.gameObject.GetComponent<PlayerMovement> ().StartTracking ();
            StartCoroutine("WordDisplay");
        }
    }

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			col.transform.parent.gameObject.GetComponent<PlayerMovement> ().StopTracking ();
		}
	}

	float CalculateJitter()
	{
		float jitterTime = Random.Range (0.75f, 1f);
	    PlayerMovement.Instance.CalculatePlayerSpeed (jitterTime);
		return jitterTime;
	}

    IEnumerator WaitForShortTime(float jitter)
    {
        float currentTime = 0.0f;
        while (currentTime < jitter)
        {
            currentTime += Time.deltaTime;

            //Debug.Log(currentTime.ToString());
            yield return 0;
        }

    }

    IEnumerator WordDisplay()
    {
		if (billboardCount <= 12) {
			float jitTime = CalculateJitter ();
			float waitTime = 1.6f + jitTime;
			UnityEngine.Debug.Log ("displaying word");
			billboardText.gameObject.SetActive (true);
			WordListGenerator.Instance.wordEncodingLogTrack.LogWordTextOn (wordToAssign, BillboardText.billboardCount);
			yield return StartCoroutine (WaitForShortTime (waitTime));
			billboardText.gameObject.SetActive (false);
			WordListGenerator.Instance.wordEncodingLogTrack.LogWordTextOff ();
			yield return StartCoroutine ("InitiateWordAssignment");
		}
        yield return null;

    }

    public void AssignWord()
    {
        billboardText.text = wordToAssign;
    }
}
