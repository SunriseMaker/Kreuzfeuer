using Game.Helpers;
using UnityEngine;

namespace Game
{
	public sealed class BlinkEffect : CoroutineHandler
	{
		private readonly Color _color;
		private readonly int _count;
		private readonly float _interval;
		private readonly GameObject _gameObject;

		public BlinkEffect(Color color, int count, float interval, GameObject gameObject, MonoBehaviour monoBehaviour) : base(monoBehaviour)
		{
			_color = color;
			_count = count;
			_interval = interval;
			_gameObject = gameObject;
		}

		protected override Coroutine Execute()
		{
			return MonoBehaviour.StartCoroutine(VisualEffects.Blink(_color, _count, _interval, _gameObject, OnStop, CancelFunc));
		}
	}
}
