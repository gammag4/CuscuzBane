using CuscuzBane.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class ObstacleCreator : MonoBehaviour
    {
        private int numObstacles = 1000;

        public List<GameObject> obstacles;
        public List<float> probabilities;

        private bool initialized;

        private void Start()
        {
        }

        // Start is called before the first frame update
        void Update()
        {
            if (initialized) return;

            initialized = true;

            float totalProbability = probabilities.Sum();

            for (int i = 0; i < numObstacles; i++)
            {
                GameObject obstacle = Utils.ChooseObject(obstacles, probabilities, totalProbability);

                var x = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);
                var y = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);

                Vector3 position = new Vector3(x, y, 0);
                Instantiate(obstacle, position, Quaternion.identity);
            }
        }
    }
}
