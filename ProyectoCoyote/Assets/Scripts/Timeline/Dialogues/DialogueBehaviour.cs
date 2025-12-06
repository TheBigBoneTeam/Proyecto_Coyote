using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueBehaviour : PlayableBehaviour
{
    public string dialogText;
    //public string[] dialogTexts;
    public float leaveTime;
    public int startChars;
    public int[] startCharsList;
    //  public Language languageForEditor;
    int maxVisible;
    public float width;
    bool first = true;
    // public int personaje;
    public VocesDialogo personaje;
    // public int lang;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // lang = 0;
        //if (settingManager.Instance != null)
        //{
        //    Debug.Log("findinstance");
        //    lang = (int)settingManager.Instance.getLanguage();
        //}
        //else
        //{

        //    lang = (int)languageForEditor;
        //    Debug.Log("findeditor" + lang+" " + startCharsList.Length);
        //}
        TMP_Text text = playerData as TMP_Text;
        text.ForceMeshUpdate();
        //if (lang < startCharsList.Length)
        //{
        //    text.text = dialogTexts[lang];
        //    Debug.Log(dialogTexts[lang]);
        //}
        //else
        //{
        //   // text.text = dialogText;
        //    Debug.Log("Fac");
        //}
        text.text = dialogText;

        //if (first)
        //{
        //    Debug.Log("chunda");
        //    first = false;
        //    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 2);
        //}
        if (text != null)
        {
            int startCharacters;
            startCharacters = startChars;

            //if (lang < startCharsList.Length) {
            //     startCharacters = startCharsList[lang];
            //}
            //else
            //{
            //     startCharacters = startChars;
            //}
            text.maxVisibleCharacters = startCharacters + Mathf.CeilToInt((text.textInfo.characterCount - startCharacters) * Mathf.Clamp(System.Convert.ToSingle(playable.GetTime() / (Mathf.Max(System.Convert.ToSingle(playable.GetDuration() - leaveTime), 0))), 0, 1));
            if (maxVisible != text.maxVisibleCharacters)
            {
                maxVisible = text.maxVisibleCharacters;
                if (maxVisible == startCharacters + 1)
                {
                    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 2);
                    Debug.Log("PRIMERO");
                }
                if (text.textInfo.characterCount > 0)
                {
                    //Debug.Log(maxVisible + "Bip");
                    if (ServiceLocator.Instance && !ServiceLocator.Instance.Get<IcutsceneManager>().isSkipingCutscene())
                    {
                        //int numeroAleatorio = Random.Range(0, 3);

                        //if (numeroAleatorio == 0)
                        //{
                        //    if (AudioManager.Instance != null)
                        //    {
                        //        if (personaje == 0) // es la prota
                        //        {
                        //            AudioManager.Instance.PlayDialogue("Cinematicas - Voz Coyote", 0.2f);
                        //        }
                        //        else if (personaje == 1) // es el cura
                        //        {
                        //            AudioManager.Instance.PlayDialogue("Cinematicas - Voz Perro", 0.2f);
                        //        }
                        //        else // otros
                        //        {
                        //            AudioManager.Instance.PlayDialogue("Cinematicas - Voz Denebola", 0.2f);
                        //        }
                        //    }
                        //}

                        if (AudioManager.Instance != null)
                        {
                            if (Random.value > 0.5f)
                                return;

                            switch (personaje)
                            {
                                case VocesDialogo.Silencio:
                                    break;

                                case VocesDialogo.Coyote:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Coyote", 0.2f);
                                    break;

                                case VocesDialogo.Perro:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Perro", 0.1f);
                                    break;

                                case VocesDialogo.Denebola:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Denebola", 0.2f);
                                    break;

                                case VocesDialogo.Lince:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Lince", 0.2f);
                                    break;

                                case VocesDialogo.Cultista:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Cultista", 0.2f);
                                    break;

                                case VocesDialogo.Carlos:
                                    AudioManager.Instance.PlayDialogue("Cinematicas - Voz Carlos", 0.2f);
                                    break;
                            }
                        }
                    }


                }
            }
        }
        else
        {
            text.text = "";
        }
        //text.maxVisibleCharacters = text.textInfo.characterCount / playable.GetDuration



        //int QuienHabla()
        //{
        //    // devolver 0 si es la prota, 1 si es el cura y 2 si son los malos
        //    return 1;
        //}

    }
}

public enum VocesDialogo
{
    Perro,
    Coyote,
    Denebola,
    Lince,
    Carlos,
    Cultista,
    Silencio
}
