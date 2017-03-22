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
        public void NoErrors()
        {
            var matrix = Program.CreateMatrix();
            MatrixWith18Errors(matrix);

            var errors = Program.Error(matrix);
            Assert.AreEqual(72, errors);
        }

        public static void MatrixWith18Errors(int[][] matrix)
        {
            for(var i = 0; i < matrix.Length; i++)
            {
                for(var k = 0; k < matrix[i].Length; k++)
                {
                    matrix[i][k] = k+1;
                }
            }
        }
    }
}
