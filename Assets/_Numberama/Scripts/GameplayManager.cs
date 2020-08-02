using System.Collections.Generic;
using Sirenix.OdinInspector;
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
                first.Clear();
                second.Clear();
            }

            public void Highlight()
            {
                first.SetHighlighted(true);
                second.SetHighlighted(true);
            }

            public void Check()
            {
                first.SetChecked(true);
                second.SetChecked(true);
            }
        }

        #endregion Data Structures

        #region Serialized Fields

        [SerializeField]
        private Grid _grid = null;

        [SerializeField]
        private int _initialPush = 20;

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

        private MoveInfo _moveInfo = default;

        private List<int> _lastStartingNumbers = null;

        private List<MoveInfo> _history = null;

        #endregion Private Fields

        #region MonoBehaviour

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void Start()
        {
            StartGame();
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

        private void ExecuteMove()
        {
            if (_grid.IsValidMove(_moveInfo))
            {
                _moveInfo.Check();

                bool vertical = _moveInfo.first.Coordinates.y != _moveInfo.second.Coordinates.y;

                // Clear first number's row if necessary
                if (_grid.IsRowEmpty(_moveInfo.first.Coordinates.y))
                {
                    StartCoroutine(_grid.ClearRow(_moveInfo.first.Coordinates.y));
                }

                // Clear second number's row if different from the first and necessary
                if (vertical && _grid.IsRowEmpty(_moveInfo.second.Coordinates.y))
                {
                    StartCoroutine(_grid.ClearRow(_moveInfo.second.Coordinates.y));
                }
            }
            else
            {
                _moveInfo.ResetState();
            }

            _moveInfo.Clear();

            CheckGameOverConditions();
        }

        private void CheckGameOverConditions()
        {
            // Victory
            if (_grid.IsEmpty)
            {
                _infoMessagePanel.Open(_victoryMessage);
            }
            // Failed
            else if (_grid.IsFull() && !_grid.GetNextAvailableMove(out MoveInfo move))
            {
                List<int> remainingNumbers = _grid.GetRemainingNumbers();

                // Clear all checked cells
                if (remainingNumbers.Count != _grid.Size)
                {
                    _grid.Clear();
                    _grid.Push(remainingNumbers);
                }
                // Nothing to clear
                else
                {
                    _infoMessagePanel.Open(_noMoreMovesMessage);
                }
            }
        }

        #endregion Private Methods

        public void StartGame()
        {
            _grid.Clear();
            _lastStartingNumbers = _grid.Push(_initialPush);
        }

        public void Restart()
        {
            StartGame();
        }

        public void Continue()
        {
            _grid.Clear();
            _grid.Push(_lastStartingNumbers);
        }

        public void Undo()
        {
        }

        public void HandleClick(GridCell clicked)
        {
            clicked.SetSelected(true);

            if (_moveInfo.first == null)
            {
                _moveInfo.first = clicked;
            }
            else if (_moveInfo.second == null && clicked != _moveInfo.first)
            {
                _moveInfo.second = clicked;
                ExecuteMove();
            }
        }

        #region Editor

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void Check()
        {
            _grid.Push(_grid.GetRemainingNumbers());
            CheckGameOverConditions();
        }

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void AskForTip()
        {
            if (_grid.GetNextAvailableMove(out MoveInfo move))
            {
                move.Highlight();
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
