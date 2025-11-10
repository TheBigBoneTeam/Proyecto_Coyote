using UnityEngine;

public interface IObserver
{
    void Updated(ISubject subject);
}
