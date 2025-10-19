using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) StartShake();
    }

    #region CameraShake
    void StartShake()
    {
        originalPos = Camera.main.transform.localPosition;
        InvokeRepeating("Shake", 0f, 0.01f);
        Invoke("StopShake", shakeDuration);
    }

    void Shake()
    {
        Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
        Camera.main.transform.localPosition = originalPos + shakeOffset;
    }

    void StopShake()
    {
        CancelInvoke("Shake");
        Camera.main.transform.localPosition = originalPos;
    }
    #endregion
}
