using UnityEngine;


public abstract class OutOfScreenTeleporter : UpdatableBase
{
    private readonly protected Vector2 sceneDimension;
    protected abstract Vector2 CurrentPosition { get; set; }

    private enum OutOfScreenDirection
    {
        Top, Bottom, Left, Right
    }

    public OutOfScreenTeleporter(Vector2 sceneDimension) 
    {
        this.sceneDimension = sceneDimension;
    }

    public override void Update(float deltaTime)
    {
        if (CurrentPosition.x > sceneDimension.x)
        {
            Teleport(OutOfScreenDirection.Right);
        }
        else if (CurrentPosition.x < -sceneDimension.x)
        {
            Teleport(OutOfScreenDirection.Left);
        }
        else if (CurrentPosition.y > sceneDimension.y)
        {
            Teleport(OutOfScreenDirection.Top);
        }
        else if (CurrentPosition.y < -sceneDimension.y)
        {
            Teleport(OutOfScreenDirection.Bottom);
        }
    }

    private void Teleport(OutOfScreenDirection dir) 
    {
        switch (dir)
        {
            case OutOfScreenDirection.Top:
                CurrentPosition -= new Vector2(0f, sceneDimension.y * 2f);
                break;
            case OutOfScreenDirection.Bottom:
                CurrentPosition += new Vector2(0f, sceneDimension.y * 2f);
                break;
            case OutOfScreenDirection.Right:
                CurrentPosition -= new Vector2(sceneDimension.x * 2f, 0f);
                break;
            case OutOfScreenDirection.Left:
                CurrentPosition += new Vector2(sceneDimension.x * 2f, 0f);
                break;
        }
    }
}
