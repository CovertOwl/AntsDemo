namespace Ants.Pathfinding
{
    /// <summary>
    /// Interface for pathfinding
    /// </summary>
    public interface IPathFinder
    {
        /// <summary>
        /// Start the path search. However, the path will not be completed immediately.
        /// </summary>
        /// <param name="targetGrid">The grid in which to search a path</param>
        /// <param name="startNode">The node to start pathing from</param>
        /// <param name="endNode">The node to discover a path to</param>
        void StartPathSearch(Grid targetGrid, GridNode startNode, GridNode endNode);

        /// <summary>
        /// Progresses the search
        /// </summary>
        /// <param name="searchStep">Optional parameter specifying how many nodes are stepped through before the method call exits. Set to 0 to search all nodes.</param>
        void UpdateSearch(uint searchNodeStep = 0);

        /// <summary>
        /// Use this to check whether the final path is discovered
        /// </summary>
        /// <returns>True if the final path is discovered</returns>
        bool IsPathSearchComplete();

        /// <summary>
        /// How many nodes traversed in the path
        /// </summary>
        /// <returns>How many nodes traversed in the path</returns>
        uint PathNodeCount();

        /// <summary>
        /// Get a node from the path by index
        /// </summary>
        /// <param name="index">Index of the node in the final path</param>
        /// <returns>A node from the path by index</returns>
        GridNode GetPathNode(uint index);
    }
}