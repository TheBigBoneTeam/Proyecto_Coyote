using UnityEngine;

namespace tutorial
{
    public class firstTutorial: Tutorial
    {

        EnemyLockOn lockon;
      public  int currentEsquives;
        [SerializeField] public int objectiveEsquives;
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
            congratulationState congratulationState = new congratulationState(this);
            machine.AddTransition(start, controles, new FuncPredicate(() => true));
            machine.AddTransition(controles, lockear, new FuncPredicate(()=>changeTutWait == true));
            machine.AddTransition(lockear, esquivar, new FuncPredicate(() =>lockon.currentTarget == enemy.transform));
            machine.AddTransition(esquivar, congratulationState, new FuncPredicate(() => currentEsquives >= objectiveEsquives));
            machine.AddTransition(congratulationState, end, new FuncPredicate(() => changeTutWait == true));




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

            tutorial.TutorialText.text = $"Con un enemigo marcado puedes esquivar con espacio. Los circulos marcaran por donde te estan atacando (y por donde tendrás que esquivar).";
            tutorial.player.subscribeToDodgeAttack(esquive);


        }
        public void esquive()
        {
            tutorial.currentEsquives++;
            tutorial.TutorialText.text = $"Cuando tengas a alguien marcado puedes esquivar con el spacio. Los circulos marcaran por donde te estan atacando (y por donde tendrás que esquivar). Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques para ganar.";

        }
        public override void OnExit()
        {
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

            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques para ganar.";
            tutorial.player.subscribeToDodgeAttack(esquive);


        }
        public void esquive()
        {
            tutorial.currentEsquives++;
            tutorial.TutorialText.text = $"Esquiva {tutorial.objectiveEsquives - tutorial.currentEsquives} ataques para ganar.";

        }
    }
}
