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
