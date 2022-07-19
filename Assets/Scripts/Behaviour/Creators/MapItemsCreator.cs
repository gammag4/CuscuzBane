using CuscuzBane.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CuscuzBane.Behaviour
{
    public class MapItemsCreator : MonoBehaviour
    {
        public List<TileBase> choices = new List<TileBase>();

        public float itemSpawnProbability = 0.2f;

        // Use this for initialization
        void Start()
        {
            Vector3Int[] positions = new Vector3Int[Utils.MapSize * Utils.MapSize];
            TileBase[] tileArray = new TileBase[positions.Length];

            for (int index = 0; index < positions.Length; index++)
            {
                int x = index % Utils.MapSize;
                int y = index / Utils.MapSize;

                TileBase tile = null;

                if (Random.Range(0, 1f) < itemSpawnProbability)
                {
                    tile = choices[Random.Range(0, choices.Count)];
                }

                positions[index] = new Vector3Int(x, y, 0);
                tileArray[index] = tile;
            }

            Tilemap tilemap = GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
            tilemap.SetTiles(positions, tileArray);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
