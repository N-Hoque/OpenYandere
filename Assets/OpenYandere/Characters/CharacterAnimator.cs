using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters
{
    internal struct AnimatorData
    {
        public Vector3 moveDirection;
        public bool    isRunning;
    }

    [RequireComponent(typeof(Animator))]
    internal class CharacterAnimator : MonoBehaviour
    {
        private AnimatorData m_animatorData;

        [Header("References:")] [FormerlySerializedAs("_animator")] [SerializeField]
        private Animator animator;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical   = Animator.StringToHash("Vertical");
        private static readonly int Running    = Animator.StringToHash("Running");

        private void LateUpdate()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if(animator.speed == 0f)
            {
                return;
            }

            animator.SetFloat(Horizontal, m_animatorData.moveDirection.x);
            animator.SetFloat(Vertical,   m_animatorData.moveDirection.z);
            animator.SetBool(Running, m_animatorData.isRunning);
        }

        public void UpdateData(AnimatorData animatorData)
        {
            m_animatorData = animatorData;
        }

        public void Resume()
        {
            animator.speed = 1f;
        }

        public void Pause()
        {
            animator.speed = 0f;
        }
    }
}