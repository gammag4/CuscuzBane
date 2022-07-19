using CuscuzBane.Base;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour
{
    public class ShowKillCount : MonoBehaviour
    {
        public Text text;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            text.text = Utils.Player.Kills.ToString();
        }
    }
}