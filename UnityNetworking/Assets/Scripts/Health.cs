using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	[SerializeField]
	const int maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	[SerializeField]
	RectTransform healthbar;

	Vector3 spawnPoint;

	public bool destroyOnDeath;

	void Start()
	{
		spawnPoint = this.transform.position;
	}


	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		currentHealth -= amount;
		if (currentHealth <= 0) {

			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;
				//Debug.Log ("DEAD!");
				RpcRespawn ();
			}
		}



	}

	[ClientRpc]
	private void RpcRespawn()
	{
		if (isLocalPlayer) {
			transform.position = spawnPoint;
		}
	}

	private void OnChangeHealth(int currentHealth)
	{
		healthbar.sizeDelta = new Vector2 (currentHealth, healthbar.sizeDelta.y);
	}
}
