using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Services;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] protected Transform _player;
    [SerializeField] protected List<AudioProducer> normalSounds;
    [Tooltip("Max 5 canales de música")] [SerializeField] protected AudioProducer[] musicSounds = new AudioProducer[5];
    protected List<AudioSource> pausedSounds = new List<AudioSource>();
    Player player;

    IGameStateManager gameStateManager;

    private void Awake()
    {
        if (Instance != null && Instance !=this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    protected void Start()
    {
        CheckScene();
        if(ServiceLocator.Instance != null)
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();

        if (gameStateManager != null)
        {
            gameStateManager.subscribeToStateChange(OnGameStateChange);
            // gameStateManager.subscribeToRestart(OnRestart);
            Debug.Log("[AudioManager] Suscrito correctamente a GameStateManager");
        }
        else
        {
            Debug.LogWarning("[PlayerMovement] No se encontró GameStateManager al iniciar");
        }

        for (int i=0; i<musicSounds.Length; i++)
        {
            // musicSounds[i] = null;
        }

        // Encuentra un jugador y se suscribe al metodo de bloqueo
        player = FindAnyObjectByType<Player>();
        if(player != null)
        player.subscribeToDodgeAttack(DodgeAttack);
    }
    public void SceneChange()
    {

    }

    private void DodgeAttack(HitDirections arg0)
    {
        AudioManager.Instance.PlaySimpleSound("SFX - Block Attack", false, Vector2.zero, true, false);
        // AudioManager.Instance.PlaySimpleSound("SFX - Dash", false, Vector2.zero, true, false);
    }

    private void OnGameStateChange(object sender, stateData stateInfo)
    {
        if (stateInfo.currentState == GameState.Combat)
        {
            HandleCombatState();
        }

        if (stateInfo.currentState == GameState.NonCombat)
        {
            HandleNonCombatState();
        }
    }

    private void HandleCombatState()
    {
        Debug.Log("[AudioManager] → Entrando en COMBATE");

        // Si ambas pistas están cargadas, hacemos crossfade
        if (musicSounds[0] != null && musicSounds[1] != null)
        {
            StartCoroutine(CrossFadeVolumes(musicSounds[0], musicSounds[1], 2f));
        }
        else
        {
            Debug.LogWarning("[AudioManager] No se pudieron encontrar ambas pistas para el crossfade (Base/Pelea).");
        }
    }

    private void HandleNonCombatState()
    {
        Debug.Log("[AudioManager] → Volviendo a NO COMBATE");

        if (musicSounds[0] != null && musicSounds[1] != null)
        {
            StartCoroutine(CrossFadeVolumes(musicSounds[1], musicSounds[0], 2f));
        }
    }

    private IEnumerator CrossFadeVolumes(AudioProducer from, AudioProducer to, float duration)
    {
        float timer = 0f;
        float startVolFrom = from.audioSource.volume;
        float startVolTo = to.audioSource.volume;
        float targetVolTo = to.sound.volume;

        // Ambas deben estar reproduciendo para mantener sincronía
        if (!from.audioSource.isPlaying) from.audioSource.Play();
        if (!to.audioSource.isPlaying) to.audioSource.Play();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            from.audioSource.volume = Mathf.Lerp(startVolFrom, 0f, t);
            to.audioSource.volume = Mathf.Lerp(startVolTo, targetVolTo, t);

            yield return null;
        }

        from.audioSource.volume = 0f;
        to.audioSource.volume = targetVolTo;
    }

    // Musica por escena
    protected virtual void CheckScene()
    {
        Scene _scene = SceneManager.GetActiveScene();
        
        switch(_scene.name)
        {
            case "MainMenu":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Menu", true, Vector2.zero, true, true);
                break;

            /*case "cinematicaIntro":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Intro", false, Vector2.zero, true, true);
                break;
            */

            case "tutorialBeta":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Dummy", true, Vector2.zero, true, true);
                // Debug.LogWarning("Musica reproducida exitosamente");
                break;

            case "EscenaHeavy":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(0f, "OST Cañon - Pelea", true, Vector2.zero, true, true, 1);
                if (musicSounds[1] != null)
                    musicSounds[1].audioSource.volume = 0f;

                break;

            case "EscenaHeavy_pruebaSonidos":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(0f, "OST Cañon - Pelea", true, Vector2.zero, true, true, 1);
                if (musicSounds[1] != null)
                    musicSounds[1].audioSource.volume = 0f;

                break;

            case "Nivel1":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Pueblo - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(0f, "OST Pueblo - Pelea", true, Vector2.zero, true, true, 1);
                if (musicSounds[1] != null)
                    musicSounds[1].audioSource.volume = 0f;

                break;

            case "Nivel2":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(0f, "OST Cañon - Pelea", true, Vector2.zero, true, true, 1);
                if (musicSounds[1] != null)
                    musicSounds[1].audioSource.volume = 0f;

                break;
            case "Nivel2.1":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(0f, "OST Cañon - Pelea", true, Vector2.zero, true, true, 1);
                if (musicSounds[1] != null)
                    musicSounds[1].audioSource.volume = 0f;

                break;

            case "TesteoCInematicas":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Base", true, Vector2.zero, true, true,0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Cañon - Pelea", true, Vector2.zero, true, true,1);

                break;

            case "Nivel3":
                //AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Boss Final - Loopeo", true, Vector2.zero, true, true);

                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Oasis - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Oasis - Pelea", true, Vector2.zero, true, true, 1);

                break;
            case "Nivel3.1":
                //AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Boss Final - Loopeo", true, Vector2.zero, true, true);

                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Oasis - Base", true, Vector2.zero, true, true, 0);
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Oasis - Pelea", true, Vector2.zero, true, true, 1);

                break;

            case "GameplayBeta_PruebaCombate":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Boss Final", true, Vector2.zero, true, true);
                break;

            case "Credits":
                AudioManager.Instance.PlaySimpleSoundFadeIn(2f, "OST Creditos", true, Vector2.zero, true, true);
                break;

            default:
                break;
        }
    }

    // CAMBIAR DINAMICAMENTE CANCION (transicion de 2s)
    // AudioManager.Instance.ChangeMusicAt(0, "BattleTheme", 2f, 2f);

    #region Sonido 2D
    // POR NOMBRE
    public virtual void PlaySimpleSound(string soundName, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1)
    {
        if(onlyOne && SearchSource(soundName))
        {
            return;
        }
        
        Sound Sound = SoundGallery.Instance.FindSound(soundName);

        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }

        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);

            if (!isMusic)
            {
                normalSounds.Add(ap);
            }
            else
            {
                Debug.Log("Es_ " + isMusic);
                AddToMusicArray(ap,musicAt);
            }

            float pitch=Sound.pitch;

            if (Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0)
            {
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }

            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.Play();
            //_audioSources.Add(name, a);
        }
    }

    // ALEATORIO
    public virtual void PlayCollectedSound(string collectionName, bool even, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1){
        Sound Sound = SoundGallery.Instance.FindSoundInCollectionRandom(collectionName,even);
        if (onlyOne && SearchSource(Sound.name))
        {
            return;
        }

        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }
        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);
            if(!isMusic){
                normalSounds.Add(ap);
            }else{
                AddToMusicArray(ap,musicAt);
            }
            float pitch=Sound.pitch;
            if(Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0){
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }
            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.Play();
        }
    }

    // VOCES DE DIALOGOS
    public void PlayDialogue(string soundName, float pitchVariation = 0.1f)
    {
        Sound sound = SoundGallery.Instance.FindSound(soundName);
        if (sound == null)
        {
            Debug.LogError($"El sonido '{soundName}' no se encontró en SoundGallery.");
            return;
        }

        AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
        ap.SetAudioProducer("", sound);

        pitchVariation = Mathf.Clamp(pitchVariation, 0f, 1f);
        float minPitch = 1f - pitchVariation;
        float maxPitch = 1f + pitchVariation;
        float randomPitch = Random.Range(minPitch, maxPitch);

        ap.StartAudio(false, false, Vector2.zero, randomPitch);
        normalSounds.Add(ap);
        ap.Play();
    }

    #endregion

    #region Reproduce Sonido 3D
    // POR NOMBRE
    public virtual void Play3DSound(string soundName, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1, AudioRolloffMode mode = AudioRolloffMode.Linear)
    {
        if (onlyOne && SearchSource(soundName))
        {
            return;
        }

        Sound Sound = SoundGallery.Instance.FindSound(soundName);
        
        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }

        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);
            
            if (!isMusic)
            {
                normalSounds.Add(ap);
            }
            else
            {
                AddToMusicArray(ap,musicAt);
            }

            float pitch=Sound.pitch;

            if (Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0)
            {
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }

            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.audioSource.rolloffMode = mode;
            ap.Play();
        }
    }

    // ALEATORIO
    public virtual void PlayCollected3DSound(string collectionName, bool even, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1, AudioRolloffMode mode = AudioRolloffMode.Linear){
        Sound Sound = SoundGallery.Instance.FindSoundInCollectionRandom(collectionName,even);

        if (onlyOne && SearchSource(Sound.name))
        {
            return;
        }

        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }

        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);

            if (!isMusic)
            {
                normalSounds.Add(ap);
            }
            else
            {
                AddToMusicArray(ap,musicAt);
            }

            float pitch=Sound.pitch;

            if (Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0)
            {
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }

            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.audioSource.rolloffMode = mode;
            ap.Play();
        }
    }
    #endregion

    #region FADE IN / FADE OUT
    public virtual void PlaySimpleSoundFadeIn(float fadeTime, string soundName, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1){
        if (onlyOne && SearchSource(soundName))
        {
            return;
        }

        Sound Sound = SoundGallery.Instance.FindSound(soundName);

        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }

        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);
            
            if (!isMusic)
            {
                normalSounds.Add(ap);
            }
            else
            {
                AddToMusicArray(ap,musicAt);
            }

            float pitch=Sound.pitch;

            if (Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0)
            {
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }

            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.FadeIn(fadeTime,Sound.volume);
            //_audioSources.Add(name, a);
        }
    }

    public virtual void PlayCollectedSoundFadeIn(float fadeTime, string collectionName, bool even, bool loop, Vector2 pos, bool onlyOne, bool isMusic, int musicAt=-1, string tag="", float minPitch=-1, float maxPitch=-1){
        Sound Sound = SoundGallery.Instance.FindSoundInCollectionRandom(collectionName,even);

        if(onlyOne && SearchSource(Sound.name)){return;} //!

        if (_player != null)
        {
            if (Vector2.Distance(pos, _player.position) > Sound.maxSoundDistance || Sound == null) { return; }
        }

        if (Sound != null)
        {
            AudioProducer ap = this.gameObject.AddComponent<AudioProducer>();
            ap.SetAudioProducer(tag,Sound);
            
            if (!isMusic)
            {
                normalSounds.Add(ap);
            }
            else
            {
                AddToMusicArray(ap,musicAt);
            }

            float pitch=Sound.pitch;

            if (Sound.pitchVariation.x>=0 && Sound.pitchVariation.y>=0)
            {
                pitch = Random.Range(Sound.pitchVariation.x,Sound.pitchVariation.y);
            }

            ap.StartAudio(loop,onlyOne,pos,pitch);
            ap.FadeIn(fadeTime,Sound.volume);
        }
    }

    //Cambio de sonido/musica
    public virtual void ChangeMusicAt(int i, string newMusicName, float fadeOutDuration=1f, float fadeInDuration=1f)
    {
        AudioProducer ap = musicSounds[i];
        Debug.LogWarning(ap==null);
        if(ap!=null) StartCoroutine(FadeOutFadeIn(ap, SoundGallery.Instance.FindSound(newMusicName),fadeOutDuration,fadeInDuration));
    }

    public virtual void ChangeMusicWithTag(string tag, string newMusicName, float fadeOutDuration=1f, float fadeInDuration=1f)
    {
        AudioProducer[] aps = musicSounds.Where(item => item.customTag == tag).ToArray();

        foreach(AudioProducer ap in aps)
        {
            if (ap!=null) StartCoroutine(FadeOutFadeIn(ap, SoundGallery.Instance.FindSound(newMusicName),fadeOutDuration,fadeInDuration));
        }
    }

    IEnumerator FadeOutFadeIn(AudioProducer currentAP,Sound newSound, float fadeOutDuration=1f, float fadeInDuration=1f, float waitTime=0f)
    {
        currentAP.FadeOut (fadeOutDuration,newSound==null);
        yield return new WaitForSeconds(fadeOutDuration + waitTime);

        if (newSound!=null)
        {
            currentAP.SetAudioProducer(currentAP.tag,newSound);
            currentAP.FadeIn(fadeInDuration,newSound.volume);
            yield return new WaitForSeconds(fadeInDuration);
        }
    }
    #endregion

    #region Parar Sonido o Musica
    public virtual void RemoveAudioProducer(AudioProducer a)
    {
        if (normalSounds.Contains(a)) normalSounds.Remove(a);

        if (musicSounds.Contains(a))
        {
            for (int i=0; i<musicSounds.Length; i++)
            {
                if (musicSounds[i]==a)
                {
                    musicSounds[i] = null;
                }
            }
        }
    }

    public virtual void PauseAllSounds(bool p)
    {
        foreach(AudioProducer ap in normalSounds)
        {
            ap.Pause(p);
        }
    }
    public virtual void PauseAllmusic(bool p)
    {
        foreach(AudioProducer ap in musicSounds)
        {
            ap.Pause(p);
        }
    }

    public virtual void PauseAllSoundsWithTag(string tag, bool p)
    {
        foreach (AudioProducer ap in normalSounds)
        {
            if(ap.customTag==tag)
            ap.Pause(p);
        }
    }

    public virtual void PauseMusicAt(int i, bool p)
    {
        musicSounds[i].Pause(p);
    }

    public virtual void PauseMusicWithTag(string tag, bool p)
    {
        foreach (AudioProducer ap in musicSounds)
        {
            if (ap.customTag==tag) ap.Pause(p);
        }
    }

    public virtual void StopAllSoundsWithTag(string tag)
    {
        var copy = new List<AudioProducer>(normalSounds);

        foreach (AudioProducer ap in copy)
        {
            if (ap!= null && ap.customTag==tag) ap.Stop();
        }
    }

    public virtual void StopMusicAt(int i)
    {
        musicSounds[i].Stop();
    }
    public virtual void StopMusicWithTag(string tag)
    {
        foreach (AudioProducer ap in musicSounds)
        {
            if (ap.customTag==tag) ap.Stop();
        }
    }
    #endregion

    #region Buscar Sonido
    protected virtual bool SearchSource(string soundName)
    {
        //Debug.Log(soundName);
        if (soundName==string.Empty) throw new System.Exception ("No existe el sonido");

        foreach (AudioProducer ap in normalSounds)
        {
            if (ap!=null && ap.audioSource.clip.name == soundName)
            {
                return true;
            }
        }

        foreach (AudioProducer ap in musicSounds)
        {
            if (ap!=null && ap.audioSource.clip.name == soundName)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    protected virtual void Update()
    {
        
    }


    // Otros
    public void AddToMusicArray(AudioProducer ap, int at)
    {
        Debug.LogWarning(at);
        if (at>=0)
        {
            musicSounds[at]=ap; return;
        }

        int n=0;

        for (int i=0; i<musicSounds.Length; i++)
        {
            if(musicSounds[i]!=null) n++;
        }

        if (n+1 < musicSounds.Length)
        {
            musicSounds[n+1] = ap;
        }
        else
        {
            musicSounds[musicSounds.Length-1]=ap;
        }
    }

    // Posicion del jugador para sonidos 3D
    public void SetPlayer(string name)
    {

        if(GameObject.Find(name)!=null)
        {
            _player=GameObject.Find(name).transform;
        }
        else
        {
            _player=this.gameObject.transform;
        }
    }

    #region Control de Volumen

    public virtual void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume); 

        foreach (AudioProducer ap in musicSounds)
        {
            if (ap != null && ap.audioSource != null)
            {
                ap.audioSource.volume = volume * ap.sound.volume; 
            }
        }
    }

    public virtual void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume); 

        foreach (AudioProducer ap in normalSounds)
        {
            if (ap != null && ap.audioSource != null)
            {
                ap.audioSource.volume = volume * ap.sound.volume; 
            }
        }
    }
    #endregion
}
