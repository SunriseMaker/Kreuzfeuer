using Game.Helpers;
using UnityEngine;

namespace Game
{
	public class Rocket : Bullet
	{
		[SerializeField] private GameObject _hitMark;

		private void Start()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(transform.position + Vector3.down, Vector3.down, out raycastHit))
			{
				var position = new Vector3(raycastHit.point.x, Finders.GetLevel().GroundY, raycastHit.point.z);
				Instantiate(_hitMark, position, Quaternion.identity);
			}
		}
	}
}
