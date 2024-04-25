using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;

namespace Labyrinth.Game.Characters.Player
{
	public class PlayerSaveDataHandler : SaveDataHandlerBase
	{
		public override void OnSave(LevelSaveData saveData)
		{
			saveData.PlayerTransform = TransformSaveData.FromTransform(transform);
		}

		public override void OnLoad(LevelSaveData saveData)
		{
			(Vector3 position, Vector3 euler) = saveData.PlayerTransform;
			transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
		}
	}
}