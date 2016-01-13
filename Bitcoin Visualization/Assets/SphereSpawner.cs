﻿using UnityEngine;
using System.Collections.Generic;
using Bitcoin;
using DAO;
using System.IO;
using System.Linq;

public class SphereSpawner : MonoBehaviour {

	private BitcoinDAO dao;
	private int numPlanets;
	private List<GameObject> planets = new List<GameObject>();
	private List<Vector3> positions = new List<Vector3>();
	private bool flight = false;
	private Vector3 newCenter = new Vector3 (0, 0, 0);
	private float speed=25;

	// Use this for initialization
	void Start () {
		var slash = Path.DirectorySeparatorChar;
		var dataPath = Application.dataPath + slash + ".." + slash + ".." + slash + "database.sqlite";
		dao = new SQLiteDAO(dataPath);

		int i = 0;
		float tau = Mathf.PI * 2;

		foreach (Address address in dao.Addresses.ToList())
		{
			if (address.FirstTransaction.Block.Height <= StartVisualization.BlocksToShow)
			{
				// init position
				positions.Add(PolarToCartesian(new Vector3(address.FirstTransaction.Block.Height/2, Random.Range(0f, tau), Random.Range(0f, tau))));

				// init planet
				float scaleFactor = (float) address.FirstTransaction.Outputs [0].Value / 50;
				Vector3 scaleVector = new Vector3 (scaleFactor, scaleFactor, scaleFactor);

				GameObject planet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				planet.GetComponent<Renderer>().material.color = HSVToRGB (Random.Range (0f, 1f), 1f, 1f);
				planet.AddComponent<OnClickCenterView>();
				planet.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet.GetComponent<OnClickCenterView>().setBlock (address.FirstTransaction.Block.Height);
				planet.GetComponent<OnClickCenterView>().setBalance ((float) address.FirstTransaction.Outputs[0].Value);

				planet.transform.localScale += scaleVector;
				planet.transform.position = (Vector3) positions[i];
				planets.Add (planet);
				i++;
			}
		}
		numPlanets = i+1;
	}
		
	// Update is called once per frame
	void Update () {
		if (flight) {
			float step = speed * Time.deltaTime;
			foreach(GameObject go in planets) {
				OnClickCenterView other = go.GetComponent ("OnClickCenterView") as OnClickCenterView;
				go.transform.position = Vector3.MoveTowards (go.transform.position, other.getTargetPosition (), step);
			}
			OnClickCenterView posCheck = ((GameObject)planets [0]).GetComponent ("OnClickCenterView") as OnClickCenterView;
			if (((GameObject)planets [0]).transform.position == posCheck.getTargetPosition ()) {
				flight = false;
			}
			//Debug.Log ("flying: "+flight);
		}
	}

	public Vector3 PolarToCartesian(Vector3 inPolar){
		float vX = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Cos (inPolar[2]);
		float vY = inPolar [0] * Mathf.Sin (inPolar [1]) * Mathf.Sin (inPolar[2]);
		float vZ = inPolar [0] * Mathf.Cos (inPolar [1]);
		
		
		return new Vector3(vX, vY, vZ);
	}

	public void CenterView(Vector3 newCenter){
		this.newCenter = newCenter;
		foreach(GameObject temp in planets) {
			//GameObject temp = (GameObject) planets [i];
			//temp.transform.position += new Vector3 (-newCenter.x, -newCenter.y, -newCenter.z);
			//planets [i] = temp;
			OnClickCenterView other = temp.GetComponent("OnClickCenterView") as OnClickCenterView;
			other.setTargetPosition(temp.transform.position-newCenter);

		}
		flight = true;
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
