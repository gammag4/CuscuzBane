using CuscuzBane.Base;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CuscuzBane.Behaviour
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public Tilemap tilemap;

        // Update is called once per frame
        void Update()
        {
            var playerPos = Utils.Player.CenterPos;
            playerPos = new Vector3(playerPos.x, playerPos.y, 0);
            var cameraPos = new Vector3(transform.position.x, transform.position.y, 0);

            var moveDirection = playerPos - cameraPos;
            moveDirection = moveDirection * Time.deltaTime * 10;
            transform.Translate(moveDirection);

            var bounds = tilemap.localBounds;
            var origin = tilemap.origin;

            var vertExtent = Camera.main.orthographicSize;
            var horzExtent = vertExtent * Screen.width / Screen.height;

            // Calculations assume map is position at the origin
            var minX = origin.x + horzExtent;
            var maxX = origin.x + bounds.size.x - horzExtent;
            var minY = origin.y + vertExtent;
            var maxY = origin.y + bounds.size.y - vertExtent;

            if (transform.position.x < minX) transform.position = new Vector3(minX, transform.position.y, transform.position.z);
            if (transform.position.x > maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
            if (transform.position.y < minY) transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            if (transform.position.y > maxY) transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }
}
