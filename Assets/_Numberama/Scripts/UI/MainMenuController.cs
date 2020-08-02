using Tools.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Button _newGameButton = null;

        [SerializeField]
        private SettingToggle _muteToggle = null;

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
                if (PlayerPrefs.GetInt(PlayerPrefKeys.HasPlayedTutorial) == 0)
                {
                    _newGameButton.onClick.AddListener(OnPlayButtonClicked);
                }
                else
                {
                    _newGameButton.onClick.AddListener(_gameMaster.Value.LaunchGame);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NavigationManager.QuitGame();
            }
        }

        private void OnPlayButtonClicked()
        {
            _infoMessagePanel.Open(_welcomeMessage);
            PlayerPrefs.SetInt(PlayerPrefKeys.HasPlayedTutorial, 1);
        }
    }
}