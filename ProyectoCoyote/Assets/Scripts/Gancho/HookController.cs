using System.Collections.Generic;
using System;
using UnityEngine;

public class HookController : MonoBehaviour, ISubject
{
    private int hookUsed { get; set; }
    public event Action<int> OnHookUsedUpdated;

    public HookController()
    {
        hookUsed = 0;
        Notify();
    }
    private void Notify()
    {
        OnHookUsedUpdated?.Invoke(hookUsed);
    }

    public void HookUsed()
    {
        hookUsed++;
        Notify();
    }
   
}
