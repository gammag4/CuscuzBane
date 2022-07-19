using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class SceneFadeInTransition : MonoBehaviour
    {
        public float transitionTime;
        private float transitionSpeed = 1;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var cg = GetComponent<CanvasGroup>();

            transitionTime += Time.deltaTime * transitionSpeed;
            if (transitionTime < 1)
            {
                cg.alpha = 1 - transitionTime;
                return;
            }

            Destroy(gameObject);
        }
    }
}