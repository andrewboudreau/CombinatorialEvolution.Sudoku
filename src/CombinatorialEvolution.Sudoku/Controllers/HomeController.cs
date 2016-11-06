using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CombinatorialEvolution.Sudoku.Controllers
{
    public class HomeController : Controller
    {
        static Random rnd = new Random();

        private readonly ILogger<HomeController> logger;

        private static void DisplayMatrix(int[][] problem, ILogger<HomeController> logger)
        {
            if (problem == null || problem.Any(x => x == null))
            {
                logger.LogDebug("null matrix displayed");
                return;
            }

            for (var row = 0; row < problem.Length; row++)
            {
                logger.LogDebug(string.Join(", ", problem[row]));
            }
        }

        private static void CopyMatrix(int[][] source, int[][] destination)
        {
            for (var row = 0; row < source.Length; row++)
            {
                for (var column = 0; column < source[row].Length; column++)
                {
                    destination[row][column] = source[row][column];
                }
            }
        }

        public class Organism
        {
            private int[][] matrix;

            public int Age { get; set; }

            public int Type { get; set; }// 0 worker, 1 explorer

            public int Error { get; set; }

            public int[][] Matrix
            {
                get { return matrix; }
                set { CopyMatrix(value, matrix); }
            }
        }

        public static int[] ReadValuesForIndicies(int[][] matrix, int[][] indicies)
        {
            var result = new List<int>();
            foreach(var index in indicies)
            {
                result.Add(matrix[index[0]][index[1]]);
            }

            return result.ToArray();
        }

        public static int[][] CreateMatrix()
        {
            int[][] result = new int[9][];
            for(var row = 0; row < 9; row++)
            {
                result[row] = new int[9];
            }

            return result;
        }

        public static int[][] NeighborMatrix(int[][] problem, int[][] matrix)
        {
            int[][] result = CreateMatrix();
            CopyMatrix(matrix, result);

            int block = rnd.Next(0, 9);

            // Deletrmine which cells have values that can be swapped
            int[][] swaps = FindIndiciesOfEmptyCellsInProblem(problem);

            // pick two of those cells: (r1, c1) (r2,c2)
            int r1 = swaps[0][0], 
                c1 = swaps[0][1], 
                r2 = swaps[1][0], 
                c2 = swaps[1][1];
            

            int tmp = result[r1][c1];
            result[r1][c1] = result[r2][c2];
            result[r2][c2] = tmp;

            return result;
        }

        private static int[][] FindIndiciesOfEmptyCellsInProblem(int[][] problem)
        {
            var result = new List<int[]>();
            return result.ToArray();
        }

        public static int[][] RandomMatrix(int[][] matrix)
        {
            int[][] result = CreateMatrix();
            CopyMatrix(matrix, result);

            for (int block = 0; block < 9; ++block)
            {
                var blockIndicies = GetBlock(block);

                var valuesInBlock = ReadValuesForIndicies(result, blockIndicies).Where(x => x != 0).ToArray();

                // Create a shuffled list missing values for block
                var list =
                    new Queue<int>(
                        Enumerable.Range(1, 9)
                        .Except(valuesInBlock)
                        .OrderBy(x => Guid.NewGuid()));

                // walk through each cell in the current block
                foreach(var index in blockIndicies)
                {
                    //if cell is empty add a value from list
                    if (result[index[0]][index[1]] == 0)
                    {
                        result[index[0]][index[1]] = list.Dequeue();
                    }
                }
            }

            return result;
        }

        public static int[][] GetBlock(int block)
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
                    throw new ArgumentOutOfRangeException("block must be between 0 and 8");
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

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {

            logger.LogDebug("Begining solving Sudoku");
            logger.LogDebug("The problem is: ");

            int[][] problem = new int[9][];
            problem[0] = new int[] { 0, 0, 6, 2, 0, 0, 0, 8, 0 };
            problem[1] = new int[] { 0, 0, 8, 9, 7, 0, 0, 0, 0 };
            problem[2] = new int[] { 0, 0, 4, 8, 1, 0, 5, 0, 0 };
            problem[3] = new int[] { 0, 0, 0, 0, 6, 0, 0, 0, 2 };
            problem[4] = new int[] { 0, 7, 0, 0, 0, 0, 0, 3, 0 };
            problem[5] = new int[] { 6, 0, 0, 0, 5, 0, 0, 0, 0 };
            problem[6] = new int[] { 0, 0, 2, 0, 4, 7, 1, 0, 0 };
            problem[7] = new int[] { 0, 0, 3, 0, 2, 8, 4, 0, 0 };
            problem[8] = new int[] { 0, 5, 0, 0, 0, 1, 2, 0, 0 };

            var foo = RandomMatrix(problem);

            DisplayMatrix(problem, logger);

            int numOrganisms = 200;
            int maxEpochs = 5000;
            int maxRestarts = 20;
            int[][] solution = Solve(problem, numOrganisms, maxEpochs, maxRestarts);

            logger.LogDebug("Best solution found: ");
            DisplayMatrix(solution, logger);

            int err = Error(problem);
            if (err == 0)
            {
                logger.LogDebug("Success");
            }
            else
            {
                logger.LogDebug("Did not find optimal solution");
            }

            logger.LogDebug("Ending solving Sudoku");

            return View(foo);
        }

        private int Error(int[][] matrix)
        {
            var err = 0;

            // Determine missing values in each row
            for (var row = 0; row < matrix.Length; row++)
            {
                var found = new bool[9] { false, false, false, false, false, false, false, false, false };
                for (var i = 0; i < matrix.Length; i++)
                {
                    var value = matrix[row][i];
                    if (value != 0)
                    {
                        found[value - 1] = true;
                    }
                }

                err += found.Count(x => !x);
            }

            // Determine missing values in each column
            for (var column = 0; column < matrix[0].Length; column++)
            {
                var found = new bool[9] { false, false, false, false, false, false, false, false, false };
                for (var i = 0; i < matrix.Length; i++)
                {
                    var value = matrix[i][column];
                    if (value != 0)
                    {
                        found[value - 1] = true;
                    }
                }

                err += found.Count(x => !x);
            }

            return err;
        }

        private int[][] Solve(int[][] problem, int numOrganisms, int maxEpochs, int maxRestarts)
        {
            return new int[9][];
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
