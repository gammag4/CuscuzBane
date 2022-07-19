using UnityEngine;

namespace CuscuzBane.Behaviour.Entity
{
    public class CapBehaviour : MonoBehaviour
    {
        float lastPosition;

        float angle;

        float angleSpeed = 150f;
        float maxAngle = 15;

        // Use this for initialization
        void Start()
        {
            lastPosition = transform.parent.position.x;
        }

        // Update is called once per frame
        void Update()
        {
            var deltaPos = transform.parent.position.x - lastPosition;

            if (Mathf.Approximately(deltaPos, 0)) return;
            lastPosition = transform.parent.position.x;

            var angleDelta = -deltaPos * angleSpeed;
            angle += angleDelta;

            if (angle > maxAngle) angle = maxAngle;
            if (angle < -maxAngle) angle = -maxAngle;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
    }
}