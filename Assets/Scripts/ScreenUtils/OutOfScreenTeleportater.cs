using UnityEngine;


public abstract class OutOfScreenTeleporter : OutOfScreenUpdatable
{
    public OutOfScreenTeleporter()
    {
        OnOutOfScreen += OutOfScreenTeleportator_OnOutOfScreen;
    }

    private void OutOfScreenTeleportator_OnOutOfScreen(OutOfScreenDirection dir)
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
