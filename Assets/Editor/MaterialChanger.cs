using UnityEngine;
using System.Collections;
using UnityEditor;

public class MaterialChanger : EditorWindow {

	public Material actualMaterial;
	public GameObject parentObject;
	[MenuItem("Window/UnityFR/MaterialChanger")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(MaterialChanger));
	}
	void OnGUI()
	{
		GUILayout.Label("Parent GameObject with children",EditorStyles.boldLabel);
		parentObject = EditorGUILayout.ObjectField(parentObject, typeof(GameObject), false, null) as GameObject;
		GUILayout.Label("Number of replications", EditorStyles.boldLabel);
		actualMaterial = EditorGUILayout.ObjectField (actualMaterial, typeof(Material), false, null) as Material;
		if (GUILayout.Button("Create Objects"))
		{
			ChangeMaterial ();
		}
	}
	void ChangeMaterial()
	{
		for (int i = 0; i < parentObject.transform.childCount; i++) {
			for (int j = 0; j < parentObject.transform.GetChild (i).childCount; j++) {
				parentObject.transform.GetChild (i).GetChild (j).gameObject.GetComponent<Renderer> ().material = actualMaterial;
			}
		}
	}


}