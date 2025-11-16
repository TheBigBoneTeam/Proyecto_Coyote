using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class tutorialGun : Gun
{
    [SerializeField] Transform[] randomPos;
    Player player;
    public override void Shoot(Vector3 obj, baseBullet bul = null)
    {
        int random = UnityEngine.Random.Range(0, randomPos.Length);
        bulletSpawnPoint = randomPos[random];
        if (bul == null)
        {
            GameObject bulet = Instantiate(bullet);
            bul = bulet.GetComponent<baseBullet>();
        }
        print("shoot" + name);
        print("canshoot" + name);
        bul.StartBulletMovement(gameCharacter, bulletSpawnPoint.position, obj);
        bul.subscribeToDestroy((b)=>StartCoroutine(shootDelay()));
        shootAction?.Invoke(bul);
    }
    public void startShooting()
    {
        randomPos = transform.GetComponentsInChildren<Transform>();
        player = FindAnyObjectByType<Player>();
        Shoot(player.transform.position);
    }
    public IEnumerator shootDelay()
    {
        yield return new WaitForSeconds(1);
        Shoot(player.transform.position);
    }
}