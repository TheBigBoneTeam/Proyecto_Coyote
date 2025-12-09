using UnityEngine;
using UnityEngine.TextCore.Text;

public class playerMagnet : MonoBehaviour
{
    [SerializeField] public bool goTowardPlayer;
    Player player;
    [SerializeField] float minspeed;
    [SerializeField] float maxspeed;
  [SerializeField]  float speed;
    [SerializeField] HealOrb orb;
    private void OnTriggerEnter(Collider other)
    {
        if (goTowardPlayer)
            return;
        Player player = other.GetComponent<Player>();
        if (player && (player.HealthPoint < player._maxHealthPoint || !orb.careAboutMaxHealth))
        {
            goTowardPlayer = true;
            speed = Random.Range(minspeed, maxspeed);

            if (player.HealthPoint == 1) AudioManager.Instance.PlaySimpleSound("SFX - Vida 1", false, Vector2.zero, true, false);
            if (player.HealthPoint == 2) AudioManager.Instance.PlaySimpleSound("SFX - Vida 2", false, Vector2.zero, true, false);
            if (player.HealthPoint == 3) AudioManager.Instance.PlaySimpleSound("SFX - Vida 3", false, Vector2.zero, true, false);
            if (player.HealthPoint == 4) AudioManager.Instance.PlaySimpleSound("SFX - Vida 4", false, Vector2.zero, true, false);
            if (player.HealthPoint == 5) AudioManager.Instance.PlaySimpleSound("SFX - Vida 5", false, Vector2.zero, true, false);
            if (player.HealthPoint == 6) AudioManager.Instance.PlaySimpleSound("SFX - Vida 6", false, Vector2.zero, true, false);
            if (player.HealthPoint == 7) AudioManager.Instance.PlaySimpleSound("SFX - Vida 7", false, Vector2.zero, true, false);
            if (player.HealthPoint == 8) AudioManager.Instance.PlaySimpleSound("SFX - Vida 8", false, Vector2.zero, true, false);
            if (player.HealthPoint == 9) AudioManager.Instance.PlaySimpleSound("SFX - Vida 9", false, Vector2.zero, true, false);
            if (player.HealthPoint == 10) AudioManager.Instance.PlaySimpleSound("SFX - Vida 10", false, Vector2.zero, true, false);
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
