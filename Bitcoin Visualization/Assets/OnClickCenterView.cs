using UnityEngine;
using System.Collections;


public class OnClickCenterView : MonoBehaviour {

	void OnMouseDown(){
		GameObject go = GameObject.Find("Center");
		SphereSpawner other = go.GetComponent("SphereSpawner") as SphereSpawner;
		other.CenterView(gameObject.transform.position);
	}
}
