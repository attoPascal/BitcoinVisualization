using UnityEngine;
using System.Collections.Generic;
using Bitcoin;
using DAO;
using System.IO;
using System.Linq;

public class TimeLineSpawner : MonoBehaviour {

	private List<GameObject> lines;

	// Use this for initialization
	void Start () {
		lines = new List<GameObject>();

		// load data
		var slash = Path.DirectorySeparatorChar;
		var dataPath = Application.dataPath + slash + ".." + slash + ".." + slash + "database.sqlite";
		Debug.Log ("Looking for SQLite file at: " + dataPath);
		BitcoinDAO dao = new SQLiteDAO(dataPath);
		var blocks = dao.Blocks.Take(5000).ToList();
        float halfBlocks = blocks.Count / 2f;

		foreach (Block b in blocks) {
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
			line.GetComponent<BlockInfos>().TimeStamp= b.Timestamp;

			lines.Add (line);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
