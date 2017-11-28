using UnityEngine;

namespace Game.Helpers
{
	public sealed class VectorBuilder
	{
		private Vector3 _vector;

		public VectorBuilder(Vector3 vector)
		{
			_vector = vector;
		}

		public VectorBuilder Normalize()
		{
			_vector = _vector.normalized;
			return this;
		}

		public VectorBuilder InvertSigns(bool invertX = true, bool invertY = true, bool invertZ = true)
		{
			_vector = new Vector3(
				(invertX ? -1 : 1) * _vector.x,
				(invertY ? -1 : 1) * _vector.y,
				(invertZ ? -1 : 1) * _vector.z
			);
			return this;
		}

		public VectorBuilder RandomizeSigns(bool randomX = true, bool randomY = true, bool randomZ = true)
		{
			_vector = new Vector3(
				(randomX ? MathEx.RandomSign() : 1) * _vector.x,
				(randomY ? MathEx.RandomSign() : 1) * _vector.y,
				(randomZ ? MathEx.RandomSign() : 1) * _vector.z
			);
			return this;
		}

		public VectorBuilder Absolute(bool absX = true, bool absY = true, bool absZ = true, float absModifier = 1.0f)
		{
			_vector = new Vector3(
				absX ? Mathf.Abs(_vector.x) * absModifier : _vector.x,
				absY ? Mathf.Abs(_vector.y) * absModifier : _vector.y,
				absZ ? Mathf.Abs(_vector.z) * absModifier : _vector.z
			);
			return this;
		}

		public VectorBuilder RandomizeWithVector(Vector3 another)
		{
			_vector = MathEx.RandomVector(_vector, another);
			return this;
		}

		public Vector3 Build()
		{
			return _vector;
		}
	}
}
