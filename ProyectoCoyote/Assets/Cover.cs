using System;
using UnityEngine;

public class Cover : MonoBehaviour
{
 [SerializeField]   Transform[] HidePoints;
    Enemy Owner;
   // OwnerableTransform[] coverList;
  [SerializeField]   LayerMask layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        //coverList = new OwnerableTransform[HidePoints.Length];
        //for (int i = 0; i < HidePoints.Length; i++)
        //{
        //    coverList[i] = new OwnerableTransform(HidePoints[i]);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void returnOwnerShip(Enemy owner)
    {
        if(Owner == owner)
        {
            Owner = null;
        }
    }
   
    public int getBestPoint(Enemy enemy,Transform playerPos,out Transform objPosition)
    {
        for (int i = 0; i < HidePoints.Length; i++)
        {
            if (Owner != null && Owner !=enemy)
            {
                continue;
            }
            objPosition = HidePoints[i];
            RaycastHit hit;
            Vector3 dir = objPosition.position - playerPos.position;
                print(objPosition.position);
            print(playerPos.position);
         //   Debug.DrawRay(playerPos.position, dir, Color.green,10);
            if (Physics.Raycast(playerPos.position, dir, out hit, dir.magnitude, layer))
            {
                print(hit.transform.name);

                if (hit.transform.IsChildOf(transform) || hit.transform == transform)
                {
                    /*coverList[i].*/
                    Owner = enemy;
                    print("Safe Spot");
                    return i;
                }
                else
                {
                    print("Hit another thing");
                }
            }
        }
        print("Not Safe Spot");
        objPosition = null;
        return -1;
}
   public bool checkSafe(Transform playerPos,int index)
    {
        RaycastHit hit;
     Vector3   objPosition = HidePoints[index].position;
        Vector3 dir = objPosition - playerPos.position;
        print($" {objPosition} + {dir} + {playerPos.position}");
        if (Physics.Raycast(playerPos.position, dir, out hit, dir.magnitude, layer))
        {
            print(hit.transform.name);
            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            {
                /*coverList[i].*/
                print("Safe Spot");
                return true;
            }
            //if (hit.transform.gameObject == gameObject)
            //{
            //    print("isSafe");
            //    return true;
            //}
            print("Safe but no attack");
            return false;
        }
        else
        {
            print("isUnsafe");
            return false;
        }
    }
   
}
