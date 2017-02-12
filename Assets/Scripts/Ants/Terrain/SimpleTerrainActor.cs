namespace Ants.Terrain
{
    /// <summary>
    /// Actor for the "Ants.Terrain.Terrain" class.
    /// </summary>
    [UnityEngine.ExecuteInEditMode]
    public class SimpleTerrainActor : TerrainActor
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
            public SimpleTerrain.TerrainGenerationData GenerationData = new SimpleTerrain.TerrainGenerationData();

            [UnityEngine.Tooltip("Data representing the tiles within the terrain.")]
            public SimpleTerrainTileActor.TileInitData[] TileInitData;
        }

        #region Properties
        /// <summary>
        /// Data used to instantiate the terrain data
        /// </summary>
        public InitData TerrainData = new InitData();
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
            else if (TerrainData.TileInitData == null)
            {
                UnityEngine.Debug.logger.LogWarning("Simple terrain has no tile init data specified.", this);
                return;
            }

            //Remove old children
            DestroyImmediateAllChildren();

            //Instantiate terrain if not yet instantiated
            if (this.Terrain == null)
            {
                this.Terrain = new SimpleTerrain();
            }

            //Execute terrain generation
            var simpleTerrain = this.Terrain as SimpleTerrain;
            simpleTerrain.GenerationData = this.TerrainData.GenerationData;
            simpleTerrain.TileData = new SimpleTerrainTile[this.TerrainData.TileInitData.Length];
            for (uint a = 0; a < this.TerrainData.TileInitData.Length; ++a)
            {
                simpleTerrain.TileData[a] = this.TerrainData.TileInitData[a].Tile;
            }
            this.Terrain.GenerateTerrain(this.TerrainData.Seed);

            //Create tile actors based on terrain
            float tileDim = this.TerrainData.GenerationData.TileDim;
            for (uint y = 0; y < this.TerrainData.GenerationData.DimY; ++y)
            {
                for (uint x = 0; x < this.TerrainData.GenerationData.DimX; ++x)
                { 
                    //Create tile object
                    string objTitle = new System.Text.StringBuilder("Tile ").Append(x).Append(", ").Append(y).ToString();
                    var newTileObj = new UnityEngine.GameObject(objTitle);

                    //Parent/position tile
                    var position = new UnityEngine.Vector3(
                        this.TerrainData.GenerationData.DimX * -0.5f * tileDim + x * tileDim + tileDim * 0.5f, 
                        this.TerrainData.GenerationData.DimY * 0.5f * tileDim - y * tileDim - tileDim * 0.5f
                        );
                    newTileObj.transform.parent = this.transform;
                    newTileObj.transform.localPosition = position;

                    //Find matching tile by name and use prefab for tile, if found
                    var terrainTileName = simpleTerrain.TileNameAt(x, y);
                    UnityEngine.GameObject tilePrefabObject = null;
                    foreach (var tileType in this.TerrainData.TileInitData)
                    {
                        if (tileType.Tile.Name == terrainTileName)
                        {
                            tilePrefabObject = tileType.TerrainPrefab;
                            break;
                        }
                    }
                    if (tilePrefabObject != null)
                    {
                        var newTilePrefabObj = UnityEngine.GameObject.Instantiate(tilePrefabObject);
                        newTilePrefabObj.transform.SetParent(newTileObj.transform, false);
                        newTilePrefabObj.name = "Terain Tile Prefab";
                    }
                }
            }
        }
        #endregion
    }
}