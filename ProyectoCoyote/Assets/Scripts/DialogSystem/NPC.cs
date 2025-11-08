using Services;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string startingLine;
    [SerializeField] StoryAction action;
    Dialogues dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = FindAnyObjectByType<Dialogues>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Comenzando Dialogo..");
            dialogue.StartDialogue(startingLine, () => action.Execute(null));
        }
    }
}
