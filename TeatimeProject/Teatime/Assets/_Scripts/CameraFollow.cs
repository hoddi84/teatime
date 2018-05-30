using UnityEngine;

public class CameraFollow : MonoBehaviour {

	static public Transform player;
	[SerializeField] private float distance = 10;
	[SerializeField] private float height = 5;
	[SerializeField] private Vector3 lookOffset = new Vector3(0,1,0);
	[SerializeField] private float cameraSpeed = 10;
	[SerializeField] private float rotSpeed = 10;

	private void Update () 
	{
		if (player)
		{
			Vector3 lookPosition = player.position + lookOffset;
			Vector3 relativePos = lookPosition - transform.position;
			Quaternion rot = Quaternion.LookRotation(relativePos);
			
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotSpeed * 0.1f);
			
			Vector3 targetPos = player.transform.position + player.transform.up * height - player.transform.forward * distance;
			
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed * 0.1f);
		}
	}
}
