using System;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth.Infrastructure.SaveSystem
{
	[Serializable]
	public class LevelSaveData
	{
		public int RemainingTime;
		public int Attempts;
		public List<EnemySaveData> EnemyPositions = new();
		public TransformSaveData PlayerTransform;
	}

	[Serializable]
	public class TransformSaveData
	{
		public Vector3 Position;
		public Vector3 Euler;

		public static TransformSaveData FromTransform(Transform transform) => new()
		{
			Euler = transform.eulerAngles,
			Position = transform.position
		};

		public void Deconstruct(out Vector3 position, out Vector3 euler)
		{
			position = Position;
			euler = Euler;
		}
	}
}