using System;
using UnityEngine;

public class OutOfScreenObject : IUpdatable
{
    private readonly Vector2 sceneDimension;
    private readonly IOutOfScreenObject obj;

    public event Action<IUpdatable, bool> SetUpdateActive;

    protected enum OutOfScreenDirection
    {
        Top, Bottom, Left, Right
    }

    public OutOfScreenObject(IOutOfScreenObject obj, Vector2 sceneDimension) 
    {
        obj.SetUpdateActive += OnSetUpdateActive;
        this.obj = obj;
        this.sceneDimension = sceneDimension;
    }

    private void OnSetUpdateActive(IUpdatable updatable, bool value) 
    {
        SetUpdateActive?.Invoke(this, value);
    }

    public void Update(float deltaTime)
    {
        if (obj.CurrentPosition.x > sceneDimension.x)
        {
            Teleport(OutOfScreenDirection.Right);
        }
        else if (obj.CurrentPosition.x < -sceneDimension.x)
        {
            Teleport(OutOfScreenDirection.Left);
        }
        else if (obj.CurrentPosition.y > sceneDimension.y)
        {
            Teleport(OutOfScreenDirection.Top);
        }
        else if (obj.CurrentPosition.y < -sceneDimension.y)
        {
            Teleport(OutOfScreenDirection.Bottom);
        }

        obj.Update(deltaTime);
    }

    private void Teleport(OutOfScreenDirection dir) 
    {
        switch (dir)
        {
            case OutOfScreenDirection.Top:
                obj.CurrentPosition -= new Vector2(0f, sceneDimension.y * 2f);
                break;
            case OutOfScreenDirection.Bottom:
                obj.CurrentPosition += new Vector2(0f, sceneDimension.y * 2f);
                break;
            case OutOfScreenDirection.Right:
                obj.CurrentPosition -= new Vector2(sceneDimension.x * 2f, 0f);
                break;
            case OutOfScreenDirection.Left:
                obj.CurrentPosition += new Vector2(sceneDimension.x * 2f, 0f);
                break;
        }
    }
}
