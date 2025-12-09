using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] Slider sliderGeneral;
    [SerializeField] Slider sliderSFX;
    [SerializeField] Slider sliderMusica;

    private float generalValue;
    private float sfxValue;
    private float musicValue;

    private SafeConfig config;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        config = FindAnyObjectByType<SafeConfig>();

        // Cargar valores guardados
        generalValue = PlayerPrefs.GetFloat("GeneralVolume", 1f);
        sfxValue = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicValue = PlayerPrefs.GetFloat("MusicVolume", 1f);

        sliderGeneral.value = generalValue;
        sliderSFX.value = sfxValue;
        sliderMusica.value = musicValue;

        // Aplicar al AudioManager
        SetGeneralVolume();
        SetSFXVolume();
        SetMusicVolume();
    }

    public void SetGeneralVolume()
    {
        generalValue = sliderGeneral.value;
        audioManager.SetGeneralVolume(generalValue);
        config.generalValue = generalValue;
        PlayerPrefs.SetFloat("GeneralVolume", generalValue);
    }

    public void SetSFXVolume()
    {
        sfxValue = sliderSFX.value;
        audioManager.SetSFXVolume(sfxValue);
        config.sfxValue = sfxValue;
        PlayerPrefs.SetFloat("SFXVolume", sfxValue);
    }

    public void SetMusicVolume()
    {
        musicValue = sliderMusica.value;
        audioManager.SetMusicVolume(musicValue);
        config.musicValue = musicValue;
        PlayerPrefs.SetFloat("MusicVolume", musicValue);
    }
}
