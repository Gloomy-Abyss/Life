using System;

namespace Life
{
    class GameEngine
    {
        private bool[,] field;
        private readonly int cols;
        private readonly int rows;
        
        public uint CGNumber { get; private set; } //Generations counter
        public bool EndOfDays { get; private set; } = false;

        private bool eod(bool[,] f2test)
        
            // Arrays.Equals & Enumerable.Equals don't work. Why? I don't know :(
            // So I use element-based arrays compare

        {
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    if(field[x, y] != f2test[x, y])
                        return false;

            return true;
        }

    public GameEngine(int cols, int rows, int density)
        {
            this.cols = cols;
            this.rows = rows;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    field[x, y] = random.Next(density) == 0;
        }

        private int CountNB(int x, int y)
        {
            int _count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {

                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;
                    if (col == x && row == y) continue;
                    var hasLife = field[col, row];

                    if (hasLife) _count++;
                }

            return _count;
        }
        public void NextGen() //Sets new state of field
        {
            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                {
                    int _nbcount = CountNB(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && _nbcount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (_nbcount < 2 || _nbcount > 3))
                        newField[x, y] = false;
                    else newField[x, y] = hasLife;
                }
            
            if (!eod(newField)) 
            {
                field = newField;
                CGNumber++;
            }
            else EndOfDays = true;
        }

        public bool[,] GetCGen() //Returns current generation field
        {
            var result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    result[x, y] = field[x, y];

            return result;
        }
        private bool isInside(int x, int y) //Check if the cell is inside the field
        {
             return (x >= 0 && y >= 0 && x < cols && y < rows);
        }
        private void UpdateCell(int x, int y, bool state)
        {
            if (isInside(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }
    }
}
