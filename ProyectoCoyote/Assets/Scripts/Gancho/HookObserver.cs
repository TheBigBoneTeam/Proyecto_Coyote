using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

// Prueba de uso del patron Observer
public class HookObserver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _nUsedHook;

    public void Start()
    {
        _nUsedHook.SetText("Number of times hook is used: ");
    }
    public void Configure(ISubject hook)
    {
        hook.OnHookUsedUpdated += Updated;
    }

    private void Updated(int hookUsed)
    {
        _nUsedHook.SetText(hookUsed.ToString());
        
    }
}
