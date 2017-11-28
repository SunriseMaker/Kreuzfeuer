using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Helpers
{
	public static class Scenes
	{
		public const int MainMenu = 0;
		public const int FirstLevel = 1;

		public static int GetCurrentSceneIndex()
		{
			return SceneManager.GetActiveScene().buildIndex;
		}

		public static IEnumerator StartNextLevel(float delay)
		{
			yield return new WaitForSeconds(delay);
			var currentIndex = GetCurrentSceneIndex();
			var nextIndex = currentIndex == SceneManager.sceneCountInBuildSettings - 1 ? 0 : currentIndex + 1;
			SceneManager.LoadScene(nextIndex);
		}

		public static IEnumerator RestartLevel(float delay)
		{
			yield return new WaitForSeconds(delay);
			SceneManager.LoadScene(GetCurrentSceneIndex());
		}
	}
}
