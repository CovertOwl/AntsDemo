namespace Ants.World
{
    /// <summary>
    /// Root simulation authorative object
    /// </summary>
    public class World
    {
        #region Properties
        /// <summary>
        /// The terrain within the simulation
        /// </summary>
        public Terrain.Terrain Terrain { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct the world.
        /// </summary>
        /// <param name="terrain">The terrain within the simulation, already instantiated.</param>
        public World(Terrain.Terrain terrain)
        {
            this.Terrain = terrain;
        }
        #endregion

        #region PublicMethods
        public void Update()
        {

        }
        #endregion
    }
}
