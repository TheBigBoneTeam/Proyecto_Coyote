using tutorial;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class AnimationEventsSender : MonoBehaviour
{
    public Animator anim;

    private float lastStepTime = 0f;
    private const float stepCooldown = 0.22f;
    ParticleSystem[] particles;
    private void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>(true);
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

    public void playParticle(string idx)
    {
        string[] parts = idx.Split('-');
        int i = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        float scale = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

        if (particles != null)
        {
            particles[i].transform.localScale = new Vector3(scale, scale, scale);
            particles[i].Play();
        }
    }
}
