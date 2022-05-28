using System;
using UnityEngine;


public abstract class OutOfScreenUpdatable : UpdatableBase
{
    private protected static Vector2 sceneDimension;
    protected abstract Vector2 CurrentPosition { get; set; }

    private protected event Action<OutOfScreenDirection> OnOutOfScreen;

    private protected enum OutOfScreenDirection
    {
        Top, Bottom, Left, Right
    }


    public static void SetSceneDimension(Vector2 value)
    {
        sceneDimension = value;
    }

    public override void Update(float deltaTime)
    {
        if (CurrentPosition.x > sceneDimension.x)
        {
            OnOutOfScreen?.Invoke(OutOfScreenDirection.Right);
        }
        else if (CurrentPosition.x < -sceneDimension.x)
        {
            OnOutOfScreen?.Invoke(OutOfScreenDirection.Left);
        }
        else if (CurrentPosition.y > sceneDimension.y)
        {
            OnOutOfScreen?.Invoke(OutOfScreenDirection.Top);
        }
        else if (CurrentPosition.y < -sceneDimension.y)
        {
            OnOutOfScreen?.Invoke(OutOfScreenDirection.Bottom);
        }
    }
}
