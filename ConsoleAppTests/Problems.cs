using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTests
{
    public class Problems
    {
        public static int[][] NonEasyProblem = new int[][]
        {
            new int[] { 0,0,6, 2,0,0, 0,8,0 },
            new int[] { 0,0,8, 9,7,0, 0,0,0 },
            new int[] { 0,0,4, 8,1,0, 5,0,0 },

            new int[] { 0,0,0, 0,6,0, 0,0,2 },
            new int[] { 0,7,0, 0,0,0, 0,3,0 },
            new int[] { 6,0,0, 0,5,0, 0,0,0 },

            new int[] { 0,0,2, 0,4,7, 1,0,0 },
            new int[] { 0,0,3, 0,2,8, 4,0,0 },
            new int[] { 0,5,0, 0,0,1, 2,0,0 }
        };

        public static int[][] NonEasyProblem_Solution = new int[][]
        {
            new int[] { 7,1,6, 2,3,5, 9,8,4 },
            new int[] { 5,2,8, 9,7,4, 3,1,6 },
            new int[] { 3,9,4, 8,1,6, 5,2,7 },

            new int[] { 8,4,5, 1,6,3, 7,9,2 },
            new int[] { 2,7,1, 4,8,9, 6,3,5 },
            new int[] { 6,3,9, 7,5,2, 8,4,1 },

            new int[] { 9,8,2, 6,4,7, 1,5,3 },
            new int[] { 1,6,3, 5,2,8, 4,7,9 },
            new int[] { 4,5,7, 3,9,1, 2,6,8 }
        };
    }
}
