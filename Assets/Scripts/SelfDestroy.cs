using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

    public float waitTime = 1f;
	// Use this for initialization
	void Start () {
        StartCoroutine("DestroySelf");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
        yield return null;
    }
}
