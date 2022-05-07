using UnityEngine;

namespace Scripts
{
    public class BoosterComponent : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _points;

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
            if (!_weaponController)
                _weaponController = FindObjectOfType<WeaponController>();

            if (!_weaponController.AllWeaponMaxOut)
            {
                _weaponController.PowerUp();
            }
            else
            {
                _gameSession.ModifyXp(1000);
                _points.Spawn();
            }
        }
    }
}
