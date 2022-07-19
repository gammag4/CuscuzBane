using System;
using UnityEngine;
using UnityEngine.UI;

namespace CuscuzBane.Behaviour
{
    public class HealthBarBehaviour : MonoBehaviour
    {
        public Func<float> GetValue { get; set; }

        public Func<bool> IsActive { get; set; } = () => true;

        public bool HideWhenNull { get; set; } = false;

        public bool HideWhenFull { get; set; } = false;

        private Slider slider;

        // Use this for initialization
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            var got = TryGetComponent<CanvasGroup>(out var cg);

            if (!IsActive())
            {
                if (got) cg.alpha = 0;
                return;
            }

            if (GetValue == null)
            {
                if (HideWhenNull && got) cg.alpha = 0;
                return;
            }

            var value = GetValue();
            if (HideWhenFull && value > 0.999f && got)
            {
                cg.alpha = 0;
            }
            else if (got)
            {
                cg.alpha = 1;
            }

            slider.value = value;
        }
    }
}
