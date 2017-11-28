using Game;
using Game.Data;
using Game.Helpers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private GameObject _selfDestructionEffect;

	protected int Damage;
	protected Aim Aim;
	
	private float _lifeTime;
	private float _spawnTime;

	public void Initialize(int damage, float lifeTime, Aim aim)
	{
		_spawnTime = Time.timeSinceLevelLoad;
		Damage = damage;
		Aim = aim.Copy();
		_lifeTime = lifeTime;
	}

	protected virtual void FixedUpdate()
	{
		if (Time.timeSinceLevelLoad - _spawnTime < _lifeTime)
			return;

		SelfDestruction();
	}

	protected void OnCollisionEnter(Collision collision)
	{
		var character = collision.transform.GetComponent<Actor>();

		if (character == null)
			collision.transform.root.GetComponent<Actor>();

		if (character != null)
			character.Health.Damage(Damage);

		SelfDestruction();
	}

	private void SelfDestruction()
	{
		if(_selfDestructionEffect)
			Instantiate(_selfDestructionEffect, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
}
