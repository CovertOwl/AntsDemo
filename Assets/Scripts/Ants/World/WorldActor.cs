namespace Ants.World
{
    /// <summary>
    /// Actor for the "Ants.World.World" class.
    /// </summary>
    [UnityEngine.ExecuteInEditMode]
    public class WorldActor : Ants.Actor.Actor
    {
        /// <summary>
        /// Terrain types for dropdwon menu
        /// </summary>
        public enum TerrainTypes
        {
            Test,
            Simple
        }

        #region Public Properties
        /// <summary>
        /// Selected terrain type
        /// </summary>
        public TerrainTypes TerrainType = TerrainTypes.Test;

        /// <summary>
        /// World object for this actor
        /// </summary>
        public World World { get; private set; }

        /// <summary>
        /// The terrain for this worldd
        /// </summary>
        public Ants.Terrain.TerrainActor TerrainActor { get; private set; }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Creates the world simulation
        /// </summary>
        public void GenerateWorld()
        {
            //Clean up terrain if it exists
            if (this.TerrainActor != null)
            {
                UnityEngine.Object.DestroyImmediate(this.TerrainActor.gameObject);
                this.TerrainActor = null;
            }

            this.World = null;

            var terrainGameObject = new UnityEngine.GameObject("Terrain Actor");
            terrainGameObject.transform.parent = this.transform;

            switch (this.TerrainType)
            {
                //If test, instantiate test terrain but not world
                case TerrainTypes.Test:
                {
                    terrainGameObject.AddComponent<Terrain.TestTerrainActor>();
                    this.TerrainActor = terrainGameObject.GetComponent<Terrain.TerrainActor>();

                    break;
                }

                //If simple, instantiate terrain and world
                case TerrainTypes.Simple:
                {
                    terrainGameObject.AddComponent<Terrain.SimpleTerrainActor>();
                    this.TerrainActor = terrainGameObject.GetComponent<Terrain.TerrainActor>();

                    this.World = new World(TerrainActor.Terrain);

                    break;
                }

                default:
                {
                    throw new System.NotImplementedException();
                }
            }
        }
        #endregion

        #region UnityInterface
        public void Start()
        {
            //Search for an existing terrain actor
            string terrainName = "Terrain Actor";
            var terrainTransform = transform.Find(terrainName);
            UnityEngine.GameObject terrainGameObject = terrainTransform != null ? terrainTransform.gameObject : null;

            //If none found, create new
            if (terrainGameObject == null)
            {
                this.GenerateWorld();
            }
            //Else couple with existing
            else
            {
                this.TerrainActor = terrainGameObject.GetComponent<Terrain.TerrainActor>();

                this.World = new World(TerrainActor.Terrain);
            }
        }

        public void Update()
        {
            if (!UnityEngine.Application.isEditor && this.World != null)
            {
                if (UnityEngine.Input.GetAxis("Exit") > 0.0f)
                {
                    UnityEngine.Application.Quit();
                }

                this.World.Update();
            }
        }
        #endregion
    }
}
