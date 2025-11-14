using UnityEngine;

public class WaveCaller : MonoBehaviour
{
  public  bool started;
  public  int waveIndex;
    combatAreaManager combatAreaManager;
    private void Start()
    {
        combatAreaManager = GetComponentInParent<combatAreaManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        print("trigger" + other.gameObject.name);
        if (other.GetComponent<Player>() != null)
        {
            if (!started)
            {
                combatAreaManager.startWaveExternal(waveIndex);
                started = true;
            }
        }
    }
    public void restart()
    {
        started = false;
    }
}