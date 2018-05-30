using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SerializeField] private GameObject _spawn;
	[SerializeField] private NetworkIdentity _objectId;
	[SerializeField] private NetworkIdentity _playerId;
	[SerializeField] private MeshRenderer _renderer;
	[SerializeField] private Transform _transform;
	[SerializeField] private Rigidbody _rb;

	public override void OnStartLocalPlayer()
	{
		CameraFollow.player = _spawn.transform;
	}

	private void Awake()
	{
		_spawn = GameObject.Find("ControllableTank(Clone)");
		_objectId = _spawn.GetComponent<NetworkIdentity>();
		_playerId = GetComponent<NetworkIdentity>();
		_renderer = _spawn.GetComponentInChildren<MeshRenderer>();
		_transform = _spawn.GetComponent<Transform>();
		_rb = _spawn.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (!isLocalPlayer) { return; }

		if (_objectId.hasAuthority)
		{
			MoveTank();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			TakeControl();
		}
	}

	private void TakeControl()
	{
		if (_objectId.clientAuthorityOwner == null)
		{
			CmdGetAuth(_objectId, _playerId);
		}

		if (!_objectId.hasAuthority)
		{
			CmdGetAuth(_objectId, _playerId);
		}
	}

	private void MoveTank()
	{
		float translation = 1 * Time.deltaTime * 5.0f;
		float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 80.0f;
		Quaternion turn = Quaternion.Euler(0, rotation, 0f);

		CmdMove(translation, rotation, turn);
	}

	[Command]
	private void CmdMove(float translation, float rotation, Quaternion turn)
	{
		RpcMove(translation, rotation, turn);
	}

	[ClientRpc]
	private void RpcMove(float translation, float rotation, Quaternion turn)
	{
		
        _rb.MovePosition(_rb.position + _rb.transform.forward * translation);
        _rb.MoveRotation(_rb.rotation * turn);
	}

	[Command]
	private void CmdGetAuth(NetworkIdentity objectId, NetworkIdentity playerId)
	{
		if (objectId.clientAuthorityOwner != playerId.connectionToClient)
		{
			if (objectId.clientAuthorityOwner != null)
			{
				objectId.RemoveClientAuthority(objectId.clientAuthorityOwner);
				objectId.AssignClientAuthority(playerId.connectionToClient);
			}
		}
		objectId.AssignClientAuthority(playerId.connectionToClient);
	}

	[Command]
	private void CmdReleaseAuuth(NetworkIdentity objectId, NetworkIdentity playerId)
	{
		objectId.RemoveClientAuthority(playerId.connectionToClient);
	}
}
