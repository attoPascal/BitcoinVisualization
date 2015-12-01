﻿using UnityEngine;
using System.Collections;
using Bitcoin;
using DAO;
using System.IO;

public class ParticleBehaviour : MonoBehaviour {

	string dataPath = @"..\data\1000blocks\";
	BitcoinDAO input;
	
	int numPlanets;
	private ParticleSystem.Particle[] planets;
	private Vector3[] inArray;
	bool fullyExpanded = false;
	private Vector3[]  currentExpansion;
	// Use this for initialization
	void Start () {
		input = new CSVDAO(dataPath);
		numPlanets = 100;
		//Debug.Log(System.Environment.CurrentDirectory);
		planets = new ParticleSystem.Particle[numPlanets];
		InitInArray();
		float increment = 1f / (numPlanets - 1);
		for (int i =0; i<numPlanets; i++) {
			float x = i * increment;
			//planets[i].position = PolarToCartesian(inArray[i]);
			planets[i].color = new Color(Random.Range (0f, 1f), Random.Range (0f, 1f),Random.Range (0f, 1f));
			planets[i].size = (float)input.Addresses[i].FirstTransaction.Outputs[0].Value/25;
		}
	}
	void expand(){
		float increment = 1f / (numPlanets - 1);
		fullyExpanded = true;
		for (int i = 0; i<numPlanets; i++) {
			if(currentExpansion[i].x < inArray[i].x){
				currentExpansion[i].x = currentExpansion[i].x + increment;
				planets[i].position = PolarToCartesian(currentExpansion[i]);
				fullyExpanded = false;
			}
		}
		
	}
	// Update is called once per framendom
	void Update () {
		if (!fullyExpanded) {
			expand ();
		}
		GetComponent<ParticleSystem> ().SetParticles (planets, planets.Length);
		
	}
	//
	void InitInArray(){
		inArray = new Vector3[numPlanets];
		float twoPi = Mathf.PI * 2;
		for (int i = 0;i<inArray.Length; i++) {
			inArray[i]= new Vector3(input.Addresses[i].FirstTransaction.Block.Height/100, Random.Range(0f, twoPi), Random.Range(0f, twoPi));
		}
		currentExpansion = new Vector3[numPlanets];
		for (int i= 0; i<currentExpansion.Length; i++) {
			currentExpansion[i] = new Vector3(0f, inArray[i].y, inArray[i].z);
		}
	}
	
	
	public Vector3 PolarToCartesian(Vector3 inPolar){
		float vX = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Cos (inPolar[2]);
		float vY = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Sin (inPolar[2]);
		float vZ = inPolar [0] * Mathf.Cos (inPolar [1]);
		
		
		return new Vector3(vX, vY, vZ);
	}
}