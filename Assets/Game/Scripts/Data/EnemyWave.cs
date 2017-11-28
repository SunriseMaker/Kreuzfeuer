namespace Game.Data
{
	[System.Serializable]
	public struct EnemyWave
	{
		public int WaveIndex;
		public Actor EnemyCharacter;
		public int EnemyCount;
		public float SpawnDelay;
	}
}
