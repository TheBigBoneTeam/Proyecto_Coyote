using UnityEngine;

public class OwnerableTransform
{
    public Transform transform;
    public Enemy Owner;
    public OwnerableTransform(Transform position)
    {
        this.transform = position;
        Owner = null;
    }
    public bool checkOwner(Enemy owner)
    {
        Debug.Log("checkOwner: "+Owner);
        if (Owner == null || this.Owner == owner)
        {
            Debug.Log("notNull");
            Owner = owner;
            return true;
        }
        else
        {
            return false;
        }
    }
}