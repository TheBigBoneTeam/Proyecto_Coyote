using UnityEngine;

public class tutorialAnimationEvent : MonoBehaviour
{
    Attack attack;
    enemigoTutorial tutorial;

    void Start()
    {
        attack = GetComponentInChildren<Attack>();
        tutorial = GetComponentInParent<enemigoTutorial>(); 
    }
    public void setAttackDataDependOnParry(AttackData data)
    {
        attack.LoadData(data);

            attack.setParry(tutorial.canBeParry);
        
    }
}