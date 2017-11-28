using System;
using UnityEngine;

namespace Game
{
	public abstract class CoroutineHandler
	{
		private bool _isRunning;
		private bool _isCancelling;

		protected readonly MonoBehaviour MonoBehaviour;
		protected readonly Action OnStop;
		protected readonly Func<bool> CancelFunc;
		private Coroutine _coroutine;

		protected CoroutineHandler(MonoBehaviour monoBehaviour)
		{
			MonoBehaviour = monoBehaviour;
			OnStop = () =>
			{
				_isRunning = false;
				_isCancelling = false;
			};
			CancelFunc = () => _isCancelling;
		}

		public void Start(bool onlyOne)
		{
			if (onlyOne && _isRunning)
				return;

			_isRunning = true;
			_coroutine = Execute();
		}

		protected abstract Coroutine Execute();

		public void Stop()
		{
			if(_isCancelling || _coroutine == null)
				return;

			_isCancelling = true;
		}
	}
}
