using UnityEngine;
[CreateAssetMenu(fileName = "StanceData", menuName = "ScriptableObjects/Combat/StanceData", order = 1)]

public class StanceData : ScriptableObject
{
    [field: SerializeField] public AnimationClip clip { get; private set; }

    [field: SerializeField] public HitDirections[] DefenseDirections { get; private set; }

}
