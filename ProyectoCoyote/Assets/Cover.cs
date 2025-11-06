using UnityEngine;

public class Cover : MonoBehaviour
{
 [SerializeField]   Transform[] HidePoints;
    bool[] occupiedPoints;
  [SerializeField]   LayerMask layer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        occupiedPoints = new bool[HidePoints.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isFree(int index)
    {
        if (HidePoints.Length <= index || occupiedPoints[index])
        {
            return false;
        }
        return true;
    }
    public int getBestPoint(Transform playerPos,out Transform objPosition)
    {
        for (int i = 0; i < HidePoints.Length; i++)
        {
            if (occupiedPoints[i])
            {
                continue;
            }
            objPosition = HidePoints[i];
            RaycastHit hit;
            Vector3 dir = playerPos.position - objPosition.position;

            if (Physics.Raycast(objPosition.position,dir.normalized, out hit, dir.magnitude, layer))
            {
                print(hit.transform.name);
                if(hit.transform.gameObject == gameObject)
                {
                    print("found");
                    return i;
                }
                else
                {
                    print("Hit another thing");
                }
            }
            else
            {
                print("Not Safe Spot");
            }

        }
        objPosition = null;
        return -1;
}
}
