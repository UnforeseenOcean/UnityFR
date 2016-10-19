using UnityEngine;
using System.Collections;

public class VisualChecker : MonoBehaviour {
    private float timer = 0f;
    private bool canSee = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnBecameVisible()
    {
        canSee = true;
        StartCoroutine("CheckTimer");
    }

    IEnumerator CheckTimer()
    {
        float currentTime = 0f;
        while(canSee)
        {
            timer+= Time.deltaTime;
            yield return 0;
        }
        
    }
    void OnBecameInvisible()
    {
        canSee = false;
        //Debug.Log("i cannot see");
        //Debug.Log("the final time is : " + timer.ToString());
    }
}
