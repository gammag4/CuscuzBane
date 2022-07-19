using CuscuzBane.Base;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class RedScreenOnLowLife : MonoBehaviour
    {
        private float time;
        private float lastM;

        private bool wasCloseToDie;
        private bool wasAlive;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            var speed = 2f;

            var cg = GetComponent<CanvasGroup>();

            if (Utils.Player.Dead)
            {
                if (wasAlive)
                    time = 0;

                wasAlive = false;

                cg.alpha = FuncAppear(time, speed, 0.45f - lastM) + lastM;
                return;
            }

            wasAlive = true;

            if (Utils.Player.CloseToDie)
            {
                if (!wasCloseToDie)
                    time = 0;

                wasCloseToDie = true;

                cg.alpha = FuncAppear(time, speed, 0.3f) + 0.15f * Mathf.Sin(time * speed);
                lastM = cg.alpha;

                return;
            }

            if (wasCloseToDie)
                time = 0;

            wasCloseToDie = false;

            if (time < 2)
                cg.alpha = FuncFade2(time, speed, lastM, 2);
            else cg.alpha = 0;
        }

        private float FuncFade(float t, float s, float m) => m / (s * t + 1);

        private float FuncAppear(float t, float s, float m) => m - FuncFade(t, s, m);

        private float FuncFade2(float t, float s, float m, float z = 100000) => FuncFade(t, s, m) - m / (s * z + 1);

        private float FuncAppear2(float t, float s, float m, float z = 100000) => m - FuncFade2(t, s, m, z);
    }
}
