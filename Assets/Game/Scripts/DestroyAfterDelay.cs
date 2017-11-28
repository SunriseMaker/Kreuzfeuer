using UnityEngine;

public sealed class DestroyAfterDelay : MonoBehaviour
{
	[SerializeField] private float _delay;

	private void Start()
	{
		Debug.Assert(_delay > 0);
		Destroy(gameObject, _delay);
	}
}