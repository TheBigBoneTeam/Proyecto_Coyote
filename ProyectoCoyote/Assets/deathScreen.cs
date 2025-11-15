using UnityEngine;
using UnityEngine.SceneManagement;

public class deathScreen : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        FindAnyObjectByType<Player>().subscribeToDie((t) => { anim.Play("fadeIn"); Cursor.visible = true; Cursor.lockState = CursorLockMode.None; Services.ServiceLocator.Instance.Get<IGameStateManager>().Die();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void restart()
    {
        anim.Play("fadeOut");
        Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Services.ServiceLocator.Instance.Get<IGameStateManager>().Restart();
    }
    public void menu()
    {
    }
}
