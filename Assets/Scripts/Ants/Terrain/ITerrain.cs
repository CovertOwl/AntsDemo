﻿namespace Ants.Terrain
{
    /// <summary>
    /// For interfacing with terrain
    /// </summary>
    public class ITerrain
    {
        /// <summary>
        /// Generate this object's terrain from seed. Using the same seed on the same machine will guarantee deterministic results.
        /// </summary>
        /// <param name="seed">Seed used for psuedo-random generation</param>
        /// <param name="gridData">Seed used for psuedo-random generation</param>
        /// <returns>An instantiated terrain object.</returns>
        void GenerateTerrain(int seed, Ants.Pathfinding.Grid.InitData gridData)
        {

        }
    }
}
