using NoiseTest;
using System.Collections;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class TreeLeaveBehaviour : MonoBehaviour
    {
        public TreeBehaviour tree;
        public float offset;
        public float scale = 2;
        public float maxDisplacement = 0.012f;
        public float order;

        private Vector3 Position => tree.transform.position;
        private OpenSimplex Noise => tree.Noise;

        private float time;


        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            transform.position = Position + new Vector3((float)Noise.Evaluate(time * scale + offset, 0) * maxDisplacement, 0, -order / 10000);
        }
    }
}
