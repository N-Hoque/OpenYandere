using System.Collections.Generic;

using OpenYandere.Characters.NPC;
using OpenYandere.Characters.Player;
using OpenYandere.Managers;

using TMPro;

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.UI.TalkCanvas
{
    public class DialogueBox : MonoBehaviour
    {
        [Header("References:")] [FormerlySerializedAs("_animator")] [SerializeField]
        private Animator animator;

        [FormerlySerializedAs("_choicesAnimator")] [SerializeField]
        private Animator choicesAnimator;

        [FormerlySerializedAs("_characterName")] [SerializeField]
        private TextMeshProUGUI characterName;

        [FormerlySerializedAs("_dialogueText")] [SerializeField]
        private TextMeshProUGUI dialogueText;
        
        private static readonly int Visible = Animator.StringToHash("Visible");

        private readonly Queue<DialogueEntry> m_dialogueEntries = new Queue<DialogueEntry>();

        private bool   m_areChoicesVisible;
        private NPC    m_interactingWithNpc;
        private bool   m_isVisible;
        private Player m_player;

        private void Awake()
        {
            m_player = GameManager.Instance.playerManager.player.GetComponent<Player>();
        }

        private void Update()
        {
            if(!m_isVisible)
            {
                return;
            }

            if(!Input.GetMouseButtonDown(0) || m_areChoicesVisible)
            {
                return;
            }

            if(m_dialogueEntries.Count > 0)
            {
                DialogueEntry dialogueEntry = m_dialogueEntries.Dequeue();

                characterName.text = dialogueEntry.characterName;
                dialogueText.text  = dialogueEntry.text; // TODO: Animate the text.
            }
            else
            {
                animator.SetBool(Visible, false);
                m_isVisible = false;
            }
        }

        public void Initialise(NPC interactingWithNpc)
        {
            m_interactingWithNpc = interactingWithNpc;
        }

        public void ShowBox()
        {
            animator.SetBool(Visible, true);
            m_isVisible = true;
        }

        public void ShowChoices()
        {
            choicesAnimator.SetBool(Visible, true);
            m_areChoicesVisible = true;
        }

        public void HideChoices()
        {
            choicesAnimator.SetBool(Visible, false);
            m_areChoicesVisible = false;
        }

        public void Queue(string newCharacterName, string newDialogueText)
        {
            m_dialogueEntries.Enqueue(new DialogueEntry
            {
                characterName = newCharacterName,
                text          = newDialogueText
            });
        }

        public void SetText(string newCharacterName, string newDialogueText)
        {
            this.characterName.text = newCharacterName;
            this.dialogueText.text  = newDialogueText;
        }

        public void OnComplimentButtonClicked()
        {
            HideChoices();

            SetText(m_player.characterName, "I just wanted to tell you that you look lovely today!");

            if(m_player.reputation >= 50)
            {
                Queue(m_interactingWithNpc.characterName, "Wow! That means a lot coming from you! Thank you so much!");
            }
            else if(m_player.reputation < 0)
            {
                Queue(m_interactingWithNpc.characterName, "Umm...thanks, I guess...?...");
            }
            else
            {
                Queue(m_interactingWithNpc.characterName, "Really? That's so nice of you to say!");
            }

            m_player.reputation += 1;
        }

        public void OnGossipButtonClicked()
        {
            HideChoices();
            // TODO
        }

        public void OnPerformTaskButtonClicked()
        {
            HideChoices();
            // TODO
        }

        public void OnAskForFavorButtonClicked()
        {
            HideChoices();
            // TODO
        }

        public void OnExitButtonClicked()
        {
            HideChoices();
            // TODO
        }

        private struct DialogueEntry
        {
            public string characterName;
            public string text;
        }
    }
}