namespace Ants.Terrain
{
    /// <summary>
    /// A tile which is composed witin simple terrain
    /// </summary>
    [System.Serializable]
    public class SimpleTerrainTile
    {
        #region Properties
        /// <summary>
        /// Name for the tile
        /// </summary>
        public string Name = "EMPTY";
        /// <summary>
        /// True if this terrain tile can be passed through by agents
        /// </summary>
        public bool Passable;
        /// <summary>
        /// Inclusive minimum value representing the range at which a terrain tile is considered to be this type
        /// </summary>
        public float HeightValMin = 0.0f;
        /// <summary>
        /// Exclusive maximum value representing the range at which a terrain tile is considered to be this type
        /// </summary>
        public float HeightValMax = 0.0f;
        #endregion
    }
}
