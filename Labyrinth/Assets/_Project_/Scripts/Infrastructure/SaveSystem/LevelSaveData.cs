using System;
using System.Collections.Generic;

namespace Labyrinth.Infrastructure.SaveSystem
{
	[Serializable]
	public class LevelSaveData
	{
		public int Attempts;
		public List<EnemySaveData> EnemyPositions;
	}
}