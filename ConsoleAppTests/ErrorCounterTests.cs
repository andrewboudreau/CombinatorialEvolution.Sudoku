using CombinatorialEvolution.Sudoku.ConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTests
{
    [TestClass]
    public class ErrorCounterTests
    {
        [TestMethod]
        public void Expect72ErrorsWithCountedBlocks()
        {
            var matrix = Program.CreateMatrix();
            CountedBlocks(matrix);

            var errors = Program.Error(matrix);
            Assert.AreEqual(72, errors);
        }

        [TestMethod]
        public void Expect144ErrorsWithAllOnes()
        {
            var matrix = Program.CreateMatrix();
            AllToGivenValue(matrix, 1);

            var errors = Program.Error(matrix);
            Assert.AreEqual(144, errors);
        }

        [TestMethod]
        public void NoErrors()
        {
            var errors = Program.Error(Problems.NonEasyProblem_Solution);
            Assert.AreEqual(0, errors);
        }

        [TestMethod]
        public void TwoErrors()
        {
            // Arrange
            var problem = Program.CopyMatrix(Problems.NonEasyProblem_Solution);
            problem[0][0] = 1;

            // Act
            var errors = Program.Error(problem);

            // Assert
            Assert.AreEqual(2, errors);
        }

        [TestMethod]
        public void AllValuesMissing()
        {
            // Arrange
            var matrix = Program.CreateMatrix();
            AllToGivenValue(matrix, 0);

            // Act
            var errors = Program.Error(matrix);

            // Assert all rows and all columns are in errors
            Assert.AreEqual((9 * 9) + (9 * 9), errors);
        }

        public static void CountedBlocks(int[][] matrix)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var k = 0; k < matrix[i].Length; k++)
                {
                    matrix[i][k] = k + 1;
                }
            }
        }

        public static void AllToGivenValue(int[][] matrix, int value)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var k = 0; k < matrix[i].Length; k++)
                {
                    matrix[i][k] = value;
                }
            }
        }
    }
}
