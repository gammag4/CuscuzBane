using System.Collections;
using UnityEngine;

namespace CuscuzBane.Behaviour.Extras
{
    public class ResetRotation : MonoBehaviour
    {
        public Quaternion rotation = Quaternion.identity;

        // Update is called once per frame
        void Update()
        {
            transform.rotation = rotation;
        }
    }
}
