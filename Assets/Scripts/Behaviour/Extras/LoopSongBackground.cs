using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class LoopSongBackground : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<AudioSource>().loop = true;
        }
    }
}
