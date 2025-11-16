using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    [Header("PC / Gamepad")]
    [SerializeField] private GameObject keyboardTutorial;
    [SerializeField] private GameObject gamepadTutorial;

    [Header("Mobile")]
    [SerializeField] private GameObject mobileTutorial;

    [SerializeField] private GameInput gameInput;

    private GameInput.DeviceType lastDevice;

    private void Start()
    {
        if (gameInput != null)
        {
            lastDevice = gameInput.CurrentDevice;
            UpdateTutorialCanvas(lastDevice);
        }
    }

    private void Update()
    {
        if (gameInput == null)
            return;

        if (gameInput.CurrentDevice != lastDevice)
        {
            lastDevice = gameInput.CurrentDevice;
            UpdateTutorialCanvas(lastDevice);
        }
    }

    private void UpdateTutorialCanvas(GameInput.DeviceType device)
    {
        keyboardTutorial.SetActive(device == GameInput.DeviceType.KeyboardMouse);
        gamepadTutorial.SetActive(device == GameInput.DeviceType.Gamepad);
        mobileTutorial.SetActive(device == GameInput.DeviceType.Mobile);
    }
}
