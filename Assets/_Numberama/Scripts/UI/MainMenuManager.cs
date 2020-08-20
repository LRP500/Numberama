using Tools.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Panels")]

        [SerializeField]
        private ColorSchemeSelection _colorSchemePanel = null;

        [SerializeField]
        private DifficultySelection _difficultyPanel = null;

        [SerializeField]
        private GameSelection _gameSelection = null;

        [SerializeField]
        private MenuPanelVariable _activeMenuPanel = null;

        [Header("Elements")]

        [SerializeField]
        private Button _newGameButton = null;

        [SerializeField]
        private SettingToggle _muteToggle = null;

        [SerializeField]
        private SettingToggle _colorThemeToggle = null;

        [Header("Info Messages")]

        [SerializeField]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        private InfoMessage _welcomeMessage = null;

        [Header("Managers")]

        [SerializeField]
        private AudioManagerVariable _audioManager = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        [SerializeField]
        private MainMenuManagerVariable _runtimeReference = null;

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        private void Start()
        {
            if (_audioManager.Value)
            {
                _muteToggle.RegisterOnClick(_audioManager.Value.Mute);
            }

            if (_gameMaster.Value)
            {
                _newGameButton.onClick.AddListener(OnPlayButtonClicked);
            }

            _difficultyPanel.Close();
            _colorThemeToggle.RegisterOnClick(_colorSchemePanel.Open);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_activeMenuPanel.Value != null)
                {
                    _activeMenuPanel.Value.Close();
                }
                else
                {
                    NavigationManager.QuitGame();
                }
            }
        }

        private void OnPlayButtonClicked()
        {
            // First load
            if (PlayerPrefs.GetInt(PlayerPrefKeys.HasPlayedTutorial) == 0)
            {
                _infoMessagePanel.Open(_welcomeMessage);
                PlayerPrefs.SetInt(PlayerPrefKeys.HasPlayedTutorial, 1);
            }
            // Game in progress
            else if (PlayerPrefs.GetInt(PlayerPrefKeys.HasGameInProgress) == 1)
            {
                NavigateToGameSelection();
            }
            // No game in progress
            else
            {
                NavigateToDifficultyChoice();
            }
        }

        public void NavigateToDifficultyChoice()
        {
            _difficultyPanel.Open();
        }

        public void NavigateToGameSelection()
        {
            _gameSelection.Open();
        }
    }
}