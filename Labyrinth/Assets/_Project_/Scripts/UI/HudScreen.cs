using System.Collections;
using Labyrinth.Game;
using Labyrinth.Infrastructure.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Labyrinth.UI
{
	public class HudScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _attemptsLabel;
		[Header("Game Saved Popup")]
		[SerializeField] private GameObject _gameSavedPopup;
		[SerializeField] private float _popupVisibilityTime = 3f;
		[Header("Pause Menu")]
		[SerializeField] private GameObject _pauseMenu;
		[SerializeField] private Button _loadButton;
		
		private ISaveManager _saveManager;
		private IAttemptTracker _attemptTracker;

		[Inject]
		private void Construct(IAttemptTracker attemptTracker, ISaveManager saveManager)
		{
			_attemptTracker = attemptTracker;
			_saveManager = saveManager;
		}

		private void OnEnable()
		{
			_attemptsLabel.text = $"Attempts: {_attemptTracker.Attempts}";
			ValidateLoadButton();
		}

		private void OnDisable()
		{
			_gameSavedPopup.SetActive(false);
		}

		public void PauseGame()
		{
			_pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		}

		public void UnpauseGame()
		{
			_pauseMenu.SetActive(false);
			Time.timeScale = 1f;
		}

		public void SaveGame()
		{
			_saveManager.PerformSave();
			ValidateLoadButton();
			
			StartCoroutine(SavePopupRoutine());
		}

		public void LoadGame()
		{
			UnpauseGame();
		}

		private void ValidateLoadButton()
		{
			_loadButton.interactable = _saveManager.SaveData != null;
		}

		private IEnumerator SavePopupRoutine()
		{
			_gameSavedPopup.SetActive(true);
			yield return new WaitForSeconds(_popupVisibilityTime);
			_gameSavedPopup.SetActive(false);
		}
	}
}