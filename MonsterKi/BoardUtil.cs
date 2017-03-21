using BattleSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public static class BoardUtil
    {
        public static bool TestBoard(int[][] board, int[][] testArray, int rowOffset, int colOffset) // TODO: pass functions to operate on the cell
        {
            for (int rowIndex = 0; rowIndex < testArray.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < testArray[rowIndex].Count(); colIndex++)
                {
                    int expectedValue = testArray[rowIndex][colIndex];

                    if (expectedValue != 256)
                    {
                        int boardRow = rowOffset + rowIndex;
                        int boardCol = colOffset + colIndex;
                        if (ExistsField(board, boardRow, boardCol))
                        {
                            int val = board[boardRow][boardCol];
                            if (expectedValue != val)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool ExistsField(int[][] board, int rowIndex, int colIndex)
        {
            if (rowIndex >= 0 && rowIndex < board.Count())
            {
                int[] row = board[rowIndex];
                if (colIndex >= 0 && colIndex < row.Count())
                {
                    return true;
                }
            }
            return false;
        }

        // TODO: allow touching ships
        public static int[][] BuildShipTemplate(int size, Direction direction, int startCol, int startRow, int shipFieldValue)
        {
            if (direction == Direction.HORIZONTAL)
            { // horizontal
                int width = size + 2;
                int[][] template = new int[3][];
                template[0] = new int[width];

                int[] shipLine = new int[width];
                for (int i = 1; i < width - 1; i++)
                {
                    shipLine[i] = shipFieldValue;
                }

                template[1] = shipLine;
                template[2] = new int[width];
                return template;
            }
            else if (direction == Direction.VERTICAL)
            { // vertical
                int height = size + 2;
                int[][] template = new int[height][];
                for (int rowIndex = 0; rowIndex < height; rowIndex++)
                {
                    int[] row = new int[3];
                    if (rowIndex > 0 && rowIndex < height - 1)
                    {
                        row[1] = shipFieldValue;
                    }
                    template[rowIndex] = row;
                }
                return template;
            }
            throw new Exception("");
        }

        public static void SetFields(int[][] board, int[][] testArray, int rowOffset, int colOffset, FieldModifier match) // TODO: pass functions to operate on the cell
        {
            for (int rowIndex = 0; rowIndex < testArray.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < testArray[rowIndex].Count(); colIndex++)
                {
                    int expectedValue = testArray[rowIndex][colIndex];

                    int boardRow = rowOffset + rowIndex;
                    int boardCol = colOffset + colIndex;
                    if (ExistsField(board, boardRow, boardCol))
                    {
                        int val = board[boardRow][boardCol];
                        if (expectedValue == val)
                        {
                            match(board, boardRow, boardCol);
                        }
                    }
                }
            }
        }
    }

    public delegate void FieldModifier(int[][] state, int row, int column);

}