using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkVariables : NetworkBehaviour {

	public class SyncListColor : SyncListStruct<Color>{}

	public SyncListColor PlayerColors = new SyncListColor();



	// Use this for initialization
	void Start () {
		if (!isServer) {
			return;
		}
		PlayerColors.Add (Color.red);
		PlayerColors.Add (Color.green);
		PlayerColors.Add (Color.blue);
		PlayerColors.Add (Color.magenta);
		Debug.Log (PlayerColors.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
