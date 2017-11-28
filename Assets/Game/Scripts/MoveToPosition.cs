using System;
using UnityEngine;

namespace Game
{
	public class MoveToPosition : MonoBehaviour
	{
		[SerializeField] private Vector3 _targetPosition;
		[SerializeField] private float _speed;

		private void FixedUpdate()
		{
			const float DISTANCE_ERROR = 0.05f;
			transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

			if (Vector3.Distance(transform.position, _targetPosition) < DISTANCE_ERROR)
				Destroy(this);
		}
	}
}
