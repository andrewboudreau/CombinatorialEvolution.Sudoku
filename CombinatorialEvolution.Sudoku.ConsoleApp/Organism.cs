namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    public class Organism
    {
        private int[][] matrix;

        public int Age { get; set; }
        
        public OrganismType OrganismType { get; set; }

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
