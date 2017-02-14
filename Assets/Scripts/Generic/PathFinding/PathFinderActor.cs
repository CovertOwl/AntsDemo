namespace Ants.Pathfinding
{
    /// <summary>
    /// Actor for the path finding class
    /// </summary>
    public class PathFinderActor : Ants.Actor.Actor, Ants.Pathfinding.IPathFinder
    {
        /// <summary>
        /// The different types of path finding that can be used
        /// </summary>
        public enum PathFinderType
        {
            AStar
        }

        #region Properties
        /// <summary>
        /// The type of path finding used by this actor
        /// </summary>
        public PathFinderType PathFindingType = PathFinderType.AStar;

        /// <summary>
        /// Whether to show lines representing the path discovered
        /// </summary>
        private bool debugView = false;
        /// <summary>
        /// Whether to show lines representing the path discovered
        /// </summary>
        public bool DebugView
        {
            get
            {
                return this.debugView;
            }

            set
            {
                this.debugView = value;

                this.pathlineRenderer.enabled = this.debugView;
            }
        }

        /// <summary>
        /// THe path finder used by this actor
        /// </summary>
        private Ants.Pathfinding.PathFinder pathFinder { get; set; }

        /// <summary>
        /// The target grid for path finding
        /// </summary>
        private Ants.Pathfinding.Grid targetGrid { get; set; }

        /// <summary>
        /// The line renderer used to render the path in debug view
        /// </summary>
        private UnityEngine.LineRenderer pathlineRenderer { get; set; }

        /// <summary>
        /// Material applied to meshes in simple terrain
        /// </summary>
        private UnityEngine.Material defaultSpriteMaterial { get; set; }
        #endregion

        #region UnityInterface
        void Awake()
        {
            //Instantiate path finder dynamically
            switch (PathFindingType)
            {
                case PathFinderType.AStar:
                {
                    this.pathFinder = new Ants.Pathfinding.AStarPathFinder();

                    break;
                }

                default:
                {
                    throw new System.Exception("Invalid type.");
                }
            }

            //Discover sprite material
            this.defaultSpriteMaterial = new UnityEngine.Material(UnityEngine.Shader.Find("Sprites/Default"));
            if (this.defaultSpriteMaterial == null)
            {
                throw new System.Exception("Failed to discover default sprite shader.");
            }

            //Instantiate the line renderer dynamically and attach as child to this ojject
            var gameObj = new UnityEngine.GameObject("Path Renderer");
            gameObj.transform.parent = transform;
            gameObj.transform.localPosition = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
            this.pathlineRenderer = gameObj.AddComponent<UnityEngine.LineRenderer>();

            this.pathlineRenderer.startColor = new UnityEngine.Color(0.0f, 0.0f, 1.0f, 0.75f);
            this.pathlineRenderer.endColor = this.pathlineRenderer.startColor * new UnityEngine.Color(0.75f, 0.75f, 0.75f, 1.0f);
            this.pathlineRenderer.startWidth = 0.1f;
            this.pathlineRenderer.endWidth = this.pathlineRenderer.startWidth;
            this.pathlineRenderer.sharedMaterial = this.defaultSpriteMaterial;
        }
        #endregion

        #region IPathFinder
        public void StartPathSearch(Ants.Pathfinding.Grid targetGrid, Ants.Pathfinding.GridNode startNode, Ants.Pathfinding.GridNode endNode)
        {
            this.targetGrid = targetGrid;
            this.pathFinder.StartPathSearch(targetGrid, startNode, endNode);
            this.pathlineRenderer.numPositions = 0;
        }

        public void UpdateSearch(uint searchNodeStep = 0)
        {
            //Perform search update
            UnityEngine.Profiling.Profiler.BeginSample("Update Search");
            this.pathFinder.UpdateSearch(searchNodeStep);
            UnityEngine.Profiling.Profiler.EndSample();

            //Update lines for path
            if (this.pathFinder.PathNodeCount() > 1 && this.pathFinder.PathNodeCount() != this.pathlineRenderer.numPositions)
            {
                UnityEngine.Profiling.Profiler.BeginSample("Update Final Path Lines");
                int numPositions = this.pathlineRenderer.numPositions;
                this.pathlineRenderer.numPositions = (int)this.pathFinder.PathNodeCount();
                for (int a = numPositions; a < this.pathFinder.PathNodeCount(); ++a)
                {
                    var currentNode = this.pathFinder.GetPathNode((uint)a);
                    this.pathlineRenderer.SetPosition((int)a, new UnityEngine.Vector3(currentNode.WorldX, currentNode.WorldY, -1.0f));
                }
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }

        public bool IsPathSearchComplete()
        {
            return this.pathFinder.IsPathSearchComplete();
        }

        public uint PathNodeCount()
        {
            return this.pathFinder.PathNodeCount();
        }

        public Ants.Pathfinding.GridNode GetPathNode(uint index)
        {
            return this.pathFinder.GetPathNode(index);
        }
        #endregion
    }
}
