using System;
using System.Linq;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game.Characters.AI
{
	[RequireComponent(typeof(EnemyAI))]
	public class EnemySaveDataHandler : MonoBehaviour, ISaveObserver, ILoadObserver
	{
		[SerializeField] private EnemyAI _enemyAI;
		[SerializeField, HideInInspector] private string _id = Guid.NewGuid().ToString();

		private ISaveManager _saveManager;

		private void Reset()
		{
			_enemyAI = GetComponent<EnemyAI>();
		}

		[Inject]
		private void Construct(ISaveManager saveManager)
		{
			saveManager.RegisterSaveObserver(this);
			saveManager.RegisterLoadObserver(this);
		}

		public void OnSave(LevelSaveData saveData)
		{
			saveData.EnemyPositions.Add(new EnemySaveData
			{
				Id = _id,
				PatrolPointIdx = _enemyAI.CurrentPatrolPoint,
				WorldPos = transform.position
			});
		}

		public void OnLoad(LevelSaveData saveData)
		{
			EnemySaveData enemySaveData = saveData.EnemyPositions.First(x => x.Id == _id);
		}
	}
}