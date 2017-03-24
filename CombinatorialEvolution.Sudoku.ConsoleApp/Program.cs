using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    public interface ILogger
    {
        void LogDebug(string msg);
    }

    public class ConsoleLogger : ILogger
    {
        public void LogDebug(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    public class Program
    {
        static Random rnd = new Random(0);

        static readonly ILogger logger = new ConsoleLogger();

        static void Main(string[] args)
        {
            logger.LogDebug("Begining solving Sudoku");
            logger.LogDebug("The problem is: ");

            //int[][] problem = SudokuProblems.MsdnProblem;
            //int[][] problem = SudokuProblems.ExtremeProblem_1;
            int[][] problem = SudokuProblems.ExtremeProblem_2;

            DisplayMatrix(problem);

            int numOrganisms = 200;
            int maxEpochs = 5000;
            int maxRestarts = 20;

            int[][] solution = Solve(problem, numOrganisms, maxEpochs, maxRestarts);

            logger.LogDebug("Best solution found: ");
            DisplayMatrix(solution);

            int err = Error(solution);
            if (err == 0)
            {
                logger.LogDebug("Success");
            }
            else
            {
                logger.LogDebug("Did not find optimal solution");
            }

            logger.LogDebug("Ending solving Sudoku");
            Console.ReadKey();
        }

        public static int[][] Solve(int[][] problem, int numOrganisms, int maxEpochs, int maxRestarts)
        {
            var restarts = 0;
            int[][] result = null;

            while (restarts < maxRestarts)
            {
                result = SolveEvo(problem, numOrganisms, maxEpochs);
                var err = Error(result);
                if (err == 0)
                {
                    break;
                }

                logger.LogDebug($"Restarting {restarts}, Best Solution has {err}");
                restarts++;
            }

            return result;
        }

        public static int[][] SolveEvo(int[][] problem, int numOrganisms, int maxEpochs)
        {
            int numWorkers = (int)(numOrganisms * 0.90);
            int numExplorer = numOrganisms - numWorkers;
            Organism[] hive = new Organism[numOrganisms];

            // initialize each Organism
            for (var i = 0; i < numOrganisms; i++)
            {
                hive[i] = new Organism()
                {
                    Matrix = CreateMatrix(),
                    Type = (--numWorkers) >= 0 ? 0 : 1
                };
            }

            int epoch = 0;
            while (epoch < maxEpochs)
            {
                for (int i = 0; i < numOrganisms; ++i)
                {
                    var organism = hive[i];

                    // 0 worker, 1 explorer
                    if (organism.Type == 0)
                    {
                        //process each organism
                        var neighbor = NeighborMatrix(problem, organism.Matrix);
                        if (Error(neighbor) <= Error(organism.Matrix) || rnd.Next(1000) < 2)
                        {
                            organism.Matrix = neighbor;
                            organism.Age = 0;
                        }
                        else
                        {
                            organism.Age++;
                        }
                    }
                    else
                    {
                        organism.Matrix = RandomMatrix(problem);
                    }
                }

                // merge best worker with best explorer, incrememnt epoch
                var bestWorker = hive.Where(x => x.Type == 0).OrderBy(x => x.Error).First().Matrix;
                var bestExplorer = hive.Where(x => x.Type == 1).OrderBy(x => x.Error).First().Matrix;

                hive.OrderByDescending(x => x.Error).First().Matrix = MergeMatrices(bestWorker, bestExplorer);
                if (hive.Any(x => x.Error == 0))
                {
                    break;
                }

                epoch++;

                if (epoch % 1000 == 0)
                {
                    logger.LogDebug($"Epoch {epoch} of {maxEpochs}");
                }
            }

            return hive.OrderBy(x => x.Error).First().Matrix;
        }

        private static void DisplayMatrix(int[][] problem)
        {
            if (problem == null || problem.Any(x => x == null))
            {
                logger.LogDebug("null matrix displayed");
                return;
            }

            logger.LogDebug(string.Empty);
            for (var row = 0; row < problem.Length; row++)
            {
                logger.LogDebug($"{problem[row][0]},{problem[row][1]},{problem[row][2]}  {problem[row][3]},{problem[row][4]},{problem[row][5]}  {problem[row][6]},{problem[row][7]},{problem[row][8]}");
                if ((row + 1) % 3 == 0)
                {
                    logger.LogDebug(string.Empty);
                }
            }
        }

        public static int[][] CopyMatrix(int[][] source)
        {
            var destination = CreateMatrix();
            CopyMatrix(source, destination);
            return destination;
        }

        public static void CopyMatrix(int[][] source, int[][] destination)
        {
            for (var row = 0; row < source.Length; row++)
            {
                for (var column = 0; column < source[row].Length; column++)
                {
                    destination[row][column] = source[row][column];
                }
            }
        }

        public static int[][] CreateMatrix()
        {
            int[][] result = new int[9][];
            for (var row = 0; row < 9; row++)
            {
                result[row] = new int[9];
            }

            return result;
        }

        public static int[][] NeighborMatrix(int[][] problem, int[][] matrix)
        {
            var result = CopyMatrix(matrix);
            var indexes = SudokuExtensions.GetBlockIndexes(rnd.Next(0, 9));

            // determine two cells in the same block that don't contain a fixed value
            int[][] swaps = indexes
                .Where(coord => problem.ValueForIndex(coord) == 0)
                .OrderBy(_ => Guid.NewGuid())
                .Take(2)
                .ToArray();

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

        public static int[][] MergeMatrices(int[][] m1, int[][] m2)
        {
            var result = CopyMatrix(m1);
            for (var i = 0; i < 9; ++i)
            {
                if (rnd.NextDouble() > 0.5)
                {
                    var indexes = SudokuExtensions.GetBlockIndexes(i);
                    foreach (var index in indexes)
                    {
                        result[index[0]][index[1]] = m2[index[0]][index[1]];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Populates a matrix such that each 3x3 grid is valid.
        /// </summary>
        /// <param name="matrix">partially populated matrix</param>
        /// <returns>a fully populated matrix where all 3x3 cells are in a valid state</returns>
        public static int[][] RandomMatrix(int[][] matrix)
        {
            var result = CopyMatrix(matrix);
            for (int block = 0; block < 9; ++block)
            {
                var valuesInBlock = result.GetBlock(block).Where(x => x != 0).ToArray();

                // Create a shuffled list missing values for block
                var list =
                    new Queue<int>(
                        Enumerable.Range(1, 9)
                        .Except(valuesInBlock)
                        .OrderBy(x => Guid.NewGuid()));

                // walk through each cell in the current block
                foreach (var index in SudokuExtensions.GetBlockIndexes(block))
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

        public static int Error(int[][] matrix)
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
            set
            {
                matrix = value;
                Error = Program.Error(matrix);
            }
        }
    }
}
