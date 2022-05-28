using System.Collections.Generic;
using UnityEngine;

public class UpdatableController : MonoBehaviour
{
    private static readonly List<IUpdatable> updatables = new List<IUpdatable>();


    public static void Add(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }

    public static void Remove(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }

    public static void RemoveAll()
    {
        updatables.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Update(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        updatables.Clear();
    }
}
