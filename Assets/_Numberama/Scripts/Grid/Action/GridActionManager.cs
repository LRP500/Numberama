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

        private void Start()
        {
            _checkAction.RegisterOnExecute(_gameplayManager.Check);
            _tipAction.RegisterOnExecute(_gameplayManager.AskForHint);
            _restartAction.RegisterOnExecute(_gameplayManager.RestartWithNewNumbers);
            _undoAction.RegisterOnExecute(_gameplayManager.UndoLastMove);
        }

        private void Update()
        {
            _undoAction.SetStackValue(_gameplayManager.UndoStackSize);
        }

        public GridAction GetAction(GridActionInfo action)
        {
            if (_checkAction.Info == action)
            {
                return _checkAction;
            }
            else if (_tipAction.Info == action)
            {
                return _tipAction;
            }
            else if (_restartAction.Info == action)
            {
                return _restartAction;
            }
            else if (_undoAction.Info == action)
            {
                return _undoAction;
            }

            return null;
        }
    }
}
