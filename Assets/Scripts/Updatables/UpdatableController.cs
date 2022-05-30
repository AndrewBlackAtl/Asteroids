using System.Collections.Generic;
using UnityEngine;

public class UpdatableController : MonoBehaviour
{
    private static UpdatableController i;
    public static UpdatableController I => i;


    private readonly LinkedList<IUpdatable> updatables = new LinkedList<IUpdatable>();



    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }
    }

    public void Add(IUpdatable updatable)
    {
        updatables.AddLast(updatable);
    }

    public void Remove(IUpdatable updatable)
    {
        updatables.Remove(updatable);
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

        if (this != null)
        {
            i = null;
        }
    }
}
