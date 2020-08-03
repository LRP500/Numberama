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
        private Grid _grid = null;

        [SerializeField]
        private int _initialPush = 20;

        [SerializeField]
        private int _historyCapacity = 10;

        [SerializeField]
        private PersistentStorage _storage = null;

        [SerializeField]
        private GameMasterVariable _gameMaster = null;

        [SerializeField]
        private GameplayManagerVariable _runtimeReference = null;

        [SerializeField]
        [BoxGroup("Info Messages")]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        [BoxGroup("Info Messages")]
        private InfoMessage _noMoreMovesMessage = null;

        [SerializeField]
        [BoxGroup("Info Messages")]
        private InfoMessage _victoryMessage = null;

        #endregion Serialized Fields

        #region Private Fields

        private MoveInfo _currentMove = default;
        private MoveInfo _currentHint = default;

        private List<int> _lastStartingNumbers = null;

        #endregion Private Fields

        #region MonoBehaviour

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void Start()
        {
            if (_gameMaster.Value == null ||
                !_gameMaster.Value.HasGameInProgress() ||
                !_storage.SaveFileExists)
            {
                Debug.Log("Starting new game");
                StartNewGame();
            }
            else
            {
                Debug.Log("Loading existing game");
                _storage.Load(_grid);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameMaster.Value?.NavigateToMainMenu();
            }
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        #endregion MonoBehaviour

        #region Private Methods

        private void StartNewGame()
        {
            // Clear
            _grid.Clear();

            // Initialize
            _lastStartingNumbers = _grid.PushRange(_initialPush);

            // Save
            Save();
        }

        private void Save()
        {
            _storage.Save(_grid);
            PlayerPrefs.SetInt(PlayerPrefKeys.HasGameInProgress, _grid.IsEmpty ? 0 : 1);
            PlayerPrefs.Save();
        }

        private void ExecuteMove()
        {
            if (_grid.IsValidMove(_currentMove))
            {
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
                List<int> remainingNumbers = _grid.GetRemainingNumbers();

                // Clear all checked cells
                if (remainingNumbers.Count != _grid.Size)
                {
                    _grid.Clear();
                    _grid.PushRange(remainingNumbers);
                }
                // Nothing to clear
                else
                {
                    _infoMessagePanel.Open(_noMoreMovesMessage);
                }
            }
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

        public void RestartWithNewNumbers()
        {
            StartNewGame();
        }

        public void RestartWithSameNumbers()
        {
            _grid.Clear();
            _grid.PushRange(_lastStartingNumbers);
        }

        public void UndoLastMove()
        {
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

        #region Editor

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void Check()
        {
            _grid.PushRange(_grid.GetRemainingNumbers());
            CheckGameOverConditions();
            Save();
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void AskForHint()
        {
            ResetHint();

            if (_grid.GetNextAvailableMove(out _currentHint))
            {
                _currentHint.Highlight(true);
            }
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
