using UnityEngine;

public class PruebasFuncionamientoDialogo : MonoBehaviour
{
    Dialogues dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = FindAnyObjectByType<Dialogues>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T)) 
        //{
        //    Debug.Log("Comenzando Dialogo..");
        //    dialogue.StartDialogue("Escena1_1");
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    dialogue.DialogueEnd();
        //    Debug.Log("Comenzando Dialogo..");
        //    dialogue.StartDialogue("anchoa_1");
        //}
    }
}
