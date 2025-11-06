using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System;
using System.Collections;
using UnityEngine;

namespace tutorial
{
    public class betaTutorial: Tutorial
    {

        EnemyLockOn lockon;
      public  int currentEsquives;
        public int currentHits;

        [SerializeField] public int objectiveEsquives;
        [SerializeField] public int objectiveHits;

        protected override void Start()
        {
            lockon = FindAnyObjectByType<EnemyLockOn>();
            print("setTUT");
            machine = new StateMachine();
            startTutorialState start = new startTutorialState(this);
            CamaraTutorial camara = new CamaraTutorial(this);
            ControlesTutorial controles = new ControlesTutorial(this);
            LockearTutorial lockear = new LockearTutorial(this);
            EsquivarTutorial esquivar = new EsquivarTutorial(this);
            endTutorialState end = new endTutorialState(this);
            TrueEsquivarTutorial trueesquivar = new TrueEsquivarTutorial(this);
            congratulationState congratulationState = new congratulationState(this);
            PegarTutorial pegar = new PegarTutorial(this);
            TruePegarTutorial truePegarTutorial = new TruePegarTutorial(this);
            machine.AddTransition(start, controles, new FuncPredicate(() => true));
            machine.AddTransition(controles, camara, new FuncPredicate(() => controles.checkMovement()));

            machine.AddTransition(camara, lockear, new FuncPredicate(() => camara.checkMovement()));
            machine.AddTransition(lockear, esquivar, new FuncPredicate(() =>lockon.currentTarget == enemy.transform));
            machine.AddTransition(esquivar, trueesquivar, new FuncPredicate(() => currentEsquives == 1));
            /*
             
            */

            machine.AddTransition(trueesquivar, pegar, new FuncPredicate(() => currentEsquives >= objectiveEsquives));
            machine.AddTransition(pegar, truePegarTutorial, new FuncPredicate(() => currentHits == 1));
            machine.AddTransition(truePegarTutorial, congratulationState, new FuncPredicate(() =>currentHits >= objectiveHits));

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
        new betaTutorial tutorial;
        public EsquivarTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Cuando enfocas a un enemigo se mostrará una interfaz encima tuya para saber sobre qué dirección se ataca o esquiva. Cuando un enemigo ataque se mostrarán en rojo las direcciones donde NO tienes que esquivar. Para esquivar presiona “espacio” o “BOTON ESQUIVE”.";
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = false;
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(0);

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
        new betaTutorial tutorial;
        public TrueEsquivarTutorial(betaTutorial _tut)
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

            tutorial.TutorialText.text = "Empecemos por lo esencial. Usa “aswd” o el joystick izquierdo para moverte por el escenario.";
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

            tutorial.TutorialText.text = "Genial parece que sabes cómo caminar, ahora usa el ratón o el joystick derecho puedes rotar la cámara para ver lo que hay a tu alrededor.";
            //tutorial.changeTutWait = false;
            tutorial.waitTime(8);



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
        new betaTutorial tutorial;
        public LockearTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.enemy.gameObject.SetActive(true);
            tutorial.TutorialText.text = "Muy bien, me has demostrado que esos ojos no los tienes solo de decoración. Ahora presiona “q” o “BOTON ENFOQUE” para enfocar y desenfocar a un enemigo, en este caso prueba con este cactus.";



        }
        public override void OnExit()
        {
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = true;
        }
    }
    public class PegarTutorial : BaseTutorialState
    {
        new betaTutorial tutorial;
        public PegarTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {

            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques.";
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(1);


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
        new betaTutorial tutorial;
        public TruePegarTutorial(betaTutorial _tut)
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
