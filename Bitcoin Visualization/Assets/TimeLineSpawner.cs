using UnityEngine;
using System.Collections;
using Bitcoin;
using DAO;
using System.IO;
using System.Linq;

public class TimeLineSpawner : MonoBehaviour {

	private ArrayList lines;
	string dataPath = "";
	BitcoinDAO input;

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			dataPath = @"../data/1000blocks.db";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			dataPath = @"..\data\1000blocks.db";
			Debug.Log ("detected Windows OS");
		}
		input = new SQLiteDAO(dataPath);
		lines = new ArrayList ();
        var blocks = input.Blocks.ToList();
        float halfBlocks = blocks.Count / 2f;
		foreach (Block b in input.Blocks) {
			GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			float blockTransactions = (float) b.Transactions.Count;
			line.transform.localScale = new Vector3(0.2f, blockTransactions*2, 0.2f);
			line.transform.position = new Vector3((b.Height-halfBlocks)*0.2f, -blockTransactions, 0);
			line.GetComponent<MeshRenderer>().enabled=false;


			Vector3 top = new Vector3((b.Height-halfBlocks)*0.2f, blockTransactions, 0);
			Vector3 bottom = new Vector3((b.Height-halfBlocks)*0.2f, -blockTransactions, 0);

			LineRenderer lRend = line.AddComponent<LineRenderer>() as LineRenderer;
			lRend.material = new Material(Shader.Find("Particles/Additive"));
			lRend.SetVertexCount(2);
			lRend.SetColors(Color.red, Color.green);
			lRend.SetPosition (0, top);
			lRend.SetPosition(1, bottom);
			lRend.SetWidth(0.2f,0.2f);

			line.AddComponent<BlockInfos>();
			line.GetComponent<BlockInfos>().Height = b.Height;
			line.GetComponent<BlockInfos>().TransactionCount = b.Transactions.Count;
			line.GetComponent<BlockInfos>().TimeStamp=b.Timestamp;

			lines.Add (line);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
