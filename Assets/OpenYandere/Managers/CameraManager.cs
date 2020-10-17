using OpenYandere.Characters.Player;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [FormerlySerializedAs("PlayerCamera")] [Header("References:")]
        public PlayerCamera playerCamera;
    }
}