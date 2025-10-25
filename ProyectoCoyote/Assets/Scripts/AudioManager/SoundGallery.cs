using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundGallery : MonoBehaviour
{
    public static SoundGallery Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject.transform.root.gameObject);
        }
        else
        {
            Instance=this;
        }
    }

    [SerializeField] protected Sound[] _sounds;
    [SerializeField] protected SoundCollection[] _soundCollecions;

    // Busca un sonido individual por su nombre
    public Sound FindSound(string _name)
    {
        Sound sound = System.Array.Find(_sounds, sound => sound.name == _name);
        return sound;
    }

    // Busca un sonido en una colección por su nombre
    public Sound FindSoundInCollection(string _soundName,string _collectionName)
    {
        SoundCollection soundC = System.Array.Find(_soundCollecions, sound => sound.name == _collectionName);
        if (soundC==null) 
        {
            return null;
        }

        return soundC.GetSound(_soundName);
    }

    // Busca un sonido aleatorio en una colección
    public Sound FindSoundInCollectionRandom(string _collectionName, bool even)
    {
        SoundCollection soundC = System.Array.Find(_soundCollecions, sound => sound.name == _collectionName);
        if (soundC==null) 
        {
            return null;
        }

        return soundC.GetRandom(even);
    }
}
