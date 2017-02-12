namespace Ants.Terrain
{
    /// <summary>
    /// Actor for the "Ants.Terrain.Terrain" class.
    /// </summary>
    [UnityEngine.ExecuteInEditMode]
    public class TestTerrainActor : TerrainActor
    {
        /// <summary>
        /// Data required to initialise the terrain
        /// </summary>
        [System.Serializable]
        public class InitData
        {
            [UnityEngine.Tooltip("Seed used to generate the terrain.")]
            public int Seed = 0;

            [UnityEngine.Tooltip("Data used to generate the terrain.")]
            public TestTerrain.TerrainGenerationData GenerationData = new TestTerrain.TerrainGenerationData();
        }

        #region Properties
        /// <summary>
        /// Data used to instantiate the terrain data
        /// </summary>
        public InitData TerrainData = new InitData();

        /// <summary>
        /// Material applied to meshes in simple terrain
        /// </summary>
        private UnityEngine.Material defaultSpriteMaterial { get; set; }
        #endregion

        #region TerrainActor
        public override void Generate()
        {
            //Validate data, exit if not yet valid
            if (this.TerrainData.GenerationData.DimX == 0)
            {
                UnityEngine.Debug.logger.LogWarning("Simple terrain dimenision X must be greater than 0.", this);
                return;
            }
            else if (this.TerrainData.GenerationData.DimY == 0)
            {
                UnityEngine.Debug.logger.LogWarning("Simple terrain dimenision Y must be greater than 0.", this);
                return;
            }
            else if (this.TerrainData.GenerationData.TileDim == 0)
            {
                UnityEngine.Debug.logger.LogWarning("Simple terrain tile dimenisions must be greater than 0.", this);
                return;
            }

            //Remove old children
            DestroyImmediateAllChildren();

            //Instantiate terrain if not yet instantiated
            if (this.Terrain == null)
            {
                this.Terrain = new TestTerrain();
            }

            //Execute terrain generation
            var testTerrain = this.Terrain as TestTerrain;
            testTerrain.GenerationData = this.TerrainData.GenerationData;
            this.Terrain.GenerateTerrain(this.TerrainData.Seed);

            //Create tile actors based on terrain. These will be coloured mesh quads
            float tileDim = this.TerrainData.GenerationData.TileDim;
            for (uint y = 0; y < this.TerrainData.GenerationData.DimY; ++y)
            {
                for (uint x = 0; x < this.TerrainData.GenerationData.DimX; ++x)
                {
                    //Create tile object
                    string objTitle = new System.Text.StringBuilder("Tile ").Append(x).Append(", ").Append(y).ToString();
                    var newTileObj = new UnityEngine.GameObject(objTitle);

                    //Add mesh
                    newTileObj.AddComponent(typeof(UnityEngine.MeshFilter));
                    var meshFilter = newTileObj.GetComponent<UnityEngine.MeshFilter>();
                    if (meshFilter.sharedMesh == null)
                    {
                        meshFilter.sharedMesh = new UnityEngine.Mesh();
                    }

                    //Add mesh verts
                    meshFilter.sharedMesh.vertices = new UnityEngine.Vector3[4]
                        {
                            new UnityEngine.Vector3(tileDim * -0.5f, tileDim * 0.5f, 0.0f),    //top-left
                            new UnityEngine.Vector3(tileDim * 0.5f, tileDim * 0.5f, 0.0f),     //top-right
                            new UnityEngine.Vector3(tileDim * 0.5f, tileDim * -0.5f, 0.0f),    //bottom-right
                            new UnityEngine.Vector3(tileDim * -0.5f, tileDim * -0.5f, 0.0f)    //bottom-left
                        };

                    //Add mesh triangles
                    meshFilter.sharedMesh.triangles = new int[6] { 0, 1, 3, 1, 2, 3 };

                    //Add mesh colours
                    var meshColour = testTerrain.ColourAt(x, y);
                    meshFilter.sharedMesh.colors = new UnityEngine.Color[4]
                        {
                            meshColour,
                            meshColour,
                            meshColour,
                            meshColour
                        };

                    //Finalise mesh
                    meshFilter.sharedMesh.RecalculateBounds();
                    meshFilter.sharedMesh = meshFilter.sharedMesh; //Forces update

                    //Add mesh renderer
                    newTileObj.AddComponent(typeof(UnityEngine.MeshRenderer));
                    var meshRenderer = newTileObj.GetComponent<UnityEngine.MeshRenderer>();
                    meshRenderer.sharedMaterial = this.defaultSpriteMaterial;

                    //Parent/position tile
                    var position = new UnityEngine.Vector3(
                        this.TerrainData.GenerationData.DimX * -0.5f * tileDim + x * tileDim + tileDim * 0.5f, 
                        this.TerrainData.GenerationData.DimY * 0.5f * tileDim - y * tileDim - tileDim * 0.5f
                        );
                    newTileObj.transform.parent = this.transform;
                    newTileObj.transform.localPosition = position;
                }
            }
        }
        #endregion

        #region UnityInterface
        public override void Start()
        {
            //Discover sprite material
            this.defaultSpriteMaterial = new UnityEngine.Material(UnityEngine.Shader.Find("Sprites/Default"));
            if (this.defaultSpriteMaterial == null)
            {
                throw new System.Exception("Failed to discover default sprite shader.");
            }

            base.Start();
        }
        #endregion
    }
}