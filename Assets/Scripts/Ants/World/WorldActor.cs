namespace Ants.World
{
    /// <summary>
    /// Actor for the "Ants.World.World" class.
    /// </summary>
    public class WorldActor : Ants.Actor.Actor
    {
        /// <summary>
        /// Data used to instantiate the terrain data
        /// </summary>
        public Ants.Terrain.TerrainActor.InitData TerrainData;

        /// <summary>
        /// World object for this actor
        /// </summary>
        public World World { get; private set; }
    }
}
