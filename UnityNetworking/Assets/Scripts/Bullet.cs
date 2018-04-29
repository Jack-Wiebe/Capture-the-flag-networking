using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField]
	int damage = 10;

	void OnCollisionEnter(Collision col)
	{
		GameObject hit = col.gameObject;
		Health health = hit.GetComponent<Health> ();
		if (health != null) {
			health.TakeDamage (damage);
		}
		Destroy (this.gameObject);
	}
}
