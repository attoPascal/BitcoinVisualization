using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class OnClickCenterView : MonoBehaviour {

	private Color startColor;
	private string address;
	private int block;
	private float balance;
	public string infos = "";
	private bool _guiOn = false;

	public void setAddress(string address){
		this.address = address;
	}
	public void setBlock(int block){
		this.block = block;
	}
	public void setBalance(float balance){
		this.balance = balance;
	}
	public string getInfos(){
		return infos;
	}

	void OnMouseDown(){
		GameObject go = GameObject.Find("Center");
		SphereSpawner other = go.GetComponent("SphereSpawner") as SphereSpawner;
		other.CenterView(gameObject.transform.position);
	}
	void OnMouseEnter(){
		startColor = gameObject.GetComponent<Renderer> ().material.color;
		float h=0f, s=0f, v=0f;
		UnityEditor.EditorGUIUtility.RGBToHSV (startColor,out h,out s,out v);
		s = s / 2; 
		gameObject.GetComponent<Renderer> ().material.color = UnityEditor.EditorGUIUtility.HSVToRGB(h, s, v);
		infos = "Address: " + address + "\n Block Height: " + block + "\n Balance: " + balance;
		_guiOn = true;
	}

	void OnMouseExit(){
		gameObject.GetComponent<Renderer> ().material.color = startColor;
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
