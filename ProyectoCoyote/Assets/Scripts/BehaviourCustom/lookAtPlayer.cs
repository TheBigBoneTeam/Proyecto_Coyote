using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Update()
    {
        Vector3 lookTarget = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookTarget);
    }
}
