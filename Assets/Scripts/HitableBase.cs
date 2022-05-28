using System;
using UnityEngine;


public class HitableBase : MonoBehaviour, IHitable, IHitSender
{
    public event Action OnHit;


    public void Hit()
    {
        OnHit?.Invoke();
    }

    private void OnDestroy()
    {
        OnHit = null;
    }
}