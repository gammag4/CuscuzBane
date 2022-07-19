using CuscuzBane.Base;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class CaveBehaviour : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var results = new List<Collider2D>();
            Physics2D.OverlapCollider(GetComponent<BoxCollider2D>(), new ContactFilter2D().NoFilter(), results);

            if (results.Contains(Utils.Player.Hitbox))
            {
                Destroy(Utils.Player.GameObject.GetComponent<Rigidbody2D>());
                GetComponent<LoadScene>().LoadNextLevel();
            }
        }
    }
}