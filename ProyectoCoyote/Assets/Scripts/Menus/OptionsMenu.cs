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

    public void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void Update()
    {
        generalValue = sliderGeneral.value;
        sfxValue = sliderSFX.value;
        musicValue = sliderMusica.value;
    }

    public void SetGeneralVolume() 
    { 
        audioManager.SetMusicVolume(generalValue);
        audioManager.SetSFXVolume(generalValue);
    }
    public void SetSFXVolume()
    {
        audioManager.SetSFXVolume(sfxValue);
    }
    public void SetMusicVolume()
    {
        audioManager.SetMusicVolume(musicValue);
    }
}
