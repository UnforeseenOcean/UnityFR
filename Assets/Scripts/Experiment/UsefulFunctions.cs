using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UsefulFunctions {

	//given the size of an array or a list, will return a list of indices in a random order
	public static List<int> GetRandomIndexOrder(int count){
		List<int> inOrderList = new List<int>();
		for(int i = 0; i < count; i++){
			inOrderList.Add(i);
		}
		
		List<int> randomOrderList = new List<int>();
		for(int i = 0; i < count; i++){
			int randomIndex = Random.Range(0, inOrderList.Count);
			randomOrderList.Add( inOrderList[randomIndex] );
			inOrderList.RemoveAt(randomIndex);
		}
		
		return randomOrderList;
	}

	//SORT BY DISTANCE
	public static List<Vector2> SortByNextClosest(List<Vector2> positions, Vector2 distancePos){
		
		List<Vector2> sortedPositions = new List<Vector2>();
		int numPositions = positions.Count;
		
		Vector2 closestPos = GetClosest (distancePos, positions);
		sortedPositions.Add (closestPos);
		positions.Remove (closestPos);
		for (int i = 1; i < numPositions; i++) {
			closestPos = GetClosest (closestPos, positions);
			sortedPositions.Add (closestPos);
			positions.Remove (closestPos);
		}
		
		return sortedPositions;
	}
	
	static Vector2 GetClosest(Vector2 position, List<Vector2> otherPositions){
		float minDist = 0.0f;
		int minDistIndex = 0;
		for (int i = 0; i< otherPositions.Count; i++) {
			float dist = UsefulFunctions.GetDistance(position, otherPositions[i]);
			if(i == 0){
				minDist = dist;
				minDistIndex = i;
			}
			else if(dist < minDist){
				minDist = dist;
				minDistIndex = i;
			}
		}
		
		return otherPositions [minDistIndex];
	}



	//set layer of gameobject and all its children using the layer ID (int)
	public static void SetLayerRecursively (GameObject obj, int newLayer){
		obj.layer = newLayer;
		
		foreach( Transform child in obj.transform )
		{
			SetLayerRecursively( child.gameObject, newLayer );
		}
	}

	//set layer of gameobject and all its children using the layer name
	public static void SetLayerRecursively (GameObject obj, string newLayer){
		obj.layer = LayerMask.NameToLayer( newLayer );

		foreach( Transform child in obj.transform )
		{
			SetLayerRecursively( child.gameObject, newLayer );
		}
	}

	public static void EnableChildren(Transform parentTransform, bool shouldEnable){
		//enable all children
		for (int i = 0; i < parentTransform.childCount; ++i) {
			parentTransform.GetChild (i).gameObject.SetActive (shouldEnable);
		}
	}

	public static void FaceObject( GameObject obj, GameObject target, bool shouldUseYPos){
		Vector3 lookAtPos = target.transform.position;
		if (!shouldUseYPos) {
			lookAtPos = new Vector3 (target.transform.position.x, obj.transform.position.y, target.transform.position.z);
		}
		obj.transform.LookAt(lookAtPos);
	}


	public static bool CheckVectorsCloseEnough(Vector3 position1, Vector3 position2, float epsilon){
		float xDiff = Mathf.Abs (position1.x - position2.x);
		float yDiff = Mathf.Abs (position1.y - position2.y);
		float zDiff = Mathf.Abs (position1.z - position2.z);
		
		if (xDiff < epsilon && yDiff < epsilon && zDiff < epsilon) {
			return true;
		}
		else {
			return false;
		}
	}

	public static float GetDistance(Vector3 startPos, Vector3 endPos){
		return (startPos - endPos).magnitude;
	}

	public static float GetDistance(Vector2 startPos, Vector2 endPos){
		return (startPos - endPos).magnitude;
	}
}
