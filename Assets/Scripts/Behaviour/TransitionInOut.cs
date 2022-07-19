using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class TransitionInOut : MonoBehaviour
    {
        public float transitionTime0;
        public float transitionTime;
        public float transitionTimeMiddle;
        public float maxTransitionTimeMiddle;
        public float transitionTime2;
        public float transitionSpeed = 1f;

        void Start()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 0;
        }

        // Update is called once per frame
        void Update()
        {
            var cg = GetComponent<CanvasGroup>();

            transitionTime0 += Time.deltaTime * transitionSpeed;
            if (transitionTime0 < 1)
            {
                cg.alpha = 0;
                return;
            }

            transitionTime += Time.deltaTime * transitionSpeed;
            if (transitionTime < 1)
            {
                cg.alpha = transitionTime;
                return;
            }

            transitionTimeMiddle += Time.deltaTime * transitionSpeed;
            if (transitionTimeMiddle < maxTransitionTimeMiddle)
            {
                cg.alpha = 1;
                return;
            }

            transitionTime2 += Time.deltaTime * transitionSpeed;
            if (transitionTime2 < 1)
            {
                cg.alpha = 1 - transitionTime2;
                return;
            }

            Destroy(gameObject);
        }
    }
}
