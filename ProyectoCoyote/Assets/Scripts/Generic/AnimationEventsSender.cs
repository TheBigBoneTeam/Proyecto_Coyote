using tutorial;
using UnityEngine;

public class AnimationEventsSender : MonoBehaviour
{
    public Animator anim;

    private float lastStepTime = 0f;
    private const float stepCooldown = 0.22f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlaySound(string soundName)
    {
        // Filtrar TODOS los pasos
        if (soundName.Contains("Paso1"))
        {
            // No reproducir sonidos si NO estamos en la anim de caminar
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_01"))
                return;

            // Cooldown para evitar duplicados
            if (Time.time - lastStepTime < stepCooldown)
                return;

            lastStepTime = Time.time;
        }

        AudioManager.Instance.PlaySimpleSound(soundName, false, Vector2.zero, true, false);
    }
    public void changeTutWait()
    {
        FindAnyObjectByType<betaTutorial>().changeTutWait = true;
    }
}
