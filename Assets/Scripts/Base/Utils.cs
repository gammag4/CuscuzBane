using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour;
using CuscuzBane.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Base
{
    public static class Utils
    {
        public static Player Player { get; set; }

        public static GameObject MiniCuz { get; set; }
        public static GameObject ZumbiCuz { get; set; }
        public static GameObject CactusCuz { get; set; }
        public static GameObject CuscuzBullet { get; set; }
        public static GameObject Bullet { get; set; }

        public static GameObject Poof { get; set; }

        public static GameObject MedKit { get; set; }
        public static GameObject Cuscuz { get; set; }
        public static GameObject BulletItem { get; set; }
        public static GameObject Peixeira { get; set; }
        public static GameObject Gun { get; set; }
        public static GameObject Cap { get; set; }

        public static GameObject PeixeiraHand { get; set; }
        public static GameObject GunHand { get; set; }
        public static GameObject CapHand { get; set; }

        public static HealthBarBehaviour CooldownBar { get; set; }

        public static GameObject CurrentShowSelectedItem { get; set; }

        public static Inventory Inventory { get; set; }

        public static bool ClearInventory { get; set; }

        public static List<ItemType> ItemsSortedToEnd { get; set; }

        public static List<KuskuzMoh> KuskuzMohs { get; } = new List<KuskuzMoh>();

        public static int NumKusKuzMohDefeated { get; set; }

        public static int Level { get; set; }

        public static bool CutscenePlayed { get; set; }

        public static int MapSize => 300;
        public static int MapRealSize => 48;

        public static float BorderDistance => 0.5f;

        public static Vector3 GetVelocityTowards(Vector3 source, Vector3 target, Vector3 currentVelocity, float accel, float damp)
        {
            var delta = target - source;
            var dir = delta.normalized;
            return currentVelocity * (1 - damp * Time.deltaTime) + dir * accel * Time.deltaTime;
        }

        public static T ChooseObject<T>(List<T> objects, List<float> probabilities, float maxProbability) where T : class
        {
            T obj = null;

            float probability = Random.Range(0f, maxProbability);
            float currProbability = 0;

            for (int j = 0; j < objects.Count; j++)
            {
                currProbability += probabilities[j];
                if (probability > currProbability) continue;

                obj = objects[j];
                break;
            }

            if (obj == null) obj = objects[objects.Count - 1];

            return obj;
        }

        public static T ChooseObject<T>(List<T> objects, List<float> probabilities) where T : class
        {
            float totalProbability = 0;
            for (int i = 0; i < probabilities.Count; i++)
            {
                totalProbability += probabilities[i];
            }

            return ChooseObject(objects, probabilities, totalProbability);
        }
    }
}
