using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera virtualCamera;

    // Shake
    public float shakeIntensity = 2f;
    public float shakeFrequency = 10f;
    public float shakeDuration = 0.5f;

    private float shakeTimer = 0f;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Start()
    {
        virtualCameraNoise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.AmplitudeGain = 0f;
        virtualCameraNoise.FrequencyGain = 0f;
    }

    // Shake
    public void TriggerShake()
    {
        if (virtualCameraNoise == null) return;

        virtualCameraNoise.AmplitudeGain = shakeIntensity;
        virtualCameraNoise.FrequencyGain = shakeFrequency;
        shakeTimer = shakeDuration;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerShake();
        }

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0 && virtualCameraNoise != null)
            {
                virtualCameraNoise.AmplitudeGain = 0f;
                virtualCameraNoise.FrequencyGain = 0f;
            }
        }
    }
}