using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class Row : Group
    {
        public override void AddCell(Cell cell)
        {
            base.AddCell(cell);
            cell.SetRow(this);
        }
    }
}
