using Game;
using Game.Enums;
using Game.Helpers;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] [Range(0, 0.5f)] private float _screenXoffset;
	[SerializeField] private Vector3 _playerOffset;

	private PlayerCharacter _playerCharacter;
	private Vector3 _center;
	

	private void Start()
	{
		_playerCharacter = Finders.GetPlayerCharacter();
		transform.position = _playerCharacter.transform.position + _playerOffset;
		_center = Finders.GetLevel().ZoneCollider(Zone.Player).bounds.center;
	}

	private void LateUpdate()
	{
		if(!_playerCharacter)
			return;
		
		var playerPosition = _playerCharacter.transform.position;
		var distance = Vector3.Distance(_center, playerPosition);
		var sign = Mathf.Sign(playerPosition.x - _center.x);
		var cameraPosition = _center + new Vector3(_playerOffset.x + sign * distance * _screenXoffset, _playerOffset.y, _playerOffset.z);
		transform.position = Vector3.MoveTowards(transform.position, cameraPosition, _speed * Time.deltaTime);
	}
}
