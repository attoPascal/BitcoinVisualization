using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Windows.Kinect;


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

		//Kinect
		public GameObject BodySrcManager;
		public JointType TrackedJoint;
		private BodySourceManager bodyManager;
		private Body[] bodies;
		public float multiplier = -5f;
		
		public bool grab = false;
		public bool grabR = false;
		public CameraSpacePoint lastPos = new CameraSpacePoint();
		public CameraSpacePoint lastPosR = new CameraSpacePoint();
		public Quaternion lastCameraPos = new Quaternion ();

		// Use this for initialization
		void Start ()
		{
			//rootPosition = transform.position;
			if (BodySrcManager == null) {
				Debug.Log ("Assign Game Object with Body Source manager");
			} else {
				bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
			}
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
				//transform.RotateAround (transform.parent.position, new Vector3 (0, 1, 0), mousePosX * 200 * deltaTime);
				//transform.RotateAround (transform.parent.position, new Vector3 (1, 0, 0), mousePosY * 300 * deltaTime);
				transform.position = Quaternion.Euler(mousePosY*200*deltaTime, mousePosX*300*deltaTime,0) *  transform.position;
				//transform.position = Quaternion.AngleAxis(mousePosX*300*deltaTime, gameObject.transform.up) * transform.position;
				transform.LookAt(transform.parent.position);
			}


			transform.Translate (-currentVector*Input.GetAxis("Mouse ScrollWheel")*2*deltaTime, Space.World);
			transform.LookAt (transform.parent.position, transform.up);
		


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


			Debug.Log(gameObject.transform.rotation.x);
			//Kinect
			if (bodyManager == null) {
				return;
			}
			bodies = bodyManager.GetData ();
			
			if (bodies == null) {
				return;
			}
			
			foreach (var body in bodies) {
				if (body == null){
					continue;
				}
				if (body.IsTracked)
				{
					
					Debug.Log(body.HandLeftState.ToString());
					
					if (body.HandLeftState == HandState.Closed){
						if(grab == false){
							lastPos = body.Joints[TrackedJoint].Position;
							lastCameraPos = gameObject.transform.rotation;
						}
						grab = true;
						
						var pos = body.Joints[TrackedJoint].Position;

						transform.RotateAround (transform.parent.position, new Vector3 (0, 1, 0), (pos.X - lastPos.X) * multiplier);
						transform.RotateAround (transform.parent.position, new Vector3 (1, 0, 0), (pos.Y - lastPos.Y) * multiplier);


						//transform.Translate (-currentVector*(pos.Z - lastPos.Z), Space.World);

						transform.LookAt (transform.parent.position, transform.up);
					}
					else if (body.HandLeftState == HandState.Open){
						grab = false;
					}

					if(body.HandRightState == HandState.Closed){
						if(grabR == false){
							lastPosR = body.Joints[JointType.HandRight].Position;
						}
						grabR = true;

						transform.Translate (-currentVector*((body.Joints[JointType.HandRight].Position.Z - lastPosR.Z)/4), Space.World);
					}
					else if (body.HandRightState == HandState.Open){
						grabR = false;
					}
				}
			}
		}
	}
}