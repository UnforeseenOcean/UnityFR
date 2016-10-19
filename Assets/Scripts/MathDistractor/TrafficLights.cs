using UnityEngine;
using System.Collections;

public class TrafficLights : MonoBehaviour {

	public GameObject greenLight;
	public GameObject redLight;
	public GameObject yellowLight;

	public Material greenMat;
	public Material redMat;
	public Material yellowMat;
	public Material plainMat;

	public GameObject barrier;
	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CorrectAnswer ()
	{
		yellowLight.GetComponent<Renderer> ().material = plainMat;
		greenLight.GetComponent<Renderer> ().material = greenMat;
	}

	public void WrongAnswer()
	{
		yellowLight.GetComponent<Renderer> ().material = plainMat;
		redLight.GetComponent<Renderer> ().material = redMat;
	}

	public void Reset()
	{
		redLight.GetComponent<Renderer> ().material = plainMat;
		greenLight.GetComponent<Renderer> ().material = plainMat;
		yellowLight.GetComponent<Renderer> ().material = yellowMat;
		StartCoroutine("ResetBarrier");
	}

	public IEnumerator RaiseBarrier()
	{	
		barrier.GetComponent<Animator> ().SetBool ("raise", true);
		yield return new WaitForSeconds (2f);
		yield return null;
	}

	public IEnumerator ResetBarrier()
	{
		barrier.GetComponent<Animator> ().SetBool ("raise", false);
		yield return null;
	}
}
