using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuscuzBane.Behaviour
{
    public class FixFontFiltering : MonoBehaviour
    {
        public Font font;

        // Start is called before the first frame update
        void Start()
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
