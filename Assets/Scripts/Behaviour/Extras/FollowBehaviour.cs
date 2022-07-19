using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class FollowBehaviour : MonoBehaviour
    {
        public GameObject target;

        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position;
        }
    }
}
