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
            Vector3 dir = playerPos.position - objPosition.position;
            print(objPosition.position);
            print(playerPos.position);

            if (Physics.Raycast(objPosition.position,dir.normalized, out hit, dir.magnitude, layer))
            {
                print(hit.transform.name);
                //if(hit.transform.gameObject == gameObject || (hit.transform.parent && hit.transform.parent == gameObject)
                //{
                    /*coverList[i].*/Owner = enemy;
                    print("found");
                    return i;
                //}
                //else
                //{
                //    print("Hit another thing");
                //}
            }
            else
            {
                print("Not Safe Spot");
            }

        }
        objPosition = null;
        return -1;
}
   public bool checkSafe(Transform playerPos,int index)
    {
        RaycastHit hit;
     Vector3   objPosition = HidePoints[index].position;
        Vector3 dir = playerPos.transform.position - objPosition;
        print($" {objPosition} + {dir} + {playerPos.position}");
        if (Physics.Raycast(objPosition, dir.normalized, out hit, dir.magnitude, layer))
        {
            print(hit.transform.name);
            if (hit.transform.gameObject.GetComponent<Player>() != null)
            {
                print("isUnsafe");
                return false;
            }
            //if (hit.transform.gameObject == gameObject)
            //{
            //    print("isSafe");
            //    return true;
            //}
            print("isSafe");
            return true;
        }
        else
        {
            print("isUnsafe");
            return false;
        }
    }
   
}
