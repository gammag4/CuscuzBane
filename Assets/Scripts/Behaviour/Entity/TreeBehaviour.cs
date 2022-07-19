using NoiseTest;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class TreeBehaviour : MonoBehaviour
    {
        public OpenSimplex Noise { get; set; }

        // Use this for initialization
        void Start()
        {
            Noise = new OpenSimplex();
        }
    }
}
