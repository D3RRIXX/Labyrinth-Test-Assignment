using System;
using UnityEngine;

namespace Labyrinth.Infrastructure.SaveSystem
{
	[Serializable]
	public class EnemySaveData
	{
		public string Id;
		public Vector3 WorldPos;
		public int PatrolPointIdx;
	}
}
