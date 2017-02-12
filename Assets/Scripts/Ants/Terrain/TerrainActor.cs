namespace Ants.Terrain
{
    /// <summary>
    /// Actor for the "Ants.Terrain.Terrain" class.
    /// </summary>
    [UnityEngine.ExecuteInEditMode]
    public abstract class TerrainActor : Ants.Actor.Actor2D
    {
        #region Properties
        /// <summary>
        /// Terrain object for this actor
        /// </summary>
        public Terrain Terrain { get; protected set; }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Call to generate the terrain
        /// </summary>
        public abstract void Generate();
        #endregion

        #region UnityInterface
        public virtual void Start()
        {
            Generate();
        }
        #endregion
    }
}
