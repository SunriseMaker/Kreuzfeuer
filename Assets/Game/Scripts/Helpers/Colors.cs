namespace Game.Helpers
{
	public static class Colors
	{
		public const string Red = "red";
		public const string Black = "black";
		public const string Grey = "grey";

		public static string AddColorTag(string s, string colorName)
		{
			return "<color=" + colorName + ">" + s + "</color>";
		}
	}
}
