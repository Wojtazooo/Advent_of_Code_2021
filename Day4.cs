namespace Advent_of_Code_2021
{
    public class Day4 : Day
    {
        public class BingoBoard
        {
            private readonly Dictionary<int, (int, int)> _positionOfNumberOnBoardDictionary = new();
            private readonly List<int>[] _markedColumnsInRows;
            private readonly List<int>[] _markedRowsInColumns;
            private readonly HashSet<int> _unmarkedSet;
            private readonly int _rowsCount;
            private readonly int _columnsCount;

            public BingoBoard(int[,] board)
            {
                _rowsCount = board.GetLength(0);
                _columnsCount = board.GetLength(1);
                _unmarkedSet = new HashSet<int>();
                _markedRowsInColumns = new List<int>[_columnsCount];
                _markedColumnsInRows = new List<int>[_columnsCount];
                InitializeMarkedNumbersCollections();
                FillPositionsDictionary(board);
                FillUnmarkedSet(board);
            }

            public void MarkNumber(int number)
            {
                if (!_positionOfNumberOnBoardDictionary.ContainsKey(number)) return;
                _unmarkedSet.Remove(number);
                var (row, col) = _positionOfNumberOnBoardDictionary[number];
                _markedRowsInColumns[col].Add(row);
                _markedColumnsInRows[row].Add(col);
            }

            public int? BingoResult(int number)
            {
                var winRow = _markedColumnsInRows.FirstOrDefault(list => list.Count == _rowsCount);
                var winCol = _markedRowsInColumns.FirstOrDefault(list => list.Count == _columnsCount);

                var unmarkedSum = _unmarkedSet.Sum();

                if (winRow != default)
                {
                    return unmarkedSum * number;
                }
                if (winCol != default)
                {
                    return unmarkedSum * number;
                }

                return null;
            }

            private void InitializeMarkedNumbersCollections()
            {
                for (var i = 0; i < _columnsCount; i++)
                {
                    _markedRowsInColumns[i] = new List<int>();
                }

                for (var i = 0; i < _rowsCount; i++)
                {
                    _markedColumnsInRows[i] = new List<int>();
                }
            }

            private void FillPositionsDictionary(int[,] board)
            {
                for (var row = 0; row < _rowsCount; row++)
                {
                    for (var col = 0; col < _columnsCount; col++)
                    {
                        _positionOfNumberOnBoardDictionary[board[row, col]] = (row, col);
                    }
                }
            }

            private void FillUnmarkedSet(int[,] board)
            {
                for (var row = 0; row < _rowsCount; row++)
                {
                    for (var col = 0; col < _columnsCount; col++)
                    {
                        _unmarkedSet.Add(board[row, col]);
                    }
                }
            }
        }

        public override void PrintOutput()
        {
            ReadData(out var numbers, out var boards);
            var (firstScore, lastScore) = SimulateGame(numbers, boards);
            Console.WriteLine($@"Day4 - part 1: {firstScore}");
            Console.WriteLine($@"Day4 - part 2: {lastScore}");
        }

        private static (int firstScore, int lastScore) SimulateGame(List<int> numbers, List<BingoBoard> boards)
        {
            var lastScore = 0;
            var finishedBoards = new bool[boards.Count];
            var firstScore = -1;

            foreach (var number in numbers)
            {
                for (var i = 0; i < boards.Count; i++)
                {
                    if (finishedBoards[i]) continue;
                    var board = boards[i];
                    board.MarkNumber(number);

                    var bingoResult = board.BingoResult(number);
                    if (bingoResult == null) continue;
                    if (firstScore == -1) firstScore = bingoResult.Value;
                    lastScore = bingoResult.Value;
                    finishedBoards[i] = true;
                }
            }

            return (firstScore, lastScore);
        }

        private static void ReadData(out List<int> numbers, out List<BingoBoard> boards)
        {
            var data = Properties.Resources.DataDay4;
            var lines = data.Split("\r\n");

            numbers = new List<int>();
            boards = new List<BingoBoard>();

            var numbersLine = lines[0];
            foreach (var number in numbersLine.Split(','))
            {
                numbers.Add(int.Parse(number));
            }

            var line = 1;
            var boardLines = new List<string>();
            while (line < lines.Length)
            {
                if (lines[line] == "" && boardLines.Count > 0)
                {
                    boards.Add(new BingoBoard(TransformToBoardNumbers(boardLines)));
                    boardLines = new List<string>();
                }
                else if (lines[line] != "")
                {
                    boardLines.Add(lines[line]);
                }

                line++;
            }

            boards.Add(new BingoBoard(TransformToBoardNumbers(boardLines)));
        }

        private static int[,] TransformToBoardNumbers(List<string> boardLines)
        {
            var rowsCount = boardLines.Count;
            var firstLineSplit = boardLines[0].Split(' ').ToList().FindAll(x => x != "");
            var columnsCount = firstLineSplit.Count;
            var boardToReturn = new int[rowsCount, columnsCount];

            for (var row = 0; row < rowsCount; row++)
            {
                var currentLine = boardLines[row];
                var numbersInLine = currentLine.Split(' ').ToList().FindAll(x => x != "");
                for (var col = 0; col < columnsCount; col++)
                {
                    boardToReturn[row, col] = int.Parse(numbersInLine[col]);
                }
            }

            return boardToReturn;
        }
    }
}