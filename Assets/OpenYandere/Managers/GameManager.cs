using OpenYandere.Managers.Traits;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Managers
{
    internal class GameManager : Singleton<GameManager>
    {
        [Header("Managers:")] [FormerlySerializedAs("UIManager")]
        public UIManager uiManager;

        [FormerlySerializedAs("CameraManager")]
        public CameraManager cameraManager;

        [FormerlySerializedAs("PlayerManager")]
        public PlayerManager playerManager;

        [FormerlySerializedAs("ObjectPoolManager")]
        public ObjectPoolManager objectPoolManager;

        public void Resume()
        {
        }

        public void Pause()
        {
        }
    }
}