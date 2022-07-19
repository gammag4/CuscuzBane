using CuscuzBane.Base;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class LevelMenuBehaviour : MonoBehaviour
    {
        public GameObject child;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Utils.Player == null) return;

            child.SetActive(Utils.Player.Escape);
        }
    }
}
