using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		this.transform.LookAt (Camera.main.transform);
	}
}
