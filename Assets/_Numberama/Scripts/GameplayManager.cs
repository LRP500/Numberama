using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Numberama
{
    public class GameplayManager : MonoBehaviour
    {
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

        [SerializeField]
        private Grid _grid = null;

        [SerializeField]
        private int _initialPush = 20;

        [Header("Info Messages")]

        [SerializeField]
        private InfoMessagePanel _infoMessagePanel = null;

        [SerializeField]
        private InfoMessage _noMoreMovesInfoMessage = null;

        [Space]
        [SerializeField]
        private GameplayManagerVariable _runtimeReference = null;

        private MoveInfo _moveInfo = default;

        #region MonoBehaviour

        private void Awake()
        {
            _runtimeReference.SetValue(this);
        }

        private void Start()
        {
            _grid.Initialize();
            _grid.Push(_initialPush);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Check();
            }
        }

        private void OnDestroy()
        {
            _runtimeReference.Clear();
        }

        #endregion MonoBehaviour

        public void Restart()
        {
            _grid.Clear();
            _grid.Push(_initialPush);
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

            CheckForBlockedBoard();
        }

        private void CheckForBlockedBoard()
        {
            if (_grid.IsFull() && !_grid.GetNextAvailableMove(out MoveInfo move))
            {
                List<int> remainingNumbers = _grid.GetRemainingNumbers();

                if (remainingNumbers.Count == _grid.Size)
                {
                    _infoMessagePanel.Open(_noMoreMovesInfoMessage);
                }
                else
                {
                    _grid.Clear();
                    _grid.Push(remainingNumbers);
                }
            }
        }

        #region Editor

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void Check()
        {
            _grid.Push(_grid.GetRemainingNumbers());
            CheckForBlockedBoard();
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
