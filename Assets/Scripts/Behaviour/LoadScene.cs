using CuscuzBane.Base;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuscuzBane.Behaviour
{
    public class LoadScene : MonoBehaviour
    {
        public GameObject sceneTransition;

        public Action AfterTransition { get; set; }

        private bool transitioning;

        private void CreateTransition(Action goToNext)
        {
            if (transitioning) return;
            transitioning = true;

            SceneManager.sceneLoaded += onLoaded;

            var obj = Instantiate(sceneTransition, Vector3.zero, Quaternion.identity);
            var st = obj.GetComponent<SceneFadeOutTransition>();
            st.GoToNextScene = () =>
            {
                goToNext?.Invoke();
            };
        }

        private void onLoaded(Scene arg0, LoadSceneMode arg1)
        {
            AfterTransition?.Invoke();
            AfterTransition = null;
            SceneManager.sceneLoaded -= onLoaded;
            transitioning = false;
        }

        public void StartGame()
        {
            CreateTransition(() =>
            {
                if (Utils.CutscenePlayed)
                {
                    Utils.Level = 0;
                    SceneManager.LoadScene("Level");
                }
                else SceneManager.LoadScene("Start Cutscene");
            });
        }

        public void LoadMenu()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Menu");
            });
        }

        public void LoadControles()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Controles");
            });
        }

        public void LoadCreditos()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Creditos");
            });
        }

        public void LoadGameOver()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Game Over");
            });
        }

        public void LoadStartCutscene()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Start Cutscene");
            });
        }

        public void LoadEndCutscene()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("End Cutscene");
            });
        }

        public void LoadEndCutscene2()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("End Cutscene 2");
            });
        }

        public void LoadCurrentLevel()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Level");
            });
        }

        public void LoadNextLevel()
        {
            CreateTransition(() =>
            {
                SceneManager.LoadScene("Level");
                Utils.Level += 1;
            });
        }

        public void LoadStartingLevel()
        {
            CreateTransition(() =>
            {
                Utils.Level = 0;
                SceneManager.LoadScene("Level");
            });
        }

        public void QuitGame()
        {
            CreateTransition(() =>
            {
                Application.Quit();
            });
        }
    }
}
