using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[RequireComponent (typeof (VisibilityToggler))]
[RequireComponent (typeof (ObjectLogTrack))]
[RequireComponent (typeof (ScaleAnimator))]
public class SpawnableObject : MonoBehaviour {

	VisibilityToggler myVisibilityToggler;
	public bool isVisible { get { return myVisibilityToggler.GetVisibility (); } }

	ObjectLogTrack myLogTrack;

	Vector3 origScale;

	private string IDstring = "";

	// Use this for initialization
	void Awake () {
		myVisibilityToggler = GetComponent<VisibilityToggler> ();
		myLogTrack = GetComponent<ObjectLogTrack> ();
		origScale = transform.localScale;
	}

	void Start(){
		if (tag == "SpecialItem") {
			ScaleUp();
			StartCoroutine(LiveLife());
		}
	}

	IEnumerator LiveLife(){
		yield return new WaitForSeconds (Config.specialObjectLifeTime);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//function to turn off (or on) the object without setting it inactive -- because we want to keep logging on
	public void TurnVisible(bool shouldBeVisible){ 
		if (myVisibilityToggler == null) {
			myVisibilityToggler = GetComponent<VisibilityToggler> ();
		}
		myVisibilityToggler.TurnVisible (shouldBeVisible);
	}

	public string GetName(){
		string name = gameObject.name;
		name = Regex.Replace( name, "(Clone)", "" );
		name = Regex.Replace( name, "[()]", "" );

		return name;
	}

	public string GetNameNoID(){
		//separate out the object name from a numeric ID
		Regex numAlpha = new Regex("(?<Alpha>[a-zA-Z ]*)(?<Numeric>[0-9]*)");
		Match match = numAlpha.Match(GetName());
		string objShortName = match.Groups["Alpha"].Value;
		//string objID = match.Groups["Numeric"].Value; //in case you need the ID num

		return objShortName;
	}

	//should be set when spawned by the ObjectController
	public void SetNameID(int ID){
		if (ID < 10) {
			IDstring = "00" + ID; 
		}
		else if(ID < 100) {
			IDstring = "0" + ID; 
		}
		else if(ID < 1000) {
			IDstring = ID.ToString(); 
		}

		gameObject.name = GetName () + IDstring;

		//set the first layer of children...
		//TODO: should do this recursively.
		//...in case one of the children gets logged or something!
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).name += IDstring;
		}
	}

	/*
	void OnCollisionEnter(Collision collision){
		if (collision.collider.tag == "Player" && gameObject.tag == "DefaultSpecialItem") {
			Experiment.Instance.objectController.SpawnSpecialObject (transform.position);
			Experiment.Instance.trialController.IncrementNumObjectsCollected ();
			Destroy (gameObject);
		}
	}
	*/


	public void Scale(float scaleMult){
		transform.localScale *= scaleMult;
	}

	public void SetOrigScale(){
		transform.localScale = origScale;
	}

	public void SetLayer(string newLayer){
		UsefulFunctions.SetLayerRecursively (gameObject, newLayer);

		myLogTrack.LogLayerChange ();
	}

	public void SetShadowCasting(bool shouldCastShadows){
		UnityEngine.Rendering.ShadowCastingMode shadowMode = UnityEngine.Rendering.ShadowCastingMode.On;
		if (!shouldCastShadows) {
			shadowMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}

		if(GetComponent<Renderer>() != null){
			GetComponent<Renderer>().shadowCastingMode = shadowMode;
		}
		
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < renderers.Length; i++){
			renderers[i].shadowCastingMode = shadowMode;
		}

		myLogTrack.LogShadowSettings (shadowMode);
	}

	void ScaleUp(){
		float timeToScaleUp = 0.3f;
		
		float fullScaleMult = 1.0f;
		float smallScaleMult = 0.5f;
		StartCoroutine( GetComponent<ScaleAnimator>().AnimateScaleUp(timeToScaleUp, fullScaleMult, smallScaleMult) );
	}

}