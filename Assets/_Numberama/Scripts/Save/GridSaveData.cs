namespace Numberama
{
    [System.Serializable]
    public class GridSaveData : SaveData
    {
        public GridCell.CellState[] Cells { get; private set; } = null;

        public GridSaveData(int capacity)
        {
            Cells = new GridCell.CellState[capacity];
        }

        public void SetCell(int index, GridCell.CellState cell)
        {
            Cells[index] = cell;
        }
    }
}
