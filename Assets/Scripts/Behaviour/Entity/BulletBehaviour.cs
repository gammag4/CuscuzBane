using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class BulletBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;

        public Bullet Bullet { get; set; }

        // Use this for initialization
        void Start()
        {
            Bullet = new Bullet(gameObject);
            Bullet.Init();
            Bullet.Hitbox = hitbox;
        }

        // Update is called once per frame
        void Update()
        {
            Bullet.Update();
        }
    }
}
