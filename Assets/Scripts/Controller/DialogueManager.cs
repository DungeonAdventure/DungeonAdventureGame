using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Controller
{
    /// <summary>
    /// Manages and displays dialogue sequences with typewriter effects, character images, and autoplay functionality.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [Tooltip("The panel that contains the dialogue UI.")]
        public GameObject dialoguePanel;

        [Tooltip("Text component for displaying dialogue lines.")]
        public TextMeshProUGUI dialogueText;

        [Tooltip("Image used to display the character portrait.")]
        public Image characterImage;

        [Header("Dialogue Settings")]
        [Tooltip("Array of character sprites to be shown during dialogue.")]
        public Sprite[] characterSprites;

        [Tooltip("Array of dialogue lines to be shown in sequence.")]
        public string[] lines;

        [Tooltip("Delay in seconds between each character typed.")]
        public float typingSpeed = 0.05f;

        [Tooltip("Whether to automatically proceed to the next line.")]
        public bool autoPlay = true;

        private int _index;
        private bool _isTyping;

        /// <summary>
        /// Indicates whether a dialogue sequence is currently playing.
        /// </summary>
        public bool IsDialoguePlaying { get; private set; }

        /// <summary>
        /// Hides the dialogue panel on start.
        /// </summary>
        void Start()
        {
            dialoguePanel.SetActive(false);
        }

        /// <summary>
        /// Starts the dialogue sequence from the beginning.
        /// </summary>
        public void BeginDialogue()
        {
            dialoguePanel.SetActive(true);
            _index = 0;
            IsDialoguePlaying = true;
            StartCoroutine(TypeLine());
        }

        /// <summary>
        /// Coroutine to type out each character of the current dialogue line.
        /// </summary>
        IEnumerator TypeLine()
        {
            _isTyping = true;
            dialogueText.text = "";

            foreach (char letter in lines[_index])
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            _isTyping = false;

            if (autoPlay)
            {
                yield return new WaitForSeconds(1.5f);
                NextLine();
            }
        }

        /// <summary>
        /// Moves to the next line in the dialogue sequence or ends the dialogue.
        /// </summary>
        void NextLine()
        {
            _index++;

            if (_index < lines.Length)
            {
                if (characterSprites.Length > _index)
                {
                    characterImage.sprite = characterSprites[_index];
                }

                StartCoroutine(TypeLine());
            }
            else
            {
                dialoguePanel.SetActive(false);
                IsDialoguePlaying = false;
                Debug.Log("Dialogue ended.");
            }
        }
    }
}