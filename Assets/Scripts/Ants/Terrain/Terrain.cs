namespace Ants.Terrain
{
    /// <summary>
    /// Represents the physical world in which actors can pass through. Terrain is tightly coupled with the pathfinding grid
    /// </summary>
    public abstract class Terrain : ITerrain
    {
        #region Properties
        /// <summary>
        /// The pathfinding grid is tightly coupled with the terrain
        /// </summary>
        protected Ants.Pathfinding.Grid grid { get; private set; }
        #endregion

        #region ITerrain
        public abstract void GenerateTerrain(int seed);
        #endregion
    }
}
