using System.Collections;
using Labyrinth.Game;
using Labyrinth.Infrastructure.GameFlowSystem;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Labyrinth.UI
{
	public class PauseMenuView : MonoBehaviour
	{
		[SerializeField] private Button _loadButton;
		[Header("Game Saved Popup")]
		[SerializeField] private GameObject _gameSavedPopup;
		[SerializeField] private float _popupVisibilityTime = 3f;
		
		private ISaveManager _saveManager;
		private HudScreen _hudScreen;
		private IGameFlowService _gameFlowService;

		[Inject]
		private void Construct(ISaveManager saveManager, IGameFlowService gameFlowService)
		{
			_gameFlowService = gameFlowService;
			_saveManager = saveManager;
			_hudScreen = GetComponentInParent<HudScreen>(true);
		}

		private void OnEnable()
		{
			ValidateLoadButton();
		}

		private void OnDisable()
		{
			_gameSavedPopup.SetActive(false);
		}

		public void SaveGame()
		{
			_saveManager.PerformSave();
			ValidateLoadButton();

			StartCoroutine(SavePopupRoutine());
		}

		public void LoadGame()
		{
			_hudScreen.UnpauseGame();
			_gameFlowService.LoadSaveForCurrentLevel();
		}

		private void ValidateLoadButton()
		{
			_loadButton.interactable = _saveManager.SaveData != null;
		}

		private IEnumerator SavePopupRoutine()
		{
			_gameSavedPopup.SetActive(true);
			yield return new WaitForSecondsRealtime(_popupVisibilityTime);
			_gameSavedPopup.SetActive(false);
		}
	}
}