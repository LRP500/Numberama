using UnityEngine;

namespace Numberama
{
    public class GridActionManager : MonoBehaviour
    {
        [SerializeField]
        private GridAction _checkAction = null;

        [SerializeField]
        private GridAction _hintAction = null;

        [SerializeField]
        private GridAction _restartAction = null;

        [SerializeField]
        private GridAction _undoAction = null;

        [SerializeField]
        private GameplayManager _gameplayManager = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        private void Start()
        {
            _checkAction.RegisterOnExecute(_gameplayManager.Check);
            _restartAction.RegisterOnExecute(_gameplayManager.RestartWithNewNumbers);
            _hintAction.RegisterOnExecute(_gameplayManager.AskForHint);
            _undoAction.RegisterOnExecute(_gameplayManager.UndoLastMove);

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            _hintAction.SetPurchased(true);
            _undoAction.SetPurchased(true);
#else
            _hintAction.SetPurchased(_gameMaster.Value.Store.IsHintBoosterPurchased());
            _undoAction.SetPurchased(_gameMaster.Value.Store.IsUndoBoosterPurchased());
#endif
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
            else if (_hintAction.Info == action)
            {
                return _hintAction;
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
