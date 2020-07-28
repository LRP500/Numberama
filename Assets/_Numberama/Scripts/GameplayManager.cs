using System;
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

        private MoveInfo _moveInfo = default;

        #region MonoBehaviour

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
                    _grid.ClearRow(_moveInfo.first.Coordinates.y);
                }

                // Clear second number's row if different from the first and necessary
                if (vertical && _grid.IsRowEmpty(_moveInfo.second.Coordinates.y))
                {
                    _grid.ClearRow(_moveInfo.second.Coordinates.y);
                }
            }
            else
            {
                _moveInfo.ResetState();
            }

            _moveInfo.Clear();

            // Check if there is still possible moves in the board
            if (_grid.IsFull() && !_grid.GetNextAvailableMove(out MoveInfo move))
            {
                List<int> remainingNumbers = _grid.GetRemainingNumbers();
                _grid.Clear();
                _grid.Push(remainingNumbers);
            }
        }

        #region Editor

        [Button]
        [ShowIf("@ UnityEngine.Application.isPlaying")]
        public void Check()
        {
            _grid.Push(_grid.GetRemainingNumbers());
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
            _grid.ClearRow(row);
        }

        #endregion Editor
    }
}
