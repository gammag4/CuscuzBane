using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour
{
    public class EntityNameBehaviour : MonoBehaviour
    {
        public Func<string> GetName { get; set; }

        public Func<bool> IsActive { get; set; } = () => true;

        public Text text;

        // Update is called once per frame
        void Update()
        {
            var got = TryGetComponent<CanvasGroup>(out var cg);

            if (!IsActive())
            {
                if (got) cg.alpha = 0;
                return;
            }

            if (GetName == null)
            {
                if (got) cg.alpha = 0;
                return;
            }

            var name = GetName();
            if (got && name.Length > 0)
            {
                cg.alpha = 1;
            }
            else if (got)
            {
                cg.alpha = 0;
            }
            text.text = name;
        }
    }
}
