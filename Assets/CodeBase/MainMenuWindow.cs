using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _pause;

        public void OnStartGame()
        {
            SceneManager.LoadScene("EndlessLevel");
        }

        public void OnExit()
        {
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }

        public void OnPause()
        {
            if (_pause.activeSelf)
                _pause.SetActive(false);
            else
                _pause.SetActive(true);
        }
    }
}
