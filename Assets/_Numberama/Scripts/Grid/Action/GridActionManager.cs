using UnityEngine;

namespace Numberama
{
    public class GridActionManager : MonoBehaviour
    {
        [SerializeField]
        private GridAction _checkAction = null;

        [SerializeField]
        private GridAction _tipAction = null;

        [SerializeField]
        private GridAction _restartAction = null;

        [SerializeField]
        private GridAction _undoAction = null;

        [SerializeField]
        private GameplayManager _gameplayManager = null;

        private void Awake()
        {
            _checkAction.RegisterOnExecute(_gameplayManager.Check);
            _tipAction.RegisterOnExecute(_gameplayManager.AskForHint);
            _restartAction.RegisterOnExecute(_gameplayManager.RestartWithNewNumbers);
            _undoAction.RegisterOnExecute(_gameplayManager.UndoLastMove);
        }
    }
}
