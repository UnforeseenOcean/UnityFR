using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public GameObject AvatarStandardCameraRig;
	public GameObject OculusRig;	//OCULUS SETTINGS ARE DESIGNED FOR MAC. IF DEVELOPING ON WINDOWS, SHOULD USE BUILD SETTINGS->PLAYER SETTINGS->OTHER SETTINGS->VIRTUAL REALITY SUPPORTED

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInGame(){

		//Debug.Log("oh hey in game cameras");
		TurnOffAllCameras();

		if(ExperimentSettings.isOculus){
			if(!OculusRig.activeSelf){
				SetOculus(true);
			}
		}
		else{
			EnableCameras(AvatarStandardCameraRig, true);
		}


	}

	void TurnOffAllCameras(){
		EnableCameras(AvatarStandardCameraRig, false);

		if(!ExperimentSettings.isOculus){
			OculusRig.SetActive(false);
		}
	}


	void EnableCameras(GameObject cameraRig, bool setOn){
		Camera[] cameras = cameraRig.GetComponentsInChildren<Camera>();
		for(int i = 0; i < cameras.Length; i++){
			cameras[i].enabled = setOn;
		}
	}

	void SetOculus(bool isActive){
		OculusRig.SetActive (isActive);
	}
}
