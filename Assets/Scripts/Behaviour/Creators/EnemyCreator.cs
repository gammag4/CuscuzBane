using CuscuzBane.Base;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class EnemyCreator : MonoBehaviour
    {
        private int numEnemies = 300;

        public GameObject miniCuz;
        public GameObject zumbiCuz;
        public GameObject cactusCuz;
        public GameObject cuscuzBullet;
        public GameObject kuskuzMoh;
        public GameObject bullet;

        public GameObject poof;

        private bool initialized;

        // Start is called before the first frame update
        void Update()
        {
            if (initialized) return;

            initialized = true;

            Utils.MiniCuz = miniCuz;
            Utils.ZumbiCuz = zumbiCuz;
            Utils.CactusCuz = cactusCuz;
            Utils.CuscuzBullet = cuscuzBullet;
            Utils.Bullet = bullet;

            Utils.Poof = poof;

            for (int i = 0; i < numEnemies; i++)
            {
                GameObject enemy;

                float probability = Random.Range(0f, 1);

                if (probability < 0.15)
                {
                    enemy = cactusCuz;
                }
                else if (probability < 0.7)
                {
                    enemy = miniCuz;
                }
                else
                {
                    enemy = zumbiCuz;
                }

                var x = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);
                var y = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);

                Vector3 position = new Vector3(x, y, 0);
                Instantiate(enemy, position, Quaternion.identity);
            }

            Utils.KuskuzMohs.Clear();

            if (Utils.Level == 0) return;

            for (int i = 0; i < Utils.Level / 3 + 1; i++)
            {
                Instantiate(kuskuzMoh, Utils.Player.Transform.position + new Vector3(4, 4, 0), Quaternion.identity);
            }
        }
    }
}
