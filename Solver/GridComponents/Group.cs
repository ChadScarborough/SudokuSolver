using System.Collections.Generic;

namespace SudokuSolver
{
    public abstract class Group
    {
        private List<Cell> _cells;

        public Group()
        {
            _cells = new List<Cell>();
        }

        public List<Cell> GetCells()
        {
            return _cells;
        }

        public virtual void AddCell(Cell cell)
        {
            _cells.Add(cell);
        }

        public void Clear()
        {
            _cells.Clear();
        }
    }
}
