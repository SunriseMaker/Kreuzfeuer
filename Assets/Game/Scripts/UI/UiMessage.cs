using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class UiMessage : MonoBehaviour
	{
		[SerializeField] private Text _message;

		private Coroutine _coroutine;

		private void Awake()
		{
			Clear();
		}

		private void Clear()
		{
			_message.text = "";
		}

		/// <param name="duration">Zero = permanently</param>
		public void Display(string text, float duration = 0)
		{
			if (_coroutine != null)
				StopCoroutine(_coroutine);

			_message.text = text;

			if (duration > 0)
				_coroutine = StartCoroutine(ClearAfterDelay(duration));
		}

		private IEnumerator ClearAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			Clear();
		}
	}
}
