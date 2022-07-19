using CuscuzBane.Base;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class ResizeCamera : MonoBehaviour
    {
        float zoom = 1.3f;

        float minZoom = 1.0f;
        float maxZoom = 1.6f;

        void Start()
        {
            GetComponent<Camera>().orthographicSize = zoom;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Utils.Player.ResizingCamera) return;

            var delta = Input.mouseScrollDelta.y;

            zoom = zoom - 0.1f * delta;

            if (zoom > maxZoom) zoom = maxZoom;
            if (zoom < minZoom) zoom = minZoom;

            GetComponent<Camera>().orthographicSize = zoom;
        }
    }
}
