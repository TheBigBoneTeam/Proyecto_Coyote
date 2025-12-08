using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Tooltip("Camera Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;
    private CameraFollow camFollow;

    [Tooltip("Controlador Cámaras")]
    Animator cinemachineAnimator;

    void Start()
    {
        camFollow = FindAnyObjectByType<CameraFollow>();
        cinemachineAnimator = GameObject.Find("State-Driven Camera").GetComponent<Animator>();
        cinemachineAnimator.Play("FollowCamera");
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R)) StartShake();
    }

    #region State Driven Camera controller
    public void ActiveTargetLookingCamera() 
    {
        cinemachineAnimator.Play("TargetLooking_Camera");
    }

    public void ActiveFollowCamera()
    {
        cinemachineAnimator.Play("FollowCamera");
        
        if (camFollow != null)
            camFollow.AlignFreeCameraBehindPlayer();
    }
    public void ActiveHookCamera()
    {
        cinemachineAnimator.Play("Hook_Camera");
    }
    #endregion
    #region CameraShake
    public void StartShake()
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
