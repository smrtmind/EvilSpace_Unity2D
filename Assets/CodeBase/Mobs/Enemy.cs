using CodeBase.Player;
using CodeBase.UI;
using CodeBase.Utils;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] protected DependencyContainer dependencyContainer;
        [SerializeField] protected PlayerStorage playerStorage;
        [SerializeField] protected EnemyStorage enemyStorage;

        [field: Header("Parent Class Settings")]
        [SerializeField] private ParticleType explosionEffect;
        [SerializeField] private float explosionAdditionalScale;
        [SerializeField] private SpriteRenderer skinRenderer;
        [SerializeField] private PopUp popUp;

        [field: Header("Enemy Unit Settings")]
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Score { get; private set; }
        [field: SerializeField] public bool IsBusy { get; private set; }

        private Color defaultColor;
        private Tween skinColorTween;
        private float currentHealth;

        private void Start()
        {
            defaultColor = skinRenderer.color;
        }

        public void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);

            if (IsBusy)
                currentHealth = Health;
            else
                skinRenderer.color = defaultColor;
        }

        public void ModifyHealth(float health)
        {
            currentHealth += health;
            if (currentHealth <= 0f)
            {
                SetBusyState(false);

                var newEffect = dependencyContainer.ParticlePool.GetFreeObject(explosionEffect);
                newEffect.gameObject.SetActive(false);
                newEffect.transform.position = transform.position;
                newEffect.transform.localScale = new Vector3(transform.localScale.x + explosionAdditionalScale, transform.localScale.y + explosionAdditionalScale, 1f);
                newEffect.SetBusyState(true);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                var projectile = Dictionaries.Projectiles.FirstOrDefault(p => p.Key == collision.gameObject.transform);
                ModifyHealth(-projectile.Value.Damage);
                if (currentHealth <= 0f) TakeScore();

                skinColorTween?.Kill();
                skinColorTween = skinRenderer.DOColor(Color.red, 0.1f).OnComplete(() => skinRenderer.color = defaultColor);

                SpawnSpark(collision.gameObject.transform.position);
            }
        }

        private void SpawnSpark(Vector3 projectilePosition)
        {
            var newEffect = dependencyContainer.ParticlePool.GetFreeObject(ParticleType.SparksHit);
            newEffect.gameObject.SetActive(false);
            newEffect.transform.position = new Vector3(projectilePosition.x, projectilePosition.y + 1f, projectilePosition.z);
            newEffect.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            newEffect.SetBusyState(true);
        }

        private void TakeScore()
        {
            popUp.SetCurrentData(transform, $"{Score}", "yellow");
            popUp.SpawnPopUp();

            playerStorage.ConcretePlayer.ModifyScore(Score);
        }
    }
}
