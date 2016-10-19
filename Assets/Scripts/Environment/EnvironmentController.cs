using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {

    public Material[] skyboxMaterials;
    int maxSkybox=17;
    int currentSkybox = 0;
	// Use this for initialization
	void Start () {

        maxSkybox = skyboxMaterials.Length;
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.N))
        {
            NextSkybox();
        }
	}

    void NextSkybox()
    {
        if(currentSkybox<maxSkybox)
        RenderSettings.skybox = skyboxMaterials[currentSkybox];
        currentSkybox++;
    }
    
    public void ChangeEnvironment()
    {
        if(SceneController.Instance.trialCount % 3 == 0)
        {
            ChangeEnvironmentLocation();
        }
        ChangeSkybox();
    }

    void ChangeEnvironmentLocation()
    {

    }

    void ChangeSkybox()
    {
        if(currentSkybox<maxSkybox)
        {

        }
    }
}
