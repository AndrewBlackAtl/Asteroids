using System.Collections.Generic;
using UnityEngine;

public class UpdatableController : MonoBehaviour
{
    private readonly LinkedList<IUpdatable> updatables = new LinkedList<IUpdatable>();


    public void Add(IUpdatable updatable)
    {
        updatable.SetUpdateActive += OnSetActive;

        updatables.AddLast(updatable);
    }

    private void OnSetActive(IUpdatable sender, bool value)
    {
        if (value && !updatables.Contains(sender))
        {
            updatables.AddLast(sender);
        }
        else if (!value && updatables.Contains(sender))
        {
            updatables.Remove(sender);
        }
    }

    public void RemoveAll()
    {
        updatables.Clear();
    }

    private void Update()
    {
        var current = updatables.First;
        while (current != null)
        {
            current.Value.Update(Time.deltaTime);
            current = current.Next;
        }
    }

    private void OnDestroy()
    {
        updatables.Clear();
    }
}
