using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private GameObject keyboardTutorial;
    [SerializeField] private GameObject gamepadTutorial;

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
        if (gameInput == null) return;

        var current = gameInput.CurrentDevice;
        if (current != lastDevice)
        {
            lastDevice = current;
            UpdateTutorialCanvas(current);
        }
    }

    private void UpdateTutorialCanvas(GameInput.DeviceType device)
    {
        keyboardTutorial.SetActive(device == GameInput.DeviceType.KeyboardMouse);
        gamepadTutorial.SetActive(device == GameInput.DeviceType.Gamepad);
    }
}