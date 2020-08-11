using System.Collections;
using System.Collections.Generic;
using Tools.Persistence;
using UnityEngine;
using UnityEngine.UI;
using static Numberama.GameplayManager;

namespace Numberama
{
    public class Grid : MonoBehaviour, IPersistable
    {
        [SerializeField]
        private Vector2Int _size = new Vector2Int(10, 10);

        [SerializeField]
        private GridCell _cellPrefab = null;

        [SerializeField]
        private GridLayoutGroup _layout = null;

        [SerializeField]
        private GameplayManager _gameplayManager = null;

        private GridCell[] _cells = null;

        public int Size => _size.x * _size.y;
        public bool IsEmpty => _lastCellIndex == 0;

        private int _lastCellIndex = 0;

        public void SetSize(int rows, int columns)
        {
            _size = new Vector2Int(rows, columns);
        }

        private void Initialize()
        {
            _cells = new GridCell[_size.y * _size.x];

            // GridLayout settings
            _layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _layout.constraintCount = _size.x;

            // GridLayout dimensions
            float width = (_size.x * _layout.cellSize.x) + ((_size.x - 1) * _layout.spacing.x);
            float height = (_size.y * _layout.cellSize.y) + ((_size.y - 1) * _layout.spacing.y);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            // Reset cell index
            _lastCellIndex = 0;
        }

        public void Clear()
        {
            if (_cells != null)
            {
                // Destroy all cells
                for (int i = 0; i < _cells.Length && _cells[i] != null; i++)
                {
                    Destroy(_cells[i].gameObject);
                }
            }

            // Reinit grid
            Initialize();
        }

        public List<int> PushRange(int count, int numberMax)
        {
            List<int> pushed = new List<int>();

            int length = Mathf.Min(_lastCellIndex + count, Size);
            for (; _lastCellIndex < length; _lastCellIndex++)
            {
                GridCell cell = Push(Random.Range(1, numberMax + 1));
                _cells[_lastCellIndex] = cell;
                pushed.Add(cell.Number);
            }

            return pushed;
        }

        public void PushRange(List<int> numbers)
        {
            foreach (int number in numbers)
            {
                if (_lastCellIndex >= Size) break;

                _cells[_lastCellIndex] = Push(number);
                _lastCellIndex++;
            }
        }

        private GridCell Push(GridCell.CellState state)
        {
            GridCell cell = Instantiate(_cellPrefab, transform);
            cell.SetCallback(_gameplayManager.HandleClick);
            cell.SetState(state);
            return cell;
        }

        private GridCell Push(int number)
        {
            return Push(new GridCell.CellState
            {
                coordinates = IndexToCoord(_lastCellIndex),
                isChecked = false,
                number = number
            });
        }

        private Vector2Int IndexToCoord(int index)
        {
            int row = index / _size.x;
            int column = index - (row * _size.x);
            return new Vector2Int(column, row);
        }

        private int CoordToIndex(int x, int y)
        {
            return (y * _size.x) + x;
        }

        private int CoordToIndex(Vector2Int coordinates)
        {
            return CoordToIndex(coordinates.x, coordinates.y);
        }

        public bool IsFull()
        {
            return _lastCellIndex == Size;
        }

        public GridCell GetCell(int x, int y)
        {
            return _cells[CoordToIndex(x, y)];
        }

        public GridCell GetCell(Vector2Int coordinates)
        {
            return _cells[CoordToIndex(coordinates.x, coordinates.y)];
        }

        public bool IsLinked(GridCell a, GridCell b)
        {
            return CheckHorizontalLink(a, b) || CheckVerticalLink(a, b);
        }

        private bool CheckHorizontalLink(GridCell a, GridCell b)
        {
            int index = CoordToIndex(a.Coordinates);
            return GetNextCellLeft(index) == b || GetNextCellRight(index) == b;
        }

        private bool CheckVerticalLink(GridCell a, GridCell b)
        {
            int index = CoordToIndex(a.Coordinates);
            return GetNextCellDown(index) == b || GetNextCellUp(index) == b;
        }
      
        public bool CheckNumberValidity(GridCell a, GridCell b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.Number == b.Number || a.Number + b.Number == 10;
        }
        
