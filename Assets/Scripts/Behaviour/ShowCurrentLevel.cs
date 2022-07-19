using CuscuzBane.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CuscuzBane.Behaviour
{
    public class ShowCurrentLevel : MonoBehaviour
    {
        public Text text;

        // Use this for initialization
        void Start()
        {
            text.text = $"Level {Utils.Level + 1}";
        }
    }
}
