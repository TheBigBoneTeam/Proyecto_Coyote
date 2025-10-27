using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]

#region Propiedades del sonido
public class Sound
{
    public string name;             // Nombre
    public AudioClip audioClip;     // Formato del audio
    [Range(0f, 1f)]

    public float volume = 1.0f;     // Volumen
    [Range(0f, 3f)]

    public float pitch = 1.0f;      // Tono

    [Tooltip("Pitch randoms del sonido, si unos de ellos es -1, se usara el default")]
    public Vector2 pitchVariation = new Vector2(-1,-1);

    public float maxSoundDistance;  // Limite para sonidos 3D
}
#endregion

[System.Serializable]

public class SoundChance
{
    public Sound sound;
    public float chance;
}

[System.Serializable]

// Crea una coleccion de sonido
public class SoundCollection
{
    public string name;
    public SoundChance[] sounds;

    public Sound GetSound(string _name)
    {
       SoundChance sound = System.Array.Find(sounds, sound => sound.sound.name == _name);
       return sound.sound;
    }

    public Sound GetRandom(bool even)
    {
        if (!even)
        {
            float totalChance = 0f;
            foreach (var sound in sounds)
            {
                totalChance += sound.chance;
            }

            // Generate a random value between 0 and the total chance sum
            float randomValue = Random.Range(0f, totalChance);

            // Iterate through the items and subtract their chance until the random value falls within the chance range
            foreach (var sound in sounds)
            {
                if (randomValue < sound.chance)
                {
                    // Return the item if the random value is within its chance range
                    return sound.sound;
                }
                randomValue -= sound.chance;
            }

            return null;
        }
        else
        {
            int r = Random.Range(0, sounds.Length);
            return sounds[r].sound;
        }
    }
}
