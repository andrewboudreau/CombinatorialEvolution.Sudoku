using System;
using System.Collections.Generic;

namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    public static class SudokuExtensions
    {
        /// <summary>
        /// Gets a <see cref="List{int[][]}">row and column coordinates</see> for the cells in the given <paramref name="block"/>.
        /// </summary>
        /// <param name="block">A value from 0 to 8 indexing the blocks a matrix.</param>
        /// <returns>A collection of row and column pairs</returns>
        public static int[][] GetBlockIndexes(int block)
        {
            int row, col;
            switch (block)
            {
                case 0:
                    row = 0;
                    col = 0;
                    break;

                case 1:
                    row = 0;
                    col = 3;
                    break;

                case 2:
                    row = 0;
                    col = 6;
                    break;

                case 3:
                    row = 3;
                    col = 0;
                    break;

                case 4:
                    row = 3;
                    col = 3;
                    break;

                case 5:
                    row = 3;
                    col = 6;
                    break;

                case 6:
                    row = 6;
                    col = 0;
                    break;

                case 7:
                    row = 6;
                    col = 3;
                    break;

                case 8:
                    row = 6;
                    col = 6;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("block must be a value from 0 to 8");
            }

            var result = new List<int[]>(9);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    result.Add(new int[2] { row + i, col + j });
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns the value at a given index in the matrix
        /// </summary>
        /// <param name="coordinates">Row and column to query</param>
        /// <returns>Value of the cell</returns>
        public static int ValueForIndex(this int[][] matrix, int[] coordinates)
        {
            return ValueForIndex(matrix, coordinates[0], coordinates[1]);
        }
        
        /// <summary>
         /// Returns the value at a given index in the matrix
         /// </summary>
         /// <returns>Value of the cell</returns>
        public static int ValueForIndex(this int[][] matrix, int row, int col)
        {
            return matrix[row][col];
        }

        /// <summary>
        /// List the 9 values in a block, blocks ordered 0 to 8, starting top left and moving right then down
        /// </summary>
        /// <param name="block">Index of block to inspect, 0 through 8</param>
        /// <returns></returns>
        public static int[] GetBlock(this int[][] matrix, int block)
        {
            if (block < 0 || block > 8)
            {
                throw new ArgumentOutOfRangeException("block must be a value from 0 to 8");
            }

            var result = new List<int>();
            foreach (var index in GetBlockIndexes(block))
            {
                result.Add(matrix[index[0]][index[1]]);
            }

            return result.ToArray();
        }
    }
}
