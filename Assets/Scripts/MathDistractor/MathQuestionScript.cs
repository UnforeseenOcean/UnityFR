using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MathQuestionScript : MonoBehaviour {

    public float speed = 10f;
    public Vector3 stopPos;
	public Vector3 beyondPos;
    public GameObject greenDust;
    public GameObject redDust;

    bool shouldDestroy = false;
    // Use this for initialization
    void Start () {
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
		float timer = 0f;
		float timePercent = 0f;
		while(timer< 1f && !shouldDestroy)
		{
			timer += Time.deltaTime;
			timePercent = timer / 1f;
			transform.position = Vector3.Lerp(transform.position, beyondPos, timePercent);
			yield return 0;
		}
		yield return null;
	}
    public IEnumerator CorrectAnswer()
    {
        if (!shouldDestroy)
        {
            Instantiate(greenDust, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        yield return null;
    }
    public IEnumerator WrongAnswer()
    {
        if (!shouldDestroy)
        {
            Instantiate(redDust, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        yield return null;
    }

    public void Destroy()
    {
        shouldDestroy = true;
        Destroy(this.gameObject);
    }
}
