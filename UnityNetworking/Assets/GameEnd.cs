using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameEnd : NetworkBehaviour {

	[SyncVar]
	public Color _color;

	[SyncVar]
	public uint Owner;

	// Use this for initialization
	void Start () {
		
	}

	void Update()
	{
		this.gameObject.GetComponent<MeshRenderer> ().material.color = _color;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.GetComponent<NetworkIdentity> () == null) {
			Debug.Log ("No Net ID");
			return;
		}
		if (col.gameObject.GetComponent<PlayerController> () == null) {
			Debug.Log ("Not Player");
			return;
		}
		if (col.gameObject.GetComponent<NetworkIdentity> ().netId.Value == Owner) {
			Debug.Log ("End");
			if (col.GetComponent<PlayerController> ().hasFlag) {
				CmdEndGame (col.gameObject.GetComponent<NetworkIdentity> ().netId.Value);
			}
		} else {
			Debug.Log ("Own End");
		}
	}

	[Command]
	private void CmdEndGame(uint _id)
	{
		GameObject[] Players = GameObject.FindGameObjectsWithTag ("Player");
		Debug.Log ("ENDING");
		foreach (GameObject player in Players) {
			if(player.GetComponent<NetworkIdentity>().netId.Value == _id)
				player.GetComponent<PlayerController> ().isWinner = true;
			player.GetComponent<PlayerController> ().isOver = true;
		}

	}

}
