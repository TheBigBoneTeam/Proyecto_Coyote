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
            Vida vida = new Vida(this);
            tutorialStateNum = 0;
            //TrueGancho truegancho = new TrueGancho (this);
            secondEnemy.gameObject.SetActive (false);
           enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);
             gamestateManager = ServiceLocator.Instance.Get<IGameStateManager>();
            gamestateManager.startCombatforTutorial();
            machine.AddTransition(start, controles, new FuncPredicate(() => true));
          machine.AddTransition(controles, camara, new FuncPredicate(() => controles.checkMovement()));
            //machine.AddTransition(controles, esquiPerfP1, new FuncPredicate(() => controles.checkMovement()));

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
            machine.AddTransition(trueEsquivePerf, vida, new FuncPredicate(() => currentEsqPerf >= objectiveEsqPerf));
            machine.AddTransition(vida, gancho1, new FuncPredicate(() => changeTutWait == true));
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
            enemy.GetComponent<DamageReceiver>().setInvincible(false);


        }

        internal void resetTarget()
        {

            lockon.resetWhenDie(enemy.transform);
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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Mientras enfocas a un enemigo podrás esquivar pulsando /esquivar/. Las direcciones donde NO tienes que esquivar se mostrarán en <color=red> ROJO </_linea_/color >. Solo podrás esquivar si estás fijando a un enemigo");

            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            tutorial.player.subscribeToDodgeAttack(esquive);


        }
        public override void Update()
        {
            base.Update();
            if(Time.frameCount % 60 == 0)
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Mientras enfocas a un enemigo podrás esquivar pulsando /esquivar/. Las direcciones donde NO tienes que esquivar se mostrarán en <color=red> ROJO </_linea_/color >. Solo podrás esquivar si estás fijando a un enemigo");
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
            float dist = Vector3.Distance(tutorial.enemy.transform.position, tutorial.secondEnemy.transform.position);
            Vector3 dir = tutorial.player.transform.position - tutorial.enemy.transform.position;
            dir.y = 0;
            dir = Vector3.Normalize(dir);
            Quaternion rotation = Quaternion.AngleAxis(40, Vector3.up);
            // Or for world axes: Quaternion rotation = Quaternion.Euler(0, degrees, 0);

            // Rotate the starting vector
            dir = rotation * dir;
            dir = Vector3.Normalize(dir);

            tutorial.secondEnemy.transform.position = tutorial.enemy.transform.position + (dir * dist);
            tutorial.secondEnemy.gameObject.SetActive(true);
            tutorial.secondEnemy.GetComponent<enemigoTutorial>().setTutorialMode(0);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("A veces te enfrentarás a varios enemigos a la vez. Puedes bloquear los ataques por la espalda pulsando /esquivaratras/ sin tener que perder de vista al enemigo al que te estás enfrentando. El punto central de la interfaz te avisará de estos ataques.");
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
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("A veces te enfrentarás a varios enemigos a la vez. Puedes bloquear los ataques por la espalda pulsando /esquivaratras/ sin tener que perder de vista al enemigo al que te estás enfrentando. El punto central de la interfaz te avisará de estos ataques.");

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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Algunos enemigos te podrán disparar. Para bloquear los disparos mantén al enemigo fijado y bloquea usando /esquivaratras/ .");
            tutorial.tutorialStateNum = 2;
            tutorial.secondEnemy.Die();
            tutorial.currentEsquives = 0;
            tutorial.player.subscribeToDodgeAttack(esquive);
            tutorial.tutorialGun.gameObject.SetActive(true);
            tutorial.tutorialGun.startShooting();
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);


        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Algunos enemigos te podrán disparar. Para bloquear los disparos mantén al enemigo fijado y bloquea usando /esquivaratras/ .");

            }



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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Ahora, vamos a lo importante. Tienes 2 direcciones de ataque: izquierda (/pegar1/) y derecha (/pegar2/). Solo podrás atacar mientras fijas a un enemigo");

            tutorial.currentHits = -1;
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(2);

        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Ahora, vamos a lo importante. Tienes 2 direcciones de ataque: izquierda (/pegar1/) y derecha (/pegar2/). Solo podrás atacar mientras fijas a un enemigo");

            }
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
            tutorial.enemy.GetComponent<enemigoTutorial>().canBeParry = true;
            tutorial.currentHits = -1;
            tutorial.TutorialText.text = $"Los enemigos pueden bloquearte también y, si atacas en la dirección en la que bloquean, no sufrirán daño. Fíjate en sus movimientos y golpea correctamente"; 
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
        bool finish;
        public EsquiPerfP2(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            finish = false;
            Time.timeScale = 0;
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Si pulsas /esquivar/ en el momento justo podrás realizar un esquive perfecto, permitiéndote hacer un contraataque devastador, que además te hará recuperar salud.");

            tutorial.currentEsqPerf = 0;
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

            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Si pulsas /esquivar/ en el momento justo podrás realizar un esquive perfecto, permitiéndote hacer un contraataque devastador, que además te hará recuperar salud.");

            }
            if (tutorial.gameInput.Evade_LeftPressed || tutorial.gameInput.Evade_RightPressed)
            {
                if (!finish)
                {
                    Time.timeScale = 1;

                    finish = true;
                }
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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Pulsa /pegar1/ o /pegar2/ ahora para hacer un contraataque");

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

            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Pulsa /pegar1/ o /pegar2/ ahora para hacer un contraataque");
            }
            if (tutorial.gameInput.BlockPressed)
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
       
            tutorial.zonaGancho1.SetActive(true);
            tutorial.zonaGancho2.SetActive(false);
            tutorial.enemigoGancho.gameObject.SetActive(true);
            tutorial.changeTutWait = false;
            tutorial.currentGanchos = 0;
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Como último detalle, tus puños son ganchos también, ¿no? Usa el gancho para acercate los enemigos o atraerlos hacia ti. Pulsa /gancho/ para apuntar al enemigo.");
        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Como último detalle, tus puños son ganchos también, ¿no? Usa el gancho para acercate los enemigos o atraerlos hacia ti. Pulsa /gancho/ para apuntar al enemigo.");
            }
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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Ahora, pulsa /pegar1/ para enganchar al enemigo.");
        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Ahora, pulsa /pegar1/ para enganchar al enemigo.");
            }
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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Pulsa /ir a enemigo/ para ir hacia él.");
        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Pulsa /ir a enemigo/ para ir hacia él.");
            }
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
            tutorial.resetTarget();
            tutorial.enemigoGancho.GetComponent<HookableObject>().canGoToTarget = false;
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Detrás de ti hay cactus que podrás enganchar para moverte por el mapa. Puedes seleccionar tu objetivo con /movimiento/. Ve a todos los objetivos.");
        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Detrás de ti hay cactus que podrás enganchar para moverte por el mapa. Puedes seleccionar tu objetivo con /movimiento/. Recorre todos los objetivos.");
            }
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
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Por último, engancha al enemigo y atráelo hacia ti usando /atraer enemigo/.");
        }
        public override void Update()
        {
            base.Update();
            if (Time.frameCount % 60 == 0)
            {
                tutorial.TutorialText.text = InputTextFormatter.Cambiar("Por último, engancha al enemigo y atráelo hacia ti usando /atraer enemigo/.");
            }
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
        bool hasRun;
        new betaTutorial tutorial;
        float runTime;
        float runTimeNeeded = 1.5f;

        public ControlesTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = true;
            tutorial.enemy.GetComponent<DamageReceiver>().setInvincible(false);
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Empecemos por lo esencial. Usa /movimiento/ para moverte por el escenario y /correr/ a la vez para correr.");
            input = GameObject.FindAnyObjectByType<GameInput>();



        }
        public override void OnExit()
        {

        }
        public bool checkMovement()
        {
            Debug.Log(runTime);

            if (tutorial.gameInput.SprintHeld)
            {
                hasRun = true;
            }
            if (input.Horizontal != 0 || input.Vertical != 0)
            {
                runTime += Time.deltaTime;
            }
            else
            {
                runTime -= Time.deltaTime;
                runTime = MathF.Max(runTime, 0);

            }
            if (hasRun && runTime > runTimeNeeded)
            {
                return true;
            }
            return false;
        }
    }
    public class CamaraTutorial : BaseTutorialState
    {
        float mouseMovementNeeded = 1.5f;
      float  currentMouseMove;
        new betaTutorial tutorial;

        public CamaraTutorial(betaTutorial _tut)
        {
            tutorial = _tut;
            
        }
        public override void OnEnter()
        {
            currentMouseMove = 0;
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Genial, parece que sabes cómo caminar. Ahora usa /camara/ para ver lo que hay a tu alrededor.");
            
        }
        public override void OnExit()
        {
            
        }
        public bool checkMovement()
        {
            Debug.Log(currentMouseMove);
            //    tutorial.gameInput.CameraInput
            if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0 || Input.GetAxis("Mouse X") > 0 || tutorial.gameInput.CameraInput.x != 0 || tutorial.gameInput.CameraInput.y != 0)
            {
                //Code for action on mouse moving left
                currentMouseMove += Time.deltaTime;
            }
            else
            {
                currentMouseMove -= Time.deltaTime;
                currentMouseMove = MathF.Max(currentMouseMove, 0);
            }
            if(currentMouseMove > mouseMovementNeeded)
            {
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

            tutorial.TutorialText.text = InputTextFormatter.Cambiar("Muy bien, me has demostrado que esos ojos no los tienes solo de decoración. Ahora presiona /lockeo/ para fijar y desfijar a un enemigo. En este caso prueba con este cactus.");
        }
        public override void OnExit()
        {
            tutorial.enemy.GetComponent<AssetBehaviourRunner>().enabled = true;
        }
    }
    public class Vida : BaseTutorialState
    {
        new betaTutorial tutorial;
        public Vida(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.secondEnemy.Die();
            tutorial.enemy.Die();

            tutorial.resetTarget();
            Debug.Log("vida");
            tutorial.enemy.gameObject.SetActive(false);
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = "Cuando mates a un enemigo, soltará una bola de vida. Tu vida máxima es 10.";
            tutorial.waitTime(5);
        }
    }

    public static class InputTextFormatter
    {
        public static string Cambiar(string og)
        {
            GameInput.DeviceType device = GameObject.FindAnyObjectByType<GameInput>().CurrentDevice;
            string[] partes = og.Split('/');
            string resultado = "";
            Debug.Log(device);
            for (int i = 0; i < partes.Length; i++)
            {
                //if (i % 2 == 0)
                //{
                //    // Texto normal
                //    resultado += partes[i];
                //    continue;
                //}

                // Texto entre / / → es un comando
                string token = partes[i].ToLower();
                switch (token)
                {
                    case "_linea_":
                        resultado += "/";
                        break;
                    case "movimiento":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>WASD</b>",
                            GameInput.DeviceType.Gamepad => "<b>Joystick Izquierdo</b>",
                            GameInput.DeviceType.Mobile => "<b>Joystick Izquierdo</b>",
                            _ => partes[i]
                        };
                        break;

                    case "camara":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>Mover Ratón</b>",
                            GameInput.DeviceType.Gamepad => "<b>Joystick Derecho</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Cámara</b>",
                            _ => partes[i]
                        };
                        break;

                    case "correr":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>LShift</b>",
                            GameInput.DeviceType.Gamepad => "<b>Botón RT / R2</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Correr</b>",
                            _ => partes[i]
                        };
                        break;

                    case "esquivar":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>A o D</b>",
                            GameInput.DeviceType.Gamepad => "<b>Joystick izquierdo a los lados</b>",
                            GameInput.DeviceType.Mobile => "<b>Joystick izquierdo a los lados</b>",
                            _ => partes[i]
                        };
                        break;

                    case "esquivaratras":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>ESPACIO</b>",
                            GameInput.DeviceType.Gamepad => "<b>Botón B / X</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Esquivar</b>",
                            _ => partes[i]
                        };
                        break;

                    case "dashear":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>ESPACIO</b>",
                            GameInput.DeviceType.Gamepad => "<b>Botón B / X</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Dash</b>",
                            _ => partes[i]
                        };
                        break;

                    case "pegar1":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>CLICK IZQUIERDO</b>",
                            GameInput.DeviceType.Gamepad => "<b>Botón LT / L2</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Ataque IZquierdo</b>",
                            _ => partes[i]
                        };
                        break;
                    case "pegar2":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>CLICK DERECHO</b>",
                            GameInput.DeviceType.Gamepad => "<b>Botón RT / R2</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Ataque Derecho</b>",
                            _ => partes[i]
                        };
                        break;

                    case "gancho":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>E</b>",
                            GameInput.DeviceType.Gamepad => "<b>B / Círculo o presionando el Joystick izquierdo</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Gancho</b>",
                            _ => partes[i]
                        };
                        break;

                    case "lockeo":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>Q</b>",
                            GameInput.DeviceType.Gamepad => "<b>LB / L1 o presionando el Joystick derecho</b>",
                            GameInput.DeviceType.Mobile => "<b>Botón Lockeo</b>",
                            _ => partes[i]
                        };
                        break;

                    case "atraer enemigo":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>S</b>",
                            GameInput.DeviceType.Gamepad => "<b>Joystick izquierdo abajo</b>",
                            GameInput.DeviceType.Mobile => "<b>Joystick izquierdo abajo</b>",
                            _ => partes[i]
                        };
                        break;

                    case "ir a enemigo":
                        resultado += device switch
                        {
                            GameInput.DeviceType.KeyboardMouse => "<b>W</b>",
                            GameInput.DeviceType.Gamepad => "<b>Joystick izquierdo arriba</b>",
                            GameInput.DeviceType.Mobile => "<b>Joystick izquierdo arriba</b>",
                            _ => partes[i]
                        };
                        break;

                    default:
                        resultado += partes[i];
                        break;
                }
            }

            return resultado;
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
