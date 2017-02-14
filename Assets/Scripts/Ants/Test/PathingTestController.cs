namespace Ants.Test
{
    /// <summary>
    /// Simple controller class hacked together for quick testing of path finding
    /// Allows use of mouse to create paths
    /// Attach to any object
    /// Mouse0 = Place origin. More than one can be placed at once.
    /// Mouse1 = Start pathing for placed origins.
    /// </summary>
    public class PathingTestController : UnityEngine.MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Target world for the scene
        /// </summary>
        public Ants.World.WorldActor World;

        /// <summary>
        /// THe node last moused over
        /// </summary>
        private Ants.Pathfinding.GridNode mousedOverNode;

        /// <summary>
        /// Active path finders
        /// </summary>
        private System.Collections.Generic.List<Ants.Pathfinding.PathFinderActor> targetPathfinders;

        /// <summary>
        /// Node origins selected
        /// </summary>
        private System.Collections.Generic.List<Ants.Pathfinding.GridNode> leftClickedNodes = new System.Collections.Generic.List<Pathfinding.GridNode>();
        #endregion

        #region UnityInterface
        public void Start()
        {
            //Discover world actor
            if (World == null)
            {
                var rootObject = UnityEngine.GameObject.Find("Root");
                World = rootObject != null ? rootObject.GetComponent<Ants.World.WorldActor>() : null;

                if (World == null)
                {
                    throw new System.Exception("World not set.");
                }
            }

            targetPathfinders = new System.Collections.Generic.List<Pathfinding.PathFinderActor>();
        }

        public void Update()
        {
            //Find path finding grid
            var targetGrid = World.TerrainActor.Terrain.pathfindingGrid;
            if (targetGrid == null)
            {
                return;
            }

            //Find node pointed at by mouse
            var worldPos = UnityEngine.Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            mousedOverNode = targetGrid.WorldToTile(worldPos);

            //If left click
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                //Add node to list as origin, if passable
                if (this.leftClickedNodes.IndexOf(mousedOverNode) == -1)
                {
                    if (mousedOverNode.Passable)
                    {
                        this.leftClickedNodes.Add(mousedOverNode);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Cannot start path on impassable terrain.");
                    }
                }
            }
            //IF right click, start pathing
            else if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                //Clear out old path finders
                foreach (var pathFinder in this.targetPathfinders)
                {
                    UnityEngine.GameObject.Destroy(pathFinder.gameObject);
                }
                targetPathfinders.Clear();

                //Start path from all origins to right click position
                if (leftClickedNodes.Count > 0)
                {
                    foreach (var node in this.leftClickedNodes)
                    {
                        var newPathFinderObj = new UnityEngine.GameObject("Path Finder");
                        newPathFinderObj.transform.parent = this.transform;
                        var newPathFinder = newPathFinderObj.AddComponent<Ants.Pathfinding.PathFinderActor>();
                        newPathFinder.StartPathSearch(targetGrid, node, mousedOverNode);

                        targetPathfinders.Add(newPathFinder);
                    }
                }

                this.leftClickedNodes.Clear();
            }
            //If path finders exist, update them
            else if (this.targetPathfinders != null)
            {
                foreach (var pathFinder in this.targetPathfinders)
                {
                    pathFinder.UpdateSearch(20);
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (UnityEngine.Application.isPlaying)
            {
                var targetGrid = World.TerrainActor.Terrain.pathfindingGrid;
                if (targetGrid == null)
                {
                    return;
                }

                //Display sphere above selected node
                if (mousedOverNode != null)
                {
                    UnityEngine.Gizmos.color = UnityEngine.Color.red;
                    UnityEngine.Gizmos.DrawSphere(new UnityEngine.Vector3(mousedOverNode.WorldX, mousedOverNode.WorldY, -1.0f), targetGrid.TileDim * 0.25f);
                }

                //Displayed spheres above all nodes that have been left clicked on
                if (this.leftClickedNodes.Count > 0)
                {
                    UnityEngine.Gizmos.color = UnityEngine.Color.red;

                    foreach (var node in this.leftClickedNodes)
                    {
                        UnityEngine.Gizmos.DrawSphere(new UnityEngine.Vector3(node.WorldX, node.WorldY, -1.0f), targetGrid.TileDim * 0.25f);
                    }
                }
            }
        }
        #endregion
    }
}