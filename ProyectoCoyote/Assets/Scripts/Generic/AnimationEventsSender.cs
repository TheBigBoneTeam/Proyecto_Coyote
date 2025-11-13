using UnityEngine;

public class AnimationEventsSender : MonoBehaviour
{
   public void PlaySound(string soundName)
   {
        AudioManager.Instance.PlaySimpleSound(soundName,false,Vector2.zero,true,false);
   }
}