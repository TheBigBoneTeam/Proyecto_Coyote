using Services;
using UnityEngine;

public class cutsceneCaller : MonoBehaviour
{
    public CutsceneData cutsceneData;
   public StoryAction storyAction;
   public bool playCutscene;
    [SerializeField] bool hasPlayed;
    [SerializeField] bool playOnRestart;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && !hasPlayed)
        {
            hasPlayed = true;
            if (playCutscene)
            {
                ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () =>
                {
                    storyAction.Execute(() =>
                    {
                        ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay();
                    });
                }, cutsceneData);
            }
            else
            {
                storyAction.Execute(() =>
                {
                    ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay();
                });
            }

        }
    }

    private void Start()
    {
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);
    }

    private void restart()
    {
        if (playOnRestart)
        {
            hasPlayed = false;
        }
    }
}