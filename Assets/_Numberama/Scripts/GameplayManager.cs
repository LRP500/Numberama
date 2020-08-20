using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tools.Persistence;
using UnityEngine;

namespace Numberama
{
    public class GameplayManager : MonoBehaviour
    {
        #region Data Structures

        public struct MoveInfo
        {
            public GridCell first;
            public GridCell second;

            public void Clear()
            {
                first = null;
                second = null;
            }

            public void ResetState()
            {
                first?.Clear();
                second?.Clear();
            }

            public void Highlight(bool value)
            {
                if (first != null && first.gameObject != null)
                {
                    first.SetHighlighted(value);
                }

                if (second != null && second.gameObject != null)
                {
                    second?.SetHighlighted(value);
                }
            }

            public void Check()
            {
                first?.SetChecked(true);
                second?.SetChecked(true);
            }
        }

        #endregion Data Structures

        #region Serialized Fields

        [SerializeField]
        protected Grid _grid = null;

        [SerializeField]
        protected int _initialPush = 20;

        [SerializeField]
        private PersistentStorage _storage = null;

        [SerializeField]
        private Difficulty _difficulty = null;

        [SerializeField]
        private MenuPanelVariable _activeMenuPanel = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        [SerializeField]
        private GameplayManagerVariable _runtimeReference = null;

        [Header("Info Messages")]

        [SerializeField]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        private InfoMessage _boardFullMessage = null;

        [SerializeField]
        private InfoMessage _gameOverMessage = null;

        [SerializeField]
        private InfoMessage _victoryMessage = null;

        #endregion Serialized Fields

        #region Properties

        public int UndoStackSize => _storage.HistorySize;

        #endregion Properties

        #region Private Fields

        private MoveInfo _currentMove = default;
        private MoveInfo _currentHint = default;

        protected List<int> _lastStartingNumbers = null;

        #endregion Private Fields

        #region MonoBehaviour

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_activeMenuPanel.Value != null)
                {
                    _activeMenuPanel.Value.Close();
                }
                else
                {
                    _gameMaster.Value?.NavigateToMainMenu();
                }
            }
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        #endregion MonoBehaviour

        #region Private Methods

        protected virtual void Initialize()
        {
            Debug.Log(_gameMaster.Value.HasGameInProgress());

            if (_gameMaster.Value == null ||
                !_gameMaster.Value.HasGameInProgress() ||
                !_storage.SaveFileExists)
            {
                _grid.Clear();
                _storage.ClearUndoHistory(_grid);
                _lastStartingNumbers = _grid.PushRange(_initialPush, _difficulty.Numbers);
                Save();

            }
            else
            {
                _storage.Load(_grid);
            }
        }

        protected void Save()
        {
            _storage.Save(_grid);
            PlayerPrefs.SetInt(PlayerPrefKeys.HasGameInProgress, _grid.IsEmpty ? 0 : 1);
            PlayerPrefs.Save();
        }

        private void ExecuteMove()
        {
            if (_grid.IsValidMove(_currentMove))
            {
                _storage.Record(_grid);

                _currentMove.Check();

                bool vertical = _currentMove.first.Coordinates.y != _currentMove.second.Coordinates.y;

                // Clear first number's row if necessary
                if (_grid.IsRowEmpty(_currentMove.first.Coordinates.y))
                {
                    StartCoroutine(_grid.ClearRow(_currentMove.first.Coordinates.y));
                }

                // Clear second number's row if different from the first and necessary
                if (vertical && _grid.IsRowEmpty(_currentMove.second.Coordinates.y))
                {
                    StartCoroutine(_grid.ClearRow(_currentMove.second.Coordinates.y));
                }

                CheckGameOverConditions();

                Save();
            }
            else
            {
                _currentMove.ResetState();
            }

            ResetHint();

            _currentMove.Clear();
        }

        private void CheckGameOverConditions()
        {
            // Victory
            if (_grid.IsEmpty)
            {
                OnVictory();
            }
            // Failed
            else if (_grid.IsFull() && !_grid.GetNextAvailableMove(out MoveInfo move))
            {
                // Clear all checked cells
                if (_grid.GetRemainingNumbers().Count != _grid.Size)
                {
                    _infoMessagePanel.Open(_boardFullMessage);
                }
                // Nothing to clear
                else
                {
                    _infoMessagePanel.Open(_gameOverMessage);
                }
            }
        }

        public void ClearCheckNumbers()
        {
            _grid.Clear();
            _grid.PushRange(_grid.GetRemainingNumbers());
        }

        private void ResetHint()
        {
            _currentHint.Highlight(false);
            _currentHint.Clear();
        }

        private void OnVictory()
        {
            _infoMessagePanel.Open(_victoryMessage);
        }

        #endregion Private Methods

        public bool RestartWithNewNumbers()
        {
            _gameMaster.Value.ClearSavedGame();
            Initialize();
            return true;
        }

        public void RestartWithSameNumbers()
        {
            _grid.Clear();
            _grid.PushRange(_lastStartingNumbers);
            _storage.ClearUndoHistory(_grid);
        }

        public bool UndoLastMove()
        {
            if (_storage.HistorySize > 0)
            {
                _storage.Undo(_grid);
                _storage.Save(_grid);
                return true;
            }

            return false;
        }

        public void GiveUp()
        {
            _grid.Clear();
            Save();
            _gameMaster.Value?.NavigateToMainMenu();
        }

        public void HandleClick(GridCell clicked)
        {
            clicked.SetSelected(true);

            if (_currentMove.first == null)
            {
                _currentMove.first = clicked;
            }
            else if (_currentMove.second == null && clicked != _currentMove.first)
            {
                _currentMove.second = clicked;
                ExecuteMove();
            }
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            _difficulty = difficulty;
        }

        #region Editor

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public bool Check()
        {
            if (!_grid.IsFull())
            {
                _storage.Record(_grid);
                _grid.PushRange(_grid.GetRemainingNumbers());
                CheckGameOverConditions();
                Save();
                return true;
            }

            return false;
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public bool AskForHint()
        {
            ResetHint();

            if (_grid.GetNextAvailableMove(out _currentHint))
            {
                _currentHint.Highlight(true);
                return true;
            }

            return false;
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void ClearRow(int row)
        {
            StartCoroutine(_grid.ClearRow(row));
        }

        #endregion Editor
    }
}
