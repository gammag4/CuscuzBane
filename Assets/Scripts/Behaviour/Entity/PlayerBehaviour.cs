using Assets.Scripts.Behaviour;
using CuscuzBane.Base;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;
        public Collider2D swordHitbox;
        public EntityNameBehaviour nameText;

        private Player player;

        // Use this for initialization
        void Start()
        {
            player = new Player(gameObject);
            player.Init();
            player.Hitbox = hitbox;
            player.SwordHitbox = swordHitbox;
            player.Animator = GetComponent<Animator>();

            if (nameText)
                nameText.GetName = () => "Canga Cero";

            Utils.Player = player;

            player.Transform.position = new Vector3(1, Utils.MapRealSize / 2f, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (Utils.Player.Dead) return;

            Utils.Player.Update();

            CheckKills();
            CheckLife();
        }

        private void CheckKills()
        {
            var maxKills = Utils.Level * 10 + 20;

            if (!(Utils.Player.Kills >= maxKills)) return;

            var loadScene = GetComponent<LoadScene>();
            loadScene.LoadNextLevel();
        }

        private void CheckLife()
        {
            if (!Utils.Player.Dead) return;

            Utils.ClearInventory = true;

            var loadScene = GetComponent<LoadScene>();
            loadScene.LoadGameOver();
        }
    }
}
