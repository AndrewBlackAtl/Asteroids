using System;
using UnityEngine;


public class CollisionEventSender : MonoBehaviour, ICollisionEventSender
{
    public event Action OnCollisionEnter;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter?.Invoke();
    }

    private void OnDestroy()
    {
        OnCollisionEnter = null;
    }
}
