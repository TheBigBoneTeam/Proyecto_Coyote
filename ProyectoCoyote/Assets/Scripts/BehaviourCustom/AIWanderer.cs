using Services;
using System;
using UnityEngine;

public class AIWanderer : MonoBehaviour
{
    [SerializeField] Transform[] wanderPoints;
    bool random;
   [SerializeField] int currentPoint;

    private void Start()
    {
        currentPoint = -1;
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);
    }

    private void restart()
    {
        currentPoint =-1;
    }

    public Transform getRandomPoint(out int next, int current = -1)
    {
        if (wanderPoints == null || wanderPoints.Length == 0)
        {
            throw new Exception($"AI wanderer of object {name} doesnt have any asigned wanderPoints");
        }
        if (wanderPoints.Length == 1)
        {
            next = 0;
            return wanderPoints[0];
        }
        int returnPoint;
        do
        {
            returnPoint = UnityEngine.Random.Range(0, wanderPoints.Length);
        } while (returnPoint == current);
        next = returnPoint;
        return wanderPoints[returnPoint];

    }
    public Transform getNextPoint(out int next, int current = -1)
    {
        if (current < -1)
        {
            throw new IndexOutOfRangeException("Current cant be less than 0");
        }
        next = (current + 1) % wanderPoints.Length;
        return wanderPoints[next];
    }
    public Transform getPoint(int current = -1)
    {
        if (random)
        {
            return getRandomPoint(out currentPoint, currentPoint);
        }
        else
        {
            return getNextPoint(out currentPoint, currentPoint);
        }
    }
}