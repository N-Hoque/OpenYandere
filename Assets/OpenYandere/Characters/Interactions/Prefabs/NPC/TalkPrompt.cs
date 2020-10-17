using OpenYandere.Managers;
using OpenYandere.UI.TalkCanvas;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.Interactions.Prefabs.NPC
{
    internal class TalkPrompt : Interactable
    {
        [FormerlySerializedAs("_npc")] [SerializeField]
        private Characters.NPC.NPC npc;

        protected override void Awake()
        {
            base.Awake();

            promptText       = "Talk";
            promptKeyCode    = KeyCode.E;
            offsetFromObject = new Vector3(0, 0.35f, 0);
        }

        protected override void OnPromptTriggered()
        {
            Debug.Log("Talking to " + npc.characterName);

            DialogueBox dialogueBox = GameManager.Instance.uiManager.dialogueBox;

            // Initialise the dialogue box.
            dialogueBox.Initialise(npc);

            // Set the dialogue text.
            dialogueBox.SetText(npc.characterName, "Do you need something?");

            // Show dialogue box and choices.
            dialogueBox.ShowBox();
            dialogueBox.ShowChoices();
        }
    }
}