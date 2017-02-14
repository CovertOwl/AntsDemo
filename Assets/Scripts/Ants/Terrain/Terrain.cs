namespace Ants.Terrain
{
    /// <summary>
    /// Represents the physical world in which actors can pass through. Terrain is tightly coupled with the pathfinding grid
    /// </summary>
    public abstract class Terrain : ITerrain
    {
        #region Properties
        /// <summary>
        /// Grid representing the pathfinding data for the terrain
        /// </summary>
        public Ants.Pathfinding.Grid pathfindingGrid { get; protected set; }
        #endregion

        #region ITerrain
        public abstract void GenerateTerrain(int seed);
        #endregion
    }
}
