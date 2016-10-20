using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentController : MonoBehaviour {

    public Material[] skyboxMaterials;
    List<Material> shufflerList=new List<Material>();
    List<Material> currentListSkybox = new List<Material>();
    public GameObject[] environmentType;
    int currentEnvironmentType = 0;
    int currentSkybox = 0;
    int tempVar = 0;
	// Use this for initialization
	void Start () {
        MakeUniqueEnvironmentList();
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.N))
        {
            //NextSkybox();
            ChangeEnvironment();
        }
	}

    void NextSkybox()
    {
        if(currentSkybox<currentListSkybox.Count)
        RenderSettings.skybox = skyboxMaterials[currentSkybox];
        currentSkybox++;
    }

    public void MakeUniqueEnvironmentList()
    {

        //store all available skyboxes in a temporary shuffle list
        for (int i = 0; i < skyboxMaterials.Length; i++)
        {
            shufflerList.Add(skyboxMaterials[i]);
        }

        //randomly shuffle them around
        for(int j=0;j<10;j++)
        {
            int randInt = Random.Range(0, shufflerList.Count);
            currentListSkybox.Add(shufflerList[randInt]);
            shufflerList.RemoveAt(randInt);
        }
    }
    
    public void ChangeEnvironment()
    {
        tempVar++;
        if(tempVar % 3==0)
        //if(SceneController.Instance.trialCount % 3 == 0)
        {
            ChangeSkybox();
        }
        ChangeEnvironmentType();
    }

    void DisableOldEnvironment(int oldEnviroment)
    {
        environmentType[oldEnviroment].SetActive(false);   
    }

    void ChangeEnvironmentType()
    {

        //disable the old environment first
        DisableOldEnvironment(currentEnvironmentType);

        //then enable it
        if (currentEnvironmentType < 2)
            currentEnvironmentType++;
        else
            currentEnvironmentType = 0;

        environmentType[currentEnvironmentType].SetActive(true);
    }

    void ChangeSkybox()
    {
        //increment the skybox value
        currentSkybox++;

        //then update it
        if(currentSkybox< currentListSkybox.Count)
        {
            RenderSettings.skybox = currentListSkybox[currentSkybox];
        }
    }
}
