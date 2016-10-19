using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private bool playerCanMove = true;
    public float playerSpeed = 3f;
	public float threshold=1661.45f;
    Vector3 startPos;
	float timer=0f;
	bool startTracking=false;
    //SINGLETON
    private static PlayerMovement _instance;

    public static PlayerMovement Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        startPos = transform.position;
        if (_instance != null)
        {
            Debug.Log("Instance already exists!");
            return;
        }
        _instance = this;

    }

    void Start () {

        //StartCoroutine("InitiatePlayerMovement");
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void FixedUpdate()
	{
		if (startTracking)
			timer += Time.deltaTime;
	}

    public void EnableCamera()
    {

        gameObject.GetComponent<Camera>().enabled = true;
        playerCanMove = true;
        StartCoroutine("InitiatePlayerMovement");
    }
    public void DisableCamera()
    {
        gameObject.GetComponent<Camera>().enabled = false;
        playerCanMove = false;
        ResetPlayerPosition();
        //reset player's position here
    }

    void ResetPlayerPosition()
    {
        transform.position = startPos;
    }
    IEnumerator InitiatePlayerMovement()
    {
        yield return StartCoroutine("Movement");
        yield return null;
    }

	public void StartTracking()
	{
		timer = 0f;
		startTracking = true;
	}

	public void StopTracking()
	{
		startTracking = false;
		//Debug.Log ("the time is: " + timer.ToString ());
	}
	public void CalculatePlayerSpeed(float jitterTime)
	{
		//baseline is 18 for 1.6s
		playerSpeed=(22f * 1.6f)/(1.6f+jitterTime);

	}

    IEnumerator Movement()
    {
        while(playerCanMove)
        {
			
            if (transform.position.z < threshold)
				transform.position += transform.forward * playerSpeed * Time.deltaTime; // 1F * 18F * 0.02F
            else
                transform.position = new Vector3(0f, 6f, -6.7f); 

            yield return 0;
        }
        yield return null;
    }
}
