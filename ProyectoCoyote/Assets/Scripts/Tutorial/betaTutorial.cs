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
        public int currentEsqPerf;
        public int currentGanchos;

        [SerializeField] public int objectiveEsquives;
        [SerializeField] public int objectiveHits;
        [SerializeField] public int objectiveEsqPerf;
        [SerializeField] public int objectiveGancho;

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
            //PegarTutorial pegar = new PegarTutorial(this);
            TruePegarTutorial truePegarTutorial = new TruePegarTutorial(this);

            EsquiveTrasero esquiveTrasero = new EsquiveTrasero (this);
            AtaqueP1 ataquep1 = new AtaqueP1 (this);
            AtaqueP2 ataqueP2 = new AtaqueP2 (this);
            EsquiPerfP1 esquiPerfP1 = new EsquiPerfP1 (this);
            EsquiPerfP2 esquiPerfP2 = new EsquiPerfP2 (this);
            TrueEsquivePerf trueEsquivePerf = new TrueEsquivePerf (this);




            machine.AddTransition(start, controles, new FuncPredicate(() => true));
            machine.AddTransition(controles, camara, new FuncPredicate(() => controles.checkMovement()));

            machine.AddTransition(camara, lockear, new FuncPredicate(() => camara.checkMovement()));
            machine.AddTransition(lockear, esquivar, new FuncPredicate(() =>lockon.currentTarget == enemy.transform));
            machine.AddTransition(esquivar, trueesquivar, new FuncPredicate(() => currentEsquives == 1));

            machine.AddTransition(trueesquivar, esquiveTrasero, new FuncPredicate(() => currentEsquives >= objectiveEsquives));  
            machine.AddTransition(esquiveTrasero, ataquep1, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(ataquep1, truePegarTutorial, new FuncPredicate(() => currentHits == 1));
            machine.AddTransition(truePegarTutorial, ataqueP2, new FuncPredicate(() =>currentHits >= objectiveHits));
            machine.AddTransition(ataqueP2, esquiPerfP1, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(esquiPerfP1, esquiPerfP2, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(ataquep1, truePegarTutorial, new FuncPredicate(() => currentEsqPerf == 1));
            machine.AddTransition(truePegarTutorial, ataqueP2, new FuncPredicate(() => currentEsqPerf >= objectiveEsqPerf));



            machine.AddTransition(congratulationState, end, new FuncPredicate(() => changeTutWait == true));




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

            tutorial.TutorialText.text = $"Cuando enfocas a un enemigo se mostrará una interfaz para saber sobre qué dirección se ataca o esquiva. Cuando un enemigo ataque se mostrarán en rojo las direcciones donde NO tienes que esquivar. Para esquivar presiona “espacio” o “BOTON ESQUIVE”.";
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
    public class EsquiveTrasero: BaseTutorialState
    {
        new betaTutorial tutorial;
        public EsquiveTrasero(betaTutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.waitTime(5);
            tutorial.changeTutWait = false;

            
            tutorial.TutorialText.text = $"Buenos reflejos campeón. Hay ciertos ataques que te vendrán del exterior. Si tienes a un enemigo fijado se te mostrará en la interfaz un símbolo “!”. " +
                $"Cuando aparezca presiona el botón de esquive y muévete hacia atrás. Si no tienes ningún enemigo fijado, con presionar el botón de esquive servirá.";
            
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
            tutorial.TutorialText.text = $"Ahora que sabes esquivar vamos a lo importante. Tienes 3 direcciones de ataque: centro, izquierda y derecha. Para atacar presione “click derecho” o “BOTON ATAQUE”. " +
                $"En caso de que quieras hacer un ataque hacia la izquierda o derecha muévase en esa dirección a la vez que atacas.";
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

            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveHits - tutorial.currentHits} ataques.";
            tutorial.enemy.subscribeToLifeChange(enemyHit);
            tutorial.enemy.GetComponent<enemigoTutorial>().setTutorialMode(1);


        }

        public void enemyHit(int currentLife)
        {

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
            tutorial.waitTime(15);
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = $"Los enemigos pueden bloquear, si atacas en la dirección  en la que bloquean el enemigo no sufren daño y en ocasiones pueden realizarar un contraataque.";

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
            tutorial.waitTime(15);
            tutorial.changeTutWait = false;
            tutorial.TutorialText.text = $"Se ve que sabes usar esos puños, pero ¿sabes cómo recuperar fuerzas? Para curarte hay dos maneras: tocando botiques que encontrarás por el camino o haciendo contrataque a los enemigos. " +
                $"Eso sí, dicho contraataque solo recuperará el daño recibido en el último ataque. Si poses menos corazones que daño recibido caerás en combate así que ten mucho cuidado.";

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
            tutorial.waitTime(15);
            tutorial.TutorialText.text = $"Cuando realizas un esquive en el momento justo podrás realizar un esquive perfecto, lo que ralentizará el tiempo y te permitirá hacer un contraataque que provoque el doble de daño.";

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

            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveEsquives - tutorial.currentEsquives} esquives perfectos y contrataca.";
            tutorial.player.subscribeToDodgeAttack(esquivePerf);


        }
        public void esquivePerf(HitDirections d)
        {

            tutorial.currentEsquives++;
            tutorial.TutorialText.text = $"Realiza {tutorial.objectiveEsquives - tutorial.currentEsquives} esquives perfectos y contrataca.";

        }
        public override void OnExit()
        {
            tutorial.player.unSubscribeToDodgeAttack(esquivePerf);

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
