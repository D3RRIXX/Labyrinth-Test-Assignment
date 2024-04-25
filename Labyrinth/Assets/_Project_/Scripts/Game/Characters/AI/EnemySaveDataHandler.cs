using System;
using System.Linq;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game.Characters.AI
{
	[RequireComponent(typeof(EnemyAI))]
	public class EnemySaveDataHandler : SaveDataHandlerBase
	{
		[SerializeField] private EnemyAI _enemyAI;
		[SerializeField] private string _id = Guid.NewGuid().ToString();

		private void Reset()
		{
			_enemyAI = GetComponent<EnemyAI>();
		}

		public override void OnSave(LevelSaveData saveData)
		{
			saveData.EnemyPositions.Add(new EnemySaveData
			{
				Id = _id,
				PatrolPointIdx = _enemyAI.CurrentPatrolPoint,
				Transform = TransformSaveData.FromTransform(transform)
			});
		}

		public override void OnLoad(LevelSaveData saveData)
		{
			EnemySaveData enemySaveData = saveData.EnemyPositions.First(x => x.Id == _id);
			(Vector3 position, Vector3 euler) = enemySaveData.Transform;

			_enemyAI.CurrentPatrolPoint = enemySaveData.PatrolPointIdx;
			
			transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
		}
	}
}