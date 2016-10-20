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
      //  MakeUniqueEnvironmentList();
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

    public IEnumerator CleanupEnvironments()
    {
        while(shufflerList.Count>0)
        {
            shufflerList.RemoveAt(shufflerList.Count - 1);
            yield return 0;
        }
        Debug.Log("Shuffler has :" + shufflerList.Count);
        while(currentListSkybox.Count>0)
        {
            currentListSkybox.RemoveAt(currentListSkybox.Count - 1);
            yield return 0;
        }

        Debug.Log("skybox list now has : " + currentListSkybox.Count);

        yield return null;
    }

    public void MakeUniqueEnvironmentList()
    {
        //reset tempvar
        tempVar = 0;

        //store all available skyboxes in a temporary shuffle list
        for (int i = 0; i < skyboxMaterials.Length; i++)
        {
            shufflerList.Add(skyboxMaterials[i]);
        }

        Debug.Log("Shuffler has :" + shufflerList.Count);
        //randomly shuffle them around
        for (int j=0;j<skyboxMaterials.Length;j++)
        {
            int randInt = Random.Range(0, shufflerList.Count);
            currentListSkybox.Add(shufflerList[randInt]);
            shufflerList.RemoveAt(randInt);
        }

        Debug.Log("skybox list now has : " + currentListSkybox.Count);
    }
    
    public void ChangeEnvironment()
    {
        if(SceneController.Instance.trialCount % 3 == 0 || SceneController.Instance.trialCount==0)
        {
            ChangeSkybox();
        }
        ChangeEnvironmentType();
        tempVar++;
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
        Debug.Log("current skybox is : " + currentSkybox);
        //then update it
        if(currentSkybox< currentListSkybox.Count)
        {
            RenderSettings.skybox = currentListSkybox[currentSkybox];
        }
    }
}
