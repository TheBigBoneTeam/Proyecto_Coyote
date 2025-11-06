using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    AGameCharacter gameCharacter;
    [SerializeField] Transform bulletSpawnPoint;
    public void Shoot(Vector3 obj)
    {
      GameObject bulet =  Instantiate(bullet);
        bulet.GetComponent<baseBullet>().StartBulletMovement(gameCharacter, bulletSpawnPoint.position, obj);
    }
    private void Start()
    {
        gameCharacter = GetComponentInParent<AGameCharacter>();
    }
}