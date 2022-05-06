using System.Windows.Forms;

namespace SudokuSolver
{
    public static class CellFactory
    {
        public static Cell CreateCell(TextBox textBox)
        {
            textBox.Text = textBox.Text.Trim();
            if(textBox.TextLength == 0)
            {
                return new VariableCell(textBox);
            }
            return new GivenCell(textBox);
        }
    }


    public abstract class Cell
    {
        protected int _value;
        protected Cell _previousCell = null;
        protected Cell _nextCell = null;
        protected TextBox _textBox;
        private Box _box;
        private Row _row;
        private Column _column;

        public Cell(TextBox textBox)
        {
            _textBox = textBox;
        }

        public int GetValue()
        {
            return _value;
        }

        public TextBox GetTextBox()
        {
            return _textBox;
        }

        public abstract void Increment();

        public void SetBox(Box box)
        {
            _box = box;
        }

        public void SetRow(Row row)
        {
            _row = row;
        }

        public void SetColumn(Column column)
        {
            _column = column;
        }

        private bool IsValidWithinGroup(Group group)
        {
            for (int i = 0; i < 9; i++)
            {
                if (group.GetCells()[i].GetValue() == _value && group.GetCells()[i] != this)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidWithinBox()
        {
            return IsValidWithinGroup(_box);
        }

        public bool IsValidWithinRow()
        {
            return IsValidWithinGroup(_row);
        }

        public bool IsValidWithinColumn()
        {
            return IsValidWithinGroup(_column);
        }

        public bool IsValid()
        {
            return IsValidWithinBox() && IsValidWithinColumn() && IsValidWithinRow();
        }

        public abstract bool IsVariableCell();
        public bool IsGivenCell()
        {
            return !IsVariableCell();
        }
    }

    public class GivenCell : Cell
    {
        public GivenCell(TextBox textBox) : base(textBox)
        {
            try
            {
                switch (textBox.Text)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        break;
                    default:
                        throw new System.Exception();
                }
                _value = int.Parse(textBox.Text);
            }
            catch
            {
                textBox.ForeColor = System.Drawing.Color.FromName("Red");
                throw new System.Exception();
            }
        }

        public override void Increment()
        {

        }

        public override bool IsVariableCell()
        {
            return false;
        }
    }

    public class VariableCell: Cell
    {
        public VariableCell(TextBox textBox) : base(textBox)
        {
            this._value = 0;
        }

        public override void Increment()
        {
            if (_value < 9)
            {
                _value++;
                _textBox.Text = _value.ToString();
                return;
            }
            _value = 0;
            _textBox.Text = "";
        }

        public override bool IsVariableCell()
        {
            return true;
        }
    }
}
