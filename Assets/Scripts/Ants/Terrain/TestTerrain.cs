namespace Ants.Terrain
{
    /// <summary>
    /// This test terrain will quickly show how the noise function is generating
    /// </summary>
    public class TestTerrain : Terrain
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
        /// Data used to initialise the terrain
        /// </summary>
        public TerrainGenerationData GenerationData { get; set; }

        /// <summary>
        /// 1D array of colours matching tile positions. Y major, x minor.
        /// I.E tile = x + y * dimX
        /// </summary>
        private UnityEngine.Color[] colors;
        #endregion

        #region PublicMethods
        /// <summary>
        /// Given an index, return the colour
        /// </summary>
        /// <param name="x">x location of the tile (0-DimX)</param>
        /// <param name="y">t location of the tile (0-DimY)</param>
        /// <returns>The colour of the tile</returns>
        public UnityEngine.Color ColourAt(uint x, uint y)
        {
            return this.colors[x + y * GenerationData.DimX];
        }
        #endregion

        #region Terrain
        public override void GenerateTerrain(int seed)
        {
            //Validate data, exit if not yet valid
            if (GenerationData.DimX == 0 || GenerationData.DimY == 0 || this.GenerationData.TileDim == 0)
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

            //Generate colours
            this.colors = new UnityEngine.Color[GenerationData.DimX * GenerationData.DimY];
            for (uint y = 0; y < GenerationData.DimY; ++y)
            {
                for (uint x = 0; x < GenerationData.DimX; ++x)
                {
                    var height = noiseGen.GetHeight(x, y);
                    this.colors[x + y* GenerationData.DimX] = new UnityEngine.Color(0, 0.5f, (height + GenerationData.Amplitude) / (GenerationData.Amplitude * 2.0f));
                }
            }
        }
        #endregion
    }
}