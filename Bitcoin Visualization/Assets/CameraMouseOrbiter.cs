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

        GameObject active = null;
        GameObject lastActive = null;

		private bool mouseClicked = false;
		//private bool repositioning = false;

		//private Vector3 rootPosition;
		private Vector3 currentVector;

		//Kinect
		public GameObject BodySrcManager;
        private BodySourceManager bodyManager;
		private Body[] bodies;
		public float multiplier = -5f;
		
		public bool grab = false;
		public bool grabR = false;
		public CameraSpacePoint lastPos = new CameraSpacePoint();
		public CameraSpacePoint lastPosR = new CameraSpacePoint();
		public Quaternion lastCameraPos = new Quaternion ();

        float difference = 0; //cursor
        bool click = false;
        float clickTime = 0;
        int klickMultiplier = 0;


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
				transform.RotateAround (transform.parent.position, new Vector3 (0, 1, 0), mousePosX * 200 * deltaTime);
				transform.RotateAround (transform.parent.position, new Vector3 (1, 0, 0), mousePosY * 300 * deltaTime);
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


			//Debug.Log(gameObject.transform.rotation.x);
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
                    
                    //Cursor
                    TrackHandMovement(body);
                    //Click gesture

                    Debug.Log(body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.HandLeft].Position.Z > 0.4);
                    
                    if (body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.HandLeft].Position.Z > 0.4)
                    //if (body.Joints[JointType.HandTipRight].Position.X > body.Joints[JointType.WristRight].Position.X + 0.02)
                    {
                        
                            Debug.Log("Click");
                        if (active != null)
                        {
                            clickTime = clickTime + deltaTime;
                            if (clickTime > 0.5)
                            {
                                click = true;
                                clickTime = 0;
                            }
                        }
                    }
                    //Debug.Log(body.Joints[JointType.HandRight].TrackingState);
                    //Cursor end


                    if (body.HandLeftState == HandState.Closed){
						if(grab == false){
							lastPos = body.Joints[JointType.HandLeft].Position;
							lastCameraPos = gameObject.transform.rotation;
						}
						grab = true;
						
						var pos = body.Joints[JointType.HandLeft].Position;

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

            //raycast
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)RightHandX + 12, Screen.height-(float)RightHandY, 0));

            if (active == null)
            {
                clickTime = 0;
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {                           
                if(active != null && active != hit.transform.gameObject)
                {
                    active.GetComponent<OnClickCenterView>().OnKinectExit();
                    clickTime = 0;
                }
                active = hit.transform.gameObject;
                //active = GameObject.Find(hit.transform.gameObject.name).transform;
                //Debug.Log("HIT");
                //Debug.Log(hit.transform.gameObject);                
                active.GetComponent<OnClickCenterView>().OnKinectEnter((float)RightHandX + 12, Screen.height - (float)RightHandY);
                if(click == true)
                {
                    active.GetComponent<OnClickCenterView>().OnMouseDown();
                    click = false;
                }                
            }
            else
            {
               if (active != null)
                {
                    active.GetComponent<OnClickCenterView>().OnKinectExit();
                    active = null;
                    clickTime = 0;
                }
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.red);

        }

        public Texture curserText;

        void OnGUI()
        {
           // Rect r = new Rect((float)RightHandX, (float)RightHandY, 100, 100);

            //Debug.Log("Right Hand: " + RightHandX + " " + RightHandY);

            //GUI.DrawTexture(new Rect((float)RightHandX, (float)RightHandY, curserText.width, curserText.height), curserText);
            GUI.DrawTexture(new Rect((float)RightHandX, (float)RightHandY, 50, 50), curserText);
        }

        public double RightHandX = 0, RightHandY = 0, xPrevious = 0, yPrevious = 0;

        private void TrackHandMovement(Body body)
        {
            double MoveThreshold = 0.005;

            Windows.Kinect.Joint leftHand = body.Joints[JointType.HandLeft];
            Windows.Kinect.Joint rightHand = body.Joints[JointType.HandRight];

            Windows.Kinect.Joint leftShoulder = body.Joints[JointType.ShoulderLeft];
            Windows.Kinect.Joint rightShoulder = body.Joints[JointType.ShoulderRight];

            Windows.Kinect.Joint rightHip = body.Joints[JointType.HipRight];

            // the right hand joint is being tracked
            
            if (rightHand.TrackingState == TrackingState.Tracked)
            {
                // the hand is sufficiently in front of the shoulder
                if (rightShoulder.Position.Z - rightHand.Position.Z > 0.2)
                {
                    double xScaled = (rightHand.Position.X - leftShoulder.Position.X) / ((rightShoulder.Position.X - leftShoulder.Position.X) * 2) * Screen.width;
                    double yScaled = (rightHand.Position.Y - rightShoulder.Position.Y) / (rightHip.Position.Y - rightShoulder.Position.Y) * Screen.height;

                    // the hand has moved enough to update screen position (jitter control / smoothing)
                    if (Math.Abs(rightHand.Position.X - xPrevious) > MoveThreshold || Math.Abs(rightHand.Position.Y - yPrevious) > MoveThreshold)
                    {
                        RightHandX = xScaled;
                        RightHandY = yScaled;

                        xPrevious = rightHand.Position.X;
                        yPrevious = rightHand.Position.Y;                  
                    }
                }
            }
        }
    }
}