using UnityEngine;
using System.Collections;
using Bitcoin;
using DAO;
using System.IO;

public class SphereSpawner : MonoBehaviour {

	string dataPath = "";
	BitcoinDAO input;
	int numPlanets;
	

	
	private ArrayList planets;
	private Vector3[] positions;

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			dataPath = @"../data/1000blocks/";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			dataPath = @"..\data\1000blocks\";
			Debug.Log ("detected Windows OS");
		}
		input = new CSVDAO(dataPath);
		//Debug.Log(System.Environment.CurrentDirectory);
		numPlanets = 1000;
		planets = new ArrayList(); 
		InitArray();

		GameObject planet;
		for (int i = 0; i < numPlanets; i++) {
			//planets[i].position = PolarToCartesian(inArray[i]);
			//planets[i]. = new Color(Random.Range (0f, 1f), Random.Range (0f, 1f),Random.Range (0f, 1f));
			float scaleFactor = (float)input.Addresses[i].FirstTransaction.Outputs[0].Value/50;
			Vector3 scaleVector = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			//Debug.Log(i);

			planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);

			planet.transform.localScale += scaleVector;
			planet.transform.position = positions[i];
			planets.Add(planet);
		}

	}


	private void InitArray(){
		float twoPi = Mathf.PI * 2;
		positions = new Vector3[numPlanets];
		for (int i = 0; i < positions.Length; i++) {
			positions[i]= PolarToCartesian(new Vector3(input.Addresses[i].FirstTransaction.Block.Height/10, Random.Range(0f, twoPi), Random.Range(0f, twoPi)));
		}
		Debug.Log("positions acquired: "+positions.Length);
	}


	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 PolarToCartesian(Vector3 inPolar){
		float vX = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Cos (inPolar[2]);
		float vY = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Sin (inPolar[2]);
		float vZ = inPolar [0] * Mathf.Cos (inPolar [1]);
		
		
		return new Vector3(vX, vY, vZ);
	}
}
