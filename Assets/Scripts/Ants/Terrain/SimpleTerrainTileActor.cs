namespace Ants.Terrain
{
    /// <summary>
    /// Actor for the "Ants.Terrain.SimpleTerrain" class.
    /// </summary>
    public class SimpleTerrainTileActor : Ants.Actor.Actor2D
    {
        [System.Serializable]
        public class TileInitData
        {
            #region Properties
            /// <summary>
            /// The tile associated with this actor
            /// </summary>
            public SimpleTerrainTile Tile;

            /// <summary>
            /// Prefab which is spawned to represent this tile.
            /// </summary>
            public UnityEngine.GameObject TerrainPrefab;
            #endregion
        }
    }
}
