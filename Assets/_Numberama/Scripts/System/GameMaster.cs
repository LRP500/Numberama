using Tools.Navigation;
using UnityEngine;

namespace Numberama
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private NavigationManager _navigationManager = null;

        [SerializeField]
        private SceneReference _mainMenuScene = null;

        [SerializeField]
        private SceneReference _gameScene = null;

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

        public void LaunchGame()
        {
            StartCoroutine(_navigationManager.FastLoad(_gameScene));
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
            if (IsFirstLoad())
            {
                return false;
            }

            return PlayerPrefs.GetInt(PlayerPrefKeys.HasGameInProgress) == 1 ? true : false;
        }

        #endregion Player Prefs
    }
}
