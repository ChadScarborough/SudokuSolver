using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        List<TextBox> textBoxes;
        List<Cell> cells;
        List<Box> boxes;
        List<Row> rows;
        List<Column> columns;

        private enum State { incrementing, decrementing };
        private const State INCREMENTING = State.incrementing;
        private const State DECREMENTING = State.decrementing;
        private const int TOTAL_NUMBER_OF_CELLS = 81;
        private const int NUMBER_OF_CELLS_PER_GROUP = 9;
        private const int BOX_SIDE_LENGTH = 3;


        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
            InitializeTextBoxes();
        }

        private void InitializeGrid()
        {
            cells = new List<Cell>();
            InitializeGroups();
        }

        private void InitializeGroups()
        {
            rows = new List<Row>();
            columns = new List<Column>();
            boxes = new List<Box>();
            PopulateGroups();
        }

        private void PopulateGroups()
        {
            for (int i = 0; i < NUMBER_OF_CELLS_PER_GROUP; i++)
            {
                rows.Add(new Row());
                columns.Add(new Column());
                boxes.Add(new Box());
            }
        }

        private void SolveBtn_Click(object sender, EventArgs e)
        {
            SolveBtn.Enabled = false;
            ErrorLbl.Text = "";
            ResetTextColor();
            if (FailedToCreateCells()) return;
            CreateGroups();
            if (IsGivenStateInvalid()) return;
            Solve();
        }

        private bool IsGivenStateInvalid()
        {
            try
            {
                foreach (Cell cell in cells)
                {
                    if (cell.IsGivenCell() && cell.IsValid() == false)
                    {
                        cell.GetTextBox().ForeColor = System.Drawing.Color.FromName("Red");
                        ErrorLbl.Text = "Invalid starting state";
                        SolveBtn.Enabled = true;
                        ClearGroups();
                        ClearCells();
                        throw new System.Exception();
                    }
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        private void ResetTextColor()
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.ForeColor = System.Drawing.Color.FromName("Black");
            }
        }

        private bool FailedToCreateCells()
        {
            try
            {
                CreateCells();
            }
            catch
            {
                ErrorLbl.Text = "One or more cells contains\nan invalid value.";
                SolveBtn.Enabled = true;
                return true;
            }
            return false;
        }

        private void CreateCells()
        {
            for (int i = 0; i < TOTAL_NUMBER_OF_CELLS; i++)
            {
                cells.Add(CellFactory.CreateCell(textBoxes[i]));
            }
        }

        private void CreateGroups()
        {
            CreateRows();
            CreateColumns();
            CreateBoxes();
        }

        private void CreateRows()
        {
            for (int i = 0; i < NUMBER_OF_CELLS_PER_GROUP; i++)
            {
                for (int j = 0; j < NUMBER_OF_CELLS_PER_GROUP; j++)
                {
                    rows[i].AddCell(cells[(i * NUMBER_OF_CELLS_PER_GROUP) + j]);
                }
            }
        }

        private void CreateColumns()
        {
            for (int i = 0; i < NUMBER_OF_CELLS_PER_GROUP; i++)
            {
                for (int j = 0; j < NUMBER_OF_CELLS_PER_GROUP; j++)
                {
                    columns[i].AddCell(cells[i + (j * NUMBER_OF_CELLS_PER_GROUP)]);
                }
            }
        }

        private void CreateBoxes()
        {
            for (int i = 0; i < BOX_SIDE_LENGTH; i++)
            {
                for (int j = 0; j < BOX_SIDE_LENGTH; j++)
                {
                    for (int k = 0; k < BOX_SIDE_LENGTH; k++)
                    {
                        for (int l = 0; l < BOX_SIDE_LENGTH; l++)
                        {
                            int cellNumber = (i * 27) + (j * 3) + (k * 9) + l; //Essentially counting as a ternary number
                            boxes[(i * BOX_SIDE_LENGTH) + j].AddCell(cells[cellNumber]);
                        }
                    }
                }
            }
        }

        private void Solve()
        {
            try
            {
                State state = INCREMENTING;
                int i = 0;
                while (i < TOTAL_NUMBER_OF_CELLS)
                {
                    if (i < 0) throw new Exception();
                    IncrementAndCheckCell(ref i, ref state);
                }
            }
            catch
            {
                ErrorLbl.Text = "No valid solution";
                SolveBtn.Enabled = true;
            }
        }

        private void IncrementAndCheckCell(ref int i, ref State state)
        {
            if (HandledGivenCell(ref i, state)) return;
            cells[i].Increment();
            if (HandledCellOverflow(ref i, ref state)) return;
            if (CellAtIndexIsNotValid(i)) return;
            state = INCREMENTING;
            i++;
        }

        private bool CellAtIndexIsNotValid(int i)
        {
            return cells[i].IsValid() == false;
        }

        private bool HandledGivenCell(ref int i, State state)
        {
            if (cells[i].IsGivenCell())
            {
                if (state == INCREMENTING) i++;
                else i--;
                return true;
            }
            return false;
        }

        private bool HandledCellOverflow(ref int i, ref State state)
        {
            if (cells[i].GetValue() == 0)
            {
                i--;
                state = DECREMENTING;
                return true;
            }
            return false;
        }

        private void InitializeTextBoxes()
        {
            textBoxes = new List<TextBox>();

            //Row 1
            textBoxes.Add(Box1Cell1tb);
            textBoxes.Add(Box1Cell2tb);
            textBoxes.Add(Box1Cell3tb);
            textBoxes.Add(Box2Cell1tb);
            textBoxes.Add(Box2Cell2tb);
            textBoxes.Add(Box2Cell3tb);
            textBoxes.Add(Box3Cell1tb);
            textBoxes.Add(Box3Cell2tb);
            textBoxes.Add(Box3Cell3tb);

            //Row 2
            textBoxes.Add(Box1Cell4tb);
            textBoxes.Add(Box1Cell5tb);
            textBoxes.Add(Box1Cell6tb);
            textBoxes.Add(Box2Cell4tb);
            textBoxes.Add(Box2Cell5tb);
            textBoxes.Add(Box2Cell6tb);
            textBoxes.Add(Box3Cell4tb);
            textBoxes.Add(Box3Cell5tb);
            textBoxes.Add(Box3Cell6tb);

            //Row 3
            textBoxes.Add(Box1Cell7tb);
            textBoxes.Add(Box1Cell8tb);
            textBoxes.Add(Box1Cell9tb);
            textBoxes.Add(Box2Cell7tb);
            textBoxes.Add(Box2Cell8tb);
            textBoxes.Add(Box2Cell9tb);
            textBoxes.Add(Box3Cell7tb);
            textBoxes.Add(Box3Cell8tb);
            textBoxes.Add(Box3Cell9tb);

            //Row 4
            textBoxes.Add(Box4Cell1tb);
            textBoxes.Add(Box4Cell2tb);
            textBoxes.Add(Box4Cell3tb);
            textBoxes.Add(Box5Cell1tb);
            textBoxes.Add(Box5Cell2tb);
            textBoxes.Add(Box5Cell3tb);
            textBoxes.Add(Box6Cell1tb);
            textBoxes.Add(Box6Cell2tb);
            textBoxes.Add(Box6Cell3tb);

            //Row 5
            textBoxes.Add(Box4Cell4tb);
            textBoxes.Add(Box4Cell5tb);
            textBoxes.Add(Box4Cell6tb);
            textBoxes.Add(Box5Cell4tb);
            textBoxes.Add(Box5Cell5tb);
            textBoxes.Add(Box5Cell6tb);
            textBoxes.Add(Box6Cell4tb);
            textBoxes.Add(Box6Cell5tb);
            textBoxes.Add(Box6Cell6tb);

            //Row 6
            textBoxes.Add(Box4Cell7tb);
            textBoxes.Add(Box4Cell8tb);
            textBoxes.Add(Box4Cell9tb);
            textBoxes.Add(Box5Cell7tb);
            textBoxes.Add(Box5Cell8tb);
            textBoxes.Add(Box5Cell9tb);
            textBoxes.Add(Box6Cell7tb);
            textBoxes.Add(Box6Cell8tb);
            textBoxes.Add(Box6Cell9tb);

            //Row 7
            textBoxes.Add(Box7Cell1tb);
            textBoxes.Add(Box7Cell2tb);
            textBoxes.Add(Box7Cell3tb);
            textBoxes.Add(Box8Cell1tb);
            textBoxes.Add(Box8Cell2tb);
            textBoxes.Add(Box8Cell3tb);
            textBoxes.Add(Box9Cell1tb);
            textBoxes.Add(Box9Cell2tb);
            textBoxes.Add(Box9Cell3tb);

            //Row 8
            textBoxes.Add(Box7Cell4tb);
            textBoxes.Add(Box7Cell5tb);
            textBoxes.Add(Box7Cell6tb);
            textBoxes.Add(Box8Cell4tb);
            textBoxes.Add(Box8Cell5tb);
            textBoxes.Add(Box8Cell6tb);
            textBoxes.Add(Box9Cell4tb);
            textBoxes.Add(Box9Cell5tb);
            textBoxes.Add(Box9Cell6tb);

            //Row 9
            textBoxes.Add(Box7Cell7tb);
            textBoxes.Add(Box7Cell8tb);
            textBoxes.Add(Box7Cell9tb);
            textBoxes.Add(Box8Cell7tb);
            textBoxes.Add(Box8Cell8tb);
            textBoxes.Add(Box8Cell9tb);
            textBoxes.Add(Box9Cell7tb);
            textBoxes.Add(Box9Cell8tb);
            textBoxes.Add(Box9Cell9tb);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            ClearGroups();
            ClearCells();
            SolveBtn.Enabled = true;
            ErrorLbl.Text = "";
        }

        private void ClearCells()
        {
            cells.Clear();
        }

        private void ClearGroups()
        {
            for (int i = 0; i < NUMBER_OF_CELLS_PER_GROUP; i++)
            {
                rows[i].Clear();
                columns[i].Clear();
                boxes[i].Clear();
            }
        }

        private void ClearTextBoxes()
        {
            ResetTextColor();
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = "";
            }
        }
    }
}
