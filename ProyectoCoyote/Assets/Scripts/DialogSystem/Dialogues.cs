using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    // Excel
    [Header("Documento de texto en .csv ")]
    [SerializeField] TextAsset[] textDialogues;
    private Dictionary<string, DialogueLine> DialogueHash;
    private List<string> dialogueKeys;
    private int currentKeyIndex = 0;
    public string startingLine = "1";
    private Coroutine typingCoroutine;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float WaitSpeed = 2f;
    [SerializeField] private UnityEngine.UI.Image dialogueImage;
    Action action1 = null;
    private bool isTyping = false;
    private bool isWaitingAfterSkip = false;

    private string currentFullText = "";
    private string currentPrefix = "";


    Transform UIText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueText = FindAnyObjectByType<TextMeshProUGUI>();
        UIText = GameObject.Find("UIText").transform;
        UIText.gameObject.SetActive(false);

        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipLine();
        }
    }


    #region Funciones Accesibles de DialogueSystem
    public void StartDialogue(string startingLine, Action action) 
    {
        LoadDialogues();
        currentKeyIndex = dialogueKeys.IndexOf(startingLine);
        currentPrefix = GetPrefix(startingLine);
        ShowText(dialogueKeys[currentKeyIndex]);
        UIText.gameObject.SetActive(true);
        action1 = action;   
    }
    public void DialogueEnd()
    {
        action1?.Invoke();
        UIText.gameObject.SetActive(false);
        Debug.Log("Fin del Diálogo");
    }

    #endregion

    #region Logic
    private void LoadDialogues()
    {
        DialogueHash = new Dictionary<string, DialogueLine>();
        dialogueKeys = new List<string>();

        foreach (var textDialogue in textDialogues)
        {
            if (textDialogue == null) continue;

            string[] lines = textDialogue.text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

                string[] parts = trimmedLine.Split(';');
                if (parts.Length < 3) continue;

                string key = parts[0];
                string character = parts[1];
                List<string> dialogueLines = new List<string>(parts);
                dialogueLines.RemoveRange(0, 2); // Remove key and character

                DialogueHash[key] = new DialogueLine(character, dialogueLines);
                dialogueKeys.Add(key);
            }
        }
    }
    private string GetPrefix(string key)
    {
        int underscoreIndex = key.IndexOf('_');
        return underscoreIndex >= 0 ? key.Substring(0, underscoreIndex) : key;
    }

    private void ShowText(string key)
    {
        if (DialogueHash.TryGetValue(key, out var dialogue))
        {
            currentFullText = string.Join("\n", dialogue.textLines);
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeText(currentFullText));
            ShowImageForCharacter(dialogue.character);
        }
        else
        {
            dialogueText.text = $"[Diálogo no encontrado para clave: {key}]";
            dialogueImage.enabled = false;
        }
    }

    private void ShowImageForCharacter(string character)
    {
        Sprite sprite = Resources.Load<Sprite>($"DialogueNPC/{character}");
        if (sprite != null)
        {
            dialogueImage.sprite = sprite;
            dialogueImage.enabled = true;
        }
        else
        {
            Debug.LogWarning($"Imagen no encontrada para personaje: {character}");
            dialogueImage.enabled = false;
        }
    }


    private void ShowNextLine() 
    {
        currentKeyIndex++;
        if (currentKeyIndex < dialogueKeys.Count)
        {
            string nextKey = dialogueKeys[currentKeyIndex];
            string nextPrefix = GetPrefix(nextKey);

            if (nextPrefix != currentPrefix)
            {
                DialogueEnd(); 
            }
            else
            {
                ShowText(nextKey);
            }
        }
        else
        {
            DialogueEnd();
        }
    }

    public void SkipLine()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            dialogueText.text = currentFullText;
            isTyping = false;
            isWaitingAfterSkip = true;
            StartCoroutine(SkipAndWait());
        }
        else if (isWaitingAfterSkip)
        {
            ForceSkipWait();
        }
        else
        {
            ShowNextLine();
        }
    }

    #endregion
    #region Enumerators
    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        isWaitingAfterSkip = false;
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        isWaitingAfterSkip = true;
        yield return new WaitForSeconds(WaitSpeed);
        isWaitingAfterSkip = false;
        ShowNextLine();

    }
    private IEnumerator SkipAndWait()
    {
        yield return new WaitForSeconds(WaitSpeed);
        isWaitingAfterSkip = false;
        ShowNextLine();
    }
    private void ForceSkipWait()
    {
        StopAllCoroutines(); 
        isWaitingAfterSkip = false;
        ShowNextLine();
    }

    #endregion

}
