using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatorialEvolution.Sudoku.ConsoleApp
{

    public class Program
    {
        static Random rnd = new Random(0);

        static readonly ILogger logger = new ConsoleLogger();

        static void Main(string[] args)
        {
            SolveWithDefaultSettingsAndOutputDefaultSettings(SudokuProblems.ExtremeProblem_1);
            SolveWithDefaultSettingsAndOutputDefaultSettings(SudokuProblems.ExtremeProblem_2);
            SolveWithDefaultSettingsAndOutputDefaultSettings(SudokuProblems.MsdnProblem);
            Console.ReadKey();
        }

        public static void SolveWithDefaultSettingsAndOutputDefaultSettings(int[][] problem)
        {
            logger.LogDebug("Begining solving Sudoku");
            logger.LogDebug("The problem is: ");

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
        }

        /// <summary>
        /// Allows the problem to run multiple times if no 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="numOrganisms"></param>
        /// <param name="maxEpochs"></param>
        /// <param name="maxRestarts"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Iterates the evolutionary steps of a set
        /// </summary>
        /// <param name="problem">Sudoku problem</param>
        /// <param name="numOrganisms">Number of organisms used to solve the set</param>
        /// <param name="maxEpochs">max number of evolutions before giving up</param>
        /// <returns></returns>
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
                    Matrix = MatrixExtensions.CreateMatrix(),
                    OrganismType = (--numWorkers) >= 0 ? OrganismType.Worker : OrganismType.Explorer
                };
            }

            int epoch = 0;
            while (epoch < maxEpochs)
            {
                for (int i = 0; i < numOrganisms; ++i)
                {
                    var organism = hive[i];

                    // 0 worker, 1 explorer
                    if (organism.OrganismType == OrganismType.Worker)
                    {
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
                var bestWorkers = hive.Where(x => x.OrganismType == OrganismType.Worker).OrderBy(x => x.Error).ToList();
                var bestExplorers = hive.Where(x => x.OrganismType == OrganismType.Explorer).OrderBy(x => x.Error).ToList();
                var worstOfHive = hive.OrderByDescending(x => x.Error).ToList();

                for (var i = 0; i < 20; i++)
                {
                    worstOfHive.ElementAt(i).Matrix = MergeMatrices(bestWorkers.ElementAt(i).Matrix, bestExplorers.ElementAt(i).Matrix);
                }

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

        /// <summary>
        /// Creates a new matrix by first randomly selecting a block from <paramref name="matrix"/> then swapping to non-fixed cell values.
        /// </summary>
        /// <param name="problem">Sudoku problem used for referencing initial start values</param>
        /// <param name="matrix">Matrix to swap neighbors</param>
        /// <returns>A new matrix with swapped neighbors</returns>
        public static int[][] NeighborMatrix(int[][] problem, int[][] matrix)
        {
            var block = rnd.Next(0, 8);
            var result = matrix.Duplicate();
            var indexes = SudokuExtensions.GetBlockIndexes(block);

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

        /// <summary>
        /// Creates a new matrix by randomly copying blocks from <param name="m1">Matrix 1</param> and <param name="m2">Matrix 2</param>.
        /// </summary>
        /// <returns>A new matrix containing blocks from both inputs</returns>
        public static int[][] MergeMatrices(int[][] m1, int[][] m2)
        {
            var result = m1.Duplicate();
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
            var result = matrix.Duplicate();
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

        /// <summary>
        /// Counts the Errors in a sudoku problem
        /// </summary>
        /// <param name="matrix">Sudoku problem to inspect</param>
        /// <returns>The number of errors found</returns>
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

        /// <summary>
        /// Prints the given sudoku problem
        /// </summary>
        /// <param name="problem">The sudoku problem to print</param>
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
                logger.LogDebug($"{CellDisplayValue(problem, row, 0)}, {CellDisplayValue(problem, row, 1)},{CellDisplayValue(problem, row, 2)}  {CellDisplayValue(problem, row, 3)},{CellDisplayValue(problem, row, 4)},{CellDisplayValue(problem, row, 5)}  {CellDisplayValue(problem, row, 6)},{CellDisplayValue(problem, row, 7)},{CellDisplayValue(problem, row, 8)}");
                if ((row + 1) % 3 == 0)
                {
                    logger.LogDebug(string.Empty);
                }
            }
        }

        /// <summary>
        /// Helper function for printing empty cells.
        /// </summary>
        private static string CellDisplayValue(int[][] problem, int row, int col)
        {
            var value = problem.ValueForIndex(row, col);
            if (value == 0)
            {
                return "_";
            }

            return value.ToString();
        }

    }

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
}
