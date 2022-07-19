using CuscuzBane.Base;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CuscuzBane.Behaviour
{
    public class MapWallCreator : MonoBehaviour
    {
        public Tile left;

        public Tile cornerLeftTop1;
        public Tile cornerLeftTop2;

        public Tile top1;
        public Tile top2;

        public Tile cornerRightTop1;
        public Tile cornerRightTop2;

        public Tile right;

        public Tile cornerRightBottm;

        public Tile bottom;

        public Tile cornerLeftBottom;

        // Use this for initialization
        void Start()
        {
            var mapSize = Utils.MapSize / 2;

            var topSize = mapSize * 2;
            var bottomSize = mapSize;
            var sideSize = mapSize - 2;

            Vector3Int[] positions = new Vector3Int[topSize + bottomSize + sideSize * 2 + 7];
            TileBase[] tileArray = new TileBase[positions.Length];

            for (int i = 2; i < Utils.MapSize - 1; i += 2)
            {
                // create top
                tileArray[i / 2] = top1;
                positions[i / 2] = new Vector3Int(i, Utils.MapSize - 1, 0);

                tileArray[i / 2 + mapSize] = top2;
                positions[i / 2 + mapSize] = new Vector3Int(i, Utils.MapSize - 3, 0);

                // create bottom
                tileArray[i / 2 + topSize] = bottom;
                positions[i / 2 + topSize] = new Vector3Int(i, 0, 0);
            }

            for (int i = 2; i < Utils.MapSize - 3; i += 2)
            {
                // create left
                tileArray[i / 2 + topSize + bottomSize] = left;
                positions[i / 2 + topSize + bottomSize] = new Vector3Int(0, i, 0);

                // create right
                tileArray[i / 2 + topSize + bottomSize + sideSize] = right;
                positions[i / 2 + topSize + bottomSize + sideSize] = new Vector3Int(Utils.MapSize - 1, i, 0);
            }

            // create corners
            var offset = topSize + bottomSize + sideSize * 2;

            tileArray[offset + 1] = cornerLeftTop1;
            positions[offset + 1] = new Vector3Int(0, Utils.MapSize - 1, 0);

            tileArray[offset + 2] = cornerLeftTop2;
            positions[offset + 2] = new Vector3Int(0, Utils.MapSize - 3, 0);

            tileArray[offset + 3] = cornerRightTop1;
            positions[offset + 3] = new Vector3Int(Utils.MapSize - 1, Utils.MapSize - 1, 0);

            tileArray[offset + 4] = cornerRightTop2;
            positions[offset + 4] = new Vector3Int(Utils.MapSize - 1, Utils.MapSize - 3, 0);

            tileArray[offset + 5] = cornerRightBottm;
            positions[offset + 5] = new Vector3Int(Utils.MapSize - 1, 0, 0);

            tileArray[offset + 6] = cornerLeftBottom;
            positions[offset + 6] = new Vector3Int(0, 0, 0);

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
