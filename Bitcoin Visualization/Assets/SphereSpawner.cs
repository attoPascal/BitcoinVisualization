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
			//planet.GetComponent<Renderer> ().material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
			planet.GetComponent<Renderer> ().material.color = HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
			planet.AddComponent<OnClickCenterView>();
			planet.GetComponent<OnClickCenterView>().setAddress(input.Addresses[i].ID);
			planet.GetComponent<OnClickCenterView>().setBlock(input.Addresses[i].FirstTransaction.Block.Height);
			planet.GetComponent<OnClickCenterView>().setBalance((float)input.Addresses[i].FirstTransaction.Outputs[0].Value);

			//	planet.GetComponent<Renderer>.material.setColor();
			planet.transform.localScale += scaleVector;
			planet.transform.position = positions[i];
			planets.Add(planet);
		}

	}


	private void InitArray(){
		float twoPi = Mathf.PI * 2;
		positions = new Vector3[numPlanets];
		for (int i = 0; i < positions.Length; i++) {
			positions[i]= PolarToCartesian(new Vector3(input.Addresses[i].FirstTransaction.Block.Height/2, Random.Range(0f, twoPi), Random.Range(0f, twoPi)));
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

	public void CenterView(Vector3 newCenter){
		for (int i = 0; i < numPlanets; i++) {
			GameObject temp = (GameObject) planets [i];
			temp.transform.position += new Vector3 (-newCenter.x, -newCenter.y, -newCenter.z);
			planets [i] = temp;
		}
	}

	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f) {
			return new Color (V, V, V);
		}
		else if (V == 0f){
			Color ret = new Color();
			ret= Color.black;
			return ret;
		}else{
			Color col = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 1:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			case 2:
				col.r = v2;
				col.g = V;
				col.b = v1;
				break;
			case 3:
				col.r = v1;
				col.g = V;
				col.b = v3;
				break;
			case 4:
				col.r = v1;
				col.g = v2;
				col.b = V;
				break;
			case 5:
				col.r = v3;
				col.g = v1;
				col.b = V;
				break;
			case 6:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 7:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			}
			col.r = Mathf.Clamp(col.r, 0f, 1f);
			col.g = Mathf.Clamp(col.g, 0f, 1f);
			col.b = Mathf.Clamp(col.b, 0f, 1f);
			return col;
		}
	}
}
