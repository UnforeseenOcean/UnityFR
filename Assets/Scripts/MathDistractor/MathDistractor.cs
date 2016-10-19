using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MathDistractor : MonoBehaviour
{


    public Text number1;
    public Text number2;
    public Text number3;
	public Text answer;

	//FOR gamified prototype
	public Text protonumber1;
	public Text protonumber2;
	public Text protonumber3;
    public Text answer1;
	public Text answer2;
	public Text answer3;
	private int firstAnswerOptionInt;
	private int secondAnswerOptionInt;
	private int thirdAnswerOptionInt;
	public Transform questionTarget;
	public float speed=10f;

    private int firstRandInt;
    private int secondRandInt;
    private int thirdRandInt;
    private int correctAnswer;
    private string[] currentAnswer = new string[2];
    private int answerIndex = 0;
    private bool allowMathDistractor = false;
    private bool shouldGenerateNewProblem = false;
    public GameObject mathCanvas;
	public GameObject mathGamifiedCanvas;
	public MathDistractorLogTrack mathDistractorLogTrack;
	public GameObject segments;
	public GameObject mathBlock;
	public Vector3 mathBlockSpawnPos;
    public Vector3 finalPos;

    //for 3d math distractor
    public Vector3 mathQuestionSpawnPos;
    GameObject currentQuestion;
    public GameObject mathQuestionPrefab;

    private string selectedDirection="";
	public GameObject mainSegments;
	//traffic lights
	public TrafficLights leftTraffic;
	public TrafficLights rightTraffic;
	public TrafficLights straightTraffic;


	private Vector3 originalSegmentPosition;
	GameObject currentBlock;
    //SINGLETON
    private static MathDistractor _instance;

    public static MathDistractor Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
		
		originalSegmentPosition = mainSegments.transform.localPosition;
        if (_instance != null)
        {
            Debug.Log("Instance already exists!");
            return;
        }
        _instance = this;
	}

	public void EnableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = true;
		if (!Config.isGamified) {
			mathCanvas.SetActive (true);
			mainSegments.SetActive (false);
            ResetAnswer();
			StartCoroutine ("ResetLights");
			mathGamifiedCanvas.SetActive (false);
		} else {
			//mathGamifiedCanvas.SetActive (true);
			//mainSegments.SetActive (true);
			mathCanvas.SetActive (false);
		}
	}

	public void DisableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = false;
		if (!Config.isGamified) {
			mathCanvas.SetActive (false);
		} else {
            if(currentQuestion!=null)
            {
                currentQuestion.GetComponent<MathQuestionScript>().Destroy();
            }
			//mathGamifiedCanvas.SetActive (false);
		}
	}
    
    void GenerateNewMathProblem()
    {
        firstRandInt = Random.Range(0, 10);
        secondRandInt = Random.Range(0, 10);
        thirdRandInt = Random.Range(0, 10);
		correctAnswer = firstRandInt + secondRandInt + thirdRandInt;
			Debug.Log ("NOT GAMIFIED");
			number1.text = firstRandInt.ToString ();
			number2.text = secondRandInt.ToString ();
			number3.text = thirdRandInt.ToString ();
        shouldGenerateNewProblem = false;
    }

	void AssignAnswerOptions()
	{

		int rand = Random.Range (0, 3);
		switch (rand) {

		case 0:
			firstAnswerOptionInt = correctAnswer;
			secondAnswerOptionInt = Random.Range (0, 9);
			thirdAnswerOptionInt = Random.Range (0, 9);
			break;
		case 1:
			firstAnswerOptionInt = Random.Range (0, 9);
			secondAnswerOptionInt = correctAnswer;
			thirdAnswerOptionInt = Random.Range (0, 9);
			break;
		case 2:
			firstAnswerOptionInt = Random.Range (0, 9);
			secondAnswerOptionInt = Random.Range (0, 9);
			thirdAnswerOptionInt = correctAnswer;
			break;
		}
	
	}

    IEnumerator CheckAnswers()
    {
        correctAnswer = firstRandInt + secondRandInt + thirdRandInt;
        bool isAnswerCorrect = false;
        if(!Config.isGamified)
        {
            isAnswerCorrect = (int.Parse(answer.text) == correctAnswer);
        }
        else
        {
            isAnswerCorrect = (int.Parse(currentQuestion.GetComponent<MathQuestionScript>().answer.text)== correctAnswer);
        }
        if(isAnswerCorrect)
        {
            Debug.Log("CORRECT ANSWER");
            if(!Config.isGamified)
            {
                
            }
            else
            {

                yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().CorrectAnswer());
            }
        }
        else
        {
            Debug.Log("WRONG ANSWER");
            if (!Config.isGamified)
            {

            }
            else
            {

               yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().WrongAnswer());
            }
        }
        ResetAnswer();
        yield return null;
    }
    void ResetAnswer()
    {
        if (!Config.isGamified)
        {
            answer.text = "";
            currentAnswer[0] = "";
            currentAnswer[1] = "";
            answerIndex = 0;
        }
        else
        {
            currentAnswer[0] = "";
            currentAnswer[1] = "";
            answerIndex = 0;

        }

        }
    // Use this for initialization
    void Start()
	{
			currentAnswer [0] = "";
			currentAnswer [1] = "";
			answerIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				AppendToAnswer ("0");
			} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
				AppendToAnswer ("1");
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				AppendToAnswer ("2");
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				AppendToAnswer ("3");
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				AppendToAnswer ("4");
			} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
				AppendToAnswer ("5");
			} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
				AppendToAnswer ("6");
			} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
				AppendToAnswer ("7");
			} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
				AppendToAnswer ("8");
			} else if (Input.GetKeyDown (KeyCode.Alpha9)) {
				AppendToAnswer ("9");
			} else if (Input.GetKeyDown (KeyCode.Backspace)) {
				DeleteDigit ();
			}
    }


	void InstantiateMathBlock()
	{
        currentQuestion = Instantiate(mathQuestionPrefab, mathQuestionSpawnPos, Quaternion.identity) as GameObject;
        currentQuestion.GetComponent<MathQuestionScript>().AssignNumbers(firstRandInt, secondRandInt, thirdRandInt);
    }

	/// <summary>
	/// MAIN LOGIC OF THE MATH DISTRACTOR
	/// </summary>
	/// <returns>The math problems.</returns>
	IEnumerator DisplayMathProblems()
	{
		shouldGenerateNewProblem = true;
		while(allowMathDistractor)
		{
			if (!Config.isGamified) {
				if (shouldGenerateNewProblem)
					GenerateNewMathProblem ();
				if (Input.GetKeyDown (KeyCode.Return)) {
					yield return StartCoroutine("CheckAnswers");
					shouldGenerateNewProblem = true;
				}
			} else {
                if (shouldGenerateNewProblem)
                {
                    GenerateNewMathProblem();
                    InstantiateMathBlock();
                    yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().ApproachCamera());
                    shouldGenerateNewProblem = false;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    yield return StartCoroutine("CheckAnswers");
                    shouldGenerateNewProblem = true;
                }
                //ResetMathBlock ();
                //yield return StartCoroutine ("ApproachQuestion");
                //segments.GetComponent<Animator> ().enabled = false;
                //yield return StartCoroutine ("WaitForSubmit");
                //shouldGenerateNewProblem = true;

            }
			yield return 0;
		}
		yield return null;
	}

	IEnumerator ApproachQuestion()
	{
        float timer = 0f;
        float timePercent = 0f;
       /* while(Vector3.Distance(questionTarget.position,transform.position)>5f)
        {
            questionTarget.Translate(-Vector3.forward * speed);
            yield return 0;
        }
        */
		while (timer<1f) {
            timer += Time.deltaTime;
            timePercent = timer / 1f;
			questionTarget.localPosition= Vector3.Lerp(mathBlockSpawnPos,finalPos,timePercent);

			yield return 0;
		}
		yield return null;
	}

	IEnumerator WaitForSubmit()
	{	
		bool hasPressedButton = false;
		if (!Config.isGamified) {
			while (!hasPressedButton) {
				if (Input.GetKeyDown(KeyCode.Return)) {
					hasPressedButton = true;
					Debug.Log ("PRESSED BUTTON");
				}
				yield return 0;
			}
		} else {
			while (!hasPressedButton) {
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					hasPressedButton = true;
					Debug.Log ("go left");
					selectedDirection = "Left";
					yield return StartCoroutine ("ShiftLeft");

				} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
					hasPressedButton = true;
					Debug.Log ("go straight");
					selectedDirection = "Straight";
					yield return StartCoroutine("ShiftStraight");
				}
				else if(Input.GetKeyDown(KeyCode.RightArrow))
				{
					hasPressedButton = true;
					Debug.Log ("go right");
					selectedDirection = "Right";
					yield return StartCoroutine ("ShiftRight");
				}
				yield return 0;
			}
		}
		yield return null;
	}

	IEnumerator ShiftLeft()
	{
		Debug.Log ("shifting left");
		yield return StartCoroutine ("StrafeLeft");
		Debug.Log ("moving towards left");
		yield return StartCoroutine ("MoveTowards");
		yield return StartCoroutine ("WaitForAnswerCheck", firstAnswerOptionInt);
		yield return null;
	}

	IEnumerator StrafeLeft()
	{
		Debug.Log ("strafing left");
		float timer = 0f;
		while(timer < 1f) {
			timer += Time.deltaTime;
			float timePercent = timer / 1f;
			mainSegments.transform.localPosition = Vector3.Lerp (mainSegments.transform.localPosition, new Vector3 (5.2f, mainSegments.transform.localPosition.y, mainSegments.transform.localPosition.z), timePercent);
			questionTarget.localPosition = Vector3.Lerp (questionTarget.localPosition, new Vector3 (4.41f, -9.29f,-951f), timePercent);
			yield return 0;
		}
		yield return null;		
	}

	IEnumerator MoveTowards()
	{
		Debug.Log ("moving towards");
		float timer = 0f;
		//segments.GetComponent<Animator> ().enabled = true;
		//yield return StartCoroutine ("WaitForSubmit");
		while(timer < 1.3f) {
			//Debug.Log ("the time is : " + timer);
//			if(timer>0.9f)
//				segments.GetComponent<Animator> ().enabled = false;
			timer += Time.deltaTime;
			float timePercent = timer / 1.3f;
			Debug.Log ("in timer loop");
			questionTarget.localPosition=Vector3.Lerp (questionTarget.localPosition, new Vector3 (questionTarget.localPosition.x, -8.69f,-963.2f), timePercent);
			yield return 0;
		}

		yield return null;
	}


	IEnumerator ShiftStraight()
	{
		Debug.Log ("moving straight ahead");
		yield return StartCoroutine ("MoveTowards");
		yield return StartCoroutine ("WaitForAnswerCheck",secondAnswerOptionInt);
		yield return null;
	}

	IEnumerator ShiftRight()
	{
		Debug.Log ("shifting right");
		yield return StartCoroutine ("StrafeRight");
		Debug.Log ("moving towards right");
		yield return StartCoroutine ("MoveTowards");
		yield return StartCoroutine ("WaitForAnswerCheck",thirdAnswerOptionInt);
		yield return null;
	}

	IEnumerator StrafeRight()
	{
		Debug.Log ("strafing right");
		float timer = 0f;

		while(timer < 1f) {
			timer += Time.deltaTime;
			float timePercent = timer / 1f;
			mainSegments.transform.localPosition = Vector3.Lerp (mainSegments.transform.localPosition, new Vector3 (-5.2f, mainSegments.transform.localPosition.y, mainSegments.transform.localPosition.z), timePercent);
			questionTarget.localPosition = Vector3.Lerp (questionTarget.localPosition, new Vector3 (-2.41f, -9.29f,-951f), timePercent);
			yield return 0;
		}
	}


	IEnumerator WaitForAnswerCheck(int answer)
	{
		if (answer == correctAnswer) {
			Debug.Log ("correct answer");
			yield return StartCoroutine ("ShowGreenLight");

		} else {
			Debug.Log ("wrong answer");
			yield return StartCoroutine ("ShowRedLight");
		}
		yield return StartCoroutine ("RaiseBarrier");
		yield return StartCoroutine ("MoveBeyondBarrier");
		StartCoroutine ("ResetLights");
		yield return null;
	}

	IEnumerator ShowGreenLight()
	{
		Debug.Log ("showing green light");
		if (selectedDirection == "Left") {
			leftTraffic.CorrectAnswer ();
		} else if (selectedDirection == "Straight") {
			straightTraffic.CorrectAnswer ();
		} else if (selectedDirection == "Right") {
			rightTraffic.CorrectAnswer ();
		}
		yield return null;
	}

	IEnumerator ShowRedLight()
	{
		Debug.Log ("showing red light");
		if (selectedDirection == "Left") {
			leftTraffic.WrongAnswer ();
		} else if (selectedDirection == "Straight") {
			straightTraffic.WrongAnswer ();
		} else if (selectedDirection == "Right") {
			rightTraffic.WrongAnswer ();
		}
		yield return null;
	}

	IEnumerator MoveBeyondBarrier()
	{

		float timer = 0f;

		while(timer < 1.5f) {
            if (timer > 0.8f)
            {
                segments.GetComponent<Animator>().enabled = true;
               
            }
                timer += Time.deltaTime;
			float timePercent = timer / 1.5f;
			questionTarget.localPosition = Vector3.Lerp (questionTarget.localPosition, new Vector3 (questionTarget.localPosition.x,-8.5f, -969f), timePercent);
			yield return 0;
		}
        ResetMathBlock();
        yield return null;
	}

	IEnumerator ResetLights()
	{
		StartCoroutine ("ResetSegments");
		leftTraffic.Reset ();
		rightTraffic.Reset ();
		straightTraffic.Reset ();
		yield return null;
	}

	IEnumerator ResetSegments()
	{
		float timer = 0f;
		while (timer < 1f) {
			timer += Time.deltaTime;
			float timePercent = timer / 1f;
			mainSegments.transform.localPosition = Vector3.Lerp (mainSegments.transform.localPosition, originalSegmentPosition, timePercent);
			yield return 0;
		}
		yield return null;
	}

	IEnumerator RaiseBarrier()
	{
		if (selectedDirection == "Left") {
			yield return StartCoroutine(leftTraffic.RaiseBarrier ());
		} else if (selectedDirection == "Straight") {
			yield return StartCoroutine (straightTraffic.RaiseBarrier ());
		} else if (selectedDirection == "Right") {
			yield return StartCoroutine(rightTraffic.RaiseBarrier ());
		}

		yield return null;
	}

	void ResetMathBlock()
	{
		Debug.Log ("resetting math block");
		questionTarget.localPosition= mathBlockSpawnPos;
	}

    void DeleteDigit()
    {
		switch (answerIndex) {
		case -1:
			answerIndex = -1;
			break;
		case 0:
			currentAnswer [0] = "";
			answerIndex = -1;
			break;
		case 1:
			currentAnswer [1] = "";
			answerIndex = 0;
			break;
		case 2:
			currentAnswer [1] = "";
			answerIndex = 0;
			break;
		}
        DisplayCurrentAnswer();
    }

    void AppendToAnswer(string digit)
    {
		switch (answerIndex) {
		case -1:
			answerIndex = 0;
			currentAnswer [answerIndex] = digit;
			answerIndex++;
			break;
		case 0:
			currentAnswer [0] = digit;
			answerIndex++;
			break;
		case 1:
			currentAnswer [1] = digit;
			answerIndex=2;
			break;
		case 2:
			answerIndex = 2;
			break;
			
		}
     
        DisplayCurrentAnswer();
            
    }
    void DisplayCurrentAnswer()
    {
        string tempString = "";
        if (!Config.isGamified)
        {
            if (answerIndex >= 1)
                tempString = string.Concat(currentAnswer[0], currentAnswer[1]);
            else
                tempString = currentAnswer[0];

            answer.text = tempString;
        }
        else
        {

            if (answerIndex >= 1)
                tempString = string.Concat(currentAnswer[0], currentAnswer[1]);
            else
                tempString = currentAnswer[0];
            currentQuestion.GetComponent<MathQuestionScript>().answer.text = tempString;
        }
    }
    public IEnumerator RunMathDistractor()
    {
        allowMathDistractor = true;
        StartCoroutine("DisplayMathProblems");
        yield return new WaitForSeconds(20f);
        allowMathDistractor = false;
        yield return null;
    }
}
