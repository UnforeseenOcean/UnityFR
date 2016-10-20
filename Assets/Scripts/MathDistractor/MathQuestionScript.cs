using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MathQuestionScript : MonoBehaviour {

    public float speed = 10f;
    public TextMesh number1;
    public TextMesh number2;
    public TextMesh number3;
    public TextMesh answer;
    public Vector3 stopPos;

    public GameObject greenDust;
    public GameObject redDust;

    bool shouldDestroy = false;
    // Use this for initialization
    void Start () {
        answer.text = "";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AssignNumbers(int rand1, int rand2, int rand3)
    {
        number1.text = rand1.ToString();
        number2.text = rand2.ToString();
        number3.text = rand3.ToString();
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
    public IEnumerator CorrectAnswer()
    {
        yield return new WaitForSeconds(1f);
        if (!shouldDestroy)
        {
            Instantiate(greenDust, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        yield return null;
    }
    public IEnumerator WrongAnswer()
    {
        yield return new WaitForSeconds(1f);
        if (!shouldDestroy)
        {
            Instantiate(redDust, transform.position, Quaternion.identity);
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
