using UnityEngine;

public class LinearMovement 
{
    public Vector2 CurrentPos { get; set; }

    public void Move(float speed, Vector2 direction) 
    {
        CurrentPos += speed * direction;
    }
}
