using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class MathDistractor : MonoBehaviour
{


    public Text number1;
    public Text number2;
    public Text number3;
	public Text answer;

	//for final prototype
	public List<int> numbers;
	public List<GameObject> numberPrefabs;
	public GameObject questionTemplate;
	public Vector3 questionSpawnPos;
	private GameObject currentQuestion;
	private bool answerKeyPressed = false;
	private int answeredInt=0;


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
	}

	public void DisableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = false;
            if(currentQuestion!=null)
            {
            StopCoroutine(currentQuestion.GetComponent<MathQuestionScript>().ApproachCamera());
            //Destroy(currentQuestion);
                currentQuestion.GetComponent<MathQuestionScript>().Destroy();
            GameObject[] allQuestions = GameObject.FindGameObjectsWithTag("MathQuestion");
            for(int i=0;i<allQuestions.Length;i++)
            {
                Destroy(allQuestions[i]);
            }
                currentQuestion = null;
			answeredInt = 0;
			firstRandInt = 0;
			secondRandInt = 0;
            }
	}
    
	IEnumerator ReplenishNumberList()
	{
		//first clean up the list
		if(numbers.Count>0)
		{
			numbers.RemoveRange (0, numbers.Count);
		}
	//	Debug.Log ("there are : " + numbers.Count + " objects left");

		//then fill it up with standard 1-8 numbers
		for (int j = 1; j<= 8; j++) {
			//Debug.Log ("NOW " + j);
			numbers.Add (j);
		}
		yield return null;
	}
    void GenerateNewMathProblem()
    {
		int firstRandIndex = Random.Range (0, numbers.Count-1);
		firstRandInt = numbers[firstRandIndex];
		//Debug.Log ("removing " + firstRandInt + " at index " + firstRandIndex);
		int upperLimit = 10 - firstRandInt;
		//Debug.Log ("upper limit: " + upperLimit);
		int secondRandIndex = Random.Range (0,upperLimit-1);
        Debug.Log("first index: " + firstRandIndex + " and second: " + secondRandIndex);
        if(firstRandIndex==secondRandIndex)
        {
            if (firstRandIndex == 0)
            {
                secondRandIndex = 1;
            }
            else if (firstRandIndex == numbers.Count - 1)
            {
                secondRandIndex = numbers.Count - 2;
            }
            else
                secondRandIndex = firstRandIndex + 1;
        }
		secondRandInt = numbers [secondRandIndex];
        /*
        if(numbers.Count > firstRandIndex)
            numbers.RemoveAt(firstRandIndex);
        if(numbers.Count>secondRandIndex)
            numbers.RemoveAt(secondRandIndex);
       */
		//Debug.Log ("removing " + secondRandInt + " at index " + secondRandIndex);
		correctAnswer = firstRandInt + secondRandInt;

		//should spawn relevant numbers
		SpawnNumbers(firstRandIndex,secondRandIndex);

		shouldGenerateNewProblem = false;
    }

	void SpawnNumbers(int firstRand, int secondRand)
	{
		currentQuestion = Instantiate (questionTemplate, questionSpawnPos, Quaternion.identity) as GameObject;
		Vector3 firstNumPos = currentQuestion.transform.GetChild (0).position;
		Vector3 secondNumPos = currentQuestion.transform.GetChild (1).position;
		Vector3 firstNumAngle = currentQuestion.transform.GetChild (0).localEulerAngles;
		Vector3 secondNumAngle = currentQuestion.transform.GetChild (1).localEulerAngles;
		GameObject firstNumber = Instantiate (numberPrefabs [firstRand], firstNumPos, Quaternion.Euler(firstNumAngle)) as GameObject;
		firstNumber.transform.parent = currentQuestion.transform;
		GameObject secondNumber = Instantiate (numberPrefabs [secondRand], secondNumPos, Quaternion.Euler(secondNumAngle)) as GameObject;
		firstNumber.transform.parent = currentQuestion.transform;
		secondNumber.transform.parent = currentQuestion.transform;
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
		//Debug.Log(firstRandInt + " and " + secondRandInt);
        correctAnswer = firstRandInt + secondRandInt;
        //Debug.Log ("correct answer: " + correctAnswer);
        if (currentQuestion != null)
        {
            if (answeredInt == correctAnswer)
            {
                yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().CorrectAnswer());
            }
            else
            {
                yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().WrongAnswer());
            }
        }
        yield return null;
    }

    // Use this for initialization
    void Start()
	{
		ReplenishNumberList ();
			currentAnswer [0] = "";
			currentAnswer [1] = "";
			answerIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (!answerKeyPressed) {
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				CreateAnswer (0);
			} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
				CreateAnswer (1);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				CreateAnswer (2);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				CreateAnswer (3);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				CreateAnswer (4);
			} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
				CreateAnswer (5);
			} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
				CreateAnswer (6);
			} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
				CreateAnswer (7);
			} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
				CreateAnswer (8);
			} else if (Input.GetKeyDown (KeyCode.Alpha9)) {
				CreateAnswer (9);
			}
		}
    }

	void CreateAnswer(int answer)
	{
		answeredInt = answer;
		if(currentQuestion!=null)
		{
			Vector3 answerPos = currentQuestion.transform.GetChild (2).position;
			Vector3 answerAngle = currentQuestion.transform.GetChild (2).localEulerAngles;
			GameObject answerObj = Instantiate (numberPrefabs [answer - 1], answerPos, Quaternion.Euler (answerAngle)) as GameObject;
			answerObj.transform.parent = currentQuestion.transform;
			//StartCoroutine(currentQuestion.GetComponent<MathQuestionScript> ().BeyondCamera ());

		}

        answerKeyPressed = true;
    }

	/*
	void InstantiateMathBlock()
	{
 
            currentQuestion = Instantiate(mathQuestionPrefab, mathQuestionSpawnPos, Quaternion.identity) as GameObject;
        if(currentQuestion!=null)
            currentQuestion.GetComponent<MathQuestionScript>().AssignNumbers(firstRandInt, secondRandInt, thirdRandInt);
    }
*/

	/// <summary>
	/// MAIN LOGIC OF THE MATH DISTRACTOR
	/// </summary>
	/// <returns>The math problems.</returns>
	/// 
	public IEnumerator RunMathDistractor()
	{
		allowMathDistractor = true;
		StartCoroutine("DisplayMathProblems");
		yield return new WaitForSeconds(20f);
		allowMathDistractor = false;
		yield return null;
	}

	IEnumerator DisplayMathProblems()
	{
		shouldGenerateNewProblem = true;
		while(allowMathDistractor)
		{
                if (shouldGenerateNewProblem)
			{		
				yield return StartCoroutine("ReplenishNumberList");
                    GenerateNewMathProblem();
//                    InstantiateMathBlock();
                   if (currentQuestion != null)
                        yield return StartCoroutine(currentQuestion.GetComponent<MathQuestionScript>().ApproachCamera());
                    shouldGenerateNewProblem = false;
                }
			if (answerKeyPressed)
                {
                    yield return StartCoroutine("CheckAnswers");
                    shouldGenerateNewProblem = true;
				answerKeyPressed = false;
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
	/*
    void ResetMathBlock()
    {
        Debug.Log("resetting math block");
        questionTarget.localPosition = mathBlockSpawnPos;
    }

    void DeleteDigit()
    {
        switch (answerIndex)
        {
            case -1:
                answerIndex = -1;
                break;
            case 0:
                currentAnswer[0] = "";
                answerIndex = -1;
                break;
            case 1:
                currentAnswer[1] = "";
                answerIndex = 0;
                break;
            case 2:
                currentAnswer[1] = "";
                answerIndex = 0;
                break;
        }
        DisplayCurrentAnswer();
    }

    void AppendToAnswer(string digit)
    {
        switch (answerIndex)
        {
            case -1:
                answerIndex = 0;
                currentAnswer[answerIndex] = digit;
                answerIndex++;
                break;
            case 0:
                currentAnswer[0] = digit;
                answerIndex++;
                break;
            case 1:
                currentAnswer[1] = digit;
                answerIndex = 2;
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
            if (currentQuestion != null)
                currentQuestion.GetComponent<MathQuestionScript>().answer.text = tempString;
        }
    }
    */

    /*

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
    */
}
