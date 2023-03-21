using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] protected DependencyContainer dependencyContainer;
        [SerializeField] protected PlayerStorage playerStorage;

        [field: Header("Parent Class Settings")]
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [SerializeField] private ParticleType particleType;
        [SerializeField] private SpriteRenderer skinRenderer;
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public bool IsBusy { get; private set; }

        private float defaultHealth;
        private Color defaultColor;
        private Tween skinColorTween;

        //private void OnEnable()
        //{
        //    defaultHealth = 10f;
        //    Health = defaultHealth;
        //}

        private void Start()
        {
            defaultColor = skinRenderer.color;
        }

        public void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);

            if (IsBusy)
            {
                Health = 3f;
            }
            else
            {
                skinRenderer.color = defaultColor;
                //var newEffect = dependencyContainer.ParticlePool.GetFreeObject(particleType);
                //newEffect.gameObject.SetActive(false);
                //newEffect.transform.position = transform.position;
                //newEffect.transform.localScale = new Vector3(transform.localScale.x + 3f, transform.localScale.y + 3f, 1f);
                //newEffect.SetBusyState(true);

                //destroyEffect.SetActive(true);
            }
        }

        public void ModifyHealth(float health)
        {
            Health += health;
            if (Health <= 0f)
            {
                SetBusyState(false);

                var newEffect = dependencyContainer.ParticlePool.GetFreeObject(particleType);
                newEffect.gameObject.SetActive(false);
                newEffect.transform.position = transform.position;
                newEffect.transform.localScale = new Vector3(transform.localScale.x + 2f, transform.localScale.y + 2f, 1f);
                newEffect.SetBusyState(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                ModifyHealth(-1);

                skinColorTween?.Kill();
                skinColorTween = skinRenderer.DOColor(Color.red, 0.1f).OnComplete(() => skinRenderer.color = defaultColor);
            }
        }
    }
}
