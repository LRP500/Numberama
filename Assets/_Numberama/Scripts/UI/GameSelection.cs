using UnityEngine;
using UnityEngine.UI;

namespace Numberama
{
    public class GameSelection : MenuPanel
    {
        [SerializeField]
        private Button _continueButton = null;

        [SerializeField]
        private Button _newGameButton = null;

        [SerializeField]
        private MainMenuManager _mainMenuManager = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        private void Awake()
        {
            if (_gameMaster.Value)
            {
                _continueButton.onClick.AddListener(_gameMaster.Value.ContinueGame);
                _newGameButton.onClick.AddListener(_mainMenuManager.NavigateToDifficultyChoice);
            }
        }
    }
}
