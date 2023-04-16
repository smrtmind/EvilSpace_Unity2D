using UnityEngine;

namespace CodeBase.Service
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
    }
}
