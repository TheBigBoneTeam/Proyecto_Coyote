using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using Unity.VisualScripting;

namespace tutorial
{
    public class Tutorial : MonoBehaviour
    {
      protected  StateMachine machine;

     public    TMP_Text TutorialText;

        public Enemy enemy;

        public Player player;

        public string NextScene;

        public bool changeTutWait;
        private void Update()
        {
            machine.Update();
        }
        protected  virtual void Start()
        {
            machine = new StateMachine();

        }
        public void endTutorial()
        {
            SceneManager.LoadScene(NextScene);
            //if (restart)
            //{
            //    ServiceLocator.Instance.Get<ILevelController>().reStart();
            //}
            //else
            //{
            //    ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.NormalTime);
            //}
            //Destroy(gameObject);
        }
        public void startTutorial()
        {
          
                machine.SetState(new startTutorialState(this));
        }
        public void waitTime(float time)
        {
            StartCoroutine(IEwaitTime(time));
        }
         IEnumerator IEwaitTime(float time)
        {

            yield return new WaitForSeconds(time);
            changeTutWait = true;
        }
    }
    public abstract class BaseTutorialState : IState
    {
        protected Tutorial tutorial;
        public virtual void OnEnter()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        public virtual void OnExit()
        {

        }
    }

    public class startTutorialState : BaseTutorialState
    {
        public startTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
          //  ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Tutorial);

        }
    }
    public class ControlesTutorial : BaseTutorialState
    {
        public ControlesTutorial(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
       
         tutorial.TutorialText.text = "Muevete con WASD. Con click izq puedes golpear en diferentes direcciones.";
            tutorial.changeTutWait = false;
            tutorial.waitTime(8);
                
            
     
        }
        public override void OnExit()
        {
           
        }
    }
    public class LockearTutorial : BaseTutorialState
    {
        new firstTutorial tutorial;
        public LockearTutorial(firstTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.enemy.gameObject.SetActive(true);
            tutorial.box.gameObject.SetActive(false);
            tutorial.TutorialText.text = "Solo podrás esquivar si tienes marcado a algun enemigo. Pulsa Q para marcar al enemigo";



        }
        public override void OnExit()
        {
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = true;
        }
    }

    
    public class endTutorialState : BaseTutorialState
    {
        public endTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            Debug.Log("endstate");
            tutorial.endTutorial();


        }
    }
    public class congratulationState : BaseTutorialState
    {
        public congratulationState(Tutorial _tut)
        {

        }
        public override void OnEnter()
        {
            
            tutorial.enemy.gameObject.SetActive(false);
            tutorial.TutorialText.text = "Enhorabuena ya puedes enfrentarte a un enemigo de verdad";
            tutorial.waitTime(4);

        }
    }

}
