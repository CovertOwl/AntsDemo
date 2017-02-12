using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ants.Terrain
{
    /// <summary>
    /// Actor for the "Ants.Terrain.Terrain" class.
    /// </summary>
    public class TerrainActor : Ants.Actor.Actor2D
    {
        /// <summary>
        /// Data required to initialise the terrain
        /// </summary>
        [System.Serializable]
        public class InitData
        {
            [Tooltip("Data used to instantiate the pathfinding grid.")]
            public Ants.Pathfinding.Grid.InitData GridData;

            [Tooltip("Prefab gameobject used to represent rock terrain.")]
            public GameObject RockTerrainPrefab;

            [Tooltip("Prefab gameobject used to represent sand terrain.")]
            public GameObject SandTerrainPrefab;

            [Tooltip("Prefab gameobject used to represent a hole in the ground for an ant nest (as terrain).")]
            public GameObject NestHoleTerrainPrefab;

            [Tooltip("Prefab gameobject used to represent the mound around a hole in the ground for an ant nest (as terrain).")]
            public GameObject NestMoundTerrainPrefab;
        }

        /// <summary>
        /// Terrain object for this actor
        /// </summary>
        public Terrain Terrain { get; private set;}
    }
}
