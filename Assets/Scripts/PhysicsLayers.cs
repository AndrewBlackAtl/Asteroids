using UnityEngine;


public class ProjectPhysics
{
    private const string PlayerLayer = "Player";
    private const string AsteroidLayer = "Asteroid";
    private const string UFOLayer = "UFO";

    public static int GetPlayerHitMask => LayerMask.GetMask(AsteroidLayer, UFOLayer);
    public static int GetEnemyHitMask => LayerMask.GetMask(PlayerLayer);
}