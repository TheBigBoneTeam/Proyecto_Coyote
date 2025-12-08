using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public class CharacterColor
{
    public string characterName;
    public Color textColor;
}

public class Dialogues : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    private GameInput gameInput;
    // Excel
    [Header("Documento de texto en .csv ")]
    [SerializeField] TextAsset[] textDialogues;
    private CameraController CamControl;
    private Dictionary<string, DialogueLine> DialogueHash;
    private List<string> dialogueKeys;
    private int currentKeyIndex = 0;
    private Coroutine typingCoroutine;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float WaitSpeed = 2f;
    [Header("Colores por personaje")]
    [SerializeField] private List<CharacterColor> characterColors;

    // [SerializeField] private UnityEngine.UI.Image dialogueImage;
    Action action1 = null;
    private bool isTyping = false;
    private bool isInDialogue = false;
    private bool isWaitingAfterSkip = false;

    private string currentFullText = "";
    private string currentPrefix = "";
    private Transform targetLocator;
    private PlayerMovement movement;
    private NPC _npc;

    Transform UIText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();

        UIText = GameObject.Find("UIText").transform;
        dialogueText = UIText.Find("CuadroDeTexto").
            GetComponent<TextMeshProUGUI>();
        UIText.gameObject.SetActive(false);
        CamControl = FindAnyObjectByType<CameraController>();
        targetLocator = GameObject.Find("HookableObjectLocator").transform;
        movement = FindAnyObjectByType<PlayerMovement>();   
        _npc = FindAnyObjectByType<NPC>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInDialogue)
        {
            if (/*gameInput.SaltarDialogo*/Input.GetMouseButtonDown(0))
            {
                SkipLine();
            }
        }
        
    }


    #region Funciones Accesibles de DialogueSystem
    public void StartDialogue(string startingLine, Action action, NPC npc, Transform npcTransform) 
    {
        LoadDialogues();
       // movement.StopMovement();
        targetLocator.position = npcTransform.position;
        _npc = npc;

        CamControl.ActiveHookCamera();
        currentKeyIndex = dialogueKeys.IndexOf(startingLine);
        currentPrefix = GetPrefix(startingLine);
        ShowText(dialogueKeys[currentKeyIndex]);
        UIText.gameObject.SetActive(true);
        action1 = action;
        isInDialogue = true;
    }
    public void DialogueEnd()
    {
        if (_npc == null) return;
       action1?.Invoke();
        CamControl.ActiveFollowCamera();
        _npc.playingDialogue = false;
        UIText.gameObject.SetActive(false);
        movement.RestartMovement();
        isInDialogue = false;
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
            // ShowImageForCharacter(dialogue.character);
            ShowColorForCharacter(dialogue.character);
        }
        else
        {
            dialogueText.text = $"[Diálogo no encontrado para clave: {key}]";
            // dialogueImage.enabled = false;
        }
    }

    //private void ShowImageForCharacter(string character)
    //{
    //    Sprite sprite = Resources.Load<Sprite>($"DialogueNPC/{character}");
    //    if (sprite != null)
    //    {
    //        dialogueImage.sprite = sprite;
    //        dialogueImage.enabled = true;
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"Imagen no encontrada para personaje: {character}");
    //        dialogueImage.enabled = false;
    //    }
    //}

    private void PlayCharacterTalking(string character) 
    {
        if (AudioManager.Instance != null)
        {
            if (UnityEngine.Random.value > 0.5f)
                return;

            switch (character)
            {
                case "":
                    break;

                case "Coyote":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Coyote", 0.2f);
                    break;

                case "perro":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Perro", 0.1f);
                    break;

                case "Denebola":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Denebola", 0.2f);
                    break;

                case "Lince":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Lince", 0.2f);
                    break;

                case "Cultista":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Cultista", 0.2f);
                    break;

                case "Carlos":
                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Carlos", 0.2f);
                    break;
            }
        }
    }


    private void ShowColorForCharacter(string character)
    {
        CharacterColor config = characterColors.Find(c => c.characterName == character);

        if (config != null)
        {
            dialogueText.color = config.textColor;
        }
        else
        {
            dialogueText.color = Color.white;
        }
    }



    private void ShowNextLine() 
    {
        if (dialogueKeys == null) { return; }

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
