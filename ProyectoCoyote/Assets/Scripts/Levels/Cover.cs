using CombatEffect;
using System;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] Transform[] HidePoints;
    Enemy Owner;
    // OwnerableTransform[] coverList;
    [SerializeField] LayerMask environmentLayer;
    [SerializeField] LayerMask fullLayer;
    [SerializeField] float chickenHeight;
    Vector3 heightVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        heightVector = new Vector3(0,chickenHeight,0);

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
        if (Owner == owner)
        {
            Owner = null;
        }
    }

    public int getBestPoint(Enemy enemy, Transform playerPos, out Transform objPosition)
    {
        if (Owner != null && Owner != enemy)
        {
            print("Not Safe Spot");
            objPosition = null;
            return -1;
        }
        for (int i = 0; i < HidePoints.Length; i++)
        {

            objPosition = HidePoints[i];
            RaycastHit hit;
            Vector3 dir = objPosition.position - playerPos.position;
            print(objPosition.position);
            print(playerPos.position);
            Debug.DrawRay(playerPos.position, dir, Color.green, 2);
            if (Physics.Raycast(playerPos.position, dir, out hit, dir.magnitude, environmentLayer))
            {
                print($"Cover Found {name} : {hit.transform.name}");
                Vector3 shootPos = HidePoints[i].position + heightVector;
                dir = playerPos.position - shootPos;
                Debug.DrawRay(playerPos.position, dir, Color.blue,2);
                print("Safe Spot");
                if (Physics.Raycast(shootPos, dir, out hit, dir.magnitude, fullLayer))
                {
                    if (hit.transform == playerPos)
                    {
                        Owner = enemy;
                        print("Can Shoot Spot");
                        return i;
                    }
                }
                //if (hit.transform.IsChildOf(transform) || hit.transform == transform)
                //{
                //    /*coverList[i].*/
                //    Owner = enemy;
                //    print("Safe Spot");
                //    return i;
                //}
                //else
                //{
                //    print("Hit another thing");
                //}
            }
        }
        print("Not Safe Spot");
        objPosition = null;
        return -1;
    }
    public bool checkSafe(Transform enemyPos, Transform playerPos, int index)
    {
        RaycastHit hit;
        Vector3 objPosition = HidePoints[index].position;
        Vector3 dir = objPosition - playerPos.position;
        print($" {objPosition} + {dir} + {playerPos.position}");
        Debug.DrawRay(playerPos.position, dir, Color.green, 2);

        if (Physics.Raycast(playerPos.position, dir, out hit, dir.magnitude, environmentLayer))
        {
            print(hit.transform.name);
            dir = playerPos.position - enemyPos.position;
            Debug.DrawRay(playerPos.position, dir, Color.blue, 2);

            print("Safe Spot");
           
                    return true;
        //if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            //{

            //    /*coverList[i].*/
            //    print($"Check Cover {name} Safe Spot");
            //    return true;
            //}
            ////if (hit.transform.gameObject == gameObject)
            ////{
            ////    print("isSafe");
            ////    return true;
            ////}
            //print($"Check Cover {name} Safe but no attack");
            //return false;
        }
        
            print($"Check Cover {name} isUnsafe or cant shoot");
            return false;
        
    }
    public bool canShootPlayer(Transform playerPos,int index)
    {
        Vector3 objPosition = HidePoints[index].position + heightVector;
        Vector3 dir = playerPos.position - objPosition;
        Debug.DrawRay(playerPos.position, dir, Color.blue, 2);
        RaycastHit hit;

        print("Safe Spot");
        if (Physics.Raycast(objPosition, dir, out hit, dir.magnitude, fullLayer))
        {
            if (hit.transform == playerPos)
            {
                print("Can Shoot Spot");
                return true;
            }
        }
        return false;
    }

}
