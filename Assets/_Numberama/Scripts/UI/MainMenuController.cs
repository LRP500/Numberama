using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Button _newGameButton = null;

        [SerializeField]
        private Button _continueButton = null;

        [SerializeField]
        private SettingToggle _muteToggle = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        [SerializeField]
        private AudioManagerVariable _audioManager = null;

        private void Awake()
        {
            if (_audioManager.Value)
            {
                _muteToggle.RegisterOnClick(_audioManager.Value.Mute);
            }

            if (_gameMaster.Value)
            {
                _newGameButton.onClick.AddListener(_gameMaster.Value.LaunchGame);
            }
        }
    }
}