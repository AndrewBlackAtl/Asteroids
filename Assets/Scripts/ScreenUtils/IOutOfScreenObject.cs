using UnityEngine;

public interface IOutOfScreenObject : IUpdatable
{
    public Vector2 CurrentPosition { get; set; }
}
