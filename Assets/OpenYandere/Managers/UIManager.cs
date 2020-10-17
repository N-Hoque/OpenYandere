using System;
using System.Collections.Generic;

using OpenYandere.Characters.Interactions;
using OpenYandere.UI.Interactions;
using OpenYandere.UI.TalkCanvas;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Managers
{
    internal class UIManager : MonoBehaviour
    {
        private readonly Dictionary<KeyCode, Interactable> m_registeredInteractables =
            new Dictionary<KeyCode, Interactable>();

        private ObjectPoolManager m_objectPoolManager;

        [Header("References:")] [FormerlySerializedAs("DialogueBox")]
        public DialogueBox dialogueBox;

        private void Awake()
        {
            m_objectPoolManager = GameManager.Instance.objectPoolManager;
        }

        public GameObject GetRadialPrompt
        (
            Interactable interactable,    string  promptText,       KeyCode promptKeyCode,
            Transform    attachTransform, Vector3 offsetFromObject, Action  onPromptTriggered
        )
        {
            // If a prompt is already using the same key, return null.
            if(m_registeredInteractables.ContainsKey(promptKeyCode))
            {
                return null;
            }

            // Attempt to get a radial prompt from the pool.
            GameObject radialPromptObject = m_objectPoolManager["Radial Prompts"];

            // If it failed to get a radial prompt, return null.
            if(radialPromptObject == null)
            {
                return null;
            }

            // Initialise the radial prompt.
            var radialPrompt = radialPromptObject.GetComponent<RadialPrompt>();
            radialPrompt.Initialize(promptText, promptKeyCode, attachTransform, offsetFromObject, onPromptTriggered);

            // Associate the key code with the prompt. 
            m_registeredInteractables.Add(promptKeyCode, interactable);

            return radialPromptObject;
        }

        public void ReleaseRadialPrompt(KeyCode promptKeyCode, GameObject radialPromptObject)
        {
            m_registeredInteractables.Remove(promptKeyCode);
            radialPromptObject.SetActive(false);
        }

        public GameObject GetSquareOutline(Transform attachTransform, Vector3 offsetFromObject)
        {
            // Get a square outline from the pool.
            GameObject squareOutlineObject = m_objectPoolManager["Square Outlines"];

            // Initialise the square outline.
            var squareOutline = squareOutlineObject.GetComponent<SquareOutline>();
            squareOutline.Initialize(attachTransform, offsetFromObject);

            return squareOutlineObject;
        }

        public Interactable GetInteractable(KeyCode keyCode) =>
            // If the key code is registered, return the interactable. Otherwise, return null.
            m_registeredInteractables.ContainsKey(keyCode) ? m_registeredInteractables[keyCode] : null;
    }
}