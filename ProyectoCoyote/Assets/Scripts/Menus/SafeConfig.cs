using UnityEngine;

public class SafeConfig : MonoBehaviour
{
    public static SafeConfig Instance;

    public float generalValue { get; set; }
    public float sfxValue { get; set; }
    public float musicValue { get; set; }

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



    
}
