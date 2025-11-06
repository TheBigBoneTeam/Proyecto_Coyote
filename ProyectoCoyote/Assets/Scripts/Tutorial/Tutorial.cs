using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;

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
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            Debug.Log("entercongrartulation");
            tutorial.enemy.gameObject.SetActive(false);
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = "Ahora ya posees los conocimientos necesarios yo creo que ya estás preparado para continuar tu viaje, mucha suerte chaval.";
            tutorial.waitTime(4);
        }
    }

}
