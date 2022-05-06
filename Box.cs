using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    public class Box : Group
    {
        public override void AddCell(Cell cell)
        {
            base.AddCell(cell);
            cell.SetBox(this);
        }
    }
}
