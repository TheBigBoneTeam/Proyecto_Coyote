using UnityEngine;
using UnityEngine.UI;

public class MobileInputUI : MonoBehaviour
{
    [Header("References")]
    public GameInput gameInput;
    public FloatingJoystick moveJoystick;

    [Header("Buttons")]
    public Button hookAimButton;
    public Button dashButton;
    public Button hookButton;
    public Button hookConfirmButton;
    public Button hookTPButton;
    public Button attackButton;
    public Button evadeButton;


    private void Awake()
    {
        if(!Application.isMobilePlatform)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (gameInput == null)
            gameInput = FindAnyObjectByType<GameInput>();

        // Asignar listeners a botones
        if (hookAimButton) hookAimButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerHookAim()));
        if (dashButton) dashButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerDash()));
        if (hookButton) hookButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerHookAim()));
        if (hookConfirmButton) hookConfirmButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerHookConfirm()));
        if (hookTPButton) hookTPButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerHookTP()));
        if (attackButton) attackButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerAttack()));
        if (evadeButton) evadeButton.onClick.AddListener(() => SimulatePress(() => gameInput.TriggerEvade()));
    }

    private void Update()
    {
        if (moveJoystick != null && gameInput != null)
        {
            gameInput.SetMobileMovement(moveJoystick.Horizontal, moveJoystick.Vertical);
        }
    }

    private void SimulatePress(System.Action action)
    {
        action?.Invoke();
        // Al siguiente frame, se resetea como GameInput hace en LateUpdate
    }
}
