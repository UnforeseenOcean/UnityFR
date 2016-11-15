using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MathQuestionScript : MonoBehaviour {

    public float speed = 10f;
    public Vector3 stopPos;
	public Vector3 beyondPos;
    public GameObject greenDust;
    public GameObject redDust;

	public Material greenMat;
	public Material redMat;

	public AudioClip correctSound;
	public AudioClip wrongSound;


	private AudioSource mySource;
    bool shouldDestroy = false;
    // Use this for initialization
    void Start () {

		mySource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ApproachCamera()
    {
        float timer = 0f;
        float timePercent = 0f;
        while(timer< 1.5f && !shouldDestroy)
        {
            timer += Time.deltaTime;
            timePercent = timer / 1.5f;
            transform.position = Vector3.Lerp(transform.position, stopPos, timePercent);
            yield return 0;
        }
        yield return null;
    }

	public IEnumerator BeyondCamera()
	{
		Debug.Log ("trying to go beyond camera");
		float timer = 0f;
		float timePercent = 0f;
		while(timer< 1f && !shouldDestroy)
		{
			timer += Time.deltaTime;
			timePercent = timer / 1f;
			transform.position = Vector3.Lerp(transform.position, beyondPos, timePercent);
			yield return 0;
		}
		Destroy ();
		yield return null;
	}
    public IEnumerator CorrectAnswer()
    {
        if (!shouldDestroy)
        {
			ChangeMaterial (0);
			mySource.PlayOneShot (correctSound);
			yield return new WaitForSeconds(0.75f);
            if (!shouldDestroy)
                StartCoroutine ("BeyondCamera");
         //   Destroy(gameObject);
        }
        yield return null;
    }
    public IEnumerator WrongAnswer()
    {
        if (!shouldDestroy)
        {
			ChangeMaterial (1);
			mySource.PlayOneShot (wrongSound);
			yield return new WaitForSeconds(0.75f);
            if (!shouldDestroy)
                StartCoroutine ("BeyondCamera");
         //   Destroy(gameObject);
        }
        yield return null;
    }

	void ChangeMaterial(int answerStatus) //0 for correct, 1 for wrong
    {
        int childCap = transform.childCount - 1;
        if (answerStatus == 0) {
			for (int i = 3; i <= childCap; i++) {
                if(!shouldDestroy)
				transform.GetChild (i).GetChild(0).GetChild(1).GetComponent<Renderer> ().material = greenMat;
			}
		} else {
			for (int i = 3; i <= childCap; i++) {
                if(!shouldDestroy)
				transform.GetChild (i).GetChild(0).GetChild(1).GetComponent<Renderer> ().material = redMat;
			}
		}
	}

    public void Destroy()
    {
        shouldDestroy = true;
        Destroy(this.gameObject);
    }
}
