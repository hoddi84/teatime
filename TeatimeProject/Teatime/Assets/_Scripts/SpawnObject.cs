using UnityEngine;
using UnityEngine.Networking;

public class SpawnObject : NetworkBehaviour {

	[SerializeField] private GameObject _spawnablePrefab;

	public override void OnStartServer()
	{
		GameObject spawn = Instantiate(_spawnablePrefab);
		NetworkServer.Spawn(spawn);
	}
}
