using System.Collections.Generic;
using System.Linq;

namespace CombinatorialEvolution.Sudoku.ConsoleApp
{
    public static class HiveExtensions
    {
        /// <summary>
        /// Filters the list to only contain <see cref="OrganismType.Explorer"/>. 
        /// </summary>
        /// <param name="hive">Hive to filter</param>
        /// <returns>An explorer <see cref="IEnumerable{Organism}">hive</see></returns>
        public static IEnumerable<Organism> Explorers(this Organism[] hive)
        {
            return hive.Where(x => x.OrganismType == OrganismType.Explorer);
        }

        /// <summary>
        /// Filters the list to only contain <see cref="OrganismType.Worker"/>. 
        /// </summary>
        /// <param name="hive">Hive to filter</param>
        /// <returns>A worker <see cref="IEnumerable{Organism}">hive</see></returns>
        public static IEnumerable<Organism> Workers(this Organism[] hive)
        {
            return hive.Where(x => x.OrganismType == OrganismType.Worker);
        }
    }
}
