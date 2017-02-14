namespace Ants.Terrain
{
    /// <summary>
    /// This simple terrain will allow tiles to be spawned based on the noise function
    /// </summary>
    public class SimpleTerrain : Terrain
    {
        /// <summary>
        /// Data required to generate the terrain
        /// </summary>
        [System.Serializable]
        public class TerrainGenerationData
        {
            /// <summary>
            /// Noise generation value for  amplitude persistence
            /// </summary>
            public float AmplitudePersistence = 0.2f;

            /// <summary>
            /// Noise generation value for frequency persistence
            /// </summary>
            public float FrequencyPersistence = 2.0f;

            /// <summary>
            /// Noise generation value for frequency
            /// </summary>
            public float Frequency = 0.4f;

            /// <summary>
            /// Noise generation value for amplitude
            /// </summary>
            public float Amplitude = 0.5f;

            /// <summary>
            /// Noise generation value for number of octaves
            /// </summary>
            public uint Octaves = 6;

            /// <summary>
            /// How wide (along X axis) the terrain is in tiles
            /// </summary>
            public uint DimX = 32;

            /// <summary>
            /// How high (along Y axis) the terrain is in tiles
            /// </summary>
            public uint DimY = 32;

            /// <summary>
            /// How wide/high (X & Y axes respectively) a terrain tile is
            /// </summary>
            public uint TileDim = 1;
        }

        #region Properties
        /// <summary>
        /// Data required to generate the terrain
        /// </summary>
        public TerrainGenerationData GenerationData { get; set; }

        /// <summary>
        /// Data used to generate tiles within the terrain
        /// </summary>
        public SimpleTerrainTile[] TileData { get; set; }

        /// <summary>
        /// The tiles within the terrain
        /// </summary>
        private SimpleTerrainTile[] tiles;
        #endregion

        #region PublicMethods
        /// <summary>
        /// Returns true if the tile at location in the terrain is passable
        /// </summary>
        /// <param name="x">x location of the tile (0-DimX)</param>
        /// <param name="y">t location of the tile (0-DimY)</param>
        /// <returns>True if the tile at location in the terrain is passable</returns>
        public bool IsPassable(uint x, uint y)
        {
            var tile = this.TileAt(x, y);
            return tile != null ? tile.Passable : true;
        }

        /// <summary>
        /// Find the tile name given a tile position
        /// </summary>
        /// <param name="x">x location of the tile (0-DimX)</param>
        /// <param name="y">t location of the tile (0-DimY)</param>
        /// <returns>The tile name or null if no tile exists</returns>
        public string TileNameAt(uint x, uint y)
        {
            var tile = this.TileAt(x, y);
            return tile != null ? tile.Name : null;
        }

        /// <summary>
        /// Find the tile grid node given a tile position
        /// </summary>
        /// <param name="x">x location of the tile (0-DimX)</param>
        /// <param name="y">t location of the tile (0-DimY)</param>
        /// <returns>The tile grid node or null if no tile exists</returns>
        public Ants.Pathfinding.GridNode GridNodeAt(uint x, uint y)
        {
            return this.pathfindingGrid.NodeAt(x, y);
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Find the tile by tile position
        /// </summary>
        /// <param name="x">x location of the tile (0-DimX)</param>
        /// <param name="y">t location of the tile (0-DimY)</param>
        /// <returns>The tile or null if no tile exists</returns>
        private SimpleTerrainTile TileAt(uint x, uint y)
        {
            return this.tiles[x + y * this.GenerationData.DimX];
        }
        #endregion

        #region Terrain
        public override void GenerateTerrain(int seed)
        {
            //Validate data, exit if not yet valid
            if (this.GenerationData.DimX == 0 || this.GenerationData.DimY == 0 || this.GenerationData.TileDim == 0 || this.TileData == null)
            {
                return;
            }

            //Create noise function
            var noiseGen = new Noise.Noise2D(
                GenerationData.AmplitudePersistence,
                GenerationData.FrequencyPersistence,
                GenerationData.Frequency,
                GenerationData.Amplitude,
                GenerationData.Octaves,
                seed);

            //Generate tiles
            this.tiles = new SimpleTerrainTile[this.GenerationData.DimX * this.GenerationData.DimY];
            var gridNodeData = new Ants.Pathfinding.GridNode.InitData[this.GenerationData.DimX, this.GenerationData.DimY];
            for (uint y = 0; y < this.GenerationData.DimY; ++y)
            {
                for (uint x = 0; x < this.GenerationData.DimX; ++x)
                {
                    var height = noiseGen.GetHeight(x, y);

                    //Find tile base on noise height
                    SimpleTerrainTile tile = null;
                    foreach (var tileType in this.TileData)
                    {
                        if (height >= tileType.HeightValMin && height < tileType.HeightValMax)
                        {
                            tile = tileType;

                            break;
                        }
                    }

                    this.tiles[x + y * GenerationData.DimX] = tile;

                    //Update the relevant grid node
                    gridNodeData[x, y] = new Pathfinding.GridNode.InitData();
                    gridNodeData[x, y].Passable = tile != null ? tile.Passable : false;
                    gridNodeData[x, y].TravelCostModifier = 1.0f;
                    gridNodeData[x, y].X = x;
                    gridNodeData[x, y].Y = y;
                    gridNodeData[x, y].WorldX = this.GenerationData.DimX * -0.5f * this.GenerationData.TileDim + x * this.GenerationData.TileDim + this.GenerationData.TileDim * 0.5f;
                    gridNodeData[x, y].WorldY = this.GenerationData.DimY * 0.5f * this.GenerationData.TileDim - y * this.GenerationData.TileDim - this.GenerationData.TileDim * 0.5f;
                }
            }
            this.pathfindingGrid = new Pathfinding.Grid(this.GenerationData.DimX, this.GenerationData.DimY, this.GenerationData.TileDim, gridNodeData);
        }
        #endregion
    }
}