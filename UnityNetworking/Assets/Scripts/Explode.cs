using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explode : NetworkBehaviour {

	[SyncVar]
	public uint Sender;

	// Use this for initialization
	void Start() {

		Debug.Log ("BLOW UP");

		Collider[] colliders = Physics.OverlapSphere (this.transform.position, 10);

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();
			if (rb != null && rb.gameObject.GetComponent<NetworkIdentity>().netId.Value != Sender) {
				rb.AddExplosionForce (1000, this.transform.position, 10);
				PlayerController pc = rb.gameObject.GetComponent<PlayerController> ();
				if (pc != null && pc.hasFlag) {
					pc.hasFlag = false;
					pc.AttachedFlag.transform.parent = this.transform.parent;
					pc.AttachedFlag.transform.position = pc.AttachedFlag.GetComponent<Flag> ().startPos;
					pc.AttachedFlag.GetComponent<Flag> ().pRef.Play ();
					pc.AttachedFlag = null;
				}
			}
		}

		Destroy (this.gameObject);
	}


}
