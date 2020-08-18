using Tools.Navigation;
using UnityEngine;

namespace Numberama
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private bool _playTutorial = false;

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

        public void LaunchGame(Difficulty difficulty)
        {
            StartCoroutine(_navigationManager.FastLoad(_gameScene, () =>
            {
                _gameplayManager.Value.SetDifficulty(difficulty);
            }));
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
#if UNITY_EDITOR
            return _playTutorial;
#endif
            return PlayerPrefs.HasKey(PlayerPrefKeys.FirstLoad);
        }

        public bool HasGameInProgress()
        {
            if (IsFirstLoad())
            {
                return false;
            }

            return PlayerPrefs.GetInt(PlayerPrefKeys.HasGameInProgress) == 1 ? true : false;
        }

        #endregion Player Prefs
    }
}
