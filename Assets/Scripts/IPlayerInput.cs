using System;
using UnityEngine;


public interface IPlayerInput
{
    public event Action<Vector2> OnStartTurning;
    public event Action OnStopTurning;
    public event Action OnStartMoving;
    public event Action OnStopMoving;
    public event Action OnFire;
    public event Action<int> OnSwitchWeapon;
}
