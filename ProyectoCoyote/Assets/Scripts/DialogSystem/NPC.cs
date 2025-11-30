using Services;
using TMPro;

using UnityEngine;


public class NPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hover;
    [SerializeField] string startingLine;
    [SerializeField] StoryAction action;
    [SerializeField] float noticeZone;
    [SerializeField] Transform lookAtPlayer;

    private CameraController CamControl;
    private GameInput gameInput;
    private Transform player;
    private Transform enemyTarget_Locator;
    private Dialogues dialogue;
    private EnemyLockOn lockOn;
    private PlayerMovement movement;

    public bool playingDialogue;
    private bool prevLockPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
        dialogue = FindAnyObjectByType<Dialogues>();
        gameInput = FindAnyObjectByType<GameInput>();
        CamControl = FindAnyObjectByType<CameraController>();
        lockOn = FindAnyObjectByType<EnemyLockOn>();    
        movement = FindAnyObjectByType<PlayerMovement>();
        playingDialogue = false;
        hover.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        bool currentLock = gameInput != null && gameInput.LockPressed;
        if (LookForPlayer() == true && !playingDialogue && !lockOn.enemyLocked)
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

        Vector3 npcPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);

        float distance = Vector3.Distance(npcPos, playerPos);

        // Debug.Log("Distancia: " + distance);
        return distance <= noticeZone;
    }
    private void PlayDialogue() 
    {
        Debug.Log("Comenzando Dialogo..");
        hover.gameObject.SetActive(false);
        playingDialogue = true;
        dialogue.StartDialogue(startingLine, () => action.Execute(null), this, lookAtPlayer);

    }


}
