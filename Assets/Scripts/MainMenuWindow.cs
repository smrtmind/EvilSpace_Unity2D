using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class MainMenuWindow : MonoBehaviour
    {
        public void OnStartGame()
        {
            SceneManager.LoadScene("EndlessLevel");
        }

        public void OnExit()
        {
            //Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
