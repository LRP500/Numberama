using System;
using Tools.Navigation;
using UnityEngine;

namespace Numberama
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private StorePanel _storePanel = null;

        [SerializeField]
        private NavigationManager _navigationManager = null;

        [SerializeField]
        private SceneReference _mainMenuScene = null;

        [SerializeField]
        private SceneReference _gameScene = null;

        [SerializeField]
        private SceneReference _tutorialScene = null;

        [SerializeField]
        private GameplayManagerVariable _gameplayManager = null;

        [SerializeField]
        private GameMasterVariable _runtimeReference = null;

        private void Awake()
        {
            _runtimeReference.SetValue(this);

            if (IsFirstLoad())
            {
                InitializePlayerPrefs();
            }

            NavigateToMainMenu();
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        public void NavigateToMainMenu()
        {
            StartCoroutine(_navigationManager.FastLoad(_mainMenuScene));
        }

        public void StartNewGame(Difficulty difficulty)
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HasGameInProgress, 0);

            StartCoroutine(_navigationManager.FastLoad(_gameScene, () =>
            {
                _gameplayManager.Value.SetDifficulty(difficulty);
            }));
        }

        public void ContinueGame()
        {
            StartCoroutine(_navigationManager.FastLoad(_gameScene));
        }

        public void LaunchTutorial()
        {
            StartCoroutine(_navigationManager.FastLoad(_tutorialScene));
        }

        public void OpenStore()
        {
            _storePanel?.Open();
        }

        #region Player Prefs

        private void InitializePlayerPrefs()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.FirstLoad, 1);
            PlayerPrefs.SetInt(PlayerPrefKeys.HasPlayedTutorial, 0);
            PlayerPrefs.SetInt(PlayerPrefKeys.HasGameInProgress, 0);
            PlayerPrefs.Save();
        }

        private bool IsFirstLoad()
        {
            return PlayerPrefs.HasKey(PlayerPrefKeys.FirstLoad);
        }

        public bool HasGameInProgress()
        {
            return PlayerPrefs.GetInt(PlayerPrefKeys.HasGameInProgress) == 1 ? true : false;
        }

        public void ClearSavedGame()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HasGameInProgress, 0);
        }

        #endregion Player Prefs
    }
}
