using UnityEngine;
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
	public bool play = false;
	private float expDT = 0;
	float tau = Mathf.PI * 2;
	GameObject genesis;
	Vector3 genesisPos = new Vector3(0,0,0);

	public Texture FBITexture;
	public Texture PizzaTexture;
	public Texture DollarTexture;

	int dollarNote = -1;
	int pizzaBox = -1;

	// Use this for initialization
	void Start () {
		var slash = Path.DirectorySeparatorChar;
		var dataPath = Application.dataPath + slash + "data" + slash + StartVisualization.dataset;
		dao = new SQLiteDAO(dataPath);

		int i = 0;
		float tau = Mathf.PI * 2;


		if (StartVisualization.exp) {
			play = true;
		}
		var numBlocks = StartVisualization.BlocksToShow;
		var addresses =
			from a in dao.Addresses
		/*	where a.FirstOccurrenceBlockHeight <= numBlocks && a.FirstOccurrenceBlockHeight > 0
			orderby a.FirstOccurrenceBlockHeight*/
			select a;
        if (play)
        {
            addresses = addresses.OrderBy(a => a.FirstOccurrenceBlockHeight);
        }

		// TODO: check if ToList makes it better or worse
		foreach (var address in addresses.ToList())
		{



            // init planet
            var balance = 0.0;
            float scaleFactor= 0f;
            if (play)
            {
                balance = address.BalanceAfterBlock(numBlocks);
                scaleFactor = (float) balance / 50;
            }
            else {
                balance = address.Profit;
                scaleFactor = (float)Mathf.Pow((float)address.Profit, 1 / 3f);
            }
			Vector3 scaleVector = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
            Vector3 pizzaScaleVector = new Vector3(scaleFactor, scaleFactor, 1);


            // Pizza Planet
            if (address.ID.Equals("17SkEw2md5avVNyYgj6RiXuQKNwkXaxFyQ")) {
				Debug.Log ("Found Laszlo's Pizza Address");
				GameObject planet = GameObject.Find ("PizzaBox");
				GameObject planet2 = GameObject.Find ("PizzaLayoutFront");
				GameObject planet3 = GameObject.Find ("PizzaLayoutBack");
				// init position
				positions.Add(PolarToCartesian(new Vector3(Random.Range(100f,500f), Random.Range(0f, tau), Random.Range(0f, tau))));
                planet.transform.position = (Vector3)positions[i];


                planet.transform.position = (Vector3) positions[i];

                planet.AddComponent<OnClickCenterView>();
				planet.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet.GetComponent<OnClickCenterView>().setBlock (address.FirstOccurrenceBlockHeight);
				planet.GetComponent<OnClickCenterView>().setBalance ((float) address.Profit);

                planet2.AddComponent<OnClickCenterView>();
                planet2.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet2.GetComponent<OnClickCenterView>().setBlock (address.FirstOccurrenceBlockHeight);
				planet2.GetComponent<OnClickCenterView>().setBalance ((float) address.Profit);

                planet3.AddComponent<OnClickCenterView>();
                planet3.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet3.GetComponent<OnClickCenterView>().setBlock (address.FirstOccurrenceBlockHeight);
				planet3.GetComponent<OnClickCenterView>().setBalance ((float) address.Profit);

                planet.transform.localScale = (pizzaScaleVector);
                planets.Add (planet);

                planet.GetComponent<Renderer>().enabled = true;
                planet2.GetComponent<Renderer>().enabled = true;
                planet3.GetComponent<Renderer>().enabled = true;

                pizzaBox =i;

				i++;

			}
            
			// Largest Transaction ever
			else if (address.ID.Equals("1M8s2S5bgAzSSzVTeL7zruvMPLvzSkEAuv")) {
				Debug.Log ("Largest Transaction ever");
				GameObject planet = GameObject.Find ("Dollar");
				GameObject planet2 = GameObject.Find("DollarBack");

           

                //planet.GetComponent<Renderer>().material.mainTexture = DollarTexture;
                // init position
                positions.Add(PolarToCartesian(new Vector3(Random.Range(100f, 500f), Random.Range(0f, tau), Random.Range(0f, tau))));
                planet.transform.position = (Vector3)positions[i];

                planet.AddComponent<OnClickCenterView>();
				planet.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet.GetComponent<OnClickCenterView>().setBlock (address.FirstOccurrenceBlockHeight);
				planet.GetComponent<OnClickCenterView>().setBalance ((float) address.Profit);

                planet2.AddComponent<OnClickCenterView>();
				planet2.GetComponent<OnClickCenterView>().setAddress (address.ID);
				planet2.GetComponent<OnClickCenterView>().setBlock (address.FirstOccurrenceBlockHeight);
				planet2.GetComponent<OnClickCenterView>().setBalance ((float) address.Profit);

               // scaleVector.x = Mathf.Pow(scaleVector.x, 10);
                //scaleVector.y = Mathf.Pow(scaleVector.y, 10);
                //scaleVector.z = Mathf.Pow(scaleVector.z, 10);


                planet.transform.localScale = scaleVector/4;

                planets.Add (planet);
                planets.Add(planet2);

                planet.GetComponent<Renderer>().enabled = true;
                planet2.GetComponent<Renderer>().enabled = true;


                dollarNote = i;

				i++;

			}
			else{
				GameObject planet = GameObject.CreatePrimitive (PrimitiveType.Sphere);

				if(StartVisualization.boringcolors) planet.GetComponent<Renderer>().material.color = Color.grey;
				else planet.GetComponent<Renderer>().material.color = HSVToRGB (Random.Range (0f, 1f), 1f, 1f);

                // init position
                if (play)
                {
                    positions.Add(PolarToCartesian(new Vector3(address.FirstOccurrenceBlockHeight/2, Random.Range(0f, tau), Random.Range(0f, tau))));
                    planet.transform.position = (Vector3)positions[i];

                    planet.AddComponent<OnClickCenterView>();
                    planet.GetComponent<OnClickCenterView>().setAddress(address.ID);
                    planet.GetComponent<OnClickCenterView>().setBlock(address.FirstOccurrenceBlockHeight);
                    planet.GetComponent<OnClickCenterView>().setBalance((float)balance);
                }
                else
                {
                    positions.Add(PolarToCartesian(new Vector3(Random.Range(100f, 500f), Random.Range(0f, tau), Random.Range(0f, tau))));
                    planet.transform.position = (Vector3)positions[i];

                    planet.AddComponent<OnClickCenterView>();
                    planet.GetComponent<OnClickCenterView>().setAddress(address.ID);
                    planet.GetComponent<OnClickCenterView>().setBlock(address.FirstOccurrenceBlockHeight);
                    planet.GetComponent<OnClickCenterView>().setBalance((float)address.Profit);
                }
				planet.transform.localScale = scaleVector;
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
		if (play) {
			//expansion of Universe
			expDT += Time.deltaTime;
			if (expDT >= 0.5f) {
				int i = 0;
				StartVisualization.BlocksToShow += 10;
				var numBlocks = StartVisualization.BlocksToShow;
				var addresses =
				from a in dao.Addresses
				where a.FirstOccurrenceBlockHeight <= numBlocks //&& a.FirstOccurrenceBlockHeight > 0
				orderby a.FirstOccurrenceBlockHeight
				select a;
				foreach (var address in addresses.ToList()) {
					if (i < planets.Count) { //change size and balance of currently shown planet
						float newBal = (float)address.BalanceAfterBlock (numBlocks);
						planets [i].GetComponent<OnClickCenterView> ().setBalance (newBal);
						float scaleFactor = (float)newBal / 50;
						Vector3 scaleVector = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
						planets [i].transform.localScale = scaleVector;
					} else { // add new Planet
						var balance = address.BalanceAfterBlock (numBlocks);
						float scaleFactor = (float)balance / 50;
						Vector3 scaleVector = new Vector3 (scaleFactor, scaleFactor, scaleFactor);

						GameObject planet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
						if (StartVisualization.boringcolors) {
							planet.GetComponent<Renderer> ().material.color = HSVToRGB (0f, 0f, 50f);
						} else {
							planet.GetComponent<Renderer> ().material.color = HSVToRGB (Random.Range (0f, 1f), 1f, 1f);
						}
						planet.AddComponent<OnClickCenterView> ();
						planet.GetComponent<OnClickCenterView> ().setAddress (address.ID);
						planet.GetComponent<OnClickCenterView> ().setBlock (address.FirstOccurrenceBlockHeight);
						planet.GetComponent<OnClickCenterView> ().setBalance ((float)balance);

						planet.transform.localScale = scaleVector;
						planet.transform.position = PolarToCartesian (new Vector3 (address.FirstOccurrenceBlockHeight / 2, Random.Range (0f, tau), Random.Range (0f, tau)));
						planets.Add (planet);
					}
					i++;
				}
				expDT = 0f;
			}
		}
		if (dollarNote >= 0) {
			planets [dollarNote].transform.Rotate(new Vector3(this.transform.rotation.x + Time.deltaTime * 20, 0,0));
		}
		if (pizzaBox >= 0) {
			planets [pizzaBox].transform.Rotate(new Vector3(this.transform.rotation.x+Time.deltaTime*20,this.transform.rotation.y+Time.deltaTime*20,this.transform.rotation.z+Time.deltaTime*20));
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
