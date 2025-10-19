using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DefenseAttackUIIndicator : MonoBehaviour
{
  [SerializeField] protected DamageReceiver DamageReceiver;
    [SerializeField] Attack attack;


    public GameObject[] attackUISignalers;
    public GameObject[] dodgeUISignalers;

    [SerializeField] Vector3 paddingPosition;

    CanvasGroup CanvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanvasGroup = GetComponentInChildren<CanvasGroup>();
        setUp();
       // FindAnyObjectByType<PlayerMovement>().GetComponent<DamageReceiver>().subscribeToStateChange(StateChange);
    }
    protected void setUp()
    {
        if (DamageReceiver != null)
        {
            DamageReceiver.subscribeToStateChange(DodgeStateChange);
        }
        if (attack != null)
        {
            attack.subscribeToStateChange(AttackStateChange);
        }
        else
        {
            setEnable(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void DodgeStateChange(DamageReceiver.ReceiverState state)
    {
        print("dodge state change");
        if (state.isDodge)
        {
            for (int i = 0; i < dodgeUISignalers.Length; i++)
            {
                setDodgeObject(dodgeUISignalers[i], state.directions.Contains((HitDirections)i));
            }
        }
        else
        {
            for (int i = 0; i < dodgeUISignalers.Length; i++)
            {
                setDodgeObject(dodgeUISignalers[i], false);
            }
        }
    }
    public void AttackStateChange(Attack.AttackState state)
    {
        print(attack);
        for (int i = 0; i < dodgeUISignalers.Length; i++)
        {
            if (state.hitDirections.Length == 0)
            {
                setAttackObject(attackUISignalers[i], true);

            }
            else
            {
                setAttackObject(attackUISignalers[i], state.hitDirections.Contains((HitDirections)i));
            }
        }
    }
    private void OnDestroy()
    {
        if(DamageReceiver != null)
       DamageReceiver.unSubscribeToStateChange(DodgeStateChange);

    }
    public void setEnable(bool enable)
    {
        CanvasGroup.alpha = enable ? 1:0;
    }

    public virtual void setCharacter(AGameCharacter character)
    {
        if(DamageReceiver != null)
        DamageReceiver.unSubscribeToStateChange(DodgeStateChange);
        DamageReceiver = character.GetComponent<DamageReceiver>();
        if (DamageReceiver != null)
        {

            setEnable(true);
            this.transform.parent = character.transform;
            this.transform.localPosition = Vector3.zero + paddingPosition;
            DamageReceiver.subscribeToStateChange(DodgeStateChange);
        }
        else
        {
            setEnable(false);
        }

    }
    public void setEnemy(AGameCharacter character)
    {
        if(attack != null)
        attack.unSubscribeToStateChange(AttackStateChange);
        if (character != null)
        {
            attack = character.GetComponentInChildren<Attack>();
            if (attack != null)
            attack.subscribeToStateChange(AttackStateChange);
        }
    }
    public void setDodgeObject(GameObject obj, bool on)
    {
        obj.GetComponent<Image>().enabled = on;
    }
    public void setAttackObject(GameObject obj, bool on)
    {
        obj.GetComponent<Image>().color = new Color(on ? 0 : 1, 0,0,1);

    }
}
