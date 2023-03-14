using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Mobs
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] protected PlayerStorage playerStorage;

        [Header("Parent Class Settings")]
        [SerializeField] protected float health;
        [SerializeField] protected float damage;

        [field: SerializeField] protected bool IsBusy {  get; private set; }

        protected virtual void SetBusyState(bool state) => IsBusy = state;
    }
}
