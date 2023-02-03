using CodeBase.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        private PlayerController _hero;

        private void Start()
        {
            _hero = FindObjectOfType<PlayerController>();
        }

        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
