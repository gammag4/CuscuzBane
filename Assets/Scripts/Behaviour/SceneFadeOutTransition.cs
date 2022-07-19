using System;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class SceneFadeOutTransition : MonoBehaviour
    {
        private float transitionTime;
        private float transitionSpeed = 1;

        private bool loaded;

        public Action GoToNextScene { get; set; }

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
                cg.alpha = transitionTime;
            }
            else if(!loaded)
            {
                loaded = true;
                GoToNextScene?.Invoke();
            }
        }
    }
}
