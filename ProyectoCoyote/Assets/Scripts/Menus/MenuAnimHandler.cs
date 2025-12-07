using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAnimHandler : MonoBehaviour
{
    Animator anim;
    [SerializeField] menuSceneChanger menuSceneChanger;
    public bool isNewGame = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)) { StartWalk(); }
    }
    
    public void CallPlayRandomIdle()
    {
            StartCoroutine("PlayRandomIdle");
        
    }
    
    public IEnumerator PlayRandomIdle()
    {
        Debug.Log("esperando");
        yield return new WaitForSeconds(Random.Range(5,25));
        anim.Play(((int)Random.Range(1, 4)).ToString());
        
    }

    public void StartWalk()
    {
        anim.SetBool("PlayGame",true);
    }

    public void EndWalk()
    {
        if (isNewGame) newGame(); 
        else continueGame();
    }

    public void newGame()
    {
        SceneManager.LoadScene(menuSceneChanger.primerNivel);
    }
    public void continueGame()
    {
        SceneManager.LoadScene(menuSceneChanger.continueLvl);
    }
}
