using UnityEngine;
using System.Collections;
using UnityEditor;

public class RoadBuilder : EditorWindow
{
    
    public GameObject parentObject;
    public float posX;
    public float posY;
    public float posZ;
    public int count = 100;
    public Vector3 refVector;
    [MenuItem("Window/UnityFR/RoadBuilder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RoadBuilder));
    }
    void OnGUI()
    {
        GUILayout.Label("GameObject to spawn", EditorStyles.boldLabel);
        parentObject = EditorGUILayout.ObjectField(parentObject, typeof(GameObject), false, null) as GameObject;
        refVector = EditorGUILayout.Vector3Field("Ref Transform",refVector);
        GUILayout.Label("Number of replications", EditorStyles.boldLabel);
        count = EditorGUILayout.IntField(count);
        GUILayout.Label("X increment", EditorStyles.boldLabel);
        posX = EditorGUILayout.FloatField(posX);
        GUILayout.Label("Y fixed position", EditorStyles.boldLabel);
        posY = EditorGUILayout.FloatField(posY);
        GUILayout.Label("Z increment", EditorStyles.boldLabel);
        posZ = EditorGUILayout.FloatField(posZ);
        if (GUILayout.Button("Create Objects"))
        {
            SpawnObjects();
            
        }
    }

    void SpawnObjects()
    {
        for (int i = 0; i < count; i++)
        {
			//for other objects
			/*
            Instantiate(parentObject, refVector + new Vector3(posX, posY, (posZ * i)), Quaternion.identity);

            Instantiate(parentObject, refVector + new Vector3(posX+33.2f, posY, (posZ * i)), Quaternion.identity);
			*/
            //for trees
			Vector2 randVect=Random.insideUnitCircle*15f;
			if(randVect.x <= 0)
			Instantiate(parentObject, refVector + new Vector3(-posX+randVect.x,posY,(posZ *i)+randVect.y), Quaternion.identity);
			else
			Instantiate(parentObject, refVector + new Vector3(posX+randVect.x,posY,(posZ *i)-randVect.y), Quaternion.identity);
            
        }
    }


}