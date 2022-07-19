using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Extras
{
    public class BounceItem : MonoBehaviour
    {
        public GameObject parent;

        private float maxDisplacement = 0.03f;
        private float speed = 4;

        private float time;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            var offset = Mathf.Sin(time * speed) * maxDisplacement;
            transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y + offset, parent.transform.position.z);
        }
    }
}