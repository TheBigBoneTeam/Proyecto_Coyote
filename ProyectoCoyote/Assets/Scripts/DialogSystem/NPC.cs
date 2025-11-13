using Services;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string startingLine;
    [SerializeField] StoryAction action;
    [SerializeField] float noticeZone;
    Transform player;
    Dialogues dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;

        dialogue = FindAnyObjectByType<Dialogues>();

    }

    // Update is called once per frame
    void Update()
    {
        if (LookForPlayer())
        {
            Debug.Log("Comenzando Dialogo..");
            dialogue.StartDialogue(startingLine, () => action.Execute(null));
        }
    }

    // Escanear alrededores en busca de enemigos
    private bool LookForPlayer()
    {
        

        if (Physics.OverlapSphere(transform.position, noticeZone, player.gameObject.layer) != null)
        {
            Debug.Log("Player encontrado");
            return true; 
        }
            

        return false;
    }
}
