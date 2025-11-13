using Services;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hover;
    [SerializeField] string startingLine;
    [SerializeField] StoryAction action;
    [SerializeField] float noticeZone;
    [SerializeField] Transform character;

    private CameraController CamControl;
    private GameInput gameInput;
    private Transform player;
    private Transform enemyTarget_Locator;
    private Transform cam;
    private Dialogues dialogue;
    public bool playingDialogue;
    private bool prevLockPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.transform;
        player = FindAnyObjectByType<Player>().transform;
        dialogue = FindAnyObjectByType<Dialogues>();
        gameInput = FindAnyObjectByType<GameInput>();
        CamControl = FindAnyObjectByType<CameraController>();


        playingDialogue = false;
        hover.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        bool currentLock = gameInput != null && gameInput.LockPressed;
        if (LookForPlayer() && !playingDialogue)
        {
            hover.gameObject.SetActive(true);
            if (currentLock)
            {
                PlayDialogue();
            }

        } else if (currentLock && playingDialogue) 
        {
            dialogue.DialogueEnd();
        }
        else
        {
            hover.gameObject.SetActive(false);
        }
    }

    // Escanear alrededores en busca de jugador
    private bool LookForPlayer()
    {

        Vector3 npcPos = new Vector3(character.position.x, 0, character.position.z);
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);

        float distance = Vector3.Distance(npcPos, playerPos);
        return distance <= noticeZone;
    }
    private void PlayDialogue() 
    {
        Debug.Log("Comenzando Dialogo..");
        hover.gameObject.SetActive(false);
        playingDialogue = true;
        dialogue.StartDialogue(startingLine, () => action.Execute(null));

    }


}
