using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGreeting : MonoBehaviour
{
	[Header("Greeting")]
	[SerializeField] private float _delay;
	[SerializeField] private Text _text;
	[SerializeField] private string _message;
	[SerializeField] private float _blinkInterval;

	[Header("Menu")]
	[SerializeField] private GameObject _menuNode;

	private bool _keyPressed;
	private bool _canCheckUserInput;

	private void Awake()
	{
		Debug.Assert(_text);
		Debug.Assert(_menuNode);

		_text.text = "";
		_menuNode.SetActive(false);
		StartCoroutine(Greeting());
	}

	private void Update()
	{
		if (_canCheckUserInput)
			_keyPressed = Input.anyKey || _keyPressed;
	}

	private IEnumerator Greeting()
	{
		yield return new WaitForSeconds(_delay);

		_canCheckUserInput = true;
		var visible = false;

		while (!_keyPressed)
		{
			visible = !visible;
			_text.text = visible ? _message : "";
			yield return new WaitForSeconds(_blinkInterval);
		}

		_menuNode.SetActive(true);
		Destroy(_text.gameObject);
		Destroy(this);
	}
}
