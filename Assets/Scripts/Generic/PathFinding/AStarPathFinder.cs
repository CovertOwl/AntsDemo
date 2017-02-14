namespace Ants.Pathfinding
{
    /// <summary>
    /// Class for pathfinding that uses the A* algorithm
    /// </summary>
    public class AStarPathFinder : PathFinder
    {
        /// <summary>
        /// Wrapper for grid nodes with data required by the A* algorithm
        /// </summary>
        private class AStarNodeWrapper
        {
            /// <summary>
            /// Construct the node wrapper
            /// </summary>
            /// <param name="targetNode">Target node for the wrapper</param>
            public AStarNodeWrapper(GridNode targetNode)
            {
                this.TargetNode = targetNode;
            }

            /// <summary>
            /// Target grid node
            /// </summary>
            public GridNode TargetNode { get; set; }

            /// <summary>
            /// Parent grid node (node traversed through to get to this one)
            /// </summary>
            public AStarNodeWrapper Parent { get; set; }

            /// <summary>
            /// Estimated cost from this node to reach destination
            /// </summary>
            public float HeurisiticCost { get; set; }

            /// <summary>
            /// Travel cost to reach this node from the start
            /// </summary>
            public float TravelCost { get; set; }

            /// <summary>
            /// Total cost for this node
            /// </summary>
            public float TotalCost
            {
                get
                {
                    return HeurisiticCost + TravelCost;
                }
            }
        }
        
        #region Properties
        /// <summary>
        /// A* open nodes list, ordered by total cost
        /// </summary>
        private System.Collections.Generic.LinkedList<AStarNodeWrapper> openNodesList { get; set; }

        /// <summary>
        /// A* open closed list
        /// </summary>
        private System.Collections.Generic.List<AStarNodeWrapper> closedNodesList { get; set; }

        /// <summary>
        /// List of nodes in the "final" path
        /// </summary>
        private System.Collections.Generic.List<AStarNodeWrapper> pathNodes { get; set; }

        /// <summary>
        /// Whether the search has been completed
        /// </summary>
        private bool IsComplete { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default initialisations
        /// </summary>
        public AStarPathFinder()
        {
            openNodesList = new System.Collections.Generic.LinkedList<AStarNodeWrapper>();
            closedNodesList = new System.Collections.Generic.List<AStarNodeWrapper>();
            pathNodes = new System.Collections.Generic.List<AStarNodeWrapper>();

            IsComplete = false;
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Given an origin node and an adjacent node, return distance betweeen
        /// </summary>
        /// <param name="origin">Origin node for calculation</param>
        /// <param name="destination">Adjacent node for calculation</param>
        /// <returns>Calculated distance between</returns>
        private float DistanceToAdjacentNode(GridNode origin, GridNode adjacent)
        {
            //Working with the constraint that distance between adjacent nodes in any direction is uniform AND diagonal movement is allowed
            return 1.0f * adjacent.TravelCostModifier;
        }

        /// <summary>
        /// Given an origin node and a destination node, return heuristic for distance between
        /// </summary>
        /// <param name="origin">Origin node for calculation</param>
        /// <param name="destination">Destination node for calculation</param>
        /// <returns>Calculated heuristic for distance between</returns>
        private float HeuristicBetweenNodes(GridNode origin, GridNode destination)
        {
            //Working with the constraint that distance between adjacent nodes in any direction is uniform AND diagonal movement is allowed
            var displacementX = (int)destination.X - (int)origin.X;
            var displacementY = (int)destination.Y - (int)origin.Y;
            var displacementXAbs = UnityEngine.Mathf.Abs(displacementX);
            var displacementYAbs = UnityEngine.Mathf.Abs(displacementY);
            var displacementYVertOnly = displacementYAbs - UnityEngine.Mathf.Clamp(displacementYAbs - displacementXAbs, 0.0f, displacementYAbs);
            var displacementDiagonal = UnityEngine.Mathf.Clamp(displacementYAbs - displacementYVertOnly, 0.0f, displacementYAbs) * 1.4f;
            return displacementXAbs + displacementYVertOnly + displacementDiagonal;
        }

        /// <summary>
        /// When searching through nodes, this function can be used to process nodes
        /// </summary>
        /// <param name="subjectNode">The current node whose adjacent nodes are being processed</param>
        /// <param name="adjacentNode">The node to be processed</param>
        private void ProcessAdjacentNode(AStarNodeWrapper subjectNode, GridNode adjacentNode)
        {
            //If already complete, exit
            if (this.IsComplete)
            {
                return;
            }

            //If adjacent node is invalid, exit
            if (adjacentNode == null)
            {
                return;
            }

            UnityEngine.Profiling.Profiler.BeginSample("Closed List Search");
            //If node cannot be passed through or it already exists in closed list, ignore
            if (adjacentNode.Passable != true || this.closedNodesList.Find(a => a.TargetNode == adjacentNode) != null)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("Open List Search");
            //Check if node already exists in open list
            AStarNodeWrapper adjacentNodeWrapped = null;
            foreach (var openNode in this.openNodesList)
            {
                if (openNode.TargetNode == adjacentNode)
                {
                    //If it is quicker to travel between this subject node and the adjacent node, it needs to be updated
                    var newTravelCost = this.DistanceToAdjacentNode(subjectNode.TargetNode, adjacentNode) + subjectNode.TravelCost;
                    if (openNode.TravelCost > newTravelCost)
                    {
                        adjacentNodeWrapped = openNode;
                    }
                    else
                    {
                        UnityEngine.Profiling.Profiler.EndSample();
                        return;
                    }

                    break;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            //If node was already open and needs to be updated
            if (adjacentNodeWrapped != null)
            {
                this.openNodesList.Remove(adjacentNodeWrapped);
            }
            //Node is not yet open, start tracking it
            else
            {
                adjacentNodeWrapped = new AStarNodeWrapper(adjacentNode);
            }

            adjacentNodeWrapped.TravelCost = this.DistanceToAdjacentNode(subjectNode.TargetNode, adjacentNodeWrapped.TargetNode) + subjectNode.TravelCost;
            adjacentNodeWrapped.HeurisiticCost = this.HeuristicBetweenNodes(adjacentNodeWrapped.TargetNode, this.EndNode);
            adjacentNodeWrapped.Parent = subjectNode;

            //If found end
            if (adjacentNodeWrapped.TargetNode == this.EndNode)
            {
                //Add nodes to list & complete
                for (var finalPathNodeIter = adjacentNodeWrapped; finalPathNodeIter != null; finalPathNodeIter = finalPathNodeIter.Parent)
                {
                    this.pathNodes.Add(finalPathNodeIter);
                }
                this.pathNodes.Reverse();

                this.IsComplete = true;
                return;
            }

            UnityEngine.Profiling.Profiler.BeginSample("Open List Insert");
            //Insert wrapped node into open list, ordered
            if (this.openNodesList.Count != 0)
            {
                for (var openNodeIter = this.openNodesList.First; openNodeIter != null; openNodeIter = openNodeIter.Next)
                {
                    //If cost is less than current open node iteration
                    if (adjacentNodeWrapped.TotalCost < openNodeIter.Value.TotalCost)
                    {
                        this.openNodesList.AddBefore(openNodeIter, adjacentNodeWrapped);
                        break;
                    }
                    //Else if at end of list
                    else if (openNodeIter.Next == null)
                    {
                        this.openNodesList.AddAfter(openNodeIter, adjacentNodeWrapped);
                        break;
                    }
                }
            }
            else
            {
                this.openNodesList.AddFirst(adjacentNodeWrapped);
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
        #endregion

        #region PathFinder
        public override void StartPathSearch(Grid targetGrid, GridNode startNode, GridNode endNode)
        {
            this.TargetGrid = targetGrid;
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.openNodesList = new System.Collections.Generic.LinkedList<AStarNodeWrapper>();
            this.closedNodesList = new System.Collections.Generic.List<AStarNodeWrapper>();
            this.pathNodes = new System.Collections.Generic.List<AStarNodeWrapper>();

            this.IsComplete = false;

            this.openNodesList.AddFirst(new AStarNodeWrapper(this.StartNode));
        }

        public override void UpdateSearch(uint searchNodeStep = 0)
        {
            uint currentSearchStep = 1;

            //If is not complete, there are nodes left to check and not exceeded step limit already
            while (!IsComplete && openNodesList.Count > 0 && (currentSearchStep < searchNodeStep || searchNodeStep == 0))
            {
                //Get next open node
                var subjectNode = this.openNodesList.First.Value;
                this.openNodesList.RemoveFirst();
                this.closedNodesList.Add(subjectNode);

                UnityEngine.Profiling.Profiler.BeginSample("Discover Adjacent Nodes");
                //Discover adjacent nodes
                var leftNode = this.TargetGrid.NodeAt(subjectNode.TargetNode.X - 1, subjectNode.TargetNode.Y);
                var rightNode = this.TargetGrid.NodeAt(subjectNode.TargetNode.X + 1, subjectNode.TargetNode.Y);
                var topNode = this.TargetGrid.NodeAt(subjectNode.TargetNode.X, subjectNode.TargetNode.Y - 1);
                var bottomNode = this.TargetGrid.NodeAt(subjectNode.TargetNode.X, subjectNode.TargetNode.Y + 1);
                var topLeftNode = ((leftNode != null ? leftNode.Passable : false) && (topNode != null ? topNode.Passable : false)) ? 
                                    this.TargetGrid.NodeAt(subjectNode.TargetNode.X - 1, subjectNode.TargetNode.Y - 1) : null;
                var topRightNode = ((rightNode != null ? rightNode.Passable : false) && (topNode != null ? topNode.Passable : false)) ?
                                    this.TargetGrid.NodeAt(subjectNode.TargetNode.X + 1, subjectNode.TargetNode.Y - 1) : null;
                var bottomLeftNode = ((leftNode != null ? leftNode.Passable : false) && (bottomNode != null ? bottomNode.Passable : false)) ?
                                    this.TargetGrid.NodeAt(subjectNode.TargetNode.X - 1, subjectNode.TargetNode.Y + 1) : null;
                var bottomRightNode = ((rightNode != null ? rightNode.Passable : false) && (bottomNode != null ? bottomNode.Passable : false)) ?
                                    this.TargetGrid.NodeAt(subjectNode.TargetNode.X + 1, subjectNode.TargetNode.Y + 1) : null;
                UnityEngine.Profiling.Profiler.EndSample();


                UnityEngine.Profiling.Profiler.BeginSample("Process Nodes");
                //Process adjacent nodes
                this.ProcessAdjacentNode(subjectNode, leftNode);
                this.ProcessAdjacentNode(subjectNode, rightNode);
                this.ProcessAdjacentNode(subjectNode, topNode);
                this.ProcessAdjacentNode(subjectNode, bottomNode);
                this.ProcessAdjacentNode(subjectNode, topLeftNode);
                this.ProcessAdjacentNode(subjectNode, topRightNode);
                this.ProcessAdjacentNode(subjectNode, bottomLeftNode);
                this.ProcessAdjacentNode(subjectNode, bottomRightNode);
                UnityEngine.Profiling.Profiler.EndSample();

                ++currentSearchStep;
            }

            //If reached this point and there are no open nodes left, set complete to true
            if (openNodesList.Count == 0)
            {
                IsComplete = true;
            }
        }

        public override bool IsPathSearchComplete()
        {
            return this.IsComplete;
        }

        public override uint PathNodeCount()
        {
            return this.IsPathSearchComplete() ? (uint)this.pathNodes.Count : 0;
        }

        public override GridNode GetPathNode(uint index)
        {
            return this.pathNodes[(int)index].TargetNode;
        }
        #endregion
    }
}