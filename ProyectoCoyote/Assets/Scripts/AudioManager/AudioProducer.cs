using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProducer : MonoBehaviour
{
    [Header("Manager Principal")] 
    public AudioManager audioManager;

    [Header("Configuración")]
    public string customTag;
    public Sound sound;
    public AudioSource audioSource;
    [SerializeField] bool loopingSound;
    [Tooltip("Si está activo, mientras su sonido siga sonando, no se puede crear otro igual")]
    public bool justThisOne;
    bool paused;
    void Awake(){

    }
    void Start()
    {
        
    }

    // Asigna el clip y lo configura
    public void SetAudioProducer(string tag, Sound _sound){
        if(audioSource==null)
        audioSource = this.gameObject.AddComponent<AudioSource>();

        if(tag!="")
        customTag=tag;
        else
        customTag=_sound.name;
        
        sound=_sound;
    }

    public void StartAudio(bool loop, bool onlyOne, Vector2 pos, float pitch = -1)
    {
        audioSource.maxDistance = sound.maxSoundDistance;
        audioSource.transform.position = pos;
        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        
        if (pitch >= 0)
        {
            audioSource.pitch = sound.pitch;
        }
        else
        {
            audioSource.pitch = pitch;
        }

        audioSource.loop = loop;
    }

    public void StartAudio3D(bool loop, bool onlyOne, Vector3 pos, float pitch = -1)
    {
        audioSource.maxDistance = sound.maxSoundDistance;
        audioSource.transform.position = pos; // ahora 3D real
        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        audioSource.loop = loop;

        // 100% 3D
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;

        if (pitch >= 0)
            audioSource.pitch = pitch;
        else
            audioSource.pitch = sound.pitch;
    }


    // Prepara el audio con volumen, tono y bucle (true o false)
    //public void StartAudio(bool loop, bool onlyOne, Vector2 pos, float pitch=-1){
    //    audioSource.maxDistance=sound.maxSoundDistance;
    //    audioSource.transform.position=pos;
    //    audioSource.clip = sound.audioClip;
    //    audioSource.volume = sound.volume;

    //    if (pitch >= 0f)
    //    {
    //        audioSource.pitch = pitch;
    //    }
    //    else
    //    {
    //        audioSource.pitch = sound.pitch;
    //    }

    //    audioSource.loop = loop;
    //}

    #region Reproduccion
    public void Play(){
        audioSource.Play();
    }

    public void Pause(bool b){
        paused=b;
        if(paused)
        audioSource.Pause();
        else
        audioSource.UnPause();
    }

    public void Stop(){
        AudioManager.Instance.RemoveAudioProducer(this);
        audioSource.Stop();
        Destroy(audioSource);
        Destroy(this);
    }

    public virtual void FadeOut(float fadeOutDuration, bool destroy = false)
    {
        StartCoroutine(IEFadeOut(fadeOutDuration, destroy));
    }

    protected virtual IEnumerator IEFadeOut(float fadeOutDuration, bool destroy)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + fadeOutDuration)
        {
            if (audioSource == null) { yield break; }
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeOutDuration;
            float newVolume = Mathf.Lerp(startVolume, 0f, normalizedTime);

            audioSource.volume = newVolume;
            yield return null;
        }

        Debug.LogWarning(destroy);

        if (destroy)
        {
            Stop();
        }
    }

    // TRANSICIONES
    public virtual void FadeIn(float fadeInDuration, float desiredVolume)
    {
        StartCoroutine(IEFadeIn(fadeInDuration, desiredVolume));
    }

    protected virtual IEnumerator IEFadeIn(float fadeInDuration, float desiredVolume)
    {
        float startVolume = 0f;
        audioSource.Play();
        float startTime = Time.time;

        while (Time.time < startTime + fadeInDuration)
        {
            if (audioSource == null) { yield break; }
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / fadeInDuration;
            float newVolume = Mathf.Lerp(startVolume, desiredVolume, normalizedTime);

            audioSource.volume = newVolume;
            yield return null;
        }

        audioSource.volume = desiredVolume;
    }
    #endregion

    void Update()
    {
        if (!audioSource.isPlaying && !audioSource.loop && !paused)
        {
            Stop();
        }
    }
}
