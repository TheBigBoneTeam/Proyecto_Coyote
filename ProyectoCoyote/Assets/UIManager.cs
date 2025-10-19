using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] TMP_Text lifeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<Player>().subscribeToLifeChange(changePlayerLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void changePlayerLife(int playerLife)
    {
        lifeText.text = playerLife.ToString();
    }
}
