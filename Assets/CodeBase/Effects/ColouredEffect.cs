using UnityEngine;

namespace CodeBase.Effects
{
    public class ColouredEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetColor(Color color) => spriteRenderer.color = color;
    }
}
