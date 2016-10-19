using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour {


	private VideoPlay videoQuad;
	public InstructionVideoLogTrack instructionVideoLogTrack;


	//SINGLETON
	private static InstructionsController _instance;

	public static InstructionsController Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake () {

		if (_instance != null)
		{
			Debug.Log("Instance already exists!");
			return;
		}
		_instance = this;
		videoQuad = transform.GetChild (0).gameObject.GetComponent<VideoPlay>();
	}

	public void EnableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = true;
		gameObject.GetComponent<AudioListener> ().enabled = true;
		videoQuad.gameObject.GetComponent<AudioSource> ().enabled=true;
		videoQuad.gameObject.GetComponent<AudioSource> ().Play ();
	}
	public void DisableCamera()
	{
		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<AudioListener> ().enabled = false;
		videoQuad.gameObject.GetComponent<AudioSource> ().enabled = false;
		videoQuad.gameObject.GetComponent<AudioSource> ().Stop();

	}
	public IEnumerator PlayInstructions()
	{
		yield return StartCoroutine(videoQuad.PlayVideo (161f));
		yield return null;
	}
}
