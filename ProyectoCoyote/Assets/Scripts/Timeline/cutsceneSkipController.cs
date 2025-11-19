using Services;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class cutsceneSkipController : MonoBehaviour
{
    [SerializeField] private CutsceneData currentData;
    [SerializeField] float currrentSkipPressTime;
    [SerializeField] float SkipPressTime;
    [SerializeField] Image skipCupstecenesBar;
    [SerializeField] TMP_Text textoInstruccionSaltar;
    [SerializeField] estadoMensajeSkip textoSaltarCinematicaEstado = 0;
    [SerializeField] float alphaTextoCambiar = 0;
    [SerializeField] bool cutscenePlaying = false;

    GameInput gameInput;

    IcutsceneManager cutsceneManager;

    private void Start()
    {
        cutscenePlaying = false;
        cutsceneManager = ServiceLocator.Instance.Get<IcutsceneManager>();
        gameInput = FindAnyObjectByType<GameInput>();

    }
    void Update()
    {
        if (cutscenePlaying)
        {
            if (!gameInput.SkipPressed)
            {
                currrentSkipPressTime -= Time.deltaTime;
                if (currrentSkipPressTime < 0)
                {
                    currrentSkipPressTime = 0;
                }
            }
            skipCupstecenesBar.fillAmount = currrentSkipPressTime / SkipPressTime;
            if (currentData != null && currentData.canBeSkipped == true && !cutsceneManager.isSkipingCutscene())
            {
                if (gameInput.SkipPressed)
                {
                    currrentSkipPressTime += Time.deltaTime;
                    if (currrentSkipPressTime >= SkipPressTime)
                    {
                        cutsceneManager.skipCutscene();
                    }
                }
                if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    textoSaltarCinematicaEstado = estadoMensajeSkip.turningOn;
                }
                else
                {
                    StartCoroutine(waitTurnOFfSkipAdvice());
                }
            }
            //0 = 
            switch (textoSaltarCinematicaEstado)
            {
                case estadoMensajeSkip.finished:

                    break;
                case estadoMensajeSkip.turningOn:
                    alphaTextoCambiar = textoInstruccionSaltar.color.a + (Time.deltaTime);
                    if (alphaTextoCambiar > 1)
                    {
                        alphaTextoCambiar = 1;
                        textoSaltarCinematicaEstado = 0;

                    }
                    textoInstruccionSaltar.color = new Color(1, 1, 1, alphaTextoCambiar);

                    break;

                case estadoMensajeSkip.turningOff:

                    alphaTextoCambiar = textoInstruccionSaltar.color.a - (Time.deltaTime);
                    if (alphaTextoCambiar < 0)
                    {
                        alphaTextoCambiar = 0;
                        textoSaltarCinematicaEstado = 0;
                    }
                    textoInstruccionSaltar.color = new Color(1, 1, 1, alphaTextoCambiar);

                    break;
            }
        }

    }
 
    IEnumerator waitTurnOFfSkipAdvice()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f / 10f);
            if (textoInstruccionSaltar.color.a == 0)
            {
                yield break;
            }
        }
        textoSaltarCinematicaEstado = estadoMensajeSkip.turningOff;
    }

    public void changeGameState(object sender,stateData statedata)
    {
        if(statedata.currentState== GameState.Cutscene)
        {
            cutscenePlaying = true;
            textoSaltarCinematicaEstado = estadoMensajeSkip.finished;
            textoInstruccionSaltar.color = new Color(1, 1, 1, 0);

        }
        if (statedata.oldState == GameState.Cutscene)
        {
            cutscenePlaying = false;
            textoSaltarCinematicaEstado = estadoMensajeSkip.finished;
            textoInstruccionSaltar.color = new Color(1, 1, 1, 0);
            textoInstruccionSaltar.gameObject.SetActive(false);
        }
    }

    internal void startCutscene(CutsceneData data)
    {
        currrentSkipPressTime = 0;
        currentData = data;
        cutscenePlaying = true;
        textoSaltarCinematicaEstado = estadoMensajeSkip.turningOff;
        textoInstruccionSaltar.color = new Color(1, 1, 1, 1);
    }

    internal void endCutscene()
    {
        cutscenePlaying = false;
        textoSaltarCinematicaEstado = estadoMensajeSkip.finished;
        textoInstruccionSaltar.color = new Color(1, 1, 1, 0);
    }

    enum estadoMensajeSkip
    {
        finished,
        turningOn,
        turningOff
    }

}