using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using Services;
using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.CullingGroup;

namespace tutorial
{
    public class betaTutorial: Tutorial
    {

        EnemyLockOn lockon;
        public  int currentEsquives;
        public int currentHits;
        public int currentEsqPerf;
        public int currentGanchos;

        [SerializeField] public int objectiveEsquives;
        [SerializeField] public int objectiveHits;
        [SerializeField] public int objectiveEsqPerf;
        [SerializeField] public int objectiveGancho;
        public int tutorialStateNum = 0;

        public GameInput gameInput;

        public Enemy secondEnemy;

        public IGameStateManager gamestateManager;

        public GameObject zonaGancho1;
        public GameObject zonaGancho2;

        public Enemy enemigoGancho;
        public DetectPlayer detectPlayerEnemy;
        public DetectPlayer[] detectPlayerArray;

        public tutorialGun tutorialGun;

        protected override void Start()
        {
            gameInput = FindAnyObjectByType<GameInput>();
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
            //PegarTutorial pegar = new PegarTutorial(this);
            TruePegarTutorial truePegarTutorial = new TruePegarTutorial(this);

            EsquiveTrasero esquiveTrasero = new EsquiveTrasero (this);
            AtaqueP1 ataquep1 = new AtaqueP1 (this);
            AtaqueP2 ataqueP2 = new AtaqueP2 (this);
            EsquiPerfP1 esquiPerfP1 = new EsquiPerfP1 (this);
            EsquiPerfP2 esquiPerfP2 = new EsquiPerfP2 (this);
            EsquiPerfP2_1 esquiPerf2_1 = new EsquiPerfP2_1 (this);
            TrueEsquivePerf trueEsquivePerf = new TrueEsquivePerf (this);
            EsquiveTraseroBalas esquivaBalas = new EsquiveTraseroBalas (this);
            Gancho1 gancho1 = new Gancho1 (this);
            Gancho1_1 gancho1_1 = new Gancho1_1(this);
            Gancho1_2 gancho1_2 = new Gancho1_2(this);

            Gancho2 gancho2 = new Gancho2 (this);
            Gancho3 gancho3 = new Gancho3 (this);
            tutorialStateNum = 0;
            //TrueGancho truegancho = new TrueGancho (this);
            secondEnemy.gameObject.SetActive (false);
           enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);
             gamestateManager = ServiceLocator.Instance.Get<IGameStateManager>();
            gamestateManager.startCombatforTutorial();
            machine.AddTransition(start, controles, new FuncPredicate(() => true));
            machine.AddTransition(controles, camara, new FuncPredicate(() => controles.checkMovement()));

            machine.AddTransition(camara, lockear, new FuncPredicate(() => camara.checkMovement()));
            machine.AddTransition(lockear, esquivar, new FuncPredicate(() =>lockon.currentTarget == enemy.transform));
            machine.AddTransition(esquivar, trueesquivar, new FuncPredicate(() => currentEsquives == 1));

            machine.AddTransition(trueesquivar, esquiveTrasero, new FuncPredicate(() => currentEsquives >= objectiveEsquives && tutorialStateNum == 0));
            machine.AddTransition(esquiveTrasero, trueesquivar, new FuncPredicate(() => currentEsquives == 1));
            machine.AddTransition(trueesquivar, esquivaBalas, new FuncPredicate(() => currentEsquives >= objectiveEsquives && tutorialStateNum == 1));
            machine.AddTransition(esquivaBalas, trueesquivar, new FuncPredicate(() => currentEsquives == 1));

            machine.AddTransition(trueesquivar, ataquep1, new FuncPredicate(() => currentEsquives >= objectiveEsquives && tutorialStateNum == 2));

