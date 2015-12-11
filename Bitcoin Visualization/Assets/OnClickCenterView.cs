using UnityEngine;
using System.Collections;


public class OnClickCenterView : MonoBehaviour {

	private Color startColor;

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
		
	}

	void OnMouseExit(){
		gameObject.GetComponent<Renderer> ().material.color = startColor;
	}
}
