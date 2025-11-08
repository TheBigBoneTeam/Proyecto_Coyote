using UnityEngine;
using UnityEngine.SceneManagement;

public class winScreen : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
      //  FindAnyObjectByType<Enemy>().subscribeToDie((a) => { anim.Play("fadeIn"); Cursor.visible = true; Cursor.lockState = CursorLockMode.None; });

    }
    public void Win()
    {
        anim.Play("fadeIn"); Cursor.visible = true; Cursor.lockState = CursorLockMode.None;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void menu()
    {
        SceneManager.LoadScene(0);
    }
}