            //machine.AddTransition(esquiveTrasero, ataquep1, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(ataquep1, ataqueP2, new FuncPredicate(() => currentHits == 1));
            machine.AddTransition(ataqueP2, truePegarTutorial, new FuncPredicate(() => currentHits == 1));
            machine.AddTransition(truePegarTutorial, esquiPerfP1, new FuncPredicate(() =>currentHits >= objectiveHits));
            machine.AddTransition(esquiPerfP1, esquiPerfP2, new FuncPredicate(()=>changeTutWait == true));
            machine.AddTransition(esquiPerfP2, esquiPerf2_1, new FuncPredicate(() => currentEsqPerf == 1));

            machine.AddTransition(esquiPerf2_1, trueEsquivePerf, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(trueEsquivePerf, gancho1, new FuncPredicate(() => currentEsqPerf >= objectiveEsqPerf));
            machine.AddTransition(gancho1, gancho1_1, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(gancho1_1, gancho1_2, new FuncPredicate(() => changeTutWait == true));

            machine.AddTransition(gancho1_2, gancho2, new FuncPredicate(() => currentGanchos == 1));

            machine.AddTransition(gancho2, gancho3, new FuncPredicate(() => currentGanchos == detectPlayerArray.Length));
            machine.AddTransition(gancho3, congratulationState, new FuncPredicate(() => currentGanchos == 1));


            //machine.AddTransition(ataquep1, congratulationState, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(congratulationState, end, new FuncPredicate(() => changeTutWait == true));




        }
        public void changeTutWaitSet(bool set)
        {
            changeTutWait = set;
        }

    public void startWaitEnemy()
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
            
            tutorial.TutorialText.text = $"Mientras enfocas a un enemigo podrás esquivar pulsando <Espacio>. Las direcciones donde NO tienes que esquivar se mostrarán en <color=red> ROJO </color>.";
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            tutorial.player.subscribeToDodgeAttack(esquive);


        }

        public void esquive(HitDirections d)
        {
            Debug.Log("Esquive");
            tutorial.currentEsquives++;
        }
        public override void OnExit()
        {
            tutorial.currentEsquives = 0;
            tutorial.player.unSubscribeToDodgeAttack(esquive);

        }
    }
    public class EsquiveTrasero: BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiveTrasero(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.currentEsquives = 0;
            tutorial.tutorialStateNum = 1;
            tutorial.player.subscribeToDodgeAttack(esquive);
            tutorial.secondEnemy.gameObject.SetActive(true);
            tutorial.secondEnemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);
            tutorial.TutorialText.text = $"Buenos reflejos campeón. A veces te enfrentarás a varios enemigos. Puedes bloquear los ataques de enemigos no enfocados pulsando  <Espacio> sin ninguna dirección. El punto central de la interfaz te avisará de ataques desde fuera.";
            
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
    public class EsquiveTraseroBalas : BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiveTraseroBalas(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.tutorialStateNum = 2;
            tutorial.secondEnemy.gameObject.SetActive(false);
            tutorial.currentEsquives = 0;
            tutorial.player.subscribeToDodgeAttack(esquive);
            tutorial.tutorialGun.gameObject.SetActive(true);
            tutorial.tutorialGun.startShooting();
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);

            tutorial.TutorialText.text = $"Algunos enemigos te dispararán desde la distancia. Normalmente querrás ir a por ellos los más rápido posible pero practica esquivando estas piedras.";

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
    public class AtaqueP1 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public AtaqueP1(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            Debug.Log("enterPegar1");            tutorial.tutorialGun.gameObject.SetActive(false);
            tutorial.secondEnemy.gameObject.SetActive(false);
            tutorial.enemy.GetComponent<DamageReceiver>().setDodge(false);
            tutorial.enemy.GetComponent<DamageReceiver>().clearDirection();

            tutorial.currentHits = -1;
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);

            tutorial.TutorialText.text = $"Ahora que sabes esquivar vamos a lo importante. Tienes 3 direcciones de ataque: centro, izquierda y derecha. Para atacar presione <CLICK DERECHO> + <DIRECCIÓN>.";
        }
        public void enemyHit(int currentLife)
        {
            Debug.Log("enemyIsHit");
            tutorial.currentHits++;
        }
        public override void OnExit()
        {
            Debug.Log("exitPegar1");
            tutorial.enemy.unSubscribeToLifeChange(enemyHit);
            base.OnExit();

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
            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveHits - tutorial.currentHits} ataques.";
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(1);


        }

        public void enemyHit(int currentLife)
        {
            Debug.Log("enemyIsHit");
            tutorial.currentHits++;
            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveHits - tutorial.currentHits} ataques.";

        }
        public override void OnExit()
        {
            tutorial.enemy.unSubscribeToLifeChange(enemyHit);
        }
    }
    public class AtaqueP2 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public AtaqueP2(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.currentHits = -1;
            tutorial.TutorialText.text = $"Los enemigos pueden bloquear, si atacas en la dirección en la que bloquean el enemigo no sufren daño y en ocasiones pueden realizarar un contraataque. Fijate en sus movimientos y golpeale correctamente"; 
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(1);


        }

        private void enemyHit(int currentLife)
        {
            Debug.Log("enemyIsHit");
            tutorial.currentHits++;
        }

        public override void OnExit()
        {
            tutorial.enemy.unSubscribeToLifeChange(enemyHit);
        }
    }

    public class EsquiPerfP1 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiPerfP1(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.changeTutWait = false;
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(4);
            tutorial.TutorialText.text = "";
            //tutorial.TutorialText.text = $"Se ve que sabes usar esos puños, pero ¿sabes cómo recuperar fuerzas? Para curarte hay dos maneras: tocando botiques que encontrarás por el camino o haciendo contrataque a los enemigos. " +
            //    $"Eso sí, dicho contraataque solo recuperará el daño recibido en el último ataque. Si poses menos corazones que daño recibido caerás en combate así que ten mucho cuidado.";

        }
    }
    public class EsquiPerfP2 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiPerfP2(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            Time.timeScale = 0;
            tutorial.currentEsqPerf = 0;
            tutorial.TutorialText.text = $"Cuando realizas un esquive en el momento justo podrás realizar un esquive perfecto, lo que ralentizará el tiempo y te permitirá hacer un contraataque devastador. Pulsa /ESPACIO/ para hacer el esquive perfecto";
          //  tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(Parry);
        }
        public void Parry(object sender, stateData e)
        {
            if (e.currentState == GameState.SlowDown)
            {
                tutorial.currentEsqPerf++;
            }
        }
        public override void Update()
        {
            base.Update();
            if (tutorial.gameInput.EvadePressed)
            {
                Time.timeScale = 1;
            }
        }
        public override void OnExit()
        {
            ServiceLocator.Instance.Get<IGameStateManager>().unSubscribeToStateChange(Parry);

        }
    }
    public class EsquiPerfP2_1 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiPerfP2_1(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.changeTutWait = false;

            tutorial.TutorialText.text = $"Ataca ahora para hacer un daño devastador";
            //  tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(Parry);
        }
        public void Parry(object sender, stateData e)
        {
            if (e.oldState == GameState.SlowDown)
            {
                tutorial.changeTutWait = true;
            }
        }
        public override void Update()
        {
            base.Update();
            if (tutorial.gameInput.EvadePressed)
            {
                Time.timeScale = 1;
            }
        }
    }
        public class TrueEsquivePerf : BaseTutorialState
    {
        new betaTutorial tutorial;
        public TrueEsquivePerf(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.currentEsqPerf = 0;
            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveEsqPerf - tutorial.currentEsqPerf} esquive perfecto.";
            ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(Parry);
            //private void StateChange(object sender, stateData e)


        }
         public void Parry(object sender, stateData e)
        {
            if (e.oldState == GameState.SlowDown)
            {
                tutorial.currentEsqPerf++;
                tutorial.TutorialText.text = $"Realiza {tutorial.objectiveEsqPerf - tutorial.currentEsqPerf} esquive perfecto.";
            }

        }
        public override void OnExit()
        {
            ServiceLocator.Instance.Get<IGameStateManager>().unSubscribeToStateChange(Parry);

        }
    }
    public class Gancho1: BaseTutorialState
    {
        new betaTutorial tutorial;
        public Gancho1(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.secondEnemy.Die();
            tutorial.enemy.Die();

            tutorial.zonaGancho1.SetActive(true);
            tutorial.zonaGancho2.SetActive(false);
            tutorial.enemigoGancho.gameObject.SetActive(true);
            tutorial.changeTutWait = false;
            tutorial.currentGanchos = 0;
            tutorial.TutorialText.text = $"Como último detalle, tus puchos son ganchos también ¿no? Podrás usarlo para atraer o acercarte a los enemigos. Pulsa /E/ para apuntar al enemigo.";
        }
        public override void Update()
        {
            base.Update();
            if (tutorial.player.GetComponent<Gancho>().currentTarget == tutorial.enemigoGancho.transform)
            {
                tutorial.changeTutWait = true;
            }
        }
    }
    public class Gancho1_1 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public Gancho1_1(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        { 
            tutorial.enemigoGancho.gameObject.SetActive(true);
            tutorial.enemigoGancho.GetComponent<HookableObject>().canBeHooked = false;
            tutorial.changeTutWait = false;
            tutorial.currentGanchos = 0;
            tutorial.TutorialText.text = $"Ahora pulsa <click> para enganchar al enemigo.";
        }
        public override void Update()
        {
            base.Update();
            if (tutorial.gameInput.HookConfirmPressed)
            {
                tutorial.changeTutWait = true;
            }
        }
    }
    public class Gancho1_2 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public Gancho1_2(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.enemigoGancho.gameObject.SetActive(true);
            tutorial.detectPlayerEnemy.setCharacter(tutorial.player);
            tutorial.changeTutWait = false;
            tutorial.currentGanchos = 0;
            tutorial.TutorialText.text = $"Pulsa <W> para ir hacia él.";
        }
    }
    public class Gancho2 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public Gancho2(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.zonaGancho2.SetActive(true);
            tutorial.currentGanchos = 0;
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = $"También hay objetos enganchables que podrás enganchar para moverte por el mapa. Puedes seleccionar tu objetivo con /WASD/. Recorre todos los objetivos.";
        }
    }
    public class Gancho3 : BaseTutorialState
    {
        new betaTutorial tutorial;
        public Gancho3(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.enemigoGancho.GetComponent<HookableObject>().canBeHooked = true;

            tutorial.currentGanchos = 0;
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = $"Por último, atrae hacia a ti al enemigo para terminar el tutorial";
        }
    }
    //public class Gancho4 : BaseTutorialState
    //{
    //    new betaTutorial tutorial;
    //    public Gancho3(betaTutorial _tut)
    //    {
    //        tutorial = _tut;
    //    }
    //    public override void OnEnter()
    //    {
    //        tutorial.waitTime(5);
    //        tutorial.changeTutWait = false;
    //        tutorial.TutorialText.text = $"Tu gancho tiene un tiempo de recarga, puedes esperar a que se recargue o realizar un esquive perfecto para recargarlo más rápido.";
    //    }
    //}
    /// <summary>
    /// ////////////////
    /// </summary>
    public class TrueEsquivarTutorial: BaseTutorialState
    {
        new betaTutorial tutorial;
        public TrueEsquivarTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.currentEsquives = 1;
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

            tutorial.TutorialText.text = "Empecemos por lo esencial. Usa /WASD/ o el joystick izquierdo para moverte por el escenario.";
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
            tutorial.waitTime(3);
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = "Genial parece que sabes cómo caminar, ahora usa el ratón o el joystick derecho puedes rotar la cámara para ver lo que hay a tu alrededor.";
            
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
    /*public class PegarTutorial : BaseTutorialState
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
    }*/
    
}
