﻿using UnityEngine;
using System.Collections;

public class StartVisualization : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			Application.LoadLevel ("SphereView");
		}
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit ();
		}
	}
	void OnGUI(){
		GUI.Box (new Rect (Screen.width/2-175, Screen.height/2-25, 350, 50), "press space to start visualization \n \n press escape to quit");
	}
}
