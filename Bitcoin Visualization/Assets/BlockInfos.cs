using UnityEngine;
using System.Collections;

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

	void OnMouseEnter(){

		infos = "Block Height: " + Height + "\n Transactions: " + TransactionCount + "\n TimeStamp: " + TimeStamp;
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
}

