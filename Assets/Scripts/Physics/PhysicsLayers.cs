using UnityEngine;


public class ProjectPhysics
{
    private const string PlayerLayer = "Player";
    private const string AsteroidLayer = "Asteroid";
    private const string UFOLayer = "UFO";

    public int PlayerHitMask => LayerMask.GetMask(AsteroidLayer, UFOLayer);
    public int EnemyHitMask => LayerMask.GetMask(PlayerLayer);
}