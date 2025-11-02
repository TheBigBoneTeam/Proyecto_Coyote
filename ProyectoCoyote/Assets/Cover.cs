using UnityEngine;

public class Cover : MonoBehaviour
{
    Transform[] HidePoints;
    bool[] occupiedPoints;
    LayerMask layer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
            if(Physics.Raycast(objPosition.position, Vector3.Normalize(playerPos.position - objPosition.position), out hit, 5f, layer))
            {
                if(hit.transform.gameObject == this)
                {
                    return i;
                }
            }
           
        }
        objPosition = null;
        return -1;
}
}
