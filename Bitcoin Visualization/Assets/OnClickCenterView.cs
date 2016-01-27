using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class OnClickCenterView : MonoBehaviour {

	private Color startColor, startColorKinect;
	private string address;
	private int block;
	private float balance;
	public string infos = "";
	private bool _guiOn = false, _kinectGuiOn = false;

    float x = 0, y = 0;

	private Vector3 targetPosition;

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
	public void setTargetPosition(Vector3 newTarget){
		targetPosition = newTarget;
	}
	public Vector3 getTargetPosition(){
		return targetPosition;
	}

	public void OnMouseDown(){
		GameObject go = GameObject.Find("Center");
		SphereSpawner other = go.GetComponent("SphereSpawner") as SphereSpawner;
		other.CenterView(gameObject.transform.position);
	}
	void OnMouseEnter(){
		startColor = gameObject.GetComponent<Renderer> ().material.color;
		Vector3 hsvColor = RGBToHSV (startColor);

		float hue=hsvColor.x, sat=hsvColor.y, val=hsvColor.z;

		sat = sat / 2;

		gameObject.GetComponent<Renderer> ().material.color = HSVToRGB(hue, sat, val);
		infos = "Address: " + address + "\n Block Height: " + block + "\n Balance: " + balance;
		_guiOn = true;
	}
    public void OnKinectEnter(float x, float y)
    {
        this.x = x;
        this.y = y;
        startColorKinect = gameObject.GetComponent<Renderer>().material.color;
        Vector3 hsvColor = RGBToHSV(startColorKinect);

        float hue = hsvColor.x, sat = hsvColor.y, val = hsvColor.z;

        sat = sat / 2;

        gameObject.GetComponent<Renderer>().material.color = HSVToRGB(hue, sat, val);
        infos = "Address: " + address + "\n Block Height: " + block + "\n Balance: " + balance;
        _kinectGuiOn = true;
    }
    public void OnKinectExit()
    {
        gameObject.GetComponent<Renderer>().material.color = startColorKinect;
        _kinectGuiOn = false;
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
        if (_kinectGuiOn)
        {
            float boxX = 0f;
            float boxY = 0f;
            if (x + 370 < Screen.width)
            {
                boxX = x + 50;
            }
            else {
                boxX = x - 350;
            }
            if (Screen.height - y + 50 > Screen.height)
            {
                boxY = Screen.height - y - 50;
            }
            else {
                boxY = Screen.height - y;
            }

            GUI.Box(new Rect(boxX, boxY, 350, 50), infos);
        }
	}
	public Vector3 RGBToHSV(Color rgb){
		float h, s, v;

		float min, max, delta;
		//Debug.Log ("R: " + rgb.r + "G: " + rgb.g + "B: " + rgb.b);
		min = Mathf.Min (Mathf.Min (rgb.r, rgb.g), rgb.b);
		max = Mathf.Max (Mathf.Max (rgb.r, rgb.g), rgb.b);

		v = max;
		delta = max - min;

		if (max != 0) {
			s = delta / max;
		} else {
			s = 0;
			h = -1;
			return new Vector3(h,s,v);
		}

		if (rgb.r == max) {
			h = (rgb.g-rgb.b)/delta;
		}else if(rgb.g == max){
			h = 2 + (rgb.b-rgb.r)/delta;
		}else {
			h = 4 + (rgb.r-rgb.g)/delta;
		}
		h = h*60;

		if(h<0){
			h+=360;
		}
		//Debug.Log ("hue: "+h + "saturation: "+s+"value: "+v);
		return new Vector3 (h/360, s, v);

	}
	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f) {
			return new Color (V, V, V);
		}
		else if (V == 0f){
			Color ret = new Color();
			ret= Color.black;
			return ret;
		}else{
			Color col = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 1:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			case 2:
				col.r = v2;
				col.g = V;
				col.b = v1;
				break;
			case 3:
				col.r = v1;
				col.g = V;
				col.b = v3;
				break;
			case 4:
				col.r = v1;
				col.g = v2;
				col.b = V;
				break;
			case 5:
				col.r = v3;
				col.g = v1;
				col.b = V;
				break;
			case 6:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 7:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			}
			col.r = Mathf.Clamp(col.r, 0f, 1f);
			col.g = Mathf.Clamp(col.g, 0f, 1f);
			col.b = Mathf.Clamp(col.b, 0f, 1f);
			return col;
		}
	}
}
	