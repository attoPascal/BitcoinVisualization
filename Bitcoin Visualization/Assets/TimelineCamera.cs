using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class TimelineCamera : MonoBehaviour
{
	private float mousePosX;
	private bool mouseClicked = false;

	private Vector3 translateVector;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float deltaTime = Time.deltaTime;

		mousePosX = CrossPlatformInputManager.GetAxis ("Mouse X");
		translateVector.Set (-deltaTime*50*mousePosX, 0, 0);

		if (Input.GetMouseButtonDown (0))
			mouseClicked = true;
		else if (Input.GetMouseButtonUp (0))
			mouseClicked = false;


		if (mouseClicked) {
			transform.Translate(translateVector);
		}

	}
}

