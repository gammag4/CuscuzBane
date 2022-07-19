using System;
using UnityEngine;

namespace CuscuzBane.Behaviour.Extras
{
    public class DashAnimation : MonoBehaviour
    {
        public GameObject dashParticle;

        private float distanceBetweenParticles = 0.05f;
        private float time;

        public Func<bool> Dashing { get; set; }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!Dashing())
            {
                time = distanceBetweenParticles;
                return;
            }

            time += Time.deltaTime;

            if (time > distanceBetweenParticles)
            {
                time = 0;
                Instantiate(dashParticle, transform.position, Quaternion.identity);
            }
        }
    }
}
