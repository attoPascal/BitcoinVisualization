using UnityEngine;
using System.Collections;

public class StartVisualization : MonoBehaviour {

	public static int BlocksToShow=5;
	public static bool boringcolors = true;
	public static bool exp = false;
    public static string dataset = "first-1000.sqlite";
    // Use this for initialization
    void Start () {

	}
	/*void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}*/
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			Application.LoadLevel ("TimelineView");
		}
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKey(KeyCode.Alpha2)) {
			BlocksToShow = 1000;
            dataset = "first-1000.sqlite";
            exp = false;
            Application.LoadLevel("SphereView");
            
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
			exp = true;
			BlocksToShow = 1;
            dataset = "first-1000.sqlite";
			Application.LoadLevel("SphereView");
		}
        if (Input.GetKey(KeyCode.Alpha4))
        {
            dataset = "pizza-1000.sqlite";
            Application.LoadLevel("SphereView");
            exp = false;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            exp = false;
            dataset = "biggest-tx-20.sqlite";
            Application.LoadLevel("SphereView");
        }
		if (Input.GetKey (KeyCode.Tab)) {
			if(boringcolors)boringcolors = false;
			else boringcolors = true;
		}
	}
	void OnGUI(){
		string colors = "grey";
		if (!boringcolors) {
			colors = "tru colors";
		}
		string message = "press 1 to view Timeline (deprecated)" +
			"\n \n press 2 to view static Universe"
			+ "\n \n press 3 to view dynamic Universe (highly experimental)"
			+"\n \n press 4 to view static view of time around first Pizza transaction"
            +"\n \n press 5 to view static view around time of biggest transaction ever!"
            + "\n \n press Tab to switch between 'tru colors' and grey. Currently selected: " + colors
			+"\n \n press escape to quit";
				GUI.Box (new Rect (Screen.width/2-250, Screen.height/2-75, 500, 200), message);
	}
}
