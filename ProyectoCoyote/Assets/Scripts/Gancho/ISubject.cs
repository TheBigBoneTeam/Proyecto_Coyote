using System;
using UnityEngine;

public interface ISubject
{
    event Action<int> OnHookUsedUpdated;
}
