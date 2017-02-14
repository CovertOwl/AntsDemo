namespace Ants.Pathfinding
{
    /// <summary>
    /// Base class for path finding
    /// </summary>
    public abstract class PathFinder : IPathFinder
    {
        /// <summary>
        /// The pathfinding grid used by this object
        /// </summary>
        public Grid TargetGrid { get; protected set; }

        /// <summary>
        /// The starting node for the path
        /// </summary>
        public GridNode StartNode { get; protected set; }

        /// <summary>
        /// The end node for the path
        /// </summary>
        public GridNode EndNode { get; protected set; }

        #region IPathFinder
        public abstract void StartPathSearch(Grid targetGrid, GridNode startNode, GridNode endNode);

        public abstract void UpdateSearch(uint searchNodeStep = 0);

        public abstract bool IsPathSearchComplete();

        public abstract uint PathNodeCount();

        public abstract GridNode GetPathNode(uint index);
        #endregion
    }
}