using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game.Helpers
{
	public static class VisualEffects
	{
		private const string EMISSION_PARAMETER = "_EMISSION";
		private const string EMISSION_COLOR_PARAMETER = "_EmissionColor";

		public static IEnumerator Fade(GameObject gameObject, float interval, float duration)
		{
			var renderers = gameObject.GetComponentsInChildren<Renderer>();

			while (duration > 0)
			{
				foreach (var renderer in renderers)
				{
					var materialColor = renderer.material.color;
					var newAlpha = Mathf.Lerp(materialColor.a, 0, interval / duration);
					renderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, newAlpha);
				}

				yield return new WaitForSeconds(interval);

				duration -= Time.deltaTime;
			}
		}

		public static IEnumerator Blink(Color blinkColor, int count, float interval, GameObject gameObject, Action onStop, Func<bool> shouldCancel)
		{
			var materials = gameObject
				.GetComponentsInChildren<Renderer>()
				.SelectMany(x => x.materials)
				.Where(x => x.HasProperty(EMISSION_COLOR_PARAMETER))
				.ToArray();

			if (materials.Length > 0)
			{
				var emissionColors = materials.Select(x => x.GetColor(EMISSION_COLOR_PARAMETER)).ToArray();
				var emissionValues = materials.Select(x => x.IsKeywordEnabled(EMISSION_PARAMETER)).ToArray();
				var originalColor = true;

				for (var i = 0; i < count || !originalColor; i++)
				{
					if (originalColor && shouldCancel.Invoke())
						break;

					originalColor = !originalColor;

					for (var j = 0; j < materials.Length; j++)
					{
						var material = materials[j];

						if (!originalColor || emissionValues[j])
							material.EnableKeyword(EMISSION_PARAMETER);
						else
							material.DisableKeyword(EMISSION_PARAMETER);

						material.SetColor(EMISSION_COLOR_PARAMETER, originalColor ? emissionColors[j] : blinkColor);
					}

					yield return new WaitForSeconds(interval);
				}
			}

			onStop.SafeInvoke();
		}
	}
}
