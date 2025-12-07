using UnityEngine;
using UnityEngine.TextCore.Text;

public class playerMagnet : MonoBehaviour
{
    [SerializeField] bool goTowardPlayer;
    Player player;
  [SerializeField]  float speed;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player && player.HealthPoint < player._maxHealthPoint)
        {
            goTowardPlayer = true;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void OnDisable()
    {
        goTowardPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (goTowardPlayer)
        {
            transform.parent.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
