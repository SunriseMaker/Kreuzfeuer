namespace Game.Helpers
{
	public static class TimeEx
	{
		public const int SECONDS_IN_MINUTE = 60;

		public static string FormatElapsedTime(float timeElapsed)
		{
			var elapsed = (int)timeElapsed;
			var minutes = (elapsed / SECONDS_IN_MINUTE);
			var seconds = elapsed - minutes * SECONDS_IN_MINUTE;
			return string.Format("{0:00}:{1:00}", minutes, seconds);
		}
	}
}
