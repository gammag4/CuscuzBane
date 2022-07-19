using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class CuscuzBulletBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;

        public CuscuzBullet CuscuzBullet { get; set; }

        // Use this for initialization
        void Start()
        {
            CuscuzBullet = new CuscuzBullet(gameObject);
            CuscuzBullet.Init();
            CuscuzBullet.Hitbox = hitbox;
        }

        // Update is called once per frame
        void Update()
        {
            CuscuzBullet.Update();
        }
    }
}
