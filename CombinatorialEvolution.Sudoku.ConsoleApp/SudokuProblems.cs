namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    public static class SudokuProblems
    {
        public static int[][] MsdnProblem = new int[][]
        {
            new int[] { 0,0,6, 2,0,0, 0,8,0 },
            new int[] { 0,0,8, 9,7,0, 0,0,0 },
            new int[] { 0,0,4, 8,1,0, 5,0,0 },

            new int[] { 0,0,0, 0,6,0, 0,0,2 },
            new int[] { 0,7,0, 0,0,0, 0,3,0 },
            new int[] { 6,0,0, 0,5,0, 0,0,0 },

            new int[] { 0,0,2, 0,4,7, 1,0,0 },
            new int[] { 0,0,3, 0,2,8, 4,0,0 },
            new int[] { 0,5,0, 0,0,1, 2,0,0 },
        };

        public static int[][] ExtremeProblem_1 = new int[][]
        {
            new int[] { 0,7,0, 0,2,0, 0,8,0 },
            new int[] { 1,0,0, 7,0,9, 0,0,3 },
            new int[] { 0,0,4, 0,0,0, 7,0,0 },

            new int[] { 0,8,0, 0,5,0, 0,7,0 },
            new int[] { 3,0,0, 2,0,6, 0,0,5 },
            new int[] { 0,4,0, 0,1,0, 0,6,0 },

            new int[] { 0,0,1, 0,0,0, 6,0,0 },
            new int[] { 6,0,0, 5,0,8, 0,0,4 },
            new int[] { 0,2,0, 0,6,0, 0,5,0 }
        };

        public static int[][] ExtremeProblem_2 = new int[][]
        {
            new int[] { 0,0,8, 6,0,0, 0,0,5 },
            new int[] { 0,7,0, 0,8,0, 0,1,0 },
            new int[] { 3,0,0, 0,0,9, 7,0,0 },

            new int[] { 2,0,0, 0,0,4, 3,0,0 },
            new int[] { 0,3,0, 0,2,0, 0,4,0 },
            new int[] { 0,0,7, 5,0,0, 0,0,9 },

            new int[] { 0,0,1, 8,0,0, 0,0,3 },
            new int[] { 0,6,0, 0,7,0, 0,5,0 },
            new int[] { 5,0,0, 0,0,2, 4,0,0 }
        };
    }
}
