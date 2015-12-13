using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility
{
	public class CameraMouseOrbiter : MonoBehaviour
	{

		private float m_LastRealTime;
	
		private float mousePosX;
		private float mousePosY;



		private bool mouseClicked = false;
		//private bool repositioning = false;

		//private Vector3 rootPosition;
		private Vector3 currentVector;
		


		// Use this for initialization
		void Start ()
		{
			//rootPosition = transform.position;
		}
		
		// Update is called once per frame
		void Update ()
		{
			float deltaTime = Time.deltaTime;
			currentVector = transform.position;

			mousePosX = CrossPlatformInputManager.GetAxis ("Mouse X");
			mousePosY = CrossPlatformInputManager.GetAxis ("Mouse Y");

			//Debug.Log ("MouseScroll "+Input.GetAxis ("Mouse ScrollWheel"));
			if (Input.GetMouseButtonDown (0))
				mouseClicked = true;
			else if (Input.GetMouseButtonUp (0))
				mouseClicked = false;

			if (mouseClicked) {
				transform.RotateAround (transform.parent.position, new Vector3 (0, 1, 0), mousePosX * 200 * deltaTime);
				transform.RotateAround (transform.parent.position, new Vector3 (1, 0, 0), mousePosY * 300 * deltaTime);
			}


			transform.Translate (-currentVector*Input.GetAxis("Mouse ScrollWheel")*2*deltaTime, Space.World);
			transform.LookAt (transform.parent.position);
		


			if (Input.GetKey(KeyCode.Escape)) {
				Application.LoadLevel ("TimelineView");
			}



			/*
			if (Input.GetMouseButtonDown (1))
				repositioning = true;

			if (repositioning) {
				transform.RotateAround (transform.parent.position, new Vector3 (0, 1, 0), rootPosition.x * 300 * deltaTime);
				transform.RotateAround (transform.parent.position, new Vector3 (1, 0, 0), rootPosition.y * 300 * deltaTime);
			}
			*/

		}
	}

}