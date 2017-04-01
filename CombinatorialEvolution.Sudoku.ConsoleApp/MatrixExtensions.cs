namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    /// <summary>
    /// Extensions methods for working with <see cref="int[][]"/> 
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// Copies the values from the source into the destination matrix.
        /// </summary>
        /// <param name="destination">Matrix to copy values into</param>
        public static void CopyTo(this int[][] source, int[][] destination)
        {
            for (var row = 0; row < source.Length; row++)
            {
                for (var column = 0; column < source[row].Length; column++)
                {
                    destination[row][column] = source[row][column];
                }
            }
        }

        /// <summary>
        /// Creates a duplicate of the given matrix.
        /// </summary>
        /// <returns>A new matrix with the same values as the source</returns>
        public static int[][] Duplicate(this int[][] source)
        {
            var destination = CreateMatrix();
            source.CopyTo(destination);
            return destination;
        }

        /// <summary>
        /// Creates an empty 9 by 9 matrix.
        /// </summary>
        /// <returns>Return an empty 9 by 9 matrix</returns>
        public static int[][] CreateMatrix()
        {
            int[][] result = new int[9][];
            for (var row = 0; row < 9; row++)
            {
                result[row] = new int[9];
            }

            return result;
        }
    }
}