        /// <summary>
        /// Returns true if given move is valid, else returns false.
        /// </summary>
        /// <param name="moveInfo"></param>
        /// <returns></returns>
        public bool IsValidMove(MoveInfo moveInfo)
        {
            if (CheckNumberValidity(moveInfo.first, moveInfo.second))
            {
                return IsLinked(moveInfo.first, moveInfo.second);
            }

            return false;
        }

        public List<int> GetRemainingNumbers()
        {
            List<int> remainingNumbers = new List<int>();

            for (int i = 0; i < _cells.Length && _cells[i] != null; i++)
            {
                if (!_cells[i].IsChecked)
                {
                    remainingNumbers.Add(_cells[i].Number);
                }
            }

            return remainingNumbers;
        }

        public IEnumerator ClearRow(int row)
        {
            int origin = _size.x * row;
            int cellCleared = 0;

            for (int i = 0; i < _size.x; i++)
            {
                GridCell cell = _cells[origin + i];

                if (cell == null)
                {
                    break;
                }

                Destroy(cell.gameObject);
                _cells[origin + i] = null;
                cellCleared++;
            }

            _lastCellIndex -= cellCleared;

            // Next row
            origin += _size.x;

            for (int i = origin; i < Size; i++)
            {
                GridCell cell = _cells[i];

                if (cell == null) break;

                cell.SetCoordinates(new Vector2Int(cell.Coordinates.x, cell.Coordinates.y - 1));

                _cells[i - _size.x] = cell;
                _cells[i] = null;
            }

            yield return new WaitForEndOfFrame();
        }

        public bool IsRowEmpty(int row)
        {
            for (int i = 0; i < _size.x; i++)
            {
                GridCell cell = _cells[(_size.x * row) + i];

                if (cell == null)
                {
                    break;
                }
                else if (cell.IsChecked == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool GetNextAvailableMove(out MoveInfo move)
        {
            move = new MoveInfo();

            for (int i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] == null) return false;
                if (_cells[i].IsChecked) continue;

                move.first = _cells[i];

                // Left
                move.second = GetNextCellLeft(i);
                if (CheckNumberValidity(move.first, move.second))
                {
                    return true;
                }
                
                // Right
                move.second = GetNextCellRight(i);
                if (CheckNumberValidity(move.first, move.second))
                {
                    return true;
                }
                
                // Up
                move.second = GetNextCellUp(i);
                if (CheckNumberValidity(move.first, move.second))
                {
                    return true;
                }
                
                // Down
                move.second = GetNextCellDown(i);
                if (CheckNumberValidity(move.first, move.second))
                {
                    return true;
                }
            }

            return false;
        }

        public GridCell GetNextCellLeft(int index)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (_cells[i] && !_cells[i].IsChecked)
                {
                    return _cells[i];
                }
            }

            return null;
        }

        public GridCell GetNextCellRight(int index)
        {
            for (int i = index + 1; i < _lastCellIndex; i++)
            {
                if (_cells[i] && !_cells[i].IsChecked)
                {
                    return _cells[i];
                }
            }

            return null;
        }

        public GridCell GetNextCellDown(int index)
        {
            for (int i = index - _size.x; i >= 0; i -= _size.x)
            {
                if (_cells[i] && !_cells[i].IsChecked)
                {
                    return _cells[i];
                }
            }

            return null;
        }

        public GridCell GetNextCellUp(int index)
        {
            for (int i = index + _size.x; i < _lastCellIndex; i += _size.x)
            {
                if (_cells[i] && !_cells[i].IsChecked)
                {
                    return _cells[i];
                }
            }

            return null;
        }

        #region Persistence

        public void Save(GameDataWriter writer)
        {
            // Write current cell count
            writer.Write(_lastCellIndex);

            // Write all cells
            for (int i = 0; i < _lastCellIndex; i++)
            {
                _cells[i].Save(writer);
            }
        }

        public void Load(GameDataReader reader)
        {
            Clear();

            _lastCellIndex = reader.ReadInt();

            for (int i = 0; i < _lastCellIndex; i++)
            {
                GridCell cell = Push(new GridCell.CellState());
                cell.Load(reader);
                _cells[i] = cell;
            }
        }

        #endregion Persistence
    }
}
