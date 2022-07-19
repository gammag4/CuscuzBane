using CuscuzBane.Base;
using CuscuzBane.Behaviour.Inventory;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class ItemFollowBehaviour : MonoBehaviour
    {
        private Vector3 velocity;
        private Vector3 lastStopVelocity;

        private float maxDistance = 0.6f;
        private float accel = 0.6f;
        private float stopAccel = 3f;

        private bool stopped;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var ib = GetComponent<ItemBehaviour>();
            if (!ib.CanPick) return;

            var delta = Utils.Player.CenterPos - transform.position;
            if (delta.magnitude > maxDistance)
            {
                if (!stopped)
                {
                    var lastVel = velocity;
                    velocity -= lastStopVelocity * stopAccel * Time.deltaTime;
                    transform.Translate(velocity);
                    stopped = lastVel.magnitude <= velocity.magnitude;
                }
            }
            else
            {
                stopped = false;
                velocity = Utils.GetVelocityTowards(transform.position, Utils.Player.CenterPos, velocity, accel, 10f);
                lastStopVelocity = velocity;
                transform.Translate(velocity);
            }
        }
    }
}
