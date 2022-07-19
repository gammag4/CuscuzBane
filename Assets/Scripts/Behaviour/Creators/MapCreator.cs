using CuscuzBane.Base;
using NoiseTest;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CuscuzBane.Behaviour
{
    public class MapCreator : MonoBehaviour
    {
        public Tile grass;
        public Tile path;

        public GameObject cave;

        public GameObject ColliderWallTop;
        public GameObject ColliderWallBottom;
        public GameObject ColliderWallLeft;
        public GameObject ColliderWallRight;

        float scale = 26;
        float ridgeOffset = 0.13f;

        // Use this for initialization
        void Start()
        {
            OpenSimplex noise = new OpenSimplex();

            Vector3Int[] positions = new Vector3Int[Utils.MapSize * Utils.MapSize];
            TileBase[] tileArray = new TileBase[positions.Length];

            for (int index = 0; index < positions.Length; index++)
            {
                int x = index % Utils.MapSize;
                int y = index / Utils.MapSize;

                float ridge = Mathf.Abs((float)noise.Evaluate(x / scale, y / scale));
                TileBase tile;

                if (ridge < ridgeOffset)
                    tile = path;
                else
                    tile = grass;

                positions[index] = new Vector3Int(x, y, 0);
                tileArray[index] = tile;
            }

            Tilemap tilemap = GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
            tilemap.SetTiles(positions, tileArray);

            ColliderWallTop.transform.position = new Vector3(Utils.MapRealSize / 2, Utils.MapRealSize, 0);
            ColliderWallBottom.transform.position = new Vector3(Utils.MapRealSize / 2, 0, 0);
            ColliderWallLeft.transform.position = new Vector3(0, Utils.MapRealSize / 2, 0);
            ColliderWallRight.transform.position = new Vector3(Utils.MapRealSize, Utils.MapRealSize / 2, 0);

            BoxCollider2D col;

            col = ColliderWallTop.GetComponent<BoxCollider2D>();
            col.size = new Vector2(Utils.MapRealSize, col.size.y);

            col = ColliderWallBottom.GetComponent<BoxCollider2D>();
            col.size = new Vector2(Utils.MapRealSize, col.size.y);

            col = ColliderWallLeft.GetComponent<BoxCollider2D>();
            col.size = new Vector2(col.size.x, Utils.MapRealSize);

            col = ColliderWallRight.GetComponent<BoxCollider2D>();
            col.size = new Vector2(col.size.x, Utils.MapRealSize);

            if (Utils.Level > 0) return;

            if (Random.Range(0f, 1) > 0.2f) return;

            var dec = Utils.MapRealSize / (float)Utils.MapSize;
            var caveOffset = new Vector3(0.24f, -0.11f, 0);
            Instantiate(cave, new Vector3(Utils.MapRealSize - 8 * dec, Utils.MapRealSize - 2 * dec, 0) + caveOffset, Quaternion.identity);
        }
    }
}
