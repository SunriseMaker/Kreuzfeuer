using Game.Data;
using Game.Enums;
using Game.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] Button _startGameButton;
	[SerializeField] Button _quitButton;
	[SerializeField] Button _clearStatsButton;
	[SerializeField] Toggle _infiniteWavesToggle;
	[SerializeField] Toggle _characterToggle;

	private void Awake()
	{
		Debug.Assert(_startGameButton);
		Debug.Assert(_quitButton);
		Debug.Assert(_clearStatsButton);
		Debug.Assert(_infiniteWavesToggle);
		Debug.Assert(_characterToggle);

		_startGameButton.onClick.AddListener(StartGame);
		_quitButton.onClick.AddListener(QuitGame);

		_characterToggle.isOn = SaveController.Get<int>(Save.CharacterIndex).ToBool();
		_characterToggle.onValueChanged.AddListener(OnPlayerCharacterChanged);

		_infiniteWavesToggle.isOn = SaveController.Get<int>(Save.InfiniteWaves).ToBool();
		_infiniteWavesToggle.onValueChanged.AddListener(OnInfiniteWavesChanged);

		if (SaveController.Get<int>(Save.MaxWave) == 0)
			DestroyClearStatsButton();
		else
			_clearStatsButton.onClick.AddListener(ClearStats);
	}

	private static void OnInfiniteWavesChanged(bool value)
	{
		SaveController.Set(Save.InfiniteWaves, value.ToInt());
	}

	public static void OnPlayerCharacterChanged(bool value)
	{
		SaveController.Set(Save.CharacterIndex, value.ToInt());
	}

	public static void StartGame()
	{
		SceneManager.LoadScene(Scenes.FirstLevel);
	}

	public static void QuitGame()
	{
		Application.Quit();
	}

	public void ClearStats()
	{
		SaveController.Clear();
		DestroyClearStatsButton();
	}

	private void DestroyClearStatsButton()
	{
		Destroy(_clearStatsButton.gameObject);
	}
}
