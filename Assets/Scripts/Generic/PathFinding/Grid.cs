namespace Ants.Pathfinding
{
    /// <summary>
    /// 2D grid representing the map used for pathing.
    /// The grid is composed of fixed dimension tiles represented by GridNodes
    /// </summary>
    public class Grid
    {
        #region Properties
        /// <summary>
        /// How wide (along X axis) the grid is
        /// </summary>
        public uint DimX { get; private set; }

        /// <summary>
        /// How high (along Y axis) the grid is
        /// </summary>
        public uint DimY { get; private set; }

        /// <summary>
        /// How wide/high (X & Y axes respectively) a grid tile is
        /// </summary>
        public uint TileDim { get; private set; }

        /// <summary>
        /// THe nodes that make up the grid
        /// </summary>
        private GridNode[] nodes { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct the grid
        /// </summary>
        /// <param name="dimX">How wide (along X axis) the grid is</param>
        /// <param name="dimY">How high (along Y axis) the grid is</param>
        /// <param name="tileDim">How wide/high (X & Y axes respectively) a grid tile is</param>
        /// <param name="gridNodeData">Init data for grid nodes, x major indexing</param>
        public Grid(uint dimX, uint dimY, uint tileDim, GridNode.InitData[,] gridNodeData)
        {
            this.DimX = dimX;
            this.DimY = dimY;
            this.TileDim = tileDim;

            //Validate grid node init data
            if (gridNodeData == null || gridNodeData.GetLength(0) != this.DimX || gridNodeData.GetLength(1) != this.DimY)
            {
                throw new System.Exception("Invalid grid node data.");
            }

            //Construct gride nodes
            this.nodes = new GridNode[this.DimX * this.DimY];
            for (uint x = 0; x < this.DimX; ++x)
            {
                for (uint y = 0; y < this.DimY; ++y)
                {
                    this.nodes[NodePositionToIndex(x, y)] = new GridNode(gridNodeData[x, y]);
                }
            }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Get the grid not by position
        /// </summary>
        /// <param name="x">x axis position of node</param>
        /// <param name="y">t axis position of node</param>
        /// <returns>Node at position</returns>
        public GridNode NodeAt(uint x, uint y)
        {
            return x < this.DimX && y < this.DimY ? this.nodes[x + y * this.DimX] : null;
        }

        /// <summary>
        /// Given a world position, return the grid node targeted (with respect to the node bounds).
        /// Note, this is working with the constraint the grid is always centered at 0,0,0
        /// </summary>
        /// <param name="worldPos">The world position</param>
        /// <returns>The closest node</returns>
        public GridNode WorldToTile(UnityEngine.Vector3 worldPos)
        {
            return NodeAt(
                (uint)((int)(worldPos.x + this.DimX * 0.5f * this.TileDim) / (int)this.TileDim),
                (uint)((int)(-worldPos.y + this.DimY * 0.5f * this.TileDim) / (int)this.TileDim)
                );
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Given a node position, return an index that can be used to reference the node in the 1D array internally
        /// </summary>
        /// <param name="x">x axis position</param>
        /// <param name="y">y axis position</param>
        /// <returns>The index that can be used to reference the node in the 1D array internally</returns>
        private uint NodePositionToIndex(uint x, uint y)
        {
            return x + y * this.DimX;
        }
        #endregion
    }
}