using Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDefenseAttackUIIndicator:DefenseAttackUIIndicator
{
    public override void setCharacter(AGameCharacter character)
    {
        if (DamageReceiver != null)
            DamageReceiver.GetComponent<AGameCharacter>().unSubscribeToDodgeAttack(Dodge);
        base.setCharacter(character);
        if(DamageReceiver != null)
        DamageReceiver.GetComponent<AGameCharacter>().subscribeToDodgeAttack(Dodge);

    }
    protected override void Start()
    {
        CanvasGroup = GetComponentInChildren<CanvasGroup>();
        setEnable(false);
    }
    protected override void Update()
    {

    }
    void Dodge(HitDirections d)
    {

        dodgeUISignalers[(int)d].GetComponent<Image>().color = new Color(0, 1, 0, 1);

    }
    IEnumerator endDodgeAnim(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj.GetComponent<Image>().color.b == 1)
        {
            obj.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
    }
}