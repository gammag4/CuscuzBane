using CuscuzBane.Base;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class SwordHitboxBehaviour : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var playerPos = Utils.Player.CenterPos;
            playerPos -= new Vector3(0, 0, Utils.Player.CenterPos.z);

            var mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mpos -= new Vector3(0, 0, mpos.z);

            transform.position = playerPos;
            transform.rotation = Quaternion.FromToRotation(Vector3.right, mpos - playerPos);
        }
    }
}
