using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class PoofBehaviour : MonoBehaviour
    {
        private float time;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (time > 0.3f) Destroy(gameObject);
        }
    }
}
