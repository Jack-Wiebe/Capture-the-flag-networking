using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform playerTransform;
	public int depth = -20;

	// Update is called once per frame
	void Update()
	{
		if(playerTransform != null)
		{
			
		}
	}

	public void setTarget(Transform target)
	{
		playerTransform = target;
		transform.position = playerTransform.position + new Vector3(0,4,depth);
		transform.parent = target;
	}
}