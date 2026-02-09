using System;
using System.Collections.Generic;

namespace Puzzle15
{
    public class PuzzleBoard
    {
        public const int GridSize = 4;
        public const int EmptyTileValue = 0;

        private int[] _cells;
        private int _blankPos;
        private readonly Random _rng;

        public int EmptyRow => _blankPos / GridSize;
        public int EmptyCol => _blankPos % GridSize;

        public PuzzleBoard()
        {
            _cells = new int[GridSize * GridSize];
            _rng = new Random();
            ResetToSolved();
        }

        public int GetValue(int row, int col)
        {
            return _cells[row * GridSize + col];
        }

        public bool IsValidMove(int row, int col)
        {
            int target = row * GridSize + col;
            if (target == _blankPos)
                return false;

            int diff = target - _blankPos;

            if (diff == GridSize || diff == -GridSize)
                return true;

            if (diff == 1 && target % GridSize != 0)
                return true;

            if (diff == -1 && _blankPos % GridSize != 0)
                return true;

            return false;
        }

        public bool MoveTile(int row, int col)
        {
            if (!IsValidMove(row, col))
                return false;

            int idx = row * GridSize + col;
            _cells[_blankPos] = _cells[idx];
            _cells[idx] = EmptyTileValue;
            _blankPos = idx;

            return true;
        }

        public void Shuffle(int steps = 1000)
        {
            ResetToSolved();

            int previousEmpty = -1;

            for (int n = 0; n < steps; n++)
            {
                List<int> neighbors = FindNeighbors(_blankPos);

                List<int> candidates = new List<int>();
                for (int k = 0; k < neighbors.Count; k++)
                {
                    if (neighbors[k] != previousEmpty)
                        candidates.Add(neighbors[k]);
                }

                if (candidates.Count == 0)
                    candidates = neighbors;

                int pick = candidates[_rng.Next(candidates.Count)];

                previousEmpty = _blankPos;

                int pr = pick / GridSize;
                int pc = pick % GridSize;
                MoveTile(pr, pc);
            }
        }

        public bool IsSolved()
        {
            for (int i = 0; i < _cells.Length - 1; i++)
            {
                if (_cells[i] != i + 1)
                    return false;
            }
            return _cells[_cells.Length - 1] == EmptyTileValue;
        }

        public void ResetToSolved()
        {
            for (int i = 0; i < _cells.Length - 1; i++)
                _cells[i] = i + 1;

            _cells[_cells.Length - 1] = EmptyTileValue;
            _blankPos = _cells.Length - 1;
        }

        private List<int> FindNeighbors(int pos)
        {
            List<int> result = new List<int>(4);
            int r = pos / GridSize;
            int c = pos % GridSize;

            if (r > 0)
                result.Add(pos - GridSize);
            if (r < GridSize - 1)
                result.Add(pos + GridSize);
            if (c > 0)
                result.Add(pos - 1);
            if (c < GridSize - 1)
                result.Add(pos + 1);

            return result;
        }
    }
}
