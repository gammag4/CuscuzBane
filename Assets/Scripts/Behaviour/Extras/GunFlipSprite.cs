using UnityEngine;

namespace CuscuzBane.Behaviour.Extras
{
    public class GunFlipSprite : MonoBehaviour
    {
        public SpriteRenderer renderer;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            renderer.flipY = transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270;
        }
    }
}
