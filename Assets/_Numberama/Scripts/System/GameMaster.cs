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

            InitializePlayerPrefs();
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

        private void InitializePlayerPrefs()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HasPlayedTutorial, 0);
        }
    }
}
