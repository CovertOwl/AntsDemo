namespace Ants.Pathfinding
{
    /// <summary>
    /// 2D grid representing the map used for pathing.
    /// The grid is composed of fixed dimension tiles represented by GridNodes
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Data required to initialise the grid
        /// </summary>
        [System.Serializable]
        public struct InitData
        {
            /// <summary>
            /// How wide (along X axis) the grid is
            /// </summary>
            public uint DimX;

            /// <summary>
            /// How high (along Y axis) the grid is
            /// </summary>
            public uint DimY;

            /// <summary>
            /// How wide/high (X & Y axes respectively) a grid tile is
            /// </summary>
            public uint TileDim;
        }
    }
}