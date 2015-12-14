using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Windows.Kinect;


public class TimelineCamera : MonoBehaviour
{
	private float mousePosX;
	private bool mouseClicked = false;

	private Vector3 translateVector;

	//Kinect
	public GameObject BodySrcManager;
	public JointType TrackedJoint;
	private BodySourceManager bodyManager;
	private Body[] bodies;
	public float multiplier = 10f;

	public bool grab = false;
	public CameraSpacePoint lastPos = new CameraSpacePoint();
	public Vector3 lastObjPos = new Vector3();

	// Use this for initialization
	void Start ()
	{
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

		mousePosX = CrossPlatformInputManager.GetAxis ("Mouse X");
		translateVector.Set (-deltaTime*50*mousePosX, 0, 0);

		if (Input.GetMouseButtonDown (0))
			mouseClicked = true;
		else if (Input.GetMouseButtonUp (0))
			mouseClicked = false;


		if (mouseClicked) {
			transform.Translate(translateVector);
		}



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
						lastObjPos = gameObject.transform.position;
					}
					grab = true;

					var pos = body.Joints[TrackedJoint].Position;

					Debug.Log(pos.X-lastPos.X);
					transform.position = new Vector3(lastObjPos.x + (pos.X - lastPos.X) * multiplier, 1, -10);
					//translateVector.Set (deltaTime*50*pos.X-lastPos.X , 0, 0);
					//transform.Translate(translateVector);

				}
				else if (body.HandLeftState == HandState.Open){
					grab = false;
				}
			}
		}
	}
}

