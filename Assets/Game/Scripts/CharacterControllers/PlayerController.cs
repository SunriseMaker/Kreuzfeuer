using System.Linq;
using Game.Data;
using Game.Enums;
using Game.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public sealed class PlayerController : MonoBehaviour
	{
		private PlayerCharacter _character;
		private readonly UserInput _userInput = new UserInput();
		private readonly Aim _aim = new Aim();
		private Camera _mainCamera;
		private PlayerHud _playerHud;
		private float _defaultAimDistance;

		private void Awake()
		{
			_character = transform.root.GetComponent<PlayerCharacter>();
		}

		private void Start()
		{
			_mainCamera = Finders.FirstObjectOfType<Camera>();
			_playerHud = Finders.FirstObjectOfType<PlayerHud>();
			CalcAimDistance();
		}

		private void CalcAimDistance()
		{
			var characterPosition = _character.transform.position;
			var cameraDistance = Vector3.Distance(characterPosition, _mainCamera.transform.position);
			var combatZoneCenter = Finders.GetLevel().ZoneCollider(Zone.Enemy).bounds.center;
			var combatZoneDistance = Vector3.Distance(characterPosition, combatZoneCenter);
			_defaultAimDistance = cameraDistance + combatZoneDistance;
		}

		private void Update()
		{
			if(_character.Health.IsDead)
				Destroy(this);
			else
			{
				GetUserInput();
				ControlCharacter();
			}
		}

		public void GetUserInput()
		{
			_userInput.Submit = Input.GetButtonDown(AxisNames.Submit);
			_userInput.Vertical = Input.GetAxis(AxisNames.Vertical);
			_userInput.Horizontal = Input.GetAxis(AxisNames.Horizontal);
			_userInput.Fire1 = Input.GetAxis(AxisNames.Fire1) > 0;
			_userInput.Evade = Input.GetButtonDown(AxisNames.Evade);
			_userInput.NextWeapon = Input.GetButtonDown(AxisNames.NextWeapon);
			_userInput.AutoAim = Input.GetAxis(AxisNames.AutoAim) > 0;
			_userInput.MousePosition = Input.mousePosition;
		}

		public void ControlCharacter()
		{
			_playerHud.SetCrossHairPosition(_userInput.MousePosition);

			UpdateAimTarget();
			UpdateAimDirection();

			_character.Mover.Move(0, _userInput.Horizontal, 0, Time.deltaTime);

			if (_userInput.Fire1)
				_character.Attack(_aim);
				
			if (_userInput.Evade)
				_character.Evade();

			if (_userInput.NextWeapon)
				_character.SelectNextWeapon();
		}

		private void UpdateAimTarget()
		{
			if (_userInput.AutoAim)
			{
				if (!_aim.TargetActor || _aim.TargetActor.Health.IsDead)
					_aim.TargetActor = FindTarget();
			}
			else
				_aim.TargetActor = null;
		}

		private void UpdateAimDirection()
		{
			var ray = _mainCamera.ScreenPointToRay(_userInput.MousePosition);

			Vector3 targetPoint;
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && raycastHit.transform.GetComponent<PlayerCharacter>() == null)
				targetPoint = raycastHit.point;
			else
				targetPoint = ray.origin + ray.direction * _defaultAimDistance;

			_aim.Direction = MathEx.Direction(_character.CurrentWeapon.BulletSpawnNode.position, targetPoint).normalized;
		}

		private Actor FindTarget()
		{
			return FindObjectsOfType<Actor>()
				.Where(x => !x.Health.IsDead && x.GetType() != typeof(PlayerCharacter))
				.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
				.FirstOrDefault();
		}
	}
}
