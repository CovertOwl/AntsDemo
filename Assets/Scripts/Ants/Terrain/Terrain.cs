namespace Ants.Terrain
{
    /// <summary>
    /// Represents the physical world in which actors can pass through. Terrain is tightly coupled with the pathfinding grid
    /// </summary>
    public class Terrain : ITerrain
    {
        /// <summary>
        /// The pathfinding grid is tightly coupled with the terrain
        /// </summary>
        protected Ants.Pathfinding.Grid grid { get; private set; }

        #region ITerrain
        void GenerateTerrain(int seed, Ants.Pathfinding.Grid.InitData gridData)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
