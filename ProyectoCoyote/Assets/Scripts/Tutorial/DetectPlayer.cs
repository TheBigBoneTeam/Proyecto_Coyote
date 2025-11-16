using tutorial;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{   
   [SerializeField] AGameCharacter character;
    betaTutorial tutorial;
    [SerializeField] bool found;
    private void OnTriggerEnter(Collider other)
    {
        print("trigger" + other.gameObject.name);
        if (found)
        {
            return;
        }
        if (other.GetComponent<AGameCharacter>())
        {
            if (other.transform.Equals(character.transform))
            {
                found = true;
                print("encontrao");
                tutorial.currentGanchos++;
            }
        }
    }
    public void setCharacter(AGameCharacter character)
    {
        found = false;
        this.character = character;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        found = false;

        tutorial = FindAnyObjectByType<betaTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
