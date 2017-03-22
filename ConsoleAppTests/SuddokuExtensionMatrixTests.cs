using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CombinatorialEvolution.Sudoku.ConsoleApp;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleAppTests
{
    [TestClass]
    public class SuddokuExtensionMatrixTests
    {
        [TestMethod]
        public void GetAllBlocks()
        {
            int[][] problem = new int[9][];
            problem[0] = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
            problem[1] = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
            problem[2] = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
            problem[3] = new int[] { 3, 3, 3, 4, 4, 4, 5, 5, 5 };
            problem[4] = new int[] { 3, 3, 3, 4, 4, 4, 5, 5, 5 };
            problem[5] = new int[] { 3, 3, 3, 4, 4, 4, 5, 5, 5 };
            problem[6] = new int[] { 6, 6, 6, 7, 7, 7, 8, 8, 8 };
            problem[7] = new int[] { 6, 6, 6, 7, 7, 7, 8, 8, 8 };
            problem[8] = new int[] { 6, 6, 6, 7, 7, 7, 8, 8, 8 };

            for (var i = 0; i < 9; i++)
            {
                CollectionAssert.AreEqual(Enumerable.Repeat(i, 9).ToList(), problem.GetBlock(i).ToList());
            }
        }

        [TestMethod]
        public void GetLastBlockIndex()
        {
            var indexes = SudokuExtensions.GetBlockIndexes(8);

            CollectionAssert.AreEqual(new int[2] { 6, 6 }, indexes[0]);
            CollectionAssert.AreEqual(new int[2] { 6, 7 }, indexes[1]);
            CollectionAssert.AreEqual(new int[2] { 6, 8 }, indexes[2]);
            CollectionAssert.AreEqual(new int[2] { 7, 6 }, indexes[3]);
            CollectionAssert.AreEqual(new int[2] { 7, 7 }, indexes[4]);
            CollectionAssert.AreEqual(new int[2] { 7, 8 }, indexes[5]);
            CollectionAssert.AreEqual(new int[2] { 8, 6 }, indexes[6]);
            CollectionAssert.AreEqual(new int[2] { 8, 7 }, indexes[7]);
            CollectionAssert.AreEqual(new int[2] { 8, 8 }, indexes[8]);
        }
    }
}
