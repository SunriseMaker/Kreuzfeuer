using UnityEngine;

namespace Game
{
	public sealed class ConstantRotation : MonoBehaviour
	{
		[SerializeField] private Vector3 _degreesPerSecond;
		private void FixedUpdate()
		{
			transform.Rotate(_degreesPerSecond * Time.deltaTime);
		}
	}
}
