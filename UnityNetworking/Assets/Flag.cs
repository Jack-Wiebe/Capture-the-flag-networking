using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Flag : NetworkBehaviour {

	[SyncVar]
	public Vector3 startPos;

	public ParticleSystem pRef;
	public GameObject FlagTop;
	[SyncVar]
	public Color _color;

	[SyncVar]
	public uint Owner;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		FlagTop.GetComponent<MeshRenderer> ().material.color = _color;
		pRef.startColor = _color;
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
		if (col.gameObject.GetComponent<NetworkIdentity> ().netId.Value != Owner) {
			CmdPickUpFlag (col.gameObject);
			col.gameObject.GetComponent<PlayerController> ().hasFlag = true;
			col.gameObject.GetComponent<PlayerController> ().AttachedFlag = this.gameObject;
			pRef.Stop ();
			this.gameObject.transform.parent = col.gameObject.transform;
			Debug.Log ("Flag");
		} else {
			Debug.Log ("Own Flag");
		}
	}

	[Command]
	void CmdPickUpFlag(GameObject pickup)
	{
		pickup.GetComponent<PlayerController> ().hasFlag = true;
		pickup.GetComponent<PlayerController> ().AttachedFlag = this.gameObject;
		pRef.Stop ();
		this.gameObject.transform.parent = pickup.transform;
	}
}
