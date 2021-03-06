using UnityEngine;
using System.Collections;
using System;

public class BlockInfos : MonoBehaviour
{
	public int Height;
	public int TransactionCount;
	public int TimeStamp;

	bool _guiOn = false;
	string infos;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnMouseDown(){
		StartVisualization.BlocksToShow = Height;
		Debug.Log (StartVisualization.BlocksToShow);
		Application.LoadLevel("SphereView");
	}
	void OnMouseEnter(){

		infos = "Block Height: " + Height + "\n Transactions: " + TransactionCount + "\n TimeStamp: " + UnixTimeStampToDateTime(TimeStamp).ToString();
		_guiOn = true;
	}
	
	void OnMouseExit(){

		_guiOn = false;
	}
	void OnGUI(){
		if (_guiOn) {
			float boxX=0f;
			float boxY=0f;
			if(Input.mousePosition.x+370<Screen.width){
				boxX=Input.mousePosition.x+20;
			}
			else{
				boxX = Input.mousePosition.x-370;
			}
			if(Screen.height-Input.mousePosition.y+50>Screen.height){
				boxY=Screen.height - Input.mousePosition.y-50;
			}
			else{
				boxY = Screen.height-Input.mousePosition.y;
			}
			GUI.Box (new Rect (boxX, boxY, 350, 50), infos);
		}
	}
	public static System.DateTime UnixTimeStampToDateTime( int unixTimeStamp )
	{
		// Unix timestamp is seconds past epoch
		System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
		return dtDateTime;
	}
}

