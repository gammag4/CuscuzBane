using CuscuzBane.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CuscuzBane.Behaviour
{
    public class RollCutscene : MonoBehaviour
    {
        public Text text;

        private float rollTime;

        private float speed = 20;

        // Start is called before the first frame update
        void Start()
        {
            Utils.CutscenePlayed = true;
        }

        // Update is called once per frame
        void Update()
        {
            rollTime += Time.deltaTime * speed;

            text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, rollTime - 350, text.rectTransform.localPosition.z);

            if (rollTime < text.rectTransform.sizeDelta.y + 800) return;

            var loadScene = GetComponent<LoadScene>();
            loadScene.LoadStartingLevel();
        }
    }
}
