using OpenYandere.Managers;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.Interactions
{
    internal abstract class Interactable : MonoBehaviour
    {
        [FormerlySerializedAs("_renderer")] [SerializeField]
        private Renderer interactionRenderer;

        [FormerlySerializedAs("_attachTransform")] [SerializeField]
        private Transform attachTransform;

        private bool          m_isPlayerInside;
        private PlayerManager m_playerManager;

        private   GameObject m_radialPrompt;
        private   GameObject m_squareOutline;
        private   UIManager  m_uiManager;
        protected Vector3    offsetFromObject = Vector3.zero;
        protected KeyCode    promptKeyCode;

        protected string promptText;

        protected virtual void Awake()
        {
            m_uiManager     = GameManager.Instance.uiManager;
            m_playerManager = GameManager.Instance.playerManager;
        }

        protected virtual void Update()
        {
            // If the player is not inside the interaction radius return.
            if(!m_isPlayerInside)
            {
                return;
            }

            // If the mesh is currently being rendered by the camera, attempt
            // to show a radial prompt.
            if(interactionRenderer.isVisible)
            {
                ShowRadialPrompt();
            }
            // Otherwise, hide the square outline and radial prompt.
            else
            {
                HideRadialPrompt();
                HideSquareOutline();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            // If the tag is not equal to 'Player' return.
            if(!other.CompareTag("Player"))
            {
                return;
            }

            // The player has entered the interaction radius, update the flag.
            m_isPlayerInside = true;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            // If the tag is not equal to 'Player' return.
            if(!other.CompareTag("Player"))
            {
                return;
            }

            // The player has exited the interaction radius, update the flag.
            m_isPlayerInside = false;

            // Hide the radial prompt and square outline.
            HideRadialPrompt();
            HideSquareOutline();
        }

        protected virtual void ShowRadialPrompt()
        {
            // If this interactable already has a radial prompt, return.
            if(m_radialPrompt != null)
            {
                return;
            }

            // Attempt to get a radial prompt.
            m_radialPrompt = m_uiManager.GetRadialPrompt(this, promptText, promptKeyCode, attachTransform,
                                                         offsetFromObject, OnPromptTriggered);

            if(m_radialPrompt != null)
            {
                // Hide the square outline.
                HideSquareOutline();

                // Show the radial prompt.
                m_radialPrompt.SetActive(true);
            }
            else
            {
                Interactable registeredInteractable = m_uiManager.GetInteractable(promptKeyCode);
                Vector3      playerPosition         = m_playerManager.player.transform.position;

                // The distance from the player to this interactable.
                float distanceFromPlayerToThis = Vector3.Distance(playerPosition, transform.position);

                // The distance from the player to the registered interactable.
                float distanceFromPlayerToRegistered =
                    Vector3.Distance(playerPosition, registeredInteractable.transform.position);

                // If the player is closer to this interactable.
                if(distanceFromPlayerToThis > distanceFromPlayerToRegistered)
                {
                    // Switch the registered interactable to a square outline.
                    registeredInteractable.ToSquareOutline();

                    // Attempt to show the radial prompt again.
                    ShowRadialPrompt();
                }
                else
                {
                    // This interactable should show a square outline.
                    ToSquareOutline();
                }
            }
        }

        protected virtual void HideRadialPrompt()
        {
            // If this interactable has no radial prompt, return.
            if(m_radialPrompt == null)
            {
                return;
            }

            // Release the radial prompt.
            m_uiManager.ReleaseRadialPrompt(promptKeyCode, m_radialPrompt);
            m_radialPrompt = null;
        }

        protected virtual void ToSquareOutline()
        {
            // If this interactable already has a square outline, return.
            if(m_squareOutline != null)
            {
                return;
            }

            // Hide the radial prompt.
            HideRadialPrompt();

            // Get a square outline.
            m_squareOutline = m_uiManager.GetSquareOutline(attachTransform, offsetFromObject);

            // Show a square outline.
            m_squareOutline.SetActive(true);
        }

        protected virtual void HideSquareOutline()
        {
            // If this interactable has no square outline, return.
            if(m_squareOutline == null)
            {
                return;
            }

            // Release the square outline.
            m_squareOutline.SetActive(false);
            m_squareOutline = null;
        }

        protected abstract void OnPromptTriggered();
    }
}