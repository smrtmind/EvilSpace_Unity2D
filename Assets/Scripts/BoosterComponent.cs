using UnityEngine;

namespace Scripts
{
    public class BoosterComponent : MonoBehaviour
    {
        private GameSession _gameSession;
        private WeaponController _weaponController;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _weaponController = FindObjectOfType<WeaponController>();

            Destroy(gameObject, 30f);
        }

        public void AddOneLife() => _gameSession.ModifyTries(1);

        public void PowerUp()
        {
            var player = FindObjectOfType<PlayerController>();
            if (player)
            {
                if (!player.IsDead)
                {
                    _weaponController.PowerUp();
                }
            }
        }
    }
}
