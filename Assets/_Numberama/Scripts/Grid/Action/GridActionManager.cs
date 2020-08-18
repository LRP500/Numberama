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
        private NumberamaStoreVariable _store = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        private void Start()
        {
            _checkAction.RegisterOnExecute(_gameplayManager.Check, OnActionExecuteFail);
            _restartAction.RegisterOnExecute(_gameplayManager.RestartWithNewNumbers, OnActionExecuteFail);
            _hintAction.RegisterOnExecute(_gameplayManager.AskForHint, OnActionExecuteFail);
            _undoAction.RegisterOnExecute(_gameplayManager.UndoLastMove, OnActionExecuteFail);

            _hintAction.SetPurchased(_store.Value.IsHintBoosterPurchased());
            _undoAction.SetPurchased(_store.Value.IsUndoBoosterPurchased());
        }

        private void Update()
        {
            _undoAction.SetStackValue(_gameplayManager.UndoStackSize);
        }

        private void OnActionExecuteFail()
        {
            _gameMaster.Value?.OpenStore();
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
