using UnityEngine;


public interface IAmmo
{
    public void Launch(Vector2 pos, Vector2 dir, int mask);
}