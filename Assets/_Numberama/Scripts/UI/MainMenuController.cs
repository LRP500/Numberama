using TMPro;
using Tools.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private ColorSchemeSelection _colorSchemePanel = null;

        [SerializeField]
        private DifficultySelection _difficultyPanel = null;

        [SerializeField]
        private Button _newGameButton = null;

        [SerializeField]
        private SettingToggle _muteToggle = null;

        [SerializeField]
        private SettingToggle _colorThemeToggle = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        [SerializeField]
        private AudioManagerVariable _audioManager = null;

        [Header("Info Messages")]

        [SerializeField]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        private InfoMessage _welcomeMessage = null;

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
                if (_colorSchemePanel.IsOpen)
                {
                    _colorSchemePanel.Close();
                }
                else if (_difficultyPanel.IsOpen)
                {
                    _difficultyPanel.Close();
                }
                else
                {
                    NavigationManager.QuitGame();
                }
            }
        }

        private void OnPlayButtonClicked()
        {
            if (PlayerPrefs.GetInt(PlayerPrefKeys.HasPlayedTutorial) == 0)
            {
                _infoMessagePanel.Open(_welcomeMessage);
                PlayerPrefs.SetInt(PlayerPrefKeys.HasPlayedTutorial, 1);
            }
            else
            {
                _difficultyPanel.Open();
            }
        }
    }
}