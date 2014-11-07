using UnityEngine;
using System.Collections;

// A simple script for the game management container to ensure that the container persists through the entire game

public class tmgpGameManagementContainer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
	}
}