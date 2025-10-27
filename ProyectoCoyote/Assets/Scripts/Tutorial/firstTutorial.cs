using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System;
using System.Collections;
using UnityEngine;

namespace tutorial
{
    public class firstTutorial: Tutorial
    {

        EnemyLockOn lockon;
      public  int currentEsquives;
        public int currentHits;

        [SerializeField] public int objectiveEsquives;
        [SerializeField] public int objectiveHits;

        [SerializeField] public GameObject box;
        protected override void Start()
        {
            lockon = FindAnyObjectByType<EnemyLockOn>();
            print("setTUT");
            machine = new StateMachine();
            startTutorialState start = new startTutorialState(this);
            ControlesTutorial controles = new ControlesTutorial(this);
            LockearTutorial lockear = new LockearTutorial(this);
            EsquivarTutorial esquivar = new EsquivarTutorial(this);
            endTutorialState end = new endTutorialState(this);
            TrueEsquivarTutorial trueesquivar = new TrueEsquivarTutorial(this);
            congratulationState congratulationState = new congratulationState(this);
            machine.AddTransition(start, controles, new FuncPredicate(() => true));
            machine.AddTransition(controles, lockear, new FuncPredicate(()=>changeTutWait == true));
            machine.AddTransition(lockear, esquivar, new FuncPredicate(() =>lockon.currentTarget == enemy.transform));
            machine.AddTransition(esquivar, trueesquivar, new FuncPredicate(() => currentEsquives == 1));
            machine.AddTransition(trueesquivar, congratulationState, new FuncPredicate(() => currentEsquives >= objectiveEsquives));

            machine.AddTransition(congratulationState, end, new FuncPredicate(() => changeTutWait == true));




        }

   public     void startWaitEnemy()
        {
            StartCoroutine(waitEnemyTurnOn());
        }
        IEnumerator waitEnemyTurnOn()
        {
            yield return new WaitForSeconds(2);
           enemy.GetComponent<AssetBehaviourRunner>().enabled = true;

        }
    }
    public class EsquivarTutorial : BaseTutorialState
    {
        new firstTutorial tutorial;
        public EsquivarTutorial(firstTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Con un enemigo marcado puedes esquivar con espacio + WASD. La interfaz marca la direccion donde te estan atacando";
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = false;
            tutorial.startWaitEnemy();
           tutorial.player.subscribeToDodgeAttack(esquive);


        }

        public void esquive(HitDirections d)
        {
            Debug.Log("Esquive");
            tutorial.currentEsquives++;
        }
        public override void OnExit()
        {
           tutorial.player.unSubscribeToDodgeAttack(esquive);

        }
    }
    public class TrueEsquivarTutorial: BaseTutorialState
    {
        new firstTutorial tutorial;
        public TrueEsquivarTutorial(firstTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques.";
            tutorial.player.subscribeToDodgeAttack(esquive);


        }
        public void esquive(HitDirections d)
        {

            tutorial.currentEsquives++;
            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques.";

        }
        public override void OnExit()
        {
            tutorial.player.unSubscribeToDodgeAttack(esquive);

        }
    }
    public class ControlesTutorial : BaseTutorialState
    {
        GameInput input;
        public ControlesTutorial(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = "Muevete con WASD.";
            input = GameObject.FindAnyObjectByType<GameInput>();



        }
        public override void OnExit()
        {

        }
        public bool checkMovement()
        {
            if(input.Horizontal != 0 || input.Vertical != 0)
            {
                return true;
            }
            return false;
        }
    }
    public class CamaraTutorial : BaseTutorialState
    {
        public CamaraTutorial(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = "Fuera de combate puedes mover la camara con el ratón";
            //tutorial.changeTutWait = false;
            //tutorial.waitTime(8);



        }
        public override void OnExit()
        {

        }
        public bool checkMovement()
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
                //Code for action on mouse moving left
                return true;
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                //Code for action on mouse moving right
                return true;
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                //Code for action on mouse moving left
                return true;
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                //Code for action on mouse moving right
                return true;
            }
            return false;

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
            tutorial.TutorialText.text = "Solo podrás esquivar y atacar si tienes marcado a algun enemigo. Pulsa Q para marcar al enemigo";



        }
        public override void OnExit()
        {
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = true;
        }
    }
    public class PegarTutorial : BaseTutorialState
    {
        new firstTutorial tutorial;
        public PegarTutorial(firstTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques.";
            tutorial.enemy.subscribeToLifeChange(enemyHit);


        }

        private void enemyHit(int currentLife)
        {
            tutorial.currentHits++;
        }

    
        public override void OnExit()
        {
            tutorial.enemy.unSubscribeToLifeChange(enemyHit);
        }
    }
    public class TruePegarTutorial : BaseTutorialState
    {
        new firstTutorial tutorial;
        public TruePegarTutorial(firstTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Golpea {tutorial.objectiveHits - tutorial.currentHits} veces al enemigo.";
            tutorial.enemy.subscribeToLifeChange(enemyHit);


        }

        private void enemyHit(int currentLife)
        {
            tutorial.currentHits++;
            tutorial.TutorialText.text = $"Golpea {tutorial.objectiveHits - tutorial.currentHits} veces al enemigo.";
        }

     
        
        public override void OnExit()
        {
            tutorial.enemy.unSubscribeToLifeChange(enemyHit);
        }
    }
}
