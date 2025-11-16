using Services;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timelineDirector : MonoBehaviour, IcutsceneManager
{
    Action endCutsceneAction;
    private CutsceneData currentData;
    PlayableDirector director;
    public bool SkipingCutscene;
    public bool cutscenPlaying;
    CanvasGroup canvasgroup;

    cutsceneSkipController cutsceneSkip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        canvasgroup = GetComponentInChildren<CanvasGroup>();
        cutsceneSkip = GetComponentInChildren<cutsceneSkipController>();
    }

    // Update is called once per frame


    public void startCutscene(PlayableAsset timeline, Action endAction, CutsceneData data)
    {

        ServiceLocator.Instance.Get<IGameStateManager>().startCutscene();
        currentData = data;
        director.playableAsset = timeline;
        endCutsceneAction = endAction;
        if (/*!settingManager.Instance.skipCutscenes */true || !currentData.canBeSkipped)
        {
            canvasgroup.alpha = 1;
            SkipingCutscene = false;
            director.time = 0;
            cutscenPlaying = true;
            director.Play();
            cutsceneSkip.startCutscene(data);

        }
        else
        {
            cutscenPlaying = false;
            SkipingCutscene = true;
            print("SALTANDO CINEMATICA");
            endCutscene();
            //if (!currentData.isEndLevel)
            //{

            //    director.RebuildGraph(); // the graph must be created before getting the playable graph
            //    director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
            //    director.Play();
            //}
            //else
            //{
            //    endAnimation();
            //}
        }
    }
    public void endCutscene()
    {
        if (currentData.objectsToTurnOff != null && currentData.objectsToTurnOff.Length > 0)
        {
            foreach (var item in currentData.objectsToTurnOff)
            {
                item.SetActive(false);
            }
        }
        if (currentData.objectsToTurnOn != null && currentData.objectsToTurnOn.Length > 0)
        {
            foreach (var item in currentData.objectsToTurnOn)
            {
                item.SetActive(true);
            }
        }
        canvasgroup.alpha = 0;
        cutsceneSkip.endCutscene();
        cutscenPlaying = false;
        SkipingCutscene = false;
        print("endcutscene");
        print(endCutsceneAction.ToString());
        endCutsceneAction.Invoke();
    }
    public void skipCutscene()
    {
        print("SKIP");
        SkipingCutscene = true;

        //if (currentData.objectsToTurnOff != null && currentData.objectsToTurnOff.Length > 0)
        //{
        //    foreach (var item in currentData.objectsToTurnOff)
        //    {
        //        item.SetActive(false);
        //    }
        //if (!currentData.isEndLevel)
        //{
        //    director.RebuildGraph(); // the graph must be created before getting the playable graph
        //    director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
        //    director.Play();
        //}
        //else
        //{
        endCutscene();
        //}
    }

    public void PlaySound(string sound)
    {

    }

    public bool isSkipingCutscene() => SkipingCutscene;

    public void Instantiate()
    {
        currentData = null;
    }
}
public interface IcutsceneManager : IService
{
    public void startCutscene(PlayableAsset timeline, Action endAction, CutsceneData data);
    public void skipCutscene();
    public void PlaySound(string sound);
    public bool isSkipingCutscene();
}

[System.Serializable]   
public class CutsceneData
{
  public  PlayableAsset cutscene;
    public GameObject[] objectsToTurnOff;
    public GameObject[] objectsToTurnOn;
    public bool canBeSkipped;
    public bool isEndLevel;
    public CutsceneData(PlayableAsset cutscene,bool _canSkipped, bool isEndLevel, GameObject[] _objectsOff = null, GameObject[] _objectsOn = null)
    {
        this.cutscene = cutscene;
        objectsToTurnOff = _objectsOff;
        objectsToTurnOn = _objectsOn;
        canBeSkipped = _canSkipped;
        this.isEndLevel = isEndLevel;
    }
}
public class CutsceneCaller : MonoBehaviour
{
  [SerializeField]  CutsceneData cutsceneData;
  [SerializeField]  StoryAction onFinishedAction;
}

[System.Serializable]
public class StoryAction
{
    public StoryActionType actionType;
    public bool playOnRestart =true;
    bool played = false;
    public string nameKey;
    public CutsceneData cutsceneData;

    Action postStoryAction;
    public void Execute(Action postaction)
    {
        postStoryAction = postaction;
        switch (actionType)
        {
            case StoryActionType.changeScene:
                ServiceLocator.Instance.Get<ILevelManager>().loadEscene(nameKey);
                    break;
            case StoryActionType.startCutscene:
                if (!playOnRestart && played)
                {
                    endStoryAction();
                    return;
                }
                played=true;
            

                ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, endStoryAction, cutsceneData);
                break;
            case StoryActionType.playDialog:
                if (!playOnRestart && played)
                {
                    endStoryAction();
                    return;
                }
                played = true;
                Debug.Log($"PimpumDialogo{nameKey}");
                endStoryAction();
                break;
            case StoryActionType.continueNonCombatGameplay:
                ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay();
endStoryAction();
                break;
            case StoryActionType.nothing:
                endStoryAction();
                break;
        }
    }
  
    public void endStoryAction()
    {
        postStoryAction?.Invoke();
    }
}
public enum StoryActionType
{
    changeScene,
    startCutscene,
    playDialog,
    continueNonCombatGameplay,
    nothing

}
