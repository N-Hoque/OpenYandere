using OpenYandere.Managers;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Vector3        m_currentRotation;
        private Vector3        m_currentRotationVelocity;
        private PlayerMovement m_playerMovement;

        private Vector3 m_targetHeightOffset;

        private float m_zoomDistance;

        public float HorizontalAxis { get; set; }
        public float VerticalAxis   { get; set; }

        [Header("Target Settings:")]
        /* [SerializeField] */ private Transform m_targetTransform;

        [FormerlySerializedAs("_targetHeight")] [SerializeField]
        private float targetHeight = 1.65f;
        [FormerlySerializedAs("_distanceFromTarget")] [SerializeField]
        private float distanceFromTarget = 2f;

        [Header("Vertical (Up and Down) Settings:")]
        [FormerlySerializedAs("_verticalMinLimit")] [SerializeField]
        private float verticalMinLimit = -40f;
        [FormerlySerializedAs("_verticalMaxLimit")] [SerializeField]
        private float verticalMaxLimit = 80f;

        [Header("Rotation Settings:")] 
        [FormerlySerializedAs("_rotationSmoothTime")] [SerializeField]
        private float rotationSmoothTime = 0.1f;

        [Header("Mouse Settings:")] 
        [FormerlySerializedAs("_mouseSensitivity")] [SerializeField]
        private float mouseSensitivity = 5f;
        [FormerlySerializedAs("_zoomSpeed")] [SerializeField]
        private float zoomSpeed = 150f;
        [FormerlySerializedAs("_zoomMinimumDistance")] [SerializeField]
        private float zoomMinimumDistance = 1f;
        [FormerlySerializedAs("_zoomMaximumDistance")] [SerializeField]
        private float zoomMaximumDistance = 10f;

        private void Awake()
        {
            PlayerManager playerManager = GameManager.Instance.playerManager;

            // Set player movement.
            m_playerMovement = playerManager.playerMovement;

            // Set the player as the target
            m_targetTransform    = playerManager.player.transform;
            m_targetHeightOffset = new Vector3(0, targetHeight, 0);

            // Set the axis to the current rotation of the target.
            HorizontalAxis = m_targetTransform.eulerAngles.y;
            VerticalAxis   = m_targetTransform.eulerAngles.x;

            // Set the zoom distance.
            m_zoomDistance = distanceFromTarget;
        }

        private void LateUpdate()
        {
            HorizontalAxis += Input.GetAxis("Mouse X") * mouseSensitivity;
            VerticalAxis   -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            m_zoomDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;

            // Set the camera's horizontal axis.
            m_playerMovement.SetCameraAxis(HorizontalAxis);

            // Clamp the vertical axis - looking up and down.
            VerticalAxis = Mathf.Clamp(VerticalAxis, verticalMinLimit, verticalMaxLimit);

            // Apply smoothing to the rotation.
            m_currentRotation = Vector3.SmoothDamp(m_currentRotation, new Vector3(VerticalAxis, HorizontalAxis),
                                                   ref m_currentRotationVelocity, rotationSmoothTime);

            // The camera's rotation is the mouse's X and Y.
            transform.eulerAngles = m_currentRotation;

            // Make sure the zoom distance is within the allowed range.
            m_zoomDistance = Mathf.Clamp(m_zoomDistance, zoomMinimumDistance, zoomMaximumDistance);

            // The camera's position is the target position, minus the distance from the target, plus the target height.
            transform.position = m_targetTransform.position - transform.forward * m_zoomDistance + m_targetHeightOffset;
        }

        public void SetTarget(Transform targetTransform, float targetHeight)
        {
            m_targetTransform = targetTransform;
            this.targetHeight = targetHeight;

            // Set the axis to the current rotation of the target.
            HorizontalAxis = m_targetTransform.eulerAngles.y;
            VerticalAxis   = m_targetTransform.eulerAngles.x;
        }
    }
}