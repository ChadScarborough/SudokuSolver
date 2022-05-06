namespace SudokuSolver
{
    public class Column : Group
    {
        public override void AddCell(Cell cell)
        {
            base.AddCell(cell);
            cell.SetColumn(this);
        }
    }
}
