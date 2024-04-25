using System;
using UnityEngine;

namespace Labyrinth.Infrastructure.SaveSystem
{
	[Serializable]
	public class EnemySaveData
	{
		public string Id;
		public TransformSaveData Transform;
		public int PatrolPointIdx;
	}
}
