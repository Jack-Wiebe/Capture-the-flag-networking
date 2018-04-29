using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

	[SerializeField]
	GameObject explosionPrefab;
	[SerializeField]
	GameObject FlagPrefab;
	[SerializeField]
	GameObject GameEndPrefab;

	[SerializeField]
	float m_bulletSpeed;

	public Vector3 spawnPoint;

	[SyncVar]
	public bool isOver = false;
	[SyncVar]
	public bool isWinner = false;
	[SyncVar]
	public bool hasFlag = false;
	public float m_speedMod;

	[SyncVar]
	public GameObject AttachedFlag;

	private GameObject NetworkVariables;
	public GameObject UI;

	[SyncVar]
	public Color color;

	void Start()
	{
		GetComponent<MeshRenderer>().material.color = color;
	}

	public override void OnStartLocalPlayer()
	{
		UI = GameObject.FindGameObjectWithTag ("UI");
		UI.SetActive (false);

		NetworkVariables = GameObject.FindGameObjectWithTag ("NetworkVariables");

		color = NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors [NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors.Count-1];
		CmdSetColor (this.GetComponent<NetworkIdentity>().netId);



		Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);


		spawnPoint = this.transform.position;


		CmdSpawnFlag (color, spawnPoint, this.GetComponent<NetworkIdentity>().netId.Value);
		CmdSpawnGameEnd(color, spawnPoint, this.GetComponent<NetworkIdentity>().netId.Value);

	}

	private GameObject FindPlayer(NetworkInstanceId ID)
	{
		return NetworkServer.FindLocalObject (ID);
	}

	[Command]
	public void CmdSetColor(NetworkInstanceId _id)
	{
		NetworkVariables = GameObject.FindGameObjectWithTag ("NetworkVariables");
		FindPlayer(_id).GetComponent<PlayerController>().color = NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors [NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors.Count-1];

		NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors.RemoveAt(NetworkVariables.GetComponent<NetworkVariables> ().PlayerColors.Count-1);

		FindPlayer (_id).GetComponent<MeshRenderer> ().material.color = FindPlayer (_id).GetComponent<PlayerController> ().color;
	}


	[Command]
	void CmdSpawnFlag(Color _color, Vector3 _spawnPoint, uint _owner)
	{
		GameObject Flag = Instantiate (FlagPrefab, _spawnPoint + (Vector3.left * 2), this.transform.rotation);
		Flag.GetComponent<Flag> ()._color = _color;
		Flag.GetComponent<Flag> ().Owner = _owner;
		Flag.GetComponent<Flag> ().startPos = _spawnPoint + (Vector3.left * 2);

		NetworkServer.Spawn (Flag);
	}

	[Command]
	void CmdSpawnGameEnd(Color _color, Vector3 _spawnPoint, uint _owner)
	{
		GameObject GameEnd = Instantiate (GameEndPrefab, _spawnPoint + (Vector3.down), this.transform.rotation);
		GameEnd.GetComponent<GameEnd> ()._color = _color;
		GameEnd.GetComponent<GameEnd> ().Owner = _owner;

		NetworkServer.Spawn (GameEnd);
	}

	void Update()
	{
		if (!isLocalPlayer) {
			return;
		}

		if (isOver) {
			string gameOverText;
			if (isWinner) {
				gameOverText = "Congratz!!!! You Win!";
			} else {
				gameOverText = "You Lose!!!";
			}
			UI.SetActive (true);
			UI.GetComponentInChildren<Text> ().text = gameOverText;
		}

		if (hasFlag)
			m_speedMod = .4f;
		else
			m_speedMod = 1.0f;

		float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f * m_speedMod;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdExplosion ();
		}

	}

	void OnCollisionEnter()
	{

	}

	[Command]
	private void CmdExplosion()
	{

		/*Collider[] colliders = Physics.OverlapSphere (this.transform.position, 10);

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();
			if (rb != null && rb.gameObject != this.gameObject) {
				rb.AddExplosionForce (1000, this.transform.position, 10);
			}
		}*/

		//GetComponent<Rigidbody> ().AddExplosionForce (10000, this.transform.position, 500);


		GameObject explosion = GameObject.Instantiate (explosionPrefab, this.transform.position, this.transform.rotation) as GameObject;

		explosion.GetComponent<Explode> ().Sender = this.gameObject.GetComponent<NetworkIdentity>().netId.Value;

		//bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * m_bulletSpeed;

		NetworkServer.Spawn (explosion);

		//NetworkServer.Spawn (bullet);

		//Destroy (explosion, 1.0f);
	}

}
