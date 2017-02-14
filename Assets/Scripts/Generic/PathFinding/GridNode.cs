namespace Ants.Pathfinding
{
    /// <summary>
    /// A node, or point of pathing, used to represent a single tile within the grid
    /// </summary>
    public class GridNode
    {
        /// <summary>
        /// Data required to initialise the node
        /// </summary>
        public class InitData
        {
            /// <summary>
            /// Cost to move through this node
            /// </summary>
            public float TravelCostModifier = 1.0f;

            /// <summary>
            /// Whether this node can be moved through
            /// </summary>
            public bool Passable = true;

            /// <summary>
            /// x axis position in grid
            /// </summary>
            public uint X = 0;

            /// <summary>
            /// y axis position in grid
            /// </summary>
            public uint Y = 0;

            /// <summary>
            /// x axis position in world
            /// </summary>
            public float WorldX = 0;

            /// <summary>
            /// y axis position in world
            /// </summary>
            public float WorldY = 0;
        }

        #region Properties
        /// <summary>
        /// Cost to move through this node
        /// </summary>
        public float TravelCostModifier { get; private set; }

        /// <summary>
        /// Whether this node can be moved through
        /// </summary>
        public bool Passable { get; private set; }

        /// <summary>
        /// x axis position in grid
        /// </summary>
        public uint X { get; private set; }

        /// <summary>
        /// y axis position in grid
        /// </summary>
        public uint Y { get; private set; }

        /// <summary>
        /// x axis position in world
        /// </summary>
        public float WorldX { get; private set; }

        /// <summary>
        /// y axis position in world
        /// </summary>
        public float WorldY { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct the node
        /// </summary>
        /// <param name="initData">Data required to initialise the node</param>
        public GridNode(InitData initData)
        {
            this.Passable = initData.Passable;
            this.TravelCostModifier = initData.TravelCostModifier;
            this.X = initData.X;
            this.Y = initData.Y;
            this.WorldX = initData.WorldX;
            this.WorldY = initData.WorldY;
        }
        #endregion
    }
}

