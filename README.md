# AntsDemo
A 3 day challenge to create a simulation of simple ant behaviour in a procedurally generated world.

# Quick How-To's
## How to Play
You control the camera, by default press WSAD to move and use the mouse wheel to zoom. When you are finished, press escape to exit.

## How to change world generation.
While in editor mode:
1. Open the Main scene (Scenes/Main)
2. In the scene heirachy, highlight the "Terrain Actor" node (Root/Terrain Actor)
3. In the inspector, change the values for the "Simple Terrain Actor" script.
	*Terrain Data/Seed - Seed value for world
	*Terrain Data/Generation Data/Amplitude Persistence - Noise generation variable
	*Terrain Data/Generation Data/Frequency Persistence - Noise generation variable
	*Terrain Data/Generation Data/Frequency - Noise generation variable
	*Terrain Data/Generation Data/Amplitude - Noise generation variable
	*Terrain Data/Generation Data/Octaves - Noise generation variable
	*Terrain Data/Generation Data/Dim X - Size of the terrain along the X axis, in Unity units
	*Terrain Data/Generation Data/Dim Y - Size of the terrain along the Y axis, in Unity units
	*Terrain Data/Generation Data/Tile Dim - Dimensions of terrain tiles, in Unity units
	*Terrain Data/Tile Init Data - Array of tiles which are used ase on the noise function
	*Terrain Data/Tile Init Data/X/Tile/Name - Name of the tile, must be unique
	*Terrain Data/Tile Init Data/X/Tile/Passable - Can the tile be passed through?
	*Terrain Data/Tile Init Data/X/Tile/Height Value Minimum - If the noise height value at a position in the terrain is greater than or equal to this value and less than Height Value Maximum, then this tile is spawned.
	*Terrain Data/Tile Init Data/X/Tile/Height Value Maximum - If the noise height value at a position in the terrain is greater than or equal to the Height Value Minimum and less than this value, then this tile is spawned.
	*Terrain Data/Tile Init Data/X/Terrain Prefab - The prefab spawned to represent the tile.
NOTE: Currently, the world is generated each time a value is changed.

##How to view the test world generation
While in editor mode:
1. Open the Main scene (Scenes/Main)
2. In the scene heirachy, highlight the "Root" node
3. In the inspector, change the terrain type value for the "World Actor" script to "Test".
NOTE: Switching back to the "Simple" terrain type will not restore the world as the settings will no longer be populated. Simply re-load Main scene to restore the world.

# Version History
v0.1
Simple procedurally generated world based on noise function. Generation is done in editor mode.
Simple camera control (zoom & move)