// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
//
// namespace Controller
// {
//     public class DialogueManager : MonoBehaviour
//     {
//         [Header("UI Elements")]
//         public GameObject dialoguePanel;
//         public TextMeshProUGUI dialogueText;
//         public Image characterImage;
//
//         [Header("Dialogue Settings")]
//         public Sprite[] characterSprites; // 多种头像
//         public string[] lines;            // 对话文本
//         public float typingSpeed = 0.05f;
//         public bool autoPlay = true;      // 是否自动下一句
//
//         private int _index;
//         private bool _isTyping;
//
//         public bool IsDialoguePlaying { get; private set; } // ✅ 用于外部判断状态
//
//         void Start()
//         {
//             dialoguePanel.SetActive(true);
//             _index = 0;
//             IsDialoguePlaying = true;
//             StartCoroutine(TypeLine());
//         }
//
//         void Update()
//         {
//             if (Input.GetKeyDown(KeyCode.Space))
//             {
//                 if (_isTyping)
//                 {
//                     StopAllCoroutines();
//                     dialogueText.text = lines[_index];
//                     _isTyping = false;
//                 }
//                 else
//                 {
//                     NextLine();
//                 }
//             }
//         }
//
//         IEnumerator TypeLine()
//         {
//             _isTyping = true;
//             dialogueText.text = "";
//
//             foreach (char letter in lines[_index])
//             {
//                 dialogueText.text += letter;
//                 yield return new WaitForSeconds(typingSpeed);
//             }
//
//             _isTyping = false;
//
//             if (autoPlay)
//             {
//                 yield return new WaitForSeconds(1.5f);
//                 NextLine();
//             }
//         }
//
//         void NextLine()
//         {
//             _index++;
//
//             if (_index < lines.Length)
//             {
//                 if (characterSprites.Length > _index)
//                 {
//                     characterImage.sprite = characterSprites[_index];
//                 }
//
//                 StartCoroutine(TypeLine());
//             }
//             else
//             {
//                 dialoguePanel.SetActive(false);
//                 IsDialoguePlaying = false;
//                 Debug.Log("Dialogue ended.");
//             }
//         }
//     }
// }